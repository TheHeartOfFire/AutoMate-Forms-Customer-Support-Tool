using System.Windows;
using System.Windows.Controls;
using MessageBoxResult = System.Windows.MessageBoxResult;

namespace AMFormsCST.Desktop.Services;

public class DesignTimeDialogService : IDialogService
{
    public MessageBoxResult ShowMessageBox(string messageBoxText, string caption, System.Windows.MessageBoxButton button, MessageBoxImage icon) 
    => MessageBoxResult.OK;
    

    public (bool? DialogResult, string TemplateName, string TemplateDescription, string TemplateContent) ShowNewTemplateDialog(string? name = null, string? description = null, string? content = null)
    => (true, "DesignTimeTemplate", "A template for design time", "Content");
    

    public bool ShowPageHostDialog(Page contentPage, string title = "Page Preview", bool canConfirm = false)
    => true;
    

    public string? ShowOpenFileDialog(string filter)
    => "C:\\temp\\design_time_file.formgen";
    

    public string? ShowOpenFileDialog(string filter, string defaultDirectory)
    => "C:\\temp\\design_time_file.formgen";

    public IDialogService.DialogResult ShowDialog(string title, Page content)
    {
        throw new NotImplementedException();
    }
}