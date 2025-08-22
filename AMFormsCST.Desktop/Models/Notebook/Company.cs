using AMFormsCST.Core.Interfaces.Notebook;
using AMFormsCST.Core.Types.Notebook;
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
    internal ICompany CoreType = new Core.Types.Notebook.Company();
    public Company()
    {
            
    }
    public Company(ICompany company)
    {
        CoreType = company ?? throw new ArgumentNullException(nameof(company), "Cannot create a Company from a null item.");
        Name = company.Name;
        CompanyCode = company.CompanyCode;
    }
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

    public static implicit operator Core.Types.Notebook.Company(Company company)
    {
        if (company is null) return new Core.Types.Notebook.Company();

        return new Core.Types.Notebook.Company(company.Id)
        {
            Name = company.Name ?? string.Empty,
            CompanyCode = company.CompanyCode ?? string.Empty
        };
    }
}
