using AMFormsCST.Core.Interfaces.Notebook;
using AMFormsCST.Desktop.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AMFormsCST.Desktop.Models;
public partial class TestDeal : ObservableObject, IBlankMaybe, ISelectable
{
    [ObservableProperty]
    private string? _dealNumber = string.Empty;
    [ObservableProperty]
    private string? _purpose = string.Empty;
    public bool IsBlank { get {  return string.IsNullOrEmpty(DealNumber) && string.IsNullOrEmpty(Purpose); } }
    partial void OnDealNumberChanged(string? value)
    {
        OnPropertyChanged(nameof(IsBlank)); 
    }

    partial void OnPurposeChanged(string? value)
    {
        OnPropertyChanged(nameof(IsBlank)); 
    }
    public Guid Id { get; } = Guid.NewGuid();
    [ObservableProperty]
    private bool _isSelected = false;
    internal ITestDeal CoreType = new Core.Types.Notebook.TestDeal();
    public void Select()
    {
        IsSelected = true;
    }
    public void Deselect()
    {
        IsSelected = false;
    }
    public TestDeal() { }
    public TestDeal(ITestDeal testDeal)
    {
        CoreType = testDeal ?? throw new ArgumentNullException(nameof(testDeal), "Cannot create a TestDeal from a null item.");
        DealNumber = testDeal.DealNumber ?? string.Empty;
        Purpose = testDeal.Purpose ?? string.Empty;
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
