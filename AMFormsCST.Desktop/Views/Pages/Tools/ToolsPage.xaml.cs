using AMFormsCST.Core.Interfaces;
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
using Wpf.Ui.Abstractions.Controls;
using Wpf.Ui.Controls;
using Wpf.Ui;
using AMFormsCST.Desktop.ViewModels.Pages.Tools;

namespace AMFormsCST.Desktop.Views.Pages.Tools
{
    /// <summary>
    /// Interaction logic for ToolsPage.xaml
    /// </summary>
    public partial class ToolsPage : INavigableView<ToolsViewModel>
    {
        private readonly INavigationService _navigationService;
        private readonly ILogService? _logger;

        public ToolsViewModel ViewModel { get; }

        public ToolsPage(ToolsViewModel viewModel, INavigationService navigationService, ILogService? logger = null)
        {
            ViewModel = viewModel;
            DataContext = this;
            _navigationService = navigationService;
            _logger = logger;
            InitializeComponent();
            Loaded += HandleLoaded;
            Unloaded += HandleUnloaded;
            _logger?.LogInfo("ToolsPage initialized.");
        }

        private void HandleLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                INavigationView? navigationControl = _navigationService.GetNavigationControl();
                if (
                    navigationControl?.BreadcrumbBar != null
                    && navigationControl.BreadcrumbBar.Visibility != Visibility.Visible
                )
                {
                    navigationControl.BreadcrumbBar.SetCurrentValue(VisibilityProperty, Visibility.Visible);
                }
                _logger?.LogInfo("ToolsPage loaded. BreadcrumbBar set to Visible.");
            }
            catch (Exception ex)
            {
                _logger?.LogError("Error in ToolsPage HandleLoaded.", ex);
            }
        }

        private void HandleUnloaded(object sender, RoutedEventArgs e)
        {
            try
            {
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
                _logger?.LogInfo("ToolsPage unloaded. BreadcrumbBar set to Collapsed.");
            }
            catch (Exception ex)
            {
                _logger?.LogError("Error in ToolsPage HandleUnloaded.", ex);
            }
        }
    }
}
