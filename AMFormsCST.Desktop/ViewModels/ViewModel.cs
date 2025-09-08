using CommunityToolkit.Mvvm.ComponentModel;
using Wpf.Ui.Abstractions.Controls;

namespace AMFormsCST.Desktop.ViewModels;
public abstract partial class ViewModel : ObservableObject, INavigationAware
{
    private void OnNavigatedTo() { }
    private void OnNavigatedFrom() { }
    public Task OnNavigatedToAsync()
    {
        OnNavigatedTo();

        return Task.CompletedTask;
    }
    public Task OnNavigatedFromAsync()
    {
        OnNavigatedFrom();

        return Task.CompletedTask;
    }
}
