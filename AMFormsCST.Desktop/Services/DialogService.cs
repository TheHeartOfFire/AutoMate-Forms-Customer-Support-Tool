using AMFormsCST.Desktop.Views.Dialogs;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;
using AMFormsCST.Core.Interfaces;

namespace AMFormsCST.Desktop.Services;

/// <summary>
/// A service for showing dialogs.
/// </summary>
public class DialogService : IDialogService
{
    private readonly ILogService? _logger;

    public DialogService(ILogService? logger = null)
    {
        _logger = logger;
        _logger?.LogInfo("DialogService initialized.");
    }

    /// <inheritdoc />
    public MessageBoxResult ShowMessageBox(
        string messageBoxText,
        string caption,
        MessageBoxButton button,
        MessageBoxImage icon
    )
    {
        _logger?.LogInfo($"ShowMessageBox called: '{caption}' - '{messageBoxText}'");
        return MessageBox.Show(messageBoxText, caption, button, icon);
    }

    /// <inheritdoc />
    public (bool? DialogResult, string TemplateName, string TemplateDescription, string TemplateContent) ShowNewTemplateDialog(string? name = null, string? description = null, string? content = null)
    {
        _logger?.LogInfo("ShowNewTemplateDialog called.");
        var dialog = new NewTemplateDialog(name, description, content);
        var result = dialog.ShowDialog();
        _logger?.LogInfo($"NewTemplateDialog result: {result}");
        return (result, dialog.TemplateName, dialog.TemplateDescription, dialog.TemplateContent);
    }

    /// <inheritdoc />
    public bool ShowPageHostDialog(Page contentPage, string title = "Page Prview", bool canConfirm = false)
    {
        _logger?.LogInfo($"ShowPageHostDialog called: Title='{title}', CanConfirm={canConfirm}");
        var dialog = new PageHostDialog(contentPage, title, canConfirm);
        dialog.ShowDialog();
        _logger?.LogInfo($"PageHostDialog closed: ConfirmSelected={dialog.ConfirmSelected}");
        return dialog.ConfirmSelected;
    }

    public string? ShowOpenFileDialog(string filter)
    {
        _logger?.LogInfo($"ShowOpenFileDialog called: Filter='{filter}'");
        var openFileDialog = new OpenFileDialog
        {
            Filter = filter,
            Title = "Open File"
        };

        if (openFileDialog.ShowDialog() == true)
        {
            _logger?.LogInfo($"File selected: {openFileDialog.FileName}");
            return openFileDialog.FileName;
        }

        _logger?.LogInfo("No file selected.");
        return null;
    }
    public string? ShowOpenFileDialog(string filter, string defaultDirectory)
    {
        _logger?.LogInfo($"ShowOpenFileDialog called: Filter='{filter}', DefaultDirectory='{defaultDirectory}'");
        var openFileDialog = new OpenFileDialog
        {
            Filter = filter,
            Title = "Open File",
            DefaultDirectory = defaultDirectory
        };

        if (openFileDialog.ShowDialog() == true)
        {
            _logger?.LogInfo($"File selected: {openFileDialog.FileName}");
            return openFileDialog.FileName;
        }

        _logger?.LogInfo("No file selected.");
        return null;
    }

    public IDialogService.DialogResult ShowDialog(string title, Page content)
    {
        _logger?.LogInfo($"ShowDialog called: Title='{title}'");
        var dialog = new PageHostDialog(content, title, true);
        dialog.ShowDialog();
        _logger?.LogInfo($"Dialog closed: ConfirmSelected={dialog.ConfirmSelected}");
        return dialog.ConfirmSelected ? IDialogService.DialogResult.Primary : IDialogService.DialogResult.Cancel;
    }
}