using AMFormsCST.Core.Interfaces.UserSettings;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AMFormsCST.Desktop.Models.UserSettings;

public partial class AlwaysOnTopSetting : ObservableObject, ISetting
{
    [ObservableProperty]
    private bool _isEnabled;
}