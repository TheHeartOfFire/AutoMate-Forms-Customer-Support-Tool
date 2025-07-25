using AMFormsCST.Desktop.ControlsLookup;
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
/// Interaction logic for CodeSnippetsPage.xaml
/// </summary>
[GalleryPage("Here you can find some common snippets of code for use in FormGen programming", SymbolRegular.Code20)]
public partial class CodeSnippetsPage : Page
{
    public CodeSnippetsViewModel ViewModel { get; }
    public CodeSnippetsPage(CodeSnippetsViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = ViewModel;

        InitializeComponent();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {

    }
}
