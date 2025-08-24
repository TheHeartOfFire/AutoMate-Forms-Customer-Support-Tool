using AMFormsCST.Core.Interfaces;
using AMFormsCST.Core.Interfaces.Notebook;
using AMFormsCST.Desktop.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using Serilog.Context;
using System;

namespace AMFormsCST.Desktop.Models;
public partial class TestDeal : ObservableObject, IBlankMaybe, ISelectable
{
    private readonly ILogService? _logger;

    [ObservableProperty]
    private string? _dealNumber = string.Empty;
    [ObservableProperty]
    private string? _purpose = string.Empty;
    public bool IsBlank { get {  return string.IsNullOrEmpty(DealNumber) && string.IsNullOrEmpty(Purpose); } }
    public Guid Id { get; } = Guid.NewGuid();
    [ObservableProperty]
    private bool _isSelected = false;
    internal ITestDeal? CoreType { get; set; }
    internal Form? Parent { get; set; }

    public void Select()
    {
        IsSelected = true;
        _logger?.LogInfo("TestDeal selected.");
    }
    public void Deselect()
    {
        IsSelected = false;
        _logger?.LogInfo("TestDeal deselected.");
    }
    public TestDeal(ILogService? logger = null)
    {
        _logger = logger;
        _logger?.LogInfo("TestDeal initialized.");
    }
    public TestDeal(ITestDeal testDeal, ILogService? logger = null)
    {
        _logger = logger;
        CoreType = testDeal;
        DealNumber = testDeal.DealNumber ?? string.Empty;
        Purpose = testDeal.Purpose ?? string.Empty;
        _logger?.LogInfo("TestDeal loaded from core type.");
    }

    partial void OnDealNumberChanged(string? value)
    {
        OnPropertyChanged(nameof(IsBlank));
        UpdateCore();
        using (LogContext.PushProperty("TestDealId", Id))
        using (LogContext.PushProperty("DealNumber", value))
        using (LogContext.PushProperty("Purpose", Purpose))
        {
            _logger?.LogInfo($"TestDeal number changed: {value}");
        }
    }

    partial void OnPurposeChanged(string? value)
    {
        OnPropertyChanged(nameof(IsBlank));
        UpdateCore();
        using (LogContext.PushProperty("TestDealId", Id))
        using (LogContext.PushProperty("DealNumber", DealNumber))
        using (LogContext.PushProperty("Purpose", value))
        {
            _logger?.LogInfo($"TestDeal purpose changed: {value}");
        }
    }

    internal void UpdateCore()
    {
        if (CoreType == null && Parent?.CoreType != null)
            CoreType = Parent.CoreType.TestDeals.FirstOrDefault(td => td.Id == Id);
        if (CoreType == null) return;
        CoreType.DealNumber = DealNumber ?? string.Empty;
        CoreType.Purpose = Purpose ?? string.Empty;
        Parent?.UpdateCore();
        _logger?.LogDebug("TestDeal core updated.");
    }

    public static implicit operator Core.Types.Notebook.TestDeal(TestDeal testDeal)
    {
        if (testDeal is null) return new Core.Types.Notebook.TestDeal();

        return new Core.Types.Notebook.TestDeal(testDeal.Id)
        {
            DealNumber = testDeal.DealNumber ?? string.Empty,
            Purpose = testDeal.Purpose ?? string.Empty
        };
    }
}
