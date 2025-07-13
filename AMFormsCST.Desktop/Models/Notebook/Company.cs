using AMFormsCST.Desktop.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFormsCST.Desktop.Models;
public partial class Company : ObservableObject, ISelectable, IBlankMaybe
{
    [ObservableProperty]
    private string? _name = string.Empty;
    [ObservableProperty]
    private string? _companyCode = string.Empty;
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
    public bool IsBlank { get { return string.IsNullOrEmpty(Name) && string.IsNullOrEmpty(CompanyCode); }}
    partial void OnNameChanged(string? value)
    {
        OnPropertyChanged(nameof(IsBlank)); 
    }

    partial void OnCompanyCodeChanged(string? value)
    {
        OnPropertyChanged(nameof(IsBlank)); 
    }
}
