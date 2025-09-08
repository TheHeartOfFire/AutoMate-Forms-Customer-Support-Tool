using AMFormsCST.Core.Interfaces;
using AMFormsCST.Desktop.Models.FormNameGenerator;
using AMFormsCST.Desktop.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;

namespace AMFormsCST.Desktop.ViewModels.Pages.Tools;

public partial class FormNameGeneratorViewModel : ViewModel
{
    private readonly ISupportTool _supportTool;

    [ObservableProperty]
    private Form _form;

    #region Toggle Properties
    private bool _isPdfSelected;
    public bool IsPdfSelected
    {
        get => _isPdfSelected;
        set
        {
            if (SetProperty(ref _isPdfSelected, value))
            {
                if (value)
                {
                    Form.AddTag(Form.Tag.Pdf);
                    Form.RemoveTag(Form.Tag.Impact);
                    IsImpactSelected = false;
                }
                else if (!IsImpactSelected)
                {
                    IsImpactSelected = true;
                }
            }
        }
    }

    private bool _isImpactSelected;
    public bool IsImpactSelected
    {
        get => _isImpactSelected;
        set
        {
            if (SetProperty(ref _isImpactSelected, value))
            {
                if (value)
                {
                    Form.AddTag(Form.Tag.Impact);
                    Form.RemoveTag(Form.Tag.Pdf);
                    IsPdfSelected = false;
                }
                else if (!IsPdfSelected)
                {
                    IsPdfSelected = true;
                }
            }
        }
    }

    private bool _isSoldSelected;
    public bool IsSoldSelected
    {
        get => _isSoldSelected;
        set
        {
            if (SetProperty(ref _isSoldSelected, value))
            {
                if (value)
                {
                    Form.AddTag(Form.Tag.Sold);
                    IsTradeSelected = false;
                }
                else
                {
                    Form.RemoveTag(Form.Tag.Sold);
                }
            }
        }
    }

    private bool _isTradeSelected;
    public bool IsTradeSelected
    {
        get => _isTradeSelected;
        set
        {
            if (SetProperty(ref _isTradeSelected, value))
            {
                if (value)
                {
                    Form.AddTag(Form.Tag.Trade);
                    IsSoldSelected = false;
                }
                else
                {
                    Form.RemoveTag(Form.Tag.Trade);
                }
            }
        }
    }

    private bool _isLawSelected;
    public bool IsLawSelected
    {
        get => _isLawSelected;
        set
        {
            if (SetProperty(ref _isLawSelected, value))
            {
                if (value) Form.AddTag(Form.Tag.Law); else Form.RemoveTag(Form.Tag.Law);
            }
        }
    }

    private bool _isCustomSelected;
    public bool IsCustomSelected
    {
        get => _isCustomSelected;
        set
        {
            if (SetProperty(ref _isCustomSelected, value))
            {
                if (value) Form.AddTag(Form.Tag.Custom); else Form.RemoveTag(Form.Tag.Custom);
            }
        }
    }

    private bool _isVehicleMerchandisingSelected;
    public bool IsVehicleMerchandisingSelected
    {
        get => _isVehicleMerchandisingSelected;
        set
        {
            if (SetProperty(ref _isVehicleMerchandisingSelected, value))
            {
                if (value) Form.AddTag(Form.Tag.VehicleMerchandising); else Form.RemoveTag(Form.Tag.VehicleMerchandising);
            }
        }
    }
    #endregion

    public FormNameGeneratorViewModel(ISupportTool supportTool)
    {
        _supportTool = supportTool;
        _form = new Form(_supportTool);
        ResetForm();
    }
    public FormNameGeneratorViewModel()
    {
        _supportTool = new DesignTimeSupportTool();
        _form = new Form(_supportTool)
        {
            Title = "Retail Installment Contract",
            Code = "553-TX-eps",
            RevisionDate = "01/24",
            State = "TX"
        };
        IsPdfSelected = true;
        IsLawSelected = true;
    }

    [RelayCommand]
    private void CopyFileName()
    {
        if (!string.IsNullOrEmpty(Form.FileName))
        {
            Clipboard.SetText(Form.FileName);
        }
    }

    [RelayCommand]
    private void ClearForm() => ResetForm();

    private void ResetForm()
    {
        Form.Clear();
        IsPdfSelected = true;
        IsImpactSelected = false;
        IsSoldSelected = false;
        IsTradeSelected = false;
        IsLawSelected = false;
        IsCustomSelected = false;
        IsVehicleMerchandisingSelected = false;
    }
}