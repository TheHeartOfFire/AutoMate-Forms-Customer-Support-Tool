using AMFormsCST.Core.Interfaces;
using AMFormsCST.Core.Interfaces.Notebook;
using AMFormsCST.Core.Types.Notebook;
using AMFormsCST.Desktop.BaseClasses;
using AMFormsCST.Desktop.Interfaces;
using AMFormsCST.Desktop.Types;
using CommunityToolkit.Mvvm.ComponentModel;
using Serilog.Context;
using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace AMFormsCST.Desktop.Models;
public partial class Form : ManagedObservableCollectionItem
{
    private bool _isInitializing;

    [ObservableProperty]
    private string? _name = string.Empty;
    [ObservableProperty]
    private string? _notes = string.Empty;
    public ManagedObservableCollection<TestDeal> TestDeals { get; set; }
    [ObservableProperty]
    private bool _notable = true;
    [ObservableProperty]
    private FormFormat _format = FormFormat.Pdf;
    internal IForm? CoreType { get; set; }
    internal NoteModel? Parent { get; set; }
    public override Guid Id { get; } = Guid.NewGuid();
    public override bool IsBlank
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
    public TestDeal? SelectedTestDeal => TestDeals.SelectedItem;

    public enum FormFormat
    {
        LegacyImpact,
        Pdf
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

    partial void OnNotableChanged(bool value)
    {
        UpdateCore();
        using (LogContext.PushProperty("FormId", Id))
        using (LogContext.PushProperty("Notable", value))
        {
            _logger?.LogInfo($"Form notable changed: {value}");
        }
    }

    public Form(ILogService? logger = null) : base(logger)
    {
        _isInitializing = true;
        CoreType = new Core.Types.Notebook.Form();

        InitTestDeals();

        TestDeals ??= new ManagedObservableCollection<TestDeal>(
            () => new TestDeal(_logger),
            null,
            _logger
        );

        _logger?.LogInfo("Form initialized.");
        _isInitializing = false;
    }
    public Form(IForm form, ILogService? logger = null) : base(logger)
    {
        _isInitializing = true;
        CoreType = form;

        InitTestDeals();

        TestDeals ??= new ManagedObservableCollection<TestDeal>(
            () => new TestDeal(_logger),
            null,
            _logger
        );

        Name = form.Name ?? string.Empty;
        Notes = form.Notes ?? string.Empty;
        Notable = form.Notable;
        _logger?.LogInfo("Form loaded from core type.");
        _isInitializing = false;
        UpdateCore();
    }
    private void InitTestDeals()
    {
        var testDeals = CoreType?.TestDeals.ToList()
                .Select(coreTestDeal =>
                {
                    var testDeal = new TestDeal(coreTestDeal, _logger)
                    {
                        CoreType = coreTestDeal,
                        Parent = this
                    };
                    testDeal.PropertyChanged += OnTestDealPropertyChanged;
                    return testDeal;
                });

        TestDeals = new ManagedObservableCollection<TestDeal>(
            () => new TestDeal(_logger),
            testDeals,
            _logger
        );
        TestDeals.PropertyChanged += OnTestDealsPropertyChanged;
        TestDeals.CollectionChanged += TestDeals_CollectionChanged;
        TestDeals.FirstOrDefault()?.Select();
    }

    private void OnTestDealsPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ManagedObservableCollection<TestDeal>.SelectedItem))
        {
            OnPropertyChanged(nameof(SelectedTestDeal));
        }
    }

    private void OnTestDealPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        // This handler is for property changes on individual TestDeal items.
        // It can be kept for future use or removed if not needed.
    }

    private void TestDeals_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.NewItems != null)
            foreach (TestDeal td in e.NewItems) td.Parent = this;
        UpdateCore();
        _logger?.LogDebug("TestDeals collection changed.");
    }

    internal void UpdateCore()
    {
        if (_isInitializing) return;

        if (CoreType == null && Parent?.CoreType != null)
            CoreType = Parent.CoreType.Forms.FirstOrDefault(f => f.Id == Id);
        if (CoreType == null) return;
        CoreType.Name = Name ?? string.Empty;
        CoreType.Notes = Notes ?? string.Empty;
        CoreType.Notable = Notable;
        CoreType.TestDeals.Clear();
        CoreType.TestDeals.AddRange(TestDeals.Select(td => (Core.Types.Notebook.TestDeal)td));
        Parent?.UpdateCore();
        Parent?.RaiseChildPropertyChanged();
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
