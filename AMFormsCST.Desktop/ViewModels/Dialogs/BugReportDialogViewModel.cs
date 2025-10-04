using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;

namespace AMFormsCST.Desktop.ViewModels.Dialogs
{
    public partial class BugReportDialogViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _title = string.Empty;

        [ObservableProperty]
        private string _description = string.Empty;

        [RelayCommand]
        private void Submit(Window window)
        {
            if (string.IsNullOrWhiteSpace(Title))
            {
                MessageBox.Show("Title cannot be empty.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(Description))
            {
                MessageBox.Show("Description cannot be empty.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            window.DialogResult = true;
            window.Close();
        }
    }
}