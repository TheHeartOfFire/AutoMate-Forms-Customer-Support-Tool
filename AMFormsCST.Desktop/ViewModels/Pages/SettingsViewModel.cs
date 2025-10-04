using AMFormsCST.Core.Interfaces;
using AMFormsCST.Desktop.Models.UserSettings;
using AMFormsCST.Desktop.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using Wpf.Ui.Appearance;

namespace AMFormsCST.Desktop.ViewModels.Pages;

public partial class SettingsViewModel : ViewModel
{
    private readonly IUpdateManagerService _updateManagerService;
    private readonly ISupportTool _supportTool;
    private readonly IBugReportService _bugReportService;
    private readonly ILogService? _logger;
    private bool _isInitialized = false;

    [ObservableProperty]
    private string _appVersion = string.Empty;

    [ObservableProperty]
    private ApplicationTheme _currentTheme;

    [ObservableProperty]
    private bool _alwaysOnTop;

    [ObservableProperty]
    private bool _selectNewTemplate;

    [ObservableProperty]
    private bool _capitalizeFormCode;

    [ObservableProperty]
    private string _extSeparator = string.Empty;

    public SettingsViewModel(IUpdateManagerService updateManagerService, ISupportTool supportTool, IBugReportService bugReportService, ILogService? logger = null)
    {
        _updateManagerService = updateManagerService;
        _supportTool = supportTool;
        _bugReportService = bugReportService;
        _logger = logger;
        _logger?.LogInfo("SettingsViewModel initialized.");

        PropertyChanged += OnSettingsViewModelPropertyChanged;
        if (!_isInitialized)
            InitializeViewModel();
    }

    public void OnNavigatedTo()
    {
        _logger?.LogInfo("SettingsPage navigated to.");
    }

    private void InitializeViewModel()
    {
        AppVersion = $"Version {GetAppVersion()}";
        _logger?.LogDebug($"AppVersion set: {AppVersion}");

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
        _logger?.LogInfo("SettingsViewModel initialized and settings loaded.");
    }

    private void OnSettingsViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        try
        {
            switch (e.PropertyName)
            {
                case nameof(CurrentTheme):
                    var themeSetting = _supportTool.Settings.UiSettings.Settings.OfType<ThemeSetting>().FirstOrDefault();
                    if (themeSetting != null && themeSetting.Theme != CurrentTheme)
                    {
                        themeSetting.Theme = CurrentTheme;
                        ApplicationThemeManager.Apply(CurrentTheme);
                        _logger?.LogInfo($"Theme changed: {CurrentTheme}");
                    }
                    break;

                case nameof(AlwaysOnTop):
                    var aotSetting = _supportTool.Settings.UiSettings.Settings.OfType<AlwaysOnTopSetting>().FirstOrDefault();
                    if (aotSetting != null)
                    {
                        aotSetting.IsEnabled = AlwaysOnTop;
                        _logger?.LogInfo($"AlwaysOnTop changed: {AlwaysOnTop}");
                    }
                    break;

                case nameof(SelectNewTemplate):
                    var templateSetting = _supportTool.Settings.UiSettings.Settings.OfType<NewTemplateSetting>().FirstOrDefault();
                    if (templateSetting != null)
                    {
                        templateSetting.SelectNewTemplate = SelectNewTemplate;
                        _logger?.LogInfo($"SelectNewTemplate changed: {SelectNewTemplate}");
                    }
                    break;

                case nameof(CapitalizeFormCode):
                    var formCodeSetting = _supportTool.Settings.UiSettings.Settings.OfType<CapitalizeFormCodeSetting>().FirstOrDefault();
                    if (formCodeSetting != null)
                    {
                        formCodeSetting.CapitalizeFormCode = CapitalizeFormCode;
                        _logger?.LogInfo($"CapitalizeFormCode changed: {CapitalizeFormCode}");
                    }
                    break;

                case nameof(ExtSeparator):
                    _supportTool.Settings.UserSettings.ExtSeparator = ExtSeparator;
                    _logger?.LogInfo($"ExtSeparator changed: {ExtSeparator}");
                    break;
            }

            if (_isInitialized)
            {
                _supportTool.SaveAllSettings();
                _logger?.LogInfo("Settings saved after property change.");
            }
        }
        catch (Exception ex)
        {
            _logger?.LogError("Error in OnSettingsViewModelPropertyChanged.", ex);
        }
    }

    [RelayCommand]
    private async Task CheckForUpdates()
    {
        try
        {
            await _updateManagerService.CheckForUpdatesAsync();
            _logger?.LogInfo("CheckForUpdates command executed.");
        }
        catch (Exception ex)
        {
            _logger?.LogError("Error during CheckForUpdates command.", ex);
        }
    }

    [RelayCommand]
    private async Task ReportBug()
    {
        await _bugReportService.CreateBugReportAsync();
    }

    private static string GetAppVersion() => FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion ?? "1.0.0";
    
}