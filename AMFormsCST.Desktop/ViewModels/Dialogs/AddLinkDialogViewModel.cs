using CommunityToolkit.Mvvm.ComponentModel;

namespace AMFormsCST.Desktop.ViewModels.Dialogs
{
    public partial class AddLinkDialogViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _displayName = string.Empty;

        [ObservableProperty]
        private string _path = string.Empty;
    }
}