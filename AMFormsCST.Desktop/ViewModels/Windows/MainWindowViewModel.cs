using AMFormsCST.Core.Interfaces;
using AMFormsCST.Desktop.Models.UserSettings;
using AMFormsCST.Desktop.Views.Pages;
using AMFormsCST.Desktop.Views.Pages.Tools;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace AMFormsCST.Desktop.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    private readonly ISupportTool _supportTool;

    [ObservableProperty]
    private string _applicationTitle = "Case Management Tool";

    [ObservableProperty]
    private bool _isWindowTopmost;

    [ObservableProperty]
    private bool _isLightTheme;

    [ObservableProperty]
    private ObservableCollection<object> _navigationItems = [];

    [ObservableProperty]
    private ObservableCollection<object> _navigationFooter = [];

    public MainWindowViewModel(ISupportTool supportTool)
    {
        _supportTool = supportTool;

        // Initialize and subscribe to setting changes
        var aotSetting = _supportTool.Settings.UiSettings.Settings.OfType<AlwaysOnTopSetting>().FirstOrDefault();
        if (aotSetting != null)
        {
            IsWindowTopmost = aotSetting.IsEnabled;
            aotSetting.PropertyChanged += OnUiSettingsChanged;
        }

        var themeSetting = _supportTool.Settings.UiSettings.Settings.OfType<ThemeSetting>().FirstOrDefault();
        if (themeSetting != null)
        {
            IsLightTheme = themeSetting.Theme == ApplicationTheme.Light;
            themeSetting.PropertyChanged += OnUiSettingsChanged;
        }
    }

    private void OnUiSettingsChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(AlwaysOnTopSetting.IsEnabled) && sender is AlwaysOnTopSetting aotSetting)
        {
            IsWindowTopmost = aotSetting.IsEnabled;
        }
        else if (e.PropertyName == nameof(ThemeSetting.Theme) && sender is ThemeSetting themeSetting)
        {
            IsLightTheme = themeSetting.Theme == ApplicationTheme.Light;
        }
    }

    partial void OnIsWindowTopmostChanged(bool value)
    {
        var aotSetting = _supportTool.Settings.UiSettings.Settings.OfType<AlwaysOnTopSetting>().FirstOrDefault();
        if (aotSetting != null && aotSetting.IsEnabled != value)
        {
            aotSetting.IsEnabled = value;
            _supportTool.SaveAllSettings();
        }
    }

    partial void OnIsLightThemeChanged(bool value)
    {
        var themeSetting = _supportTool.Settings.UiSettings.Settings.OfType<ThemeSetting>().FirstOrDefault();
        if (themeSetting != null)
        {
            var newTheme = value ? ApplicationTheme.Light : ApplicationTheme.Dark;
            if (themeSetting.Theme != newTheme)
            {
                themeSetting.Theme = newTheme;
                ApplicationThemeManager.Apply(newTheme);
                _supportTool.SaveAllSettings();
            }
        }
    }
}