using AMFormsCST.Core.Interfaces;
using AMFormsCST.Core.Interfaces.Notebook;
using AMFormsCST.Core.Types.Notebook;
using AMFormsCST.Desktop.Interfaces;
using AMFormsCST.Desktop.Types;
using AMFormsCST.Desktop.ViewModels.Pages;
using CommunityToolkit.Mvvm.ComponentModel;
using Serilog.Context;
using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace AMFormsCST.Desktop.Models;
public partial class NoteModel : ObservableObject, ISelectable, IBlankMaybe
{
    private readonly ILogService? _logger;
    public string DebugId => Id.ToString();

    [ObservableProperty]
    private int _uiRefreshCounter;

    private string? _caseNumber = string.Empty; 
    public string? CaseNumber 
    {
        get => _caseNumber;
        set
        {
            var oldValue = _caseNumber;
            if (SetProperty(ref _caseNumber, value))
            {
                OnPropertyChanged(nameof(IsBlank));
                UpdateCore();

                using (LogContext.PushProperty("NoteId", Id))
                using (LogContext.PushProperty("Notes", Notes))
                using (LogContext.PushProperty("Dealers", Dealers.Count))
                using (LogContext.PushProperty("Contacts", Contacts.Count))
                using (LogContext.PushProperty("Forms", Forms.Count))
                using (LogContext.PushProperty("Old Value", oldValue))
                using (LogContext.PushProperty("New Value", value))
                {
                    _logger?.LogInfo($"CaseNumber changed: {value}");
                }
            }
        }
    }
    private string? _notes; 
    public string? Notes
    {
        get => _notes;
        set
        {
            var oldValue = _notes;
            if (SetProperty(ref _notes, value))
            {
                OnPropertyChanged(nameof(IsBlank));
                UpdateCore();

                using (LogContext.PushProperty("NoteId", Id))
                using (LogContext.PushProperty("Case#", CaseNumber))
                using (LogContext.PushProperty("Dealers", Dealers.Count))
                using (LogContext.PushProperty("Contacts", Contacts.Count))
                using (LogContext.PushProperty("Forms", Forms.Count))
                using (LogContext.PushProperty("Old Value", oldValue))
                using (LogContext.PushProperty("New Value", value))
                {
                    _logger?.LogInfo($"Notes changed: {value}");
                }
            }
        }
    }

    public ManagedObservableCollection<Dealer> Dealers { get; set; }
    public ManagedObservableCollection<Contact> Contacts { get; set; }
    public ManagedObservableCollection<Form> Forms { get; set; }

    internal INote? CoreType { get; set; }

    public NoteModel(string phoneExtensionDelimiter, ILogService? logger = null)
    {
        _logger = logger;
        Dealers = new(() => new Dealer(_logger), _logger);
        Contacts = new(() => new Contact(phoneExtensionDelimiter, _logger), _logger);
        Forms = new(() => new Form(_logger), _logger);

        Dealers.CollectionChanged += Dealers_CollectionChanged;
        Contacts.CollectionChanged += Contacts_CollectionChanged;
        Forms.CollectionChanged += Forms_CollectionChanged;

        foreach (var dealer in Dealers) dealer.Parent = this;
        foreach (var contact in Contacts) contact.Parent = this;
        foreach (var form in Forms) form.Parent = this;

        SelectDealer(Dealers.FirstOrDefault(x => !x.IsBlank));
        SelectContact(Contacts.FirstOrDefault(x => !x.IsBlank));
        SelectForm(Forms.FirstOrDefault(x => !x.IsBlank));

        using (LogContext.PushProperty("NoteId", Id))
        using (LogContext.PushProperty("Case#", CaseNumber))
        using (LogContext.PushProperty("Notes", Notes))
        using (LogContext.PushProperty("Dealers", Dealers.Count))
        using (LogContext.PushProperty("Contacts", Contacts.Count))
        using (LogContext.PushProperty("Forms", Forms.Count))
        {
            _logger?.LogInfo("NoteModel initialized.");
        }
    }

    public NoteModel(INote note, string phoneExtensionDelimiter, ILogService? logger = null)
    {
        _logger = logger;
        CoreType = note;
        Dealers = new(() => new Dealer(_logger), _logger);
        foreach (var d in note.Dealers)
        {
            var dealer = new Dealer(d, _logger) { Parent = this };
            Dealers.Add(dealer);
        }

        Contacts = new(() => new Contact(phoneExtensionDelimiter, _logger), _logger);
        foreach (var c in note.Contacts)
        {
            var contact = new Contact(c, _logger) { Parent = this };
            Contacts.Add(contact);
        }

        Forms = new(() => new Form(_logger), _logger);
        foreach (var f in note.Forms)
        {
            var form = new Form(f, _logger) { Parent = this };
            Forms.Add(form);
        }

        Dealers.CollectionChanged += Dealers_CollectionChanged;
        Contacts.CollectionChanged += Contacts_CollectionChanged;
        Forms.CollectionChanged += Forms_CollectionChanged;

        SelectDealer(Dealers.FirstOrDefault(x => !x.IsBlank));
        SelectContact(Contacts.FirstOrDefault(x => !x.IsBlank));
        SelectForm(Forms.FirstOrDefault(x => !x.IsBlank));

        CaseNumber = note.CaseText;
        Notes = note.NotesText;
        _logger?.LogInfo($"NoteModel loaded from core type. ID: {Id}\tCore ID: {CoreType.Id}");
    }

    private void Dealers_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.NewItems != null)
            foreach (Dealer d in e.NewItems) d.Parent = this;
        UpdateCore();
        _logger?.LogDebug("Dealers collection changed.");
    }
    private void Contacts_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.NewItems != null)
            foreach (Contact c in e.NewItems) c.Parent = this;
        UpdateCore();
        _logger?.LogDebug("Contacts collection changed.");
    }
    private void Forms_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.NewItems != null)
            foreach (Form f in e.NewItems) f.Parent = this;
        UpdateCore();
        _logger?.LogDebug("Forms collection changed.");
    }

    private Dealer? _selectedDealer; 
    public Dealer? SelectedDealer
    {
        get => _selectedDealer;
        set => SetProperty(ref _selectedDealer, value);
    }
    private Contact? _selectedContact; 
    public Contact? SelectedContact
    {
        get => _selectedContact;
        set => SetProperty(ref _selectedContact, value);
    }
    private Form? _selectedForm;
    public Form SelectedForm
    {
        get => _selectedForm ?? Forms.FirstOrDefault();
        set => SetProperty(ref _selectedForm, value);
    }
    public Guid Id { get; } = Guid.NewGuid();
    public bool IsBlank
    {
        get
        {
            if (!string.IsNullOrEmpty(CaseNumber) || !string.IsNullOrEmpty(Notes))
                return false;
            if (Dealers.Any(d => !d.IsBlank) ||
                Contacts.Any(c => !c.IsBlank) ||
                Forms.Any(f => !f.IsBlank))
                return false;
            return true;
        }
    }

    [ObservableProperty]
    private bool _isSelected;
    public void Select()
    {
        IsSelected = true;
        _logger?.LogInfo("NoteModel selected.");
    }
    public void Deselect()
    {
        IsSelected = false;
        _logger?.LogInfo("NoteModel deselected.");
    }

    public void SelectDealer(ISelectable selectedDealer)
    {
        if (selectedDealer is null) return;
        foreach (var dealer in Dealers) dealer.Deselect();
        SelectedDealer = Dealers.FirstOrDefault(c => c.Id == selectedDealer.Id);
        SelectedDealer?.Select();
        _logger?.LogInfo($"Dealer selected: {SelectedDealer?.Id}");
    }

    public void SelectContact(ISelectable selectedContact)
    {
        if (selectedContact is null) return;
        foreach (var contact in Contacts) contact.Deselect();
        SelectedContact = Contacts.FirstOrDefault(c => c.Id == selectedContact.Id);
        SelectedContact?.Select();
        _logger?.LogInfo($"Contact selected: {SelectedContact?.Id}");
    }

    public void SelectForm(ISelectable selectedForm)
    {
        if (selectedForm is null) return;
        foreach (var form in Forms) form.Deselect();
        SelectedForm = Forms.FirstOrDefault(c => c.Id == selectedForm.Id) ?? Forms.FirstOrDefault();
        SelectedForm?.Select();
        _logger?.LogInfo($"Form selected: {SelectedForm?.Id}");
    }

    internal void UpdateCore()
    {
        if (CoreType == null) return;
        CoreType.CaseText = CaseNumber ?? string.Empty;
        CoreType.NotesText = Notes ?? string.Empty;
        CoreType.Dealers.Clear();
        CoreType.Dealers.AddRange(Dealers.Select(d => (Core.Types.Notebook.Dealer)d));
        CoreType.Contacts.Clear();
        CoreType.Contacts.AddRange(Contacts.Select(c => (Core.Types.Notebook.Contact)c));
        CoreType.Forms.Clear();
        CoreType.Forms.AddRange(Forms.Select(f => (Core.Types.Notebook.Form)f));
        _logger?.LogDebug($"NoteModel core updated. ID: {Id}\tCore ID: {CoreType.Id}");
    }

    public static implicit operator Note(NoteModel note)
    {
        if (note is null) return new Note();
        return new Note(note.Id)
        {
            CaseText = note.CaseNumber ?? string.Empty,
            NotesText = note.Notes ?? string.Empty,
            Dealers = [..note.Dealers.Select(d => (Core.Types.Notebook.Dealer)d)],
            Contacts = [..note.Contacts.Select(c => (Core.Types.Notebook.Contact)c)],
            Forms = [..note.Forms.Select(f => (Core.Types.Notebook.Form)f)]
        };
    }

    internal DashboardViewModel? Parent { get; set; }
}
