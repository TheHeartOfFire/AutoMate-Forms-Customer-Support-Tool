using AMFormsCST.Core.Interfaces;
using AMFormsCST.Desktop.ControlsLookup;
using AMFormsCST.Desktop.ViewModels.Pages.Tools;
using AMFormsCST.Desktop.ViewModels.Pages.Tools.Templates;
using System.Linq;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace AMFormsCST.Desktop.Views.Pages.Tools;
/// <summary>
/// Interaction logic for TemplatesPage.xaml
/// </summary>
[GalleryPage("This utility allows you to manage your text templates which references the other parts of this program to prefill data", SymbolRegular.MailTemplate24)]
public partial class TemplatesPage : Page
{
    public TemplatesViewModel ViewModel { get; }
    private readonly ILogService? _logger;

    public TemplatesPage(TemplatesViewModel viewModel, ILogService? logger = null)
    {
        ViewModel = viewModel;
        _logger = logger;
        DataContext = ViewModel;

        InitializeComponent();

        if (ViewModel.TemplatesView.Cast<object>().Any())
        {
            ViewModel.SelectTemplate(ViewModel.TemplatesView.Cast<object>().First() as TemplateItemViewModel);
            _logger?.LogInfo($"TemplatesPage initialized. First template selected.");
        }
        else
        {
            _logger?.LogInfo("TemplatesPage initialized. No templates available.");
        }
    }
}
