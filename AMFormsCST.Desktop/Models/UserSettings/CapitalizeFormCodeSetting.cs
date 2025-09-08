using AMFormsCST.Core.Interfaces.UserSettings;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AMFormsCST.Desktop.Models.UserSettings
{
    public partial class CapitalizeFormCodeSetting : ObservableObject, ISetting
    {
        [ObservableProperty]
        private bool _capitalizeFormCode = true;
    }
    
}
