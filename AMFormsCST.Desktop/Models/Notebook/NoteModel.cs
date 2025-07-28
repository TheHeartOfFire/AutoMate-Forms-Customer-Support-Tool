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
    public string DebugId => Id.ToString();

    [ObservableProperty]
    private int _uiRefreshCounter;

    private string? _caseNumber = string.Empty; 
    public string? CaseNumber 
    {
        get => _caseNumber;
        set
        {
            if (SetProperty(ref _caseNumber, value))
            {
                OnPropertyChanged(nameof(CaseNumber));
                OnPropertyChanged(nameof(IsBlank));
            }
        }
    }
    private string? _notes; 
    public string? Notes
    {
        get => _notes;
        set
        {
            if (SetProperty(ref _notes, value))
            {
                OnPropertyChanged(nameof(Notes));
                OnPropertyChanged(nameof(IsBlank)); 
            }
        }
    }
    public ObservableCollection<Dealer> Dealers { get; set; }
    
    public NoteModel()
    {

        Dealers = [];
        Dealers.CollectionChanged += ChildCollection_CollectionChanged;
        Dealers.Add(new Dealer()); 

        Contacts = [];
        Contacts.CollectionChanged += ChildCollection_CollectionChanged;
        Contacts.Add(new Contact()); 

        Forms = [];
        Forms.CollectionChanged += ChildCollection_CollectionChanged;
        Forms.Add(new Form()); 

        
        SubscribeToInitialChildren();

        
        SelectDealer(Dealers.FirstOrDefault() ?? new Dealer());
        SelectContact(Contacts.FirstOrDefault() ?? new Contact());
        SelectForm(Forms.FirstOrDefault() ?? new Form());
    }

    private Dealer? _selectedDealer; 
    public Dealer? SelectedDealer
    {
        get => _selectedDealer;
        set
        {
            SetProperty(ref _selectedDealer, value);
        }
    }
    public ObservableCollection<Contact> Contacts { get; set; } = [ new() ];
    private Contact? _selectedContact; 
    public Contact? SelectedContact
    {
        get => _selectedContact;
        set
        {
            SetProperty(ref _selectedContact, value);
        }
    }
    public ObservableCollection<Form> Forms { get; set; } = [ ];

    private Form? _selectedForm;
    public Form SelectedForm
    {
        get => _selectedForm ?? Forms[0];
        set
        {
            SetProperty(ref _selectedForm, value);
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

        SelectedForm = Forms.FirstOrDefault(c => c.Id == selectedForm.Id) ?? new Form();
        SelectedForm?.Select();
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
        foreach (var dealer in Dealers) { dealer.PropertyChanged += ChildItem_IsBlankChanged; }
        foreach (var contact in Contacts) { contact.PropertyChanged += ChildItem_IsBlankChanged; }
        foreach (var form in Forms) { form.PropertyChanged += ChildItem_IsBlankChanged; }
    }

    public static implicit operator Core.Types.Notebook.Note(NoteModel note)
    {
        if (note is null)
        {
            return new Core.Types.Notebook.Note();
        }
        string companiesText = note.SelectedDealer?.Companies?.Select(d => d.CompanyCode) != null
                           ? string.Join(", ", note.SelectedDealer.Companies.Select(d => d.CompanyCode))
                           : string.Empty;
        return new Core.Types.Notebook.Note(note.Id)
        {
            ServerId = note.SelectedDealer?.ServerCode,
            Companies = companiesText,
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
