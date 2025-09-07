using AMFormsCST.Core.Interfaces;
using AMFormsCST.Desktop.Interfaces;
using AMFormsCST.Desktop.ViewModels;
using AMFormsCST.Desktop.Views.Pages;
using AMFormsCST.Desktop.Views.Pages.Tools;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace AMFormsCST.Desktop;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : IWindow
{
    // This is as good of a place as any to put my 2.0 release TODO list:
    // TODO: Readme
    // TODO: Remove unnecessary comments and usings
    // TODO: Finish Velopack installation and publish v2.0
    public MainWindowViewModel ViewModel { get; }
    private readonly ILogService _logger;

    private bool _isUserClosedPane;
    private bool _isPaneOpenedOrClosedFromCode;

    public MainWindow(
        MainWindowViewModel viewModel,
        INavigationService navigationService,
        IServiceProvider serviceProvider,
        ISnackbarService snackbarService,
        IContentDialogService contentDialogService,
        ILogService logger
    )
    {
        Wpf.Ui.Appearance.SystemThemeWatcher.Watch(this);

        ViewModel = viewModel;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _logger.LogInfo("MainWindow initialized.");
        DataContext = this;

        ViewModel.NavigationItems =
        [
            new NavigationViewItem("Dashboard", SymbolRegular.Home24, typeof(DashboardPage)),
            new NavigationViewItem("Tools", SymbolRegular.Wrench24, typeof(ToolsPage))
        ];

        ViewModel.NavigationFooter =
        [
            new NavigationViewItem("Settings", SymbolRegular.Settings24, typeof(SettingsPage))
        ];

        InitializeComponent();

        snackbarService.SetSnackbarPresenter(SnackbarPresenter);
        navigationService.SetNavigationControl(NavigationView);
        contentDialogService.SetDialogHost(RootContentDialog);
    }

    private void OnNavigationSelectionChanged(object sender, RoutedEventArgs e)
    {
        if (sender is not NavigationView navigationView)
        {
            _logger.LogWarning("Navigation selection changed but sender is not NavigationView.");
            return;
        }

        NavigationView.SetCurrentValue(
            NavigationView.HeaderVisibilityProperty,
            navigationView.SelectedItem?.TargetPageType != typeof(DashboardPage)
                ? Visibility.Visible
                : Visibility.Collapsed
        );
        _logger.LogInfo($"Navigation selection changed: {navigationView.SelectedItem?.TargetPageType}");
    }

    private void MainWindow_OnSizeChanged(object sender, SizeChangedEventArgs e)
    {
        if (_isUserClosedPane)
        {
            return;
        }

        _isPaneOpenedOrClosedFromCode = true;
        NavigationView.SetCurrentValue(NavigationView.IsPaneOpenProperty, e.NewSize.Width > 1200);
        _isPaneOpenedOrClosedFromCode = false;
        _logger.LogDebug($"MainWindow size changed: {e.NewSize}");
    }

    private void NavigationView_OnPaneOpened(NavigationView sender, RoutedEventArgs args)
    {
        if (_isPaneOpenedOrClosedFromCode)
        {
            return;
        }

        _isUserClosedPane = false;
        _logger.LogInfo("Navigation pane opened.");
    }

    private void NavigationView_OnPaneClosed(NavigationView sender, RoutedEventArgs args)
    {
        if (_isPaneOpenedOrClosedFromCode)
        {
            return;
        }

        _isUserClosedPane = true;
        _logger.LogInfo("Navigation pane closed.");
    }
}