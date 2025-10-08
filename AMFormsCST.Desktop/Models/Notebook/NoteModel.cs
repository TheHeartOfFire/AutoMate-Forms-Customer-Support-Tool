﻿using AMFormsCST.Core.Interfaces;
using AMFormsCST.Core.Interfaces.Notebook;
using AMFormsCST.Core.Types.Notebook;
using AMFormsCST.Desktop.BaseClasses;
using AMFormsCST.Desktop.Types;
using AMFormsCST.Desktop.ViewModels.Pages;
using CommunityToolkit.Mvvm.ComponentModel;
using Serilog.Context;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Documents;

namespace AMFormsCST.Desktop.Models;
public partial class NoteModel : ManagedObservableCollectionItem
{
    private bool _isInit;

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
    private FlowDocument? _notes; 
    public FlowDocument? Notes
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
    public Dealer? SelectedDealer => Dealers.SelectedItem;
    public ManagedObservableCollection<Contact> Contacts { get; set; }
    public Contact? SelectedContact => Contacts.SelectedItem;
    public ManagedObservableCollection<Form> Forms { get; set; }
    public Form? SelectedForm => Forms.SelectedItem;
    public override Guid Id { get; } = Guid.NewGuid();
    public override bool IsBlank
    {
        get
        {
            if (!string.IsNullOrEmpty(CaseNumber) || Notes != null)
                return false;
            if (Dealers.Any(d => !d.IsBlank) ||
                Contacts.Any(c => !c.IsBlank) ||
                Forms.Any(f => !f.IsBlank))
                return false;
            return true;
        }
    }

    internal INote? CoreType { get; set; }
    internal DashboardViewModel? Parent { get; set; }

    public NoteModel(string phoneExtensionDelimiter, ILogService? logger = null) : base(logger)
    {
        _isInit = true;
        CoreType = new Note(logger);

        InitContacts(phoneExtensionDelimiter);
        InitDealers();
        InitForms();

        _isInit = false;
        UpdateCore();

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

    public NoteModel(INote note, string phoneExtensionDelimiter, ILogService? logger = null) : base(logger)
    {
        _isInit = true;
        CoreType = note;

        InitDealers();
        InitContacts(phoneExtensionDelimiter);
        InitForms();

        CaseNumber = note.CaseText;
        Notes = new FlowDocument();
        Notes.Blocks.Add(new Paragraph(new Run(note.NotesText)));
        _isInit = false;
        UpdateCore();

        { 
            using (LogContext.PushProperty("NoteId", Id))
            using (LogContext.PushProperty("Case#", CaseNumber))
            using (LogContext.PushProperty("Notes", Notes))
            using (LogContext.PushProperty("Dealers", Dealers.Count))
            using (LogContext.PushProperty("Contacts", Contacts.Count))
            using (LogContext.PushProperty("Forms", Forms.Count))
            {
                _logger?.LogInfo($"NoteModel loaded from core type.");
            }
        }
    }
    private void InitDealers()
    {
        var dealers = CoreType?.Dealers.ToList()
                .Select(coreDealer =>
                {
                    var dealer = new Dealer(coreDealer, _logger) { Parent = this };
                    dealer.PropertyChanged += OnDealerPropertyChanged;
                    return dealer;
                });

        Dealers = new ManagedObservableCollection<Dealer>(
            () => new Dealer(_logger) { Parent = this },
            dealers,
            _logger,
            (d) => d.PropertyChanged += OnDealerPropertyChanged
        );
        Dealers.CollectionChanged += Dealers_CollectionChanged;
        Dealers.PropertyChanged += OnDealerPropertyChanged;
        Dealers.FirstOrDefault()?.Select();
    }
    private void InitContacts(string phoneExtensionDelimiter)
    {
        var contacts = CoreType?.Contacts.ToList()
                .Select(coreContact =>
                {
                    var contact = new Contact(coreContact, _logger) { Parent = this };
                    contact.PropertyChanged += OnContactPropertyChanged;
                    return contact;
                });

        Contacts = new ManagedObservableCollection<Contact>(
            () => new Contact(phoneExtensionDelimiter, _logger) { Parent = this },
            contacts,
            _logger,
            (c) => c.PropertyChanged += OnContactPropertyChanged
        );
        Contacts.CollectionChanged += Contacts_CollectionChanged;
        Contacts.PropertyChanged += OnContactPropertyChanged;
        Contacts.FirstOrDefault()?.Select();
    }
    private void InitForms()
    {
        var forms = CoreType?.Forms.ToList()
                .Select(coreForm =>
                {
                    var form = new Form(this, coreForm, _logger);
                    form.PropertyChanged += OnFormPropertyChanged;
                    return form;
                });

        Forms = new ManagedObservableCollection<Form>(
            () => new Form(this, _logger),
            forms,
            _logger,
            (f) => f.PropertyChanged += OnFormPropertyChanged
        );
        Forms.CollectionChanged += Forms_CollectionChanged;
        Forms.PropertyChanged += OnFormPropertyChanged;
        Forms.FirstOrDefault()?.Select();
    }

    private void OnDealerPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ManagedObservableCollection<Dealer>.SelectedItem))
        {
            OnPropertyChanged(nameof(SelectedDealer));
        }
        else
        {
            OnPropertyChanged(nameof(IsBlank));
        }
    }

    private void OnContactPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ManagedObservableCollection<Contact>.SelectedItem))
        {
            OnPropertyChanged(nameof(SelectedContact));
        }
        else
        {
            OnPropertyChanged(nameof(IsBlank));
        }
    }

    private void OnFormPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ManagedObservableCollection<Form>.SelectedItem))
        {
            OnPropertyChanged(nameof(SelectedForm));
        }
        else
        {
            OnPropertyChanged(nameof(IsBlank));
        }
    }

    private void Dealers_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.NewItems != null)
            foreach (Dealer d in e.NewItems)
            {
                d.Parent = this;
                d.PropertyChanged -= OnDealerPropertyChanged;
                d.PropertyChanged += OnDealerPropertyChanged;
            }
        if (e.OldItems != null)
            foreach (Dealer d in e.OldItems)
                d.PropertyChanged -= OnDealerPropertyChanged;
        UpdateCore();
        Parent?.NotifyDealerNavigationChanged();
        _logger?.LogDebug("Dealers collection changed.");
    }
    private void Contacts_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.NewItems != null)
            foreach (Contact c in e.NewItems)
            {
                c.Parent = this;
                c.PropertyChanged -= OnContactPropertyChanged;
                c.PropertyChanged += OnContactPropertyChanged;
            }
        if (e.OldItems != null)
            foreach (Contact c in e.OldItems)
                c.PropertyChanged -= OnContactPropertyChanged;
        UpdateCore();
        Parent?.NotifyContactNavigationChanged();
        _logger?.LogDebug("Contacts collection changed.");
    }
    private void Forms_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.NewItems != null)
            foreach (Form f in e.NewItems)
            {
                f.Parent = this;
                f.PropertyChanged -= OnFormPropertyChanged;
                f.PropertyChanged += OnFormPropertyChanged;
            }

        if (e.OldItems != null)
            foreach (Form f in e.OldItems)
                f.PropertyChanged -= OnFormPropertyChanged;

        UpdateCore();
        Parent?.NotifyFormNavigationChanged();
        _logger?.LogDebug("Forms collection changed.");
    }

    internal void UpdateCore()
    {
        if (CoreType == null || _isInit) return;
        CoreType.CaseText = CaseNumber ?? string.Empty;
        CoreType.NotesText = GetFlowDocumentPlainText(Notes ?? new()) ?? string.Empty;
        CoreType.Dealers.Clear();
        CoreType.Dealers.AddRange(Dealers.Select(d => (Core.Types.Notebook.Dealer)d));
        CoreType.Dealers.SelectedItem = Dealers?.SelectedItem?.CoreType;
        CoreType.Contacts.Clear();
        CoreType.Contacts.AddRange(Contacts.Select(c => (Core.Types.Notebook.Contact)c));
        CoreType.Contacts.SelectedItem = Contacts?.SelectedItem?.CoreType;
        CoreType.Forms.Clear();
        CoreType.Forms.AddRange(Forms.Select(f => (Core.Types.Notebook.Form)f));
        CoreType.Forms.SelectedItem = Forms?.SelectedItem?.CoreType;
        _logger?.LogDebug($"NoteModel core updated. ID: {Id}\tCore ID: {CoreType.Id}");
    }
    public static string GetFlowDocumentPlainText(FlowDocument document)
    {
        // Create a TextRange from the beginning (ContentStart) to the end (ContentEnd) of the document.
        TextRange textRange = new TextRange(
            document.ContentStart,
            document.ContentEnd
        );

        // The Text property of the TextRange object returns the plain text content as a string.
        return textRange.Text;
    }

    public static implicit operator Note(NoteModel note)
    {
        if (note is null || note.CoreType is null) return new Note();
        return new Note(note.CoreType.Id)
        {
            CaseText = note.CaseNumber ?? string.Empty,
            NotesText = GetFlowDocumentPlainText(note.Notes ?? new()) ?? string.Empty,
            Dealers = [..note.Dealers.Select(d => (Core.Types.Notebook.Dealer)d)],
            Contacts = [..note.Contacts.Select(c => (Core.Types.Notebook.Contact)c)],
            Forms = [..note.Forms.Select(f => (Core.Types.Notebook.Form)f)]
        };
    }

    internal void RaiseChildPropertyChanged()
    {
        OnPropertyChanged(string.Empty); 
    }

}
