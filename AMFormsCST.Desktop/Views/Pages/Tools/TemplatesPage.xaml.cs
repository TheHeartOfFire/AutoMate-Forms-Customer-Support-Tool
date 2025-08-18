using AMFormsCST.Desktop.ControlsLookup;
using AMFormsCST.Desktop.ViewModels;
using AMFormsCST.Desktop.ViewModels.Pages.Tools;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wpf.Ui.Controls;

namespace AMFormsCST.Desktop.Views.Pages.Tools;
/// <summary>
/// Interaction logic for TemplatesPage.xaml
/// </summary>
[GalleryPage("This utility allows you to manage your text templates which references the other parts of this program to prefill data", SymbolRegular.MailTemplate24)]
public partial class TemplatesPage : Page
{
    public TemplatesViewModel ViewModel { get; }
    public TemplatesPage(TemplatesViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = ViewModel;

        InitializeComponent();

        // Ensure the first template is selected if any exist
        if (ViewModel.Templates is { Count: > 0 })
        {
            ViewModel.SelectTemplate(ViewModel.Templates.First());
        }
    }
}
