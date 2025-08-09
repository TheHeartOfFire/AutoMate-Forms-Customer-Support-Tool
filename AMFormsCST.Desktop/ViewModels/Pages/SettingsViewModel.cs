using AMFormsCST.Core.Interfaces;
using AMFormsCST.Desktop.Models.UserSettings;
using AMFormsCST.Desktop.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace AMFormsCST.Desktop.ViewModels.Pages;

public partial class SettingsViewModel : ViewModel
{
    private readonly IUpdateManagerService _updateManagerService;
    private readonly ISupportTool _supportTool;
    private bool _isInitialized = false;

    [ObservableProperty]
    private string _appVersion = string.Empty;

    // UI Settings Properties
    [ObservableProperty]
    private ApplicationTheme _currentTheme;

    [ObservableProperty]
    private bool _alwaysOnTop;

    // User Settings Properties
    [ObservableProperty]
    private bool _selectNewTemplate;

    [ObservableProperty]
    private bool _capitalizeFormCode;

    [ObservableProperty]
    private string _extSeparator = string.Empty;

    public SettingsViewModel(IUpdateManagerService updateManagerService, ISupportTool supportTool)
    {
        _updateManagerService = updateManagerService;
        _supportTool = supportTool;

        // Subscribe to property changes on this ViewModel
        PropertyChanged += OnSettingsViewModelPropertyChanged;
        if (!_isInitialized)
            InitializeViewModel();
    }


    public void OnNavigatedTo()
    {
    }


    private void InitializeViewModel()
    {
        AppVersion = $"Version {GetAppVersion()}";

        // Load settings from the core service
        var themeSetting = _supportTool.Settings.UiSettings.Settings.OfType<ThemeSetting>().FirstOrDefault();
        if (themeSetting != null) CurrentTheme = themeSetting.Theme;

        var aotSetting = _supportTool.Settings.UiSettings.Settings.OfType<AlwaysOnTopSetting>().FirstOrDefault();
        if (aotSetting != null) AlwaysOnTop = aotSetting.IsEnabled;

        var templateSetting = _supportTool.Settings.UiSettings.Settings.OfType<NewTemplateSetting>().FirstOrDefault();
        if (templateSetting != null) SelectNewTemplate = templateSetting.SelectNewTemplate;

        var formCodeSetting = _supportTool.Settings.UiSettings.Settings.OfType<CapitalizeFormCodeSetting>().FirstOrDefault();
        if (formCodeSetting != null) CapitalizeFormCode = formCodeSetting.CapitalizeFormCode;

        var userSettings = _supportTool.Settings.UserSettings;
        ExtSeparator = userSettings.ExtSeparator;

        _isInitialized = true;
    }

    private void OnSettingsViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        // This method now handles saving settings in real-time.
        switch (e.PropertyName)
        {
            case nameof(CurrentTheme):
                var themeSetting = _supportTool.Settings.UiSettings.Settings.OfType<ThemeSetting>().FirstOrDefault();
                if (themeSetting != null && themeSetting.Theme != CurrentTheme)
                {
                    themeSetting.Theme = CurrentTheme;
                    ApplicationThemeManager.Apply(CurrentTheme);
                }
                break;

            case nameof(AlwaysOnTop):
                var aotSetting = _supportTool.Settings.UiSettings.Settings.OfType<AlwaysOnTopSetting>().FirstOrDefault();
                if (aotSetting != null) aotSetting.IsEnabled = AlwaysOnTop;
                break;

            case nameof(SelectNewTemplate):
                var templateSetting = _supportTool.Settings.UiSettings.Settings.OfType<NewTemplateSetting>().FirstOrDefault();
                if (templateSetting != null) templateSetting.SelectNewTemplate = SelectNewTemplate;
                break;

            case nameof(CapitalizeFormCode):
                var formCodeSetting = _supportTool.Settings.UiSettings.Settings.OfType<CapitalizeFormCodeSetting>().FirstOrDefault();
                if (formCodeSetting != null) formCodeSetting.CapitalizeFormCode = CapitalizeFormCode;
                break;

            case nameof(ExtSeparator):
                _supportTool.Settings.UserSettings.ExtSeparator = ExtSeparator;
                break;
        }

        // Save settings if the view model is initialized
        if (_isInitialized)
        {
            _supportTool.SaveAllSettings();
        }
    }

    [RelayCommand]
    private async Task CheckForUpdates()
    {
        await _updateManagerService.CheckForUpdatesAsync();
    }

    private static string GetAppVersion()
    {
        return FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion ?? "1.0.0";
    }
}