using AMFormsCST.Core.Interfaces;
using AMFormsCST.Core.Interfaces.Notebook;
using AMFormsCST.Core.Types.Notebook;
using AMFormsCST.Desktop.Interfaces;
using AMFormsCST.Desktop.Types;
using CommunityToolkit.Mvvm.ComponentModel;
using Serilog.Context;
using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace AMFormsCST.Desktop.Models;
public partial class Form : ObservableObject, ISelectable, IBlankMaybe
{
    private readonly ILogService? _logger;

    [ObservableProperty]
    private string? _name = string.Empty;
    [ObservableProperty]
    private string? _notes = string.Empty;

    public ManagedObservableCollection<TestDeal> TestDeals { get; set; }
    internal IForm? CoreType { get; set; }
    internal NoteModel? Parent { get; set; }

    private TestDeal? _selectedTestDeal;
    public TestDeal? SelectedTestDeal
    {
        get => _selectedTestDeal;
        set => SetProperty(ref _selectedTestDeal, value);
    }
    [ObservableProperty]
    private bool _notable = true;
    [ObservableProperty]
    private FormFormat _format = FormFormat.Pdf;

    public enum FormFormat
    {
        LegacyImpact,
        Pdf
    }
    public Guid Id { get; } = Guid.NewGuid();
    [ObservableProperty]
    private bool _isSelected  = false;
    internal IForm CoreTypeInstance = new Core.Types.Notebook.Form();
    public void Select()
    {
        IsSelected = true;
        _logger?.LogInfo("Form selected.");
    }
    public void Deselect()
    {
        IsSelected = false;
        _logger?.LogInfo("Form deselected.");
    }

    public bool IsBlank
    {
        get
        {
            if (!string.IsNullOrEmpty(Name) || !string.IsNullOrEmpty(Notes))
                return false;
            if (TestDeals.Any(td => !td.IsBlank))
                return false;
            return true;
        }
    }
    partial void OnNameChanged(string? value)
    {
        OnPropertyChanged(nameof(IsBlank));
        UpdateCore();
        using (LogContext.PushProperty("FormId", Id))
        using (LogContext.PushProperty("Name", value))
        using (LogContext.PushProperty("Notes", Notes))
        using (LogContext.PushProperty("TestDeals", TestDeals.Count))
        {
            _logger?.LogInfo($"Form name changed: {value}");
        }
    }
    partial void OnNotesChanged(string? value)
    {
        OnPropertyChanged(nameof(IsBlank));
        UpdateCore();
        using (LogContext.PushProperty("FormId", Id))
        using (LogContext.PushProperty("Name", Name))
        using (LogContext.PushProperty("Notes", value))
        using (LogContext.PushProperty("TestDeals", TestDeals.Count))
        {
            _logger?.LogInfo($"Form notes changed: {value}");
        }
    }

    public Form(ILogService? logger = null)
    {
        _logger = logger;
        TestDeals = new(() => new TestDeal(_logger), _logger);
        TestDeals.CollectionChanged += TestDeals_CollectionChanged;
        foreach (var td in TestDeals) td.Parent = this;
        SelectTestDeal(TestDeals.FirstOrDefault(x => !x.IsBlank));
        _logger?.LogInfo("Form initialized.");
    }
    public Form(IForm form, ILogService? logger = null)
    {
        _logger = logger;
        CoreType = form;
        TestDeals = new(() => new TestDeal(_logger), _logger);
        foreach (var td in form.TestDeals)
        {
            var testDeal = new TestDeal(td, _logger) { Parent = this };
            TestDeals.Add(testDeal);
        }
        TestDeals.CollectionChanged += TestDeals_CollectionChanged;
        SelectTestDeal(TestDeals.FirstOrDefault(x => !x.IsBlank));
        Name = form.Name ?? string.Empty;
        Notes = form.Notes ?? string.Empty;
        Notable = form.Notable;
        _logger?.LogInfo("Form loaded from core type.");
    }

    private void TestDeals_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.NewItems != null)
            foreach (TestDeal td in e.NewItems) td.Parent = this;
        UpdateCore();
        _logger?.LogDebug("TestDeals collection changed.");
    }

    public void SelectTestDeal(ISelectable selectedTestDeal)
    {
        if (selectedTestDeal is null) return; 
        foreach (var deal in TestDeals) deal.Deselect();
        SelectedTestDeal = TestDeals.FirstOrDefault(c => c.Id == selectedTestDeal.Id);
        SelectedTestDeal?.Select();
        _logger?.LogInfo($"TestDeal selected: {SelectedTestDeal?.Id}");
    }

    internal void UpdateCore()
    {
        if (CoreType == null && Parent?.CoreType != null)
            CoreType = Parent.CoreType.Forms.FirstOrDefault(f => f.Id == Id);
        if (CoreType == null) return;
        CoreType.Name = Name ?? string.Empty;
        CoreType.Notes = Notes ?? string.Empty;
        CoreType.Notable = Notable;
        CoreType.TestDeals.Clear();
        CoreType.TestDeals.AddRange(TestDeals.Select(td => (Core.Types.Notebook.TestDeal)td));
        Parent?.UpdateCore();
        _logger?.LogDebug("Form core updated.");
    }

    public static implicit operator Core.Types.Notebook.Form(Form form)
    {
        if (form is null) return new Core.Types.Notebook.Form();
        return new Core.Types.Notebook.Form(form.Id)
        {
            Name = form.Name ?? string.Empty,
            Notes = form.Notes ?? string.Empty,
            Notable = form.Notable,
            TestDeals = [..form.TestDeals.Select(td => (Core.Types.Notebook.TestDeal)td)]
        };
    }
}
