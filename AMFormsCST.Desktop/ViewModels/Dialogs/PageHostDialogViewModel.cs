using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;

namespace AMFormsCST.Desktop.ViewModels.Dialogs;

public partial class PageHostDialogViewModel : ObservableObject
{
    [ObservableProperty]
    private string _dialogTitle = "Page Preview";

    [ObservableProperty]
    private bool _confirmSelected = false;

    public object HostedPageViewModel { get; }

    public PageHostDialogViewModel(object hostedPageViewModel)
    {
        HostedPageViewModel = hostedPageViewModel;
    }

    [RelayCommand]
    private void Close(Window window)
    {
        window.Close();
    }

    [RelayCommand]
    private void Confirm(Window window)
    {
        ConfirmSelected = true;
        window.Close();
    }

    // Add other commands here if the dialog needs more interaction (e.g., Save, Apply)
}