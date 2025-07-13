using AMFormsCST.Core.Utils;
using AMFormsCST.Desktop.Interfaces;
using AMFormsCST.Desktop.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace AMFormsCST.Desktop.ViewModels.Pages;

public partial class DashboardViewModel : ViewModel
{
    [ObservableProperty]
    private ObservableCollection<Note> _notes = [];

    private Note _selectedNote;
    public Note SelectedNote
    {
        get => _selectedNote;
        set
        {
                // Unsubscribe from the old note before replacing it
                if (_selectedNote != null)
                {
                    _selectedNote.PropertyChanged -= OnModelPropertyChanged;
                }

                SetProperty(ref _selectedNote, value);

                // Subscribe to the new note
                if (_selectedNote != null)
                {
                    _selectedNote.PropertyChanged += OnModelPropertyChanged; 
                
                EnsureBlankItem(_selectedNote.Dealers, () => new Dealer(this));
                EnsureBlankItem(_selectedNote.Contacts, () => new Contact());
                EnsureBlankItem(_selectedNote.Forms, () => new Form(this));
            }

                // Manually trigger updates for all dependent properties
                // in case the initial state needs to be wired up
                OnModelPropertyChanged(this, new PropertyChangedEventArgs(nameof(Note.SelectedDealer)));
                OnModelPropertyChanged(this, new PropertyChangedEventArgs(nameof(Note.SelectedContact)));
                OnModelPropertyChanged(this, new PropertyChangedEventArgs(nameof(Note.SelectedForm)));
            
        }
    }

    [ObservableProperty]
    private bool _isDebugMode = false;

    [ObservableProperty]
    private Visibility _debugVisibility = Visibility.Collapsed; 
    
    private int _uiRefreshCounter;
    public int UiRefreshCounter
    {
        get => _uiRefreshCounter;
        private set => SetProperty(ref _uiRefreshCounter, value); // Assumes SetProperty raises PropertyChanged
    }


    public DashboardViewModel()
    {
        //// Initialize the Notes collection with a default Note if needed
        Notes.Add(new Note(this));
        //if (Notes.Count == 0)
        //{
        //    Notes.Add(new Note(this));
        //    Notes.Add(new Note(this));
        //    Notes.Add(new Note(this));
        //}
        //// Set the initial selected note to the first one in the collection
        SelectedNote = Notes[0];
        Notes[0].Select();

        //SelectedNote.Dealers.Clear();
        //SelectedNote.Dealers.Add(new Dealer(this) { Name = "Default Dealer 1" });
        //SelectedNote.Dealers.Add(new Dealer(this) { Name = "Default Dealer 2" });
        //SelectedNote.Dealers.Add(new Dealer(this) { Name = "Default Dealer 3" });
        //SelectedNote.SelectedDealer = SelectedNote.Dealers[0];
        //SelectedNote.SelectedDealer.Select();

        //SelectedNote.SelectedDealer.Companies.Clear();
        //SelectedNote.SelectedDealer.Companies.Add(new Company { Name = "Default Company 1" });
        //SelectedNote.SelectedDealer.Companies.Add(new Company { Name = "Default Company 2" });
        //SelectedNote.SelectedDealer.Companies.Add(new Company { Name = "Default Company 3" });
        //SelectedNote.SelectedDealer.SelectedCompany = SelectedNote.SelectedDealer.Companies[0];
        //SelectedNote.SelectedDealer.SelectedCompany.Select();

        //SelectedNote.Contacts.Clear();
        //SelectedNote.Contacts.Add(new Contact { Name = "Contact 1" });
        //SelectedNote.Contacts.Add(new Contact { Name = "Contact 2" });
        //SelectedNote.Contacts.Add(new Contact { Name = "Contact 3" });
        //SelectedNote.SelectedContact = SelectedNote.Contacts[0];
        //SelectedNote.SelectedContact.Select();

        //SelectedNote.Forms.Clear();
        //SelectedNote.Forms.Add(new Form { Name = "Form 1" });
        //SelectedNote.Forms.Add(new Form { Name = "Form 2" });
        //SelectedNote.Forms.Add(new Form { Name = "Form 3" });
        //SelectedNote.SelectedForm = SelectedNote.Forms[0];
        //SelectedNote.SelectedForm.Select();
#if DEBUG
        IsDebugMode = true;
#endif

        if (IsDebugMode) _debugVisibility = Visibility.Visible;
    }

    [RelayCommand]
    private void OnNoteClicked(Guid caseId)
    {
        if (SelectedNote is null || 
            caseId == SelectedNote.Id) return;

        foreach (var note in Notes.ToList())
        {
            if (note.Id == caseId)
            {
                SelectedNote = note;
                note.Select();
                continue;
            }
            note.Deselect();
        }
    }

    [RelayCommand]
    private void OnDealerClicked(ISelectable company)
    {
        if (SelectedNote is null ||
            SelectedNote.SelectedDealer is null || 
            company.Id == SelectedNote.SelectedDealer.Id) return;

        SelectedNote.SelectDealer(company);
    }

    [RelayCommand]
    private void OnCompanyClicked(ISelectable company)
    {
        if (SelectedNote is null ||
            SelectedNote.SelectedDealer is null || 
            SelectedNote.SelectedDealer.SelectedCompany is null ||
            company.Id == SelectedNote.SelectedDealer.SelectedCompany.Id) return;

        SelectedNote.SelectedDealer.SelectCompany(company);
    }

    [RelayCommand]
    private void OnContactClicked(ISelectable contact)
    {
        if (SelectedNote is null ||
            SelectedNote.SelectedContact is null ||
            contact.Id == SelectedNote.SelectedContact.Id) return;

        SelectedNote.SelectContact(contact);
    }

    [RelayCommand]
    private void OnFormClicked(ISelectable form)
    {
        if (SelectedNote is null ||
            SelectedNote.SelectedForm is null ||
            form.Id == SelectedNote.SelectedForm.Id) return;

        SelectedNote.SelectForm(form);
    }

    [RelayCommand]
    private void OnDealClicked(ISelectable deal)
    {
        if (SelectedNote is null ||
            SelectedNote.SelectedForm is null ||
            SelectedNote.SelectedForm.SelectedTestDeal is null ||
            deal.Id == SelectedNote.SelectedForm.SelectedTestDeal.Id) return;

        SelectedNote.SelectedForm.SelectTestDeal(deal);
    }

    [RelayCommand]
    private void OnDeleteItemClicked(object itemToDelete)
    {
        if (itemToDelete == null) return;

        // Check if the item is a Note
        if (itemToDelete is Note noteToDelete)
        {
            // Check if we are deleting the currently selected note
            bool isDeletingSelected = SelectedNote == noteToDelete;
            Notes.Remove(noteToDelete);

            // If the selected note was deleted, select the first one in the list
            if (isDeletingSelected)
            {
                SelectedNote = Notes.FirstOrDefault();
                SelectedNote.Select();
            }
        }
        // Check if the item is a Dealer
        else if (itemToDelete is Dealer dealerToDelete)
        {
            bool isDeletingSelected = SelectedNote.SelectedDealer == dealerToDelete;
            SelectedNote.Dealers.Remove(dealerToDelete);

            if (isDeletingSelected)
            {
                SelectedNote.SelectDealer(SelectedNote.Dealers.FirstOrDefault());
                SelectedNote.SelectedDealer?.Select();
            }
        }
        // Check if the item is a Company
        else if (itemToDelete is Company companyToDelete)
        {
            bool isDeletingSelected = SelectedNote.SelectedDealer.SelectedCompany == companyToDelete;
            SelectedNote.SelectedDealer.Companies.Remove(companyToDelete);

            if (isDeletingSelected)
            {
                SelectedNote.SelectedDealer.SelectCompany(SelectedNote.SelectedDealer.Companies.FirstOrDefault());
                SelectedNote.SelectedDealer.SelectedCompany?.Select();
            }
        }
        // Add similar blocks for Contact and Form...
        else if (itemToDelete is Contact contactToDelete)
        {
            bool isDeletingSelected = SelectedNote.SelectedContact == contactToDelete;
            SelectedNote.Contacts.Remove(contactToDelete);

            if (isDeletingSelected)
            {
                SelectedNote.SelectContact(SelectedNote.Contacts.FirstOrDefault());
                SelectedNote.SelectedContact?.Select();
            }
        }
        else if (itemToDelete is Form formToDelete)
        {
            bool isDeletingSelected = SelectedNote.SelectedForm == formToDelete;
            SelectedNote.Forms.Remove(formToDelete);

            if (isDeletingSelected)
            {
                SelectedNote.SelectForm(SelectedNote.Forms.FirstOrDefault());
                SelectedNote.SelectedForm?.Select();
            }
        }
    }

    /// <summary>
    /// Set the state of Debug Mode
    /// if <param name="debugModeEnable"></param> is null, it will toggle the current state
    /// </summary>
    /// <param name="debugModeEnable"></param>
    public void ToggleDebugMode(bool? debugModeEnable)
    {
        if (!debugModeEnable.HasValue)
            debugModeEnable = !IsDebugMode;

        IsDebugMode = debugModeEnable.Value;
        DebugVisibility = IsDebugMode ? Visibility.Visible : Visibility.Collapsed;
    }
    public void TriggerRadioButtonRefresh()
    {
        UiRefreshCounter++;
    }
    public void OnModelPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        string propertyName = e.PropertyName;

        if (SelectedNote is null) return;

        if (propertyName == nameof(Note.CaseNumber) ||
            propertyName == nameof(Dealer.ServerCode) ||
            propertyName == nameof(Company.CompanyCode) ||
            propertyName == nameof(Contact.Name) ||
            propertyName == nameof(Form.Name) ||
            propertyName == nameof(TestDeal.DealNumber) ||
            propertyName == nameof(TestDeal.Purpose) ||
            propertyName == nameof(Note.Notes) ||
            propertyName == nameof(Dealer.Name) ||
            propertyName == nameof(Contact.Email) ||
            propertyName == nameof(Contact.Phone) ||
            propertyName == nameof(Contact.PhoneExtension) ||
            propertyName == nameof(Form.Notes))
        {
            UiRefreshCounter++;
        }

        if (propertyName == nameof(IBlankMaybe.IsBlank))
        {
            if (sender is Note)
            {
                EnsureBlankItem(Notes, () => new Note(this));
            }
            else if (sender is Dealer)
            {
                if (SelectedNote?.Dealers != null)
                {
                    EnsureBlankItem(SelectedNote.Dealers, () => new Dealer(this));
                }
            }
            else if (sender is Company)
            {
                if (SelectedNote?.SelectedDealer?.Companies != null)
                {
                    EnsureBlankItem(SelectedNote.SelectedDealer.Companies, () => new Company());
                }
            }
            else if (sender is Contact)
            {
                if (SelectedNote?.Contacts != null)
                {
                    EnsureBlankItem(SelectedNote.Contacts, () => new Contact());
                }
            }
            else if (sender is Form)
            {
                if (SelectedNote?.Forms != null)
                {
                    EnsureBlankItem(SelectedNote.Forms, () => new Form(this));
                }
            }
            else if (sender is TestDeal)
            {
                if (SelectedNote?.SelectedForm?.TestDeals != null)
                {
                    EnsureBlankItem(SelectedNote.SelectedForm.TestDeals, () => new TestDeal());
                }
            }
            else if (propertyName == "IsSelected")
            {
                return;
            }

        }

        
        if (propertyName == nameof(Note.SelectedDealer))
        {
            // The selected dealer has changed. We need to listen to the new one.
            var note = sender as Note;
            if (note != null)
            {
                // This assumes the Dealer object also has a PropertyChanged event
                // and you can subscribe/unsubscribe from it.
                // You will need to manage the "old" dealer to unsubscribe.
                // This often requires storing the old dealer temporarily.
            }
        }

        // Add similar logic for SelectedCompany, SelectedContact, etc.
        if (propertyName == nameof(Note.SelectedDealer))
        {
            if (SelectedNote?.SelectedDealer != null)
            {
                EnsureBlankItem(SelectedNote.SelectedDealer.Companies, () => new Company());
            }
        }
        // Add similar checks for other Selected properties if they manage nested collections.
        // This is where you put EnsureBlankItem for the *next level down* when a selection changes.
        else if (propertyName == nameof(Dealer.SelectedCompany))
        {
            // If Company had nested collections, you'd add EnsureBlankItem for them here.
        }
        else if (propertyName == nameof(Note.SelectedContact))
        {
            // If Contact had nested collections, add EnsureBlankItem for them here.
        }
        else if (propertyName == nameof(Note.SelectedForm))
        {
            if (SelectedNote?.SelectedForm != null)
            {
                EnsureBlankItem(SelectedNote.SelectedForm.TestDeals, () => new TestDeal());
            }
        }
        else if (propertyName == nameof(Form.SelectedTestDeal))
        {
            // If TestDeal had nested collections, add EnsureBlankItem for them here.
        }
    }
    private void EnsureBlankItem<T>(ObservableCollection<T> collection, Func<T> factory)
    where T : class, IBlankMaybe, INotifyPropertyChanged
    {
        if (collection == null) return;

        // 1. Find all blank items currently in the collection.
        //    Use .ToList() to create a safe copy to iterate over.
        var blankItems = collection.Where(item => item.IsBlank).ToList();

        // 2. If there are multiple blanks, consolidate them down to one.
        if (blankItems.Count > 1)
        {
            // Keep the first one, remove the rest.
            foreach (var extraBlank in blankItems.Skip(1))
            {
                collection.Remove(extraBlank);
            }
        }

        // 3. Now, get the single remaining blank item (or null if none exist).
        var theOneBlank = collection.FirstOrDefault(item => item.IsBlank);

        // 4. If no blank item exists at all, create one and add it to the end.
        if (theOneBlank == null)
        {
            var newItem = factory();
            newItem.PropertyChanged += OnModelPropertyChanged;
            collection.Add(newItem);
        }
        // 5. If a blank item DOES exist, make sure it's at the end of the list.
        else
        {
            int blankIndex = collection.IndexOf(theOneBlank);
            if (blankIndex < collection.Count - 1)
            {
                collection.Move(blankIndex, collection.Count - 1);
            }
        }
    }
}