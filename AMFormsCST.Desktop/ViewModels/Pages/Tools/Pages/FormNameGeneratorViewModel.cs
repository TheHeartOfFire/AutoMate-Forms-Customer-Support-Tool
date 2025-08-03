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
    [ObservableProperty]
    private Form _form;
    [ObservableProperty]
    private bool _isPdfSelected; // This will initially be false by default
    partial void OnIsPdfSelectedChanged(bool value)
    {
        if (value) // If this button is checked
        {
            Form.AddTag(Form.Tag.Pdf);
        }
        else // If this button is unchecked
        {
            // Only remove if it was explicitly unchecked (e.g., by another button in its group)
            Form.RemoveTag(Form.Tag.Pdf);
        }
    }

    [ObservableProperty]
    private bool _isImpactSelected;
    partial void OnIsImpactSelectedChanged(bool value)
    {
        if (value)
        {
            Form.AddTag(Form.Tag.Impact);
        }
        else
        {
            Form.RemoveTag(Form.Tag.Impact);
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
        }
        else
        {
            Form.RemoveTag(Form.Tag.Sold);
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
        }
        else
        {
            Form.RemoveTag(Form.Tag.Trade);
        }
    }

    // --- Properties for ToggleButtons (independent) ---

    [ObservableProperty]
    private bool _isLawSelected;
    partial void OnIsLawSelectedChanged(bool value)
    {
        if (value)
        {
            Form.AddTag(Form.Tag.Law);
        }
        else
        {
            Form.RemoveTag(Form.Tag.Law);
        }
    }

    [ObservableProperty]
    private bool _isCustomSelected;
    partial void OnIsCustomSelectedChanged(bool value)
    {
        if (value)
        {
            Form.AddTag(Form.Tag.Custom);
        }
        else
        {
            Form.RemoveTag(Form.Tag.Custom);
        }
    }

    [ObservableProperty]
    private bool _isVehicleMerchandisingSelected;
    partial void OnIsVehicleMerchandisingSelectedChanged(bool value)
    {
        if (value)
        {
            Form.AddTag(Form.Tag.VehicleMerchandising);
        }
        else
        {
            Form.RemoveTag(Form.Tag.VehicleMerchandising);
        }
    }

    private readonly ISupportTool _supportTool;
    // Constructor to set initial UI state and synchronize with model
    public FormNameGeneratorViewModel(ISupportTool supportTool)
    {
        _supportTool = supportTool ?? throw new ArgumentNullException(nameof(supportTool));
        _form = new Form(_supportTool);
        _isPdfSelected = true; 
    }
    [RelayCommand]
    private void CopyFileName()
    {
        if (!string.IsNullOrEmpty(Form.FileName))
        {
            Clipboard.SetText(Form.FileName);
            // Optionally, provide user feedback, e.g., a simple notification or toast.
            
        }
    }

    [RelayCommand]
    private void ClearForm()
    {
        // 1. Clear the data in the model
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
