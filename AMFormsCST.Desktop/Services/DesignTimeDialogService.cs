using AMFormsCST.Desktop.Interfaces;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Controls;
using MessageBoxResult = System.Windows.MessageBoxResult;

namespace AMFormsCST.Desktop.Services;

public class DesignTimeDialogService : IDialogService
{
    public MessageBoxResult ShowMessageBox(string messageBoxText, string caption, System.Windows.MessageBoxButton button, MessageBoxImage icon)
    {
        return MessageBoxResult.OK;
    }

    public (bool? DialogResult, string TemplateName, string TemplateDescription, string TemplateContent) ShowNewTemplateDialog(string? name = null, string? description = null, string? content = null)
    {
        return (true, "DesignTimeTemplate", "A template for design time", "Content");
    }

    public bool ShowPageHostDialog(Page contentPage, string title = "Page Preview", bool canConfirm = false)
    {
        return true;
    }

    public string? ShowOpenFileDialog(string filter)
    {
        return "C:\\temp\\design_time_file.formgen";
    }

    public string? ShowOpenFileDialog(string filter, string defaultDirectory)
    {
        return "C:\\temp\\design_time_file.formgen";
    }
}