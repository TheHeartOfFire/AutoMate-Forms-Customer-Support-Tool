using AMFormsCST.Desktop.Interfaces;
using AMFormsCST.Desktop.ViewModels;
using AMFormsCST.Desktop.ViewModels.Pages;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFormsCST.Desktop.Models;
public partial class NoteModel : ObservableObject, ISelectable, IBlankMaybe
{
    [ObservableProperty]
    private string? _caseNumber = string.Empty;
    // specifically for triggering UI updates
    [ObservableProperty]
    private int _uiRefreshCounter; 
    [ObservableProperty]
    private string? _notes;
    public ObservableCollection<Dealer> Dealers { get; set; }
    
    private readonly DashboardViewModel _viewModel;
    public NoteModel(DashboardViewModel viewModel)
    {
        _viewModel = viewModel;

        Dealers = new ObservableCollection<Dealer>();
        Dealers.CollectionChanged += ChildCollection_CollectionChanged;
        Dealers.Add(new Dealer(viewModel)); 

        Contacts = new ObservableCollection<Contact>();
        Contacts.CollectionChanged += ChildCollection_CollectionChanged;
        Contacts.Add(new Contact()); 

        Forms = new ObservableCollection<Form>();
        Forms.CollectionChanged += ChildCollection_CollectionChanged;
        Forms.Add(new Form(viewModel)); 

        
        SubscribeToInitialChildren();

        
        SelectDealer(Dealers.FirstOrDefault());
        SelectContact(Contacts.FirstOrDefault());
        SelectForm(Forms.FirstOrDefault());
    }

    private Dealer? _selectedDealer; 
    public Dealer? SelectedDealer
    {
        get => _selectedDealer;
        set
        {
            // 1. Unsubscribe from the old dealer's events
            if (_selectedDealer != null)
            {
                _selectedDealer.PropertyChanged -= _viewModel.OnModelPropertyChanged;
            }

            SetProperty(ref _selectedDealer, value);

            // 2. Subscribe to the new dealer's events
            if (_selectedDealer != null)
            {
                _selectedDealer.PropertyChanged += _viewModel.OnModelPropertyChanged;
            }
        }
    }
    public ObservableCollection<Contact> Contacts { get; set; } = [ new() ];
    private Contact? _selectedContact; 
    public Contact? SelectedContact
    {
        get => _selectedContact;
        set
        {
            // Unsubscribe from the old contact
            if (_selectedContact != null)
            {
                _selectedContact.PropertyChanged -= _viewModel.OnModelPropertyChanged;
            }

            SetProperty(ref _selectedContact, value);

            // Subscribe to the new contact
            if (_selectedContact != null)
            {
                _selectedContact.PropertyChanged += _viewModel.OnModelPropertyChanged;
            }
        }
    }
    public ObservableCollection<Form> Forms { get; set; } = [ ];

    private Form? _selectedForm;
    public Form SelectedForm
    {
        get => _selectedForm;
        set
        {
            // Unsubscribe from the old form
            if (_selectedForm != null)
            {
                _selectedForm.PropertyChanged -= _viewModel.OnModelPropertyChanged;
            }

            SetProperty(ref _selectedForm, value);

            // Subscribe to the new form
            if (_selectedForm != null)
            {
                _selectedForm.PropertyChanged += _viewModel.OnModelPropertyChanged;
            }
        }
    }
    public Guid Id { get; } = Guid.NewGuid();
    public bool IsBlank
    {
        get
        {
            if (!string.IsNullOrEmpty(CaseNumber) || !string.IsNullOrEmpty(Notes))
            {
                return false;
            }

            if (Dealers.Any(d => !d.IsBlank) ||
                Contacts.Any(c => !c.IsBlank) ||
                Forms.Any(f => !f.IsBlank))
            {
                return false;
            }

            return true;
        }
    }

    [ObservableProperty]
    private bool _isSelected;
    public void Select()
    {
        IsSelected = true;
    }
    public void Deselect()
    {
        IsSelected = false;
    }

    public void SelectDealer(ISelectable selectedDealer)
    {
        if (selectedDealer is null) return;

        foreach (var dealer in Dealers) dealer.Deselect();

        SelectedDealer = Dealers.FirstOrDefault(c => c.Id == selectedDealer.Id);
        SelectedDealer?.Select();

    }

    public void SelectContact(ISelectable selectedContact)
    {
        if (selectedContact is null) return;

        foreach (var contact in Contacts) contact.Deselect();

        SelectedContact = Contacts.FirstOrDefault(c => c.Id == selectedContact.Id);
        SelectedContact?.Select();
    }

    public void SelectForm(ISelectable selectedForm)
    {
        if (selectedForm is null) return;

        foreach (var form in Forms) form.Deselect();

        SelectedForm = Forms.FirstOrDefault(c => c.Id == selectedForm.Id);
        SelectedForm?.Select();
    }
    partial void OnCaseNumberChanged(string? value)
    {
        OnPropertyChanged(nameof(IsBlank));
    }
    partial void OnNotesChanged(string? value)
    {
        OnPropertyChanged(nameof(IsBlank)); 
    }
    private void ChildCollection_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {

        if (e.OldItems != null)
        {
            foreach (var item in e.OldItems)
            {
                if (item is System.ComponentModel.INotifyPropertyChanged notifyItem)
                {
                    notifyItem.PropertyChanged -= ChildItem_IsBlankChanged;
                }
            }
        }

        if (e.NewItems != null)
        {
            foreach (var item in e.NewItems)
            {
                if (item is System.ComponentModel.INotifyPropertyChanged notifyItem)
                {
                    notifyItem.PropertyChanged += ChildItem_IsBlankChanged;
                }
            }
        }

        OnPropertyChanged(nameof(IsBlank));
    }
    private void ChildItem_IsBlankChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(IBlankMaybe.IsBlank))
        {
            OnPropertyChanged(nameof(IsBlank)); 
        }
    }
    private void SubscribeToInitialChildren()
    {
        foreach (var dealer in Dealers) { ((ObservableObject)dealer).PropertyChanged += ChildItem_IsBlankChanged; }
        foreach (var contact in Contacts) { ((ObservableObject)contact).PropertyChanged += ChildItem_IsBlankChanged; }
        foreach (var form in Forms) { ((ObservableObject)form).PropertyChanged += ChildItem_IsBlankChanged; }
    }

    public static implicit operator Core.Types.Notebook.Note(NoteModel note)
    {
        if (note is null)
        {
            return null;
        }

        return new Core.Types.Notebook.Note(note.Id)
        {
            ServerId = note.SelectedDealer?.ServerCode,
            Companies = string.Join(", ", note.SelectedDealer.Companies.Select(d => d.CompanyCode)),
            Dealership = note.SelectedDealer?.Name,
            ContactName = note.SelectedContact?.Name,
            Email = note.SelectedContact?.Email,
            Phone = note.SelectedContact?.Phone,
            PhoneExt = note.SelectedContact?.PhoneExtension,
            NotesText = note.Notes,
            CaseText = note.CaseNumber,
            FormsText = note.SelectedForm?.Name,
            DealText = note.SelectedForm?.SelectedTestDeal?.DealNumber
        };
    }
}
