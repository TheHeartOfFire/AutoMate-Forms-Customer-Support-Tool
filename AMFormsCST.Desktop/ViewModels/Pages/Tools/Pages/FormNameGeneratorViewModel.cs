using AMFormsCST.Core.Interfaces;
using AMFormsCST.Desktop.Models.FormNameGenerator;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AMFormsCST.Desktop.ViewModels.Pages.Tools;
public partial class FormNameGeneratorViewModel : ViewModel
{
    private readonly ILogService? _logger;

    [ObservableProperty]
    private Form _form;
    [ObservableProperty]
    private bool _isPdfSelected;
    partial void OnIsPdfSelectedChanged(bool value)
    {
        if (value)
        {
            Form.AddTag(Form.Tag.Pdf);
            _logger?.LogInfo("PDF tag added.");
        }
        else
        {
            Form.RemoveTag(Form.Tag.Pdf);
            _logger?.LogInfo("PDF tag removed.");
        }
    }

    [ObservableProperty]
    private bool _isImpactSelected;
    partial void OnIsImpactSelectedChanged(bool value)
    {
        if (value)
        {
            Form.AddTag(Form.Tag.Impact);
            _logger?.LogInfo("Impact tag added.");
        }
        else
        {
            Form.RemoveTag(Form.Tag.Impact);
            _logger?.LogInfo("Impact tag removed.");
        }
    }

    [ObservableProperty]
    private bool _isSoldSelected;
    partial void OnIsSoldSelectedChanged(bool value)
    {
        if (value)
        {
            IsTradeSelected = false;
            Form.AddTag(Form.Tag.Sold);
            _logger?.LogInfo("Sold tag added.");
        }
        else
        {
            Form.RemoveTag(Form.Tag.Sold);
            _logger?.LogInfo("Sold tag removed.");
        }
    }

    [ObservableProperty]
    private bool _isTradeSelected;
    partial void OnIsTradeSelectedChanged(bool value)
    {
        if (value)
        {
            IsSoldSelected = false;
            Form.AddTag(Form.Tag.Trade);
            _logger?.LogInfo("Trade tag added.");
        }
        else
        {
            Form.RemoveTag(Form.Tag.Trade);
            _logger?.LogInfo("Trade tag removed.");
        }
    }

    [ObservableProperty]
    private bool _isLawSelected;
    partial void OnIsLawSelectedChanged(bool value)
    {
        if (value)
        {
            Form.AddTag(Form.Tag.Law);
            _logger?.LogInfo("Law tag added.");
        }
        else
        {
            Form.RemoveTag(Form.Tag.Law);
            _logger?.LogInfo("Law tag removed.");
        }
    }

    [ObservableProperty]
    private bool _isCustomSelected;
    partial void OnIsCustomSelectedChanged(bool value)
    {
        if (value)
        {
            Form.AddTag(Form.Tag.Custom);
            _logger?.LogInfo("Custom tag added.");
        }
        else
        {
            Form.RemoveTag(Form.Tag.Custom);
            _logger?.LogInfo("Custom tag removed.");
        }
    }

    [ObservableProperty]
    private bool _isVehicleMerchandisingSelected;
    partial void OnIsVehicleMerchandisingSelectedChanged(bool value)
    {
        if (value)
        {
            Form.AddTag(Form.Tag.VehicleMerchandising);
            _logger?.LogInfo("VehicleMerchandising tag added.");
        }
        else
        {
            Form.RemoveTag(Form.Tag.VehicleMerchandising);
            _logger?.LogInfo("VehicleMerchandising tag removed.");
        }
    }

    private readonly ISupportTool _supportTool;

    public FormNameGeneratorViewModel(ISupportTool supportTool, ILogService? logger = null)
    {
        _supportTool = supportTool ?? throw new ArgumentNullException(nameof(supportTool));
        _logger = logger;
        _form = new Form(_supportTool, _logger);
        _isPdfSelected = true;
        _logger?.LogInfo("FormNameGeneratorViewModel initialized.");
    }

    [RelayCommand]
    private void CopyFileName()
    {
        if (!string.IsNullOrEmpty(Form.FileName))
        {
            try
            {
                Clipboard.SetText(Form.FileName);
                _logger?.LogInfo($"Form file name copied: {Form.FileName}");
            }
            catch (Exception ex)
            {
                _logger?.LogError("Error copying form file name to clipboard.", ex);
            }
        }
        else
        {
            _logger?.LogWarning("CopyFileName called but file name is empty.");
        }
    }

    [RelayCommand]
    private void ClearForm()
    {
        Form.Clear();

        IsPdfSelected = true;
        IsImpactSelected = false;
        IsSoldSelected = false;
        IsTradeSelected = false;
        IsLawSelected = false;
        IsCustomSelected = false;
        IsVehicleMerchandisingSelected = false;
        _logger?.LogInfo("Form cleared and UI reset.");
    }
}
