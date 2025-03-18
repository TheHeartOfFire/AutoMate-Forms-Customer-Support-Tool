using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui.Abstractions.Controls;

namespace AMFormsCST.Desktop.ViewModels;
public abstract partial class ViewModel : ObservableObject, INavigationAware
{
    public Task OnNavigatedFromAsync()
    {
        OnNavigatedFrom();

        return Task.CompletedTask;
    }

    private void OnNavigatedFrom()
    {
        throw new NotImplementedException();
    }

    public Task OnNavigatedToAsync()
    {
        OnNavigatedTo();

        return Task.CompletedTask;
    }

    private void OnNavigatedTo()
    {
        throw new NotImplementedException();
    }
}
