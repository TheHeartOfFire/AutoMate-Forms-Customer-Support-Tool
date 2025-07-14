using AMFormsCST.Core.Types.Notebook;
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
    private ObservableCollection<NoteModel> _notes = [];

    private NoteModel _selectedNote;
    public NoteModel SelectedNote
    {
        get => _selectedNote;
        set
        {
                if (_selectedNote != null)
                {
                    _selectedNote.PropertyChanged -= OnModelPropertyChanged;
                }

                SetProperty(ref _selectedNote, value);

                if (_selectedNote != null)
                {
                    _selectedNote.PropertyChanged += OnModelPropertyChanged; 
                
                EnsureBlankItem(_selectedNote.Dealers, () => new Dealer(this));
                EnsureBlankItem(_selectedNote.Contacts, () => new Contact());
                EnsureBlankItem(_selectedNote.Forms, () => new Form(this));
            }

                OnModelPropertyChanged(this, new PropertyChangedEventArgs(nameof(NoteModel.SelectedDealer)));
                OnModelPropertyChanged(this, new PropertyChangedEventArgs(nameof(NoteModel.SelectedContact)));
                OnModelPropertyChanged(this, new PropertyChangedEventArgs(nameof(NoteModel.SelectedForm)));
            
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
        private set => SetProperty(ref _uiRefreshCounter, value); 
    }


    public DashboardViewModel()
    {
        Notes.Add(new NoteModel(this));

        _selectedNote = Notes[0];
        Notes[0].Select();

        #region DEBUG
#if DEBUG
        IsDebugMode = true;
#endif
        #endregion

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

        if (itemToDelete is NoteModel noteToDelete)
        {
            bool isDeletingSelected = SelectedNote == noteToDelete;
            Notes.Remove(noteToDelete);

            SelectedNote = Notes.FirstOrDefault() ?? new NoteModel(this);
            if (isDeletingSelected && Notes.Count > 0)
            {
                SelectedNote = Notes.FirstOrDefault() ?? new NoteModel(this);
                SelectedNote.Select();
            }
        }
        else if (itemToDelete is Dealer dealerToDelete)
        {
            bool isDeletingSelected = SelectedNote.SelectedDealer == dealerToDelete;
            SelectedNote.Dealers.Remove(dealerToDelete);

            if (isDeletingSelected)
            {
                SelectedNote.SelectDealer(SelectedNote.Dealers.FirstOrDefault() ?? new Dealer(this));
                SelectedNote.SelectedDealer?.Select();
            }
        }
        else if (itemToDelete is Company companyToDelete && SelectedNote.SelectedDealer is not null)
        {
            bool isDeletingSelected = SelectedNote.SelectedDealer.SelectedCompany == companyToDelete;
            SelectedNote.SelectedDealer.Companies.Remove(companyToDelete);

            if (isDeletingSelected)
            {
                SelectedNote.SelectedDealer.SelectCompany(SelectedNote.SelectedDealer.Companies.FirstOrDefault() ?? new Company());
                SelectedNote.SelectedDealer.SelectedCompany?.Select();
            }
        }
        else if (itemToDelete is Contact contactToDelete)
        {
            bool isDeletingSelected = SelectedNote.SelectedContact == contactToDelete;
            SelectedNote.Contacts.Remove(contactToDelete);

            if (isDeletingSelected)
            {
                SelectedNote.SelectContact(SelectedNote.Contacts.FirstOrDefault() ?? new Contact());
                SelectedNote.SelectedContact?.Select();
            }
        }
        else if (itemToDelete is Form formToDelete)
        {
            bool isDeletingSelected = SelectedNote.SelectedForm == formToDelete;
            SelectedNote.Forms.Remove(formToDelete);

            if (isDeletingSelected)
            {
                SelectedNote.SelectForm(SelectedNote.Forms.FirstOrDefault() ?? new Form(this));
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
    public void OnModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if(sender is null || e is null || e.PropertyName is null) return;
        string propertyName = e.PropertyName;

        if (SelectedNote is null) return;

        if (propertyName == nameof(NoteModel.CaseNumber) ||
            propertyName == nameof(Dealer.ServerCode) ||
            propertyName == nameof(Company.CompanyCode) ||
            propertyName == nameof(Contact.Name) ||
            propertyName == nameof(Form.Name) ||
            propertyName == nameof(TestDeal.DealNumber) ||
            propertyName == nameof(TestDeal.Purpose) ||
            propertyName == nameof(NoteModel.Notes) ||
            propertyName == nameof(Dealer.Name) ||
            propertyName == nameof(Contact.Email) ||
            propertyName == nameof(Contact.Phone) ||
            propertyName == nameof(Contact.PhoneExtension) ||
            propertyName == nameof(Form.Notes))
        {
            UiRefreshCounter++;
            UpdateNotebook();
        }

        if (propertyName == nameof(IBlankMaybe.IsBlank))
        {
            if (sender is NoteModel)
            {
                EnsureBlankItem(Notes, () => new NoteModel(this));
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

        if (propertyName == nameof(NoteModel.SelectedDealer))
        {
            var note = sender as NoteModel;
        }

        if (propertyName == nameof(NoteModel.SelectedDealer))
        {
            if (SelectedNote?.SelectedDealer != null)
            {
                EnsureBlankItem(SelectedNote.SelectedDealer.Companies, () => new Company());
            }
        }
        else if (propertyName == nameof(NoteModel.SelectedForm))
        {
            if (SelectedNote?.SelectedForm != null)
            {
                EnsureBlankItem(SelectedNote.SelectedForm.TestDeals, () => new TestDeal());
            }
        }
    }

    private void UpdateNotebook()
    {
        SupportTool.SupportToolInstance.Notebook.Notes.Clear();
        foreach (var note in Notes)
        {
            SupportTool.SupportToolInstance.Notebook.AddNote((Note)note, false);
        }
        SupportTool.SupportToolInstance.Notebook.SelectNote((Note)SelectedNote);
    }

    private void EnsureBlankItem<T>(ObservableCollection<T> collection, Func<T> factory)
    where T : class, IBlankMaybe, INotifyPropertyChanged
    {
        if (collection == null) return;

        var blankItems = collection.Where(item => item.IsBlank).ToList();

        if (blankItems.Count > 1)
        {
            foreach (var extraBlank in blankItems.Skip(1))
            {
                collection.Remove(extraBlank);
            }
        }

        var theOneBlank = collection.FirstOrDefault(item => item.IsBlank);

        if (theOneBlank == null)
        {
            var newItem = factory();
            newItem.PropertyChanged += OnModelPropertyChanged;
            collection.Add(newItem);
        }
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