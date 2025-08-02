using AMFormsCST.Core.Types.Notebook;
using AMFormsCST.Core.Utils;
using AMFormsCST.Desktop.Interfaces;
using AMFormsCST.Desktop.Models;
using AMFormsCST.Desktop.ViewModels.Dialogs;
using AMFormsCST.Desktop.ViewModels.Pages.Tools;
using AMFormsCST.Desktop.Views.Dialogs;
using AMFormsCST.Desktop.Views.Pages.Tools;
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
    public string DebugSelectedNoteId => SelectedNote?.Id.ToString() ?? "None";

    private NoteModel _selectedNote;
    public NoteModel SelectedNote
    {
        get => _selectedNote;
        set
        {
            if (_selectedNote != null)
            {
                _selectedNote.PropertyChanged -= OnModelPropertyChanged;
                _selectedNote.Dealers.CollectionChanged -= OnChildCollectionChanged; 
                _selectedNote.Contacts.CollectionChanged -= OnChildCollectionChanged;
                _selectedNote.Forms.CollectionChanged -= OnChildCollectionChanged;

                UnsubscribeNoteChildren(_selectedNote);
            }

            SetProperty(ref _selectedNote, value);

            if (_selectedNote != null)
            {
                _selectedNote.PropertyChanged += OnModelPropertyChanged;
                _selectedNote.Dealers.CollectionChanged += OnChildCollectionChanged; 
                _selectedNote.Contacts.CollectionChanged += OnChildCollectionChanged;
                _selectedNote.Forms.CollectionChanged += OnChildCollectionChanged;

                SubscribeNoteChildren(_selectedNote);
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
        Notes.Add(new NoteModel());

        _selectedNote = Notes[0];
        Notes[0].Select();

        if (_selectedNote != null)
        {
            _selectedNote.PropertyChanged += OnModelPropertyChanged;
            _selectedNote.Dealers.CollectionChanged += OnChildCollectionChanged;
            _selectedNote.Contacts.CollectionChanged += OnChildCollectionChanged;
            _selectedNote.Forms.CollectionChanged += OnChildCollectionChanged;
            SubscribeNoteChildren(_selectedNote);

        }
        #region DEBUG
#if DEBUG
        IsDebugMode = true;
#endif
        #endregion

        if (IsDebugMode) _debugVisibility = Visibility.Visible;

        if(_selectedNote is null) 
        {
            _selectedNote = new NoteModel();
            Notes.Add(_selectedNote);
            _selectedNote.Select();
        }
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

            SelectedNote = Notes.FirstOrDefault() ?? new NoteModel();
            if (isDeletingSelected && Notes.Count > 0)
            {
                SelectedNote = Notes.FirstOrDefault() ?? new NoteModel();
                SelectedNote.Select();
            }
        }
        else if (itemToDelete is Dealer dealerToDelete)
        {
            bool isDeletingSelected = SelectedNote.SelectedDealer == dealerToDelete;
            SelectedNote.Dealers.Remove(dealerToDelete);

            if (isDeletingSelected)
            {
                SelectedNote.SelectDealer(SelectedNote.Dealers.FirstOrDefault() ?? new Dealer());
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
                SelectedNote.SelectForm(SelectedNote.Forms.FirstOrDefault() ?? new Form());
                SelectedNote.SelectedForm?.Select();
            }
        }
    }
    [RelayCommand]
    private static void OpenTemplateDialog()
    {
        var vm = new TemplatesViewModel();
        var page = new TemplatesPage(vm);


        var dialog = new PageHostDialog(page);


        dialog.Show();
    }

    [RelayCommand]
    private static void OpenCodeSnippetDialog()
    {
        var vm = new CodeSnippetsViewModel();
        var page = new CodeSnippetsPage(vm);


        var dialog = new PageHostDialog(page);


        dialog.Show();
    }



    [RelayCommand]
    private void OpenFormNameGeneratorDialog()
    {
        var vm = new FormNameGeneratorViewModel();
        var page = new FormNameGeneratorPage(vm); 

        
        var dialog = new PageHostDialog(page, true);

        
        dialog.Show();
        dialog.Closed += FormNameDialogClosed;

        
    }

    private void FormNameDialogClosed(object? sender, EventArgs e)
    {
        if (sender is null) return;

        var dialog = (PageHostDialog)sender;
        if (dialog.DataContext is not PageHostDialogViewModel outerVm) return;

        if (outerVm.HostedPageViewModel is not FormNameGeneratorViewModel vm) return;

        var name = vm.Form.FileName ?? string.Empty;

        if (dialog.ConfirmSelected)
            if(SelectedNote.SelectedForm.IsBlank)
            {
                SelectedNote.SelectedForm.Name = name;
            }
            else
            {
                SelectedNote.Forms.Last().Name = name;
            }
        dialog.Closed -= FormNameDialogClosed;
    }

    [RelayCommand]
    private void LoadCase()
    {
        //var dialog = new Dialogs.NewTemplateDialog();

        //dialog.ShowDialog();
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

        System.Diagnostics.Debug.WriteLine($"OnModelPropertyChanged from {sender.GetType().Name}.{propertyName}");

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
            //OnPropertyChanged(nameof(SelectedNote.IsBlank));
        }

        if (propertyName == nameof(IBlankMaybe.IsBlank))
        {
            if (sender is NoteModel)
            {
                EnsureBlankItem(Notes, () => new NoteModel());
            }
            else if (sender is Dealer)
            {
                if (SelectedNote?.Dealers != null)
                {
                    EnsureBlankItem(SelectedNote.Dealers, () => new Dealer());
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
                    EnsureBlankItem(SelectedNote.Forms, () => new Form());
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

        if (sender is NoteModel note) // if a property on the NoteModel itself changed
        {
            if (propertyName == nameof(NoteModel.SelectedDealer))
            {
                EnsureBlankItem(note.Dealers, () => new Dealer());
            }
            else if (propertyName == nameof(NoteModel.SelectedForm))
            {
                EnsureBlankItem(note.Forms, () => new Form());
            }
            else if (propertyName == nameof(NoteModel.SelectedContact))
            {
                EnsureBlankItem(note.Contacts, () => new Contact());
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
    private void SubscribeNoteChildren(NoteModel note)
    {
        foreach (var dealer in note.Dealers)
        {
            dealer.PropertyChanged += OnModelPropertyChanged;
            dealer.Companies.CollectionChanged += OnChildCollectionChanged;
            SubscribeDealerChildren(dealer);
        }
        foreach (var contact in note.Contacts)
        {
            contact.PropertyChanged += OnModelPropertyChanged;
        }
        foreach (var form in note.Forms)
        {
            form.PropertyChanged += OnModelPropertyChanged;
            form.TestDeals.CollectionChanged += OnChildCollectionChanged;
            SubscribeFormChildren(form);
        }

        // Also subscribe to the *currently selected* children, as they are key
        if (note.SelectedDealer != null)
            note.SelectedDealer.PropertyChanged += OnModelPropertyChanged;
        if (note.SelectedContact != null)
            note.SelectedContact.PropertyChanged += OnModelPropertyChanged;
        if (note.SelectedForm != null)
            note.SelectedForm.PropertyChanged += OnModelPropertyChanged;
        if (note.SelectedForm?.SelectedTestDeal != null)
            note.SelectedForm.SelectedTestDeal.PropertyChanged += OnModelPropertyChanged;
    }
    private void UnsubscribeNoteChildren(NoteModel note)
    {
        foreach (var dealer in note.Dealers)
        {
            dealer.PropertyChanged -= OnModelPropertyChanged;
            dealer.Companies.CollectionChanged -= OnChildCollectionChanged;
            UnsubscribeDealerChildren(dealer);
        }
        foreach (var contact in note.Contacts)
        {
            contact.PropertyChanged -= OnModelPropertyChanged;
        }
        foreach (var form in note.Forms)
        {
            form.PropertyChanged -= OnModelPropertyChanged;
            form.TestDeals.CollectionChanged -= OnChildCollectionChanged;
            UnsubscribeFormChildren(form);
        }

        // Also unsubscribe from the *currently selected* children
        if (note.SelectedDealer != null)
            note.SelectedDealer.PropertyChanged -= OnModelPropertyChanged;
        if (note.SelectedContact != null)
            note.SelectedContact.PropertyChanged -= OnModelPropertyChanged;
        if (note.SelectedForm != null)
            note.SelectedForm.PropertyChanged -= OnModelPropertyChanged;
        if (note.SelectedForm?.SelectedTestDeal != null)
            note.SelectedForm.SelectedTestDeal.PropertyChanged -= OnModelPropertyChanged;
    }

    private void SubscribeDealerChildren(Dealer dealer)
    {
        foreach (var company in dealer.Companies)
        {
            company.PropertyChanged += OnModelPropertyChanged;
        }
        if (dealer.SelectedCompany != null)
            dealer.SelectedCompany.PropertyChanged += OnModelPropertyChanged;
    }

    private void UnsubscribeDealerChildren(Dealer dealer)
    {
        foreach (var company in dealer.Companies)
        {
            company.PropertyChanged -= OnModelPropertyChanged;
        }
        if (dealer.SelectedCompany != null)
            dealer.SelectedCompany.PropertyChanged -= OnModelPropertyChanged;
    }

    private void SubscribeFormChildren(Form form)
    {
        foreach (var testDeal in form.TestDeals)
        {
            testDeal.PropertyChanged += OnModelPropertyChanged;
        }
        if (form.SelectedTestDeal != null)
            form.SelectedTestDeal.PropertyChanged += OnModelPropertyChanged;
    }

    private void UnsubscribeFormChildren(Form form)
    {
        foreach (var testDeal in form.TestDeals)
        {
            testDeal.PropertyChanged -= OnModelPropertyChanged;
        }
        if (form.SelectedTestDeal != null)
            form.SelectedTestDeal.PropertyChanged -= OnModelPropertyChanged;
    }


    // Handle CollectionChanged events to subscribe/unsubscribe to individual items
    private void OnChildCollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        // Unsubscribe old items
        if (e.OldItems != null)
        {
            foreach (INotifyPropertyChanged item in e.OldItems)
            {
                item.PropertyChanged -= OnModelPropertyChanged;
                // Recursive unsubscribe for nested collections/selected items
                if (item is Dealer dealer) UnsubscribeDealerChildren(dealer);
                if (item is Form form) UnsubscribeFormChildren(form);
            }
        }

        // Subscribe new items
        if (e.NewItems != null)
        {
            foreach (INotifyPropertyChanged item in e.NewItems)
            {
                item.PropertyChanged += OnModelPropertyChanged;
                // Recursive subscribe for nested collections/selected items
                if (item is Dealer dealer) SubscribeDealerChildren(dealer);
                if (item is Form form) SubscribeFormChildren(form);
            }
        }
        // Always trigger IsBlank for the NoteModel itself if a child collection changes
        OnPropertyChanged(nameof(SelectedNote.IsBlank));
    }
}
