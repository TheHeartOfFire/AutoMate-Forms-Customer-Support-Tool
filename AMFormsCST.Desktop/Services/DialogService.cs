using AMFormsCST.Desktop.Views.Dialogs;
using System.Windows;
using System.Windows.Controls;

namespace AMFormsCST.Desktop.Services;

/// <summary>
/// A service for showing dialogs.
/// </summary>
public class DialogService : IDialogService
{
    /// <inheritdoc />
    public MessageBoxResult ShowMessageBox(
        string messageBoxText,
        string caption,
        MessageBoxButton button,
        MessageBoxImage icon
    )
    {
        return MessageBox.Show(messageBoxText, caption, button, icon);
    }

    /// <inheritdoc />
    public (bool? DialogResult, string TemplateName, string TemplateDescription, string TemplateContent) ShowNewTemplateDialog(string? name = null, string? description = null, string? content = null)
    {
        var dialog = new NewTemplateDialog(name, description, content);
        var result = dialog.ShowDialog();

        if (result == true)
        {
            return (true, dialog.TemplateName, dialog.TemplateDescription, dialog.TemplateContent);
        }

        return (result, string.Empty, string.Empty, string.Empty);
    }

    /// <inheritdoc />
    public bool ShowPageHostDialog(Page contentPage, bool canConfirm = false)
    {
        var dialog = new PageHostDialog(contentPage, canConfirm);
        dialog.ShowDialog();
        return dialog.ConfirmSelected;
    }
}