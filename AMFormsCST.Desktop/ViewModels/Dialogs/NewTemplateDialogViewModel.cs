using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AMFormsCST.Desktop.ViewModels.Dialogs;

public partial class NewTemplateDialogViewModel : ObservableObject
{
    [ObservableProperty]
    private string _templateName = string.Empty;

    [ObservableProperty]
    private string _templateDescription = string.Empty;

    [ObservableProperty]
    private string _templateContent = string.Empty;

    // Constructor can be empty for default values, or initialize them based on context if needed.
    public NewTemplateDialogViewModel()
    {
    }

    [RelayCommand]
    private void Create(Window window)
    {
        // Perform validation
        if (string.IsNullOrWhiteSpace(TemplateName))
        {
            MessageBox.Show("Template Name cannot be empty.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        // Set DialogResult to true to indicate success
        window.DialogResult = true;
        window.Close();
    }
}

