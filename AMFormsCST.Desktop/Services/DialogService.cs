using AMFormsCST.Desktop.Views.Dialogs;
using Microsoft.Win32;
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
        return (result, dialog.TemplateName, dialog.TemplateDescription, dialog.TemplateContent);
    }

    /// <inheritdoc />
    public bool ShowPageHostDialog(Page contentPage, bool canConfirm = false)
    {
        var dialog = new PageHostDialog(contentPage, canConfirm);
        dialog.ShowDialog();
        return dialog.ConfirmSelected;
    }

    public string? ShowOpenFileDialog(string filter)
    {
        var openFileDialog = new OpenFileDialog
        {
            Filter = filter,
            Title = "Open File"
        };

        if (openFileDialog.ShowDialog() == true)
        {
            return openFileDialog.FileName;
        }

        return null;
    }
}