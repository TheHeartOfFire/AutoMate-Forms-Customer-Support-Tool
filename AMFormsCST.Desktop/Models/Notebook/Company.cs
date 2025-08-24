using AMFormsCST.Core.Interfaces;
using AMFormsCST.Core.Interfaces.Notebook;
using AMFormsCST.Core.Types.Notebook;
using AMFormsCST.Desktop.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using Serilog.Context;
using System;

namespace AMFormsCST.Desktop.Models;
public partial class Company : ObservableObject, ISelectable, IBlankMaybe
{
    private readonly ILogService? _logger;

    [ObservableProperty]
    private string? _name = string.Empty;
    [ObservableProperty]
    private string? _companyCode = string.Empty;
    public Guid Id { get; } = Guid.NewGuid();
    [ObservableProperty]
    private bool _isSelected = false;
    internal ICompany? CoreType { get; set; }
    internal Dealer? Parent { get; set; }

    public Company(ILogService? logger = null)
    {
        _logger = logger;
        _logger?.LogInfo("Company initialized.");
    }
    public Company(ICompany company, ILogService? logger = null)
    {
        _logger = logger;
        CoreType = company;
        Name = company.Name;
        CompanyCode = company.CompanyCode;
        _logger?.LogInfo("Company loaded from core type.");
    }
    public void Select()
    {
        IsSelected = true;
        _logger?.LogInfo("Company selected.");
    }
    public void Deselect()
    {
        IsSelected = false;
        _logger?.LogInfo("Company deselected.");
    }
    public bool IsBlank { get { return string.IsNullOrEmpty(Name) && string.IsNullOrEmpty(CompanyCode); }}
    partial void OnNameChanged(string? value)
    {
        OnPropertyChanged(nameof(IsBlank));
        UpdateCore();
        using (LogContext.PushProperty("CompanyId", Id))
        using (LogContext.PushProperty("Name", value))
        using (LogContext.PushProperty("CompanyCode", CompanyCode))
        {
            _logger?.LogInfo($"Company name changed: {value}");
        }
    }

    partial void OnCompanyCodeChanged(string? value)
    {
        OnPropertyChanged(nameof(IsBlank));
        UpdateCore();
        using (LogContext.PushProperty("CompanyId", Id))
        using (LogContext.PushProperty("Name", Name))
        using (LogContext.PushProperty("CompanyCode", value))
        {
            _logger?.LogInfo($"Company code changed: {value}");
        }
    }

    internal void UpdateCore()
    {
        if (CoreType == null && Parent?.CoreType != null)
            CoreType = Parent.CoreType.Companies.FirstOrDefault(c => c.Id == Id);
        if (CoreType == null) return;
        CoreType.Name = Name ?? string.Empty;
        CoreType.CompanyCode = CompanyCode ?? string.Empty;
        Parent?.UpdateCore();
        _logger?.LogDebug("Company core updated.");
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
