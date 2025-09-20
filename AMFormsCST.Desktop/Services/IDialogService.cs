using System.Windows;
using System.Windows.Controls;

namespace AMFormsCST.Desktop.Services
{
    public interface IDialogService
    {
        public enum DialogResult
        {
            Primary,
            Secondary,
            Cancel
        }

        MessageBoxResult ShowMessageBox(string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon);

        (bool? DialogResult, string TemplateName, string TemplateDescription, string TemplateContent) ShowNewTemplateDialog(string? name = null, string? description = null, string? content = null);

        bool ShowPageHostDialog(Page contentPage, string title = "Page Prview", bool canConfirm = false);

        string? ShowOpenFileDialog(string filter);
        string? ShowOpenFileDialog(string filter, string defaultDirectory);
        DialogResult ShowDialog(string title, Page content);
    }
}