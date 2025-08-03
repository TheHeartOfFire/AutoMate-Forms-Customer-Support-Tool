using System.Windows;
using System.Windows.Controls;

namespace AMFormsCST.Desktop.Services
{
    public interface IDialogService
    {
        MessageBoxResult ShowMessageBox(string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon);

        (bool? DialogResult, string TemplateName, string TemplateDescription, string TemplateContent) ShowNewTemplateDialog(string? name = null, string? description = null, string? content = null);

        bool ShowPageHostDialog(Page contentPage, bool canConfirm = false);
    }
}