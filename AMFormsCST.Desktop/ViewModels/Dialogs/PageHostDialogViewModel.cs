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

    [ObservableProperty]
    private Visibility _canConfirm = Visibility.Collapsed;
    public object HostedPageViewModel { get; }

    public PageHostDialogViewModel(object hostedPageViewModel, string title = "Page Preview", bool canConfirm = false)
    {
        HostedPageViewModel = hostedPageViewModel;
        _canConfirm = canConfirm ? Visibility.Visible : Visibility.Collapsed;
        _dialogTitle = title;
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
}