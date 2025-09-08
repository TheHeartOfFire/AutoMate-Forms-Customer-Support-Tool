using AMFormsCST.Core.Interfaces.UserSettings;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AMFormsCST.Desktop.Models.UserSettings
{
    public partial class NewTemplateSetting : ObservableObject, ISetting
    {
        [ObservableProperty]
        private bool _selectNewTemplate = true;
    }
    
}
