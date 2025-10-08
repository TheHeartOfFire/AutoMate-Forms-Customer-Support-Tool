using AMFormsCST.Core.Types.BestPractices.TextTemplates.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using System.Windows.Documents;
using static AMFormsCST.Core.Types.BestPractices.TextTemplates.Models.TextTemplate;

namespace AMFormsCST.Desktop.ViewModels.Dialogs;

public partial class NewTemplateDialogViewModel : ObservableObject
{
    [ObservableProperty]
    private string _templateName = string.Empty;

    [ObservableProperty]
    private string _templateDescription = string.Empty;

    [ObservableProperty]
    private FlowDocument _templateContent = new();

    [ObservableProperty]
    private TemplateType _templateType;

    public NewTemplateDialogViewModel()
    {
    }

    [RelayCommand]
    private void Create(Window window)
    {
        if (string.IsNullOrWhiteSpace(TemplateName))
        {
            MessageBox.Show("Template Name cannot be empty.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        window.DialogResult = true;
        window.Close();
    }

    public IEnumerable<TextTemplate.TemplateType> TemplateTypes =>
        Enum.GetValues(typeof(TextTemplate.TemplateType)) as TextTemplate.TemplateType[];
}

