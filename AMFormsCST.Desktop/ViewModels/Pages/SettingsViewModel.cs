using AMFormsCST.Desktop.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using Velopack;
using Velopack.Sources;
using Wpf.Ui;
using Wpf.Ui.Abstractions.Controls;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace AMFormsCST.Desktop.ViewModels.Pages;

public partial class SettingsViewModel : ViewModel
{
    private bool _isInitialized = false;
    private readonly IUpdateManagerService _updateManager;

    [ObservableProperty]
    private ApplicationTheme _currentTheme = ApplicationTheme.Unknown;

    [ObservableProperty]
    private string _appVersion = String.Empty;

    public SettingsViewModel(IUpdateManagerService updateManager)
    {
        _updateManager = updateManager ?? throw new ArgumentNullException(nameof(updateManager));
        InitializeViewModel();
    }

    public void OnNavigatedTo()
    {
        if (!_isInitialized)
            InitializeViewModel();
    }

    public void OnNavigatedFrom() { }

    private void InitializeViewModel()
    {
        CurrentTheme = ApplicationThemeManager.GetAppTheme();
        AppVersion = $"Version {GetAppVersion()}";
        _isInitialized = true;
    }

    [RelayCommand]
    private void OnChangeTheme(string parameter)
    {
        var newTheme = parameter switch
        {
            "theme_light" => ApplicationTheme.Light,
            "theme_dark" => ApplicationTheme.Dark,
            _ => ApplicationTheme.Unknown
        };

        if (newTheme != ApplicationTheme.Unknown && CurrentTheme != newTheme)
        {
            ApplicationThemeManager.Apply(newTheme);
            CurrentTheme = newTheme;
        }
    }

    [RelayCommand]
    private async Task CheckForUpdates()
    {
        await _updateManager.CheckForUpdatesAsync();
    }

    private static string GetAppVersion()
    {
        return FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion ?? "1.0.0";
    }
}