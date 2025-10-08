using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace AMFormsCST.Desktop.Services;

public interface IDialogService
{
    public enum DialogResult
    {
        Primary,
        Secondary,
        Cancel
    }

    MessageBoxResult ShowMessageBox(string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon);

    (bool? DialogResult, string TemplateName, string TemplateDescription, FlowDocument TemplateContent) ShowNewTemplateDialog(string? name = null, string? description = null, FlowDocument? content = null);

    bool ShowPageHostDialog(Page contentPage, string title = "Page Prview", bool canConfirm = false);

    string? ShowOpenFileDialog(string filter);
    string? ShowOpenFileDialog(string filter, string defaultDirectory);
    DialogResult ShowDialog(string title, Page content);
    public (bool Result, string Title, string Description) ShowBugReportDialog();
}