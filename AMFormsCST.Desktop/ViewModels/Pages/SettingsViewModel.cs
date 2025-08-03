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
    private readonly ISnackbarService _snackbarService;
    private bool _isInitialized = false;

    [ObservableProperty]
    private ApplicationTheme _currentTheme = ApplicationTheme.Unknown;

    [ObservableProperty]
    private string _appVersion = String.Empty;

    public SettingsViewModel(ISnackbarService snackbarService)
    {
        _snackbarService = snackbarService;
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
        try
        {
            // Replace with your actual repository URL
            var mgr = new UpdateManager(new GithubSource("https://github.com/TheHeartOfFire/AutoMate-Forms-Customer-Support-Tool", null, false));

            var newVersion = await mgr.CheckForUpdatesAsync();
            if (newVersion == null)
            {
                _snackbarService.Show("No Updates Found", "Your application is up to date.", ControlAppearance.Success, new SymbolIcon(SymbolRegular.Checkmark24), TimeSpan.FromSeconds(3));
                return;
            }

            _snackbarService.Show("Update Available", $"Downloading version {newVersion.TargetFullRelease.Version}...", ControlAppearance.Info, new SymbolIcon(SymbolRegular.ArrowDownload24), TimeSpan.FromSeconds(5));

            await mgr.DownloadUpdatesAsync(newVersion);

            mgr.ApplyUpdatesAndRestart(newVersion);
        }
        catch (Exception ex)
        {
            _snackbarService.Show("Update Check Failed", ex.Message, ControlAppearance.Danger, new SymbolIcon(SymbolRegular.ErrorCircle24), TimeSpan.FromSeconds(5));
        }
    }

    private static string GetAppVersion()
    {
        return FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion ?? "1.0.0";
    }
}