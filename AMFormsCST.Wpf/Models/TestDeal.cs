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
    public void Select()
    {
        IsSelected = true;
    }
    public void Deselect()
    {
        IsSelected = false;
    }
}
