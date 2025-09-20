using AMFormsCST.Core.Interfaces;
using AMFormsCST.Desktop.ViewModels.Pages.Links;
using System.Windows;
using Wpf.Ui;
using Wpf.Ui.Abstractions.Controls;
using Wpf.Ui.Controls;

namespace AMFormsCST.Desktop.Views.Pages.Links
{
    /// <summary>
    /// Interaction logic for LinksPage.xaml
    /// </summary>
    public partial class LinksPage : INavigableView<LinksViewModel>
    {
        private readonly INavigationService? _navigationService;
        private readonly ILogService? _logger;

        public LinksViewModel ViewModel { get; }

        public LinksPage(LinksViewModel viewModel, INavigationService navigationService, ILogService? logger = null)
        {
            ViewModel = viewModel;
            DataContext = ViewModel;
            _navigationService = navigationService;
            _logger = logger;
            InitializeComponent();
            Loaded += HandleLoaded;
            Unloaded += HandleUnloaded;
            _logger?.LogInfo("LinksPage initialized.");
        }

        private void HandleLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_navigationService is null) return;
                INavigationView? navigationControl = _navigationService.GetNavigationControl();
                if (
                    navigationControl?.BreadcrumbBar != null
                    && navigationControl.BreadcrumbBar.Visibility != Visibility.Visible
                )
                {
                    navigationControl.BreadcrumbBar.SetCurrentValue(VisibilityProperty, Visibility.Visible);
                }
                _logger?.LogInfo("LinksPage loaded. BreadcrumbBar set to Visible.");
            }
            catch (Exception ex)
            {
                _logger?.LogError("Error in LinksPage HandleLoaded.", ex);
            }
        }

        private void HandleUnloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_navigationService is null) return;
                INavigationView? navigationControl = _navigationService.GetNavigationControl();
                if (
                    navigationControl?.BreadcrumbBar != null
                    && navigationControl.BreadcrumbBar.Visibility != Visibility.Collapsed
                )
                {
                    navigationControl.BreadcrumbBar.SetCurrentValue(VisibilityProperty, Visibility.Collapsed);
                }

                Loaded -= HandleLoaded;
                Unloaded -= HandleUnloaded;
                _logger?.LogInfo("LinksPage unloaded. BreadcrumbBar set to Collapsed.");
            }
            catch (Exception ex)
            {
                _logger?.LogError("Error in LinksPage HandleUnloaded.", ex);
            }
        }
    }
}