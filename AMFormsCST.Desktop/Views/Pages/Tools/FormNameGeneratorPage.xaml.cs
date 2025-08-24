using AMFormsCST.Core.Interfaces;
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
/// Interaction logic for FormNameGeneratorPage.xaml
/// </summary>
[GalleryPage("This utility allows you to generate form names that adhere to current naming conventions.", SymbolRegular.TextT12)]
public partial class FormNameGeneratorPage : Page
{
    public FormNameGeneratorViewModel ViewModel { get; }
    private readonly ILogService? _logger;

    public FormNameGeneratorPage(FormNameGeneratorViewModel viewModel, ILogService? logger = null)
    {
        ViewModel = viewModel;
        _logger = logger;
        DataContext = ViewModel;

        InitializeComponent();
        _logger?.LogInfo("FormNameGeneratorPage initialized.");
    }
}
