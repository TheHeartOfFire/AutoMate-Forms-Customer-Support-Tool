using AMFormsCST.Core.Interfaces;
using AMFormsCST.Desktop.ViewModels.Pages;
using System.Windows.Controls;
using Wpf.Ui.Abstractions.Controls;

namespace AMFormsCST.Desktop.Views.Pages;

public partial class SettingsPage : Page, INavigableView<SettingsViewModel>
{
    public SettingsViewModel ViewModel { get; }
    private readonly ILogService? _logger;

    public SettingsPage(SettingsViewModel viewModel, ILogService? logger = null)
    {
        ViewModel = viewModel;
        _logger = logger;
        DataContext = ViewModel;

        InitializeComponent();
        _logger?.LogInfo("SettingsPage initialized.");
    }
}