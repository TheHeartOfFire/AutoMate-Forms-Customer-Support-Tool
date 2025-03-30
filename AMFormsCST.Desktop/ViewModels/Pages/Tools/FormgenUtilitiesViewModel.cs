using AMFormsCST.Core.Interfaces.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AMFormsCST.Desktop.ViewModels.Pages.Tools;

public partial class FormgenUtilitiesViewModel : ViewModel
{
    [ObservableProperty]
    private IFormgenUtils _formgenUtils = SupportTool.SupportToolInstance.FormgenUtils;

    [ObservableProperty]
    private bool _isDebugMode = false;

    [ObservableProperty]
    private Visibility _debugVisibility = Visibility.Collapsed;

    /// <summary>
    /// Set the state of Debug Mode
    /// if <param name="debugModeEnable"></param> is null, it will toggle the current state
    /// </summary>
    /// <param name="debugModeEnable"></param>
    public void ToggleDebugMode(bool? debugModeEnable)
    {
        if (!debugModeEnable.HasValue)
            debugModeEnable = !IsDebugMode;

        IsDebugMode = debugModeEnable.Value;
        DebugVisibility = IsDebugMode ? Visibility.Visible : Visibility.Collapsed;
    }
}
