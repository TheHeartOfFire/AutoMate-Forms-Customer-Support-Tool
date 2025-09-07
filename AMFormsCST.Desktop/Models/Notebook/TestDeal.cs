using AMFormsCST.Core.Interfaces;
using AMFormsCST.Core.Interfaces.Notebook;
using AMFormsCST.Desktop.BaseClasses;
using AMFormsCST.Desktop.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using Serilog.Context;
using System;
using System.Linq;

namespace AMFormsCST.Desktop.Models;
public partial class TestDeal : ManagedObservableCollectionItem
{
    private bool _isInitializing;

    [ObservableProperty]
    private string? _dealNumber = string.Empty;
    [ObservableProperty]
    private string? _purpose = string.Empty;
    public override bool IsBlank { get {  return string.IsNullOrEmpty(DealNumber) && string.IsNullOrEmpty(Purpose); } }
    public override Guid Id { get; } = Guid.NewGuid();
    internal ITestDeal? CoreType { get; set; }
    internal Form? Parent { get; set; }

    public TestDeal(ILogService? logger = null) : base(logger)
    {
        _isInitializing = true;
        _logger?.LogInfo("TestDeal initialized.");
        _isInitializing = false;
    }
    public TestDeal(ITestDeal testDeal, ILogService? logger = null) : base(logger)
    {
        _isInitializing = true;
        CoreType = testDeal;
        DealNumber = testDeal.DealNumber ?? string.Empty;
        Purpose = testDeal.Purpose ?? string.Empty;
        _logger?.LogInfo("TestDeal loaded from core type.");
        _isInitializing = false;
        UpdateCore();
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
        if (_isInitializing) return;

        if (CoreType == null && Parent?.CoreType != null)
            CoreType = Parent.CoreType.TestDeals.FirstOrDefault(td => td.Id == Id);
        if (CoreType == null) return;
        CoreType.DealNumber = DealNumber ?? string.Empty;
        CoreType.Purpose = Purpose ?? string.Empty;
        Parent?.UpdateCore();
        Parent?.Parent?.RaiseChildPropertyChanged();
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
