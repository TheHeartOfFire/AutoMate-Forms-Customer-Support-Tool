using AMFormsCST.Core.Interfaces;
using AMFormsCST.Desktop.ViewModels.Pages;
using Wpf.Ui.Abstractions.Controls;

namespace AMFormsCST.Desktop.Views.Pages;
/// <summary>
/// Interaction logic for DashboardPage.xaml
/// </summary>
public partial class DashboardPage : INavigableView<DashboardViewModel>
{
    public DashboardViewModel ViewModel { get; }
    private readonly ILogService? _logger;

    public DashboardPage(DashboardViewModel viewModel, ILogService? logger = null)
    {
        ViewModel = viewModel;
        _logger = logger;

        DataContext = ViewModel;
        InitializeComponent();

        _logger?.LogInfo("DashboardPage initialized.");
    }
}
