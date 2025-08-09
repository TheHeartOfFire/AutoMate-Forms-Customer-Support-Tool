using AMFormsCST.Core.Interfaces.UserSettings;
using CommunityToolkit.Mvvm.ComponentModel;
using Wpf.Ui.Appearance;

namespace AMFormsCST.Desktop.Models.UserSettings;

public partial class ThemeSetting : ObservableObject, ISetting
{
    [ObservableProperty]
    private ApplicationTheme _theme = ApplicationTheme.Dark;
}