using AMFormsCST.Desktop.ViewModels.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Wpf.Ui.Controls;

namespace AMFormsCST.Desktop.Views.Dialogs;

/// <summary>
/// Interaction logic for AddTemplateDialogue.xaml
/// </summary>
public partial class NewTemplateDialog : FluentWindow
{
    public NewTemplateDialog(string? Name = null, string? Description = null, string? Content = null)
    {
        InitializeComponent();
        
        this.DataContext = new NewTemplateDialogViewModel
        {
            TemplateName = Name ?? string.Empty,
            TemplateDescription = Description ?? string.Empty,
            TemplateContent = Content ?? string.Empty
        }; // Set the DataContext

        if (!(Name + Description + Content).Equals(string.Empty)) 
            ConfirmButton.Content = "Update";

    }

    // Optional: Properties to easily access the values from the ViewModel
    public string TemplateName => ((NewTemplateDialogViewModel)DataContext).TemplateName;
    public string TemplateDescription => ((NewTemplateDialogViewModel)DataContext).TemplateDescription;
    public string TemplateContent => ((NewTemplateDialogViewModel)DataContext).TemplateContent;

    private void CustomTitleBarArea_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ButtonState == MouseButtonState.Pressed)
        {
            this.DragMove();
        }
    }
}

