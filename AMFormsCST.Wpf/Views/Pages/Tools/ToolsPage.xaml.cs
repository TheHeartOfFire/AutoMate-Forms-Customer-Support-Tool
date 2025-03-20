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
using AMFormsCST.Desktop.Effects;

namespace AMFormsCST.Desktop.Views.Pages.Tools
{
    /// <summary>
    /// Interaction logic for ToolsPage.xaml
    /// </summary>
    public partial class ToolsPage : INavigableView<ToolsViewModel>
    {
        private readonly INavigationService _navigationService;

        private SnowflakeEffect? _snowflake;
        public ToolsViewModel ViewModel { get; }

        public ToolsPage(ToolsViewModel viewModel, INavigationService navigationService)
        {
            ViewModel = viewModel;
            DataContext = this;
            _navigationService = navigationService;

            InitializeComponent();
            Loaded += HandleLoaded;
            Unloaded += HandleUnloaded;
        }

        private void HandleLoaded(object sender, RoutedEventArgs e)
        {
            INavigationView? navigationControl = _navigationService.GetNavigationControl();
            if (
                navigationControl?.BreadcrumbBar != null
                && navigationControl.BreadcrumbBar.Visibility != Visibility.Collapsed
            )
            {
                navigationControl.BreadcrumbBar.SetCurrentValue(VisibilityProperty, Visibility.Collapsed);
            }
            INavigationViewItem? selectedItem = navigationControl?.SelectedItem;
            if (selectedItem != null)
            {
                string? newTitle = selectedItem.Content?.ToString();
                if (MainTitle.Text != newTitle)
                {
                    MainTitle.SetCurrentValue(System.Windows.Controls.TextBlock.TextProperty, newTitle);
                }

                if (selectedItem.Icon is SymbolIcon selectedIcon && MainSymbolIcon.Symbol != selectedIcon.Symbol)
                {
                    MainSymbolIcon.SetCurrentValue(SymbolIcon.SymbolProperty, selectedIcon.Symbol);
                }
            }

            _snowflake ??= new(MainCanvas);
            _snowflake.Start();
        }

        private void HandleUnloaded(object sender, RoutedEventArgs e)
        {
            INavigationView? navigationControl = _navigationService.GetNavigationControl();
            if (
                navigationControl?.BreadcrumbBar != null
                && navigationControl.BreadcrumbBar.Visibility != Visibility.Visible
            )
            {
                navigationControl.BreadcrumbBar.SetCurrentValue(VisibilityProperty, Visibility.Visible);
            }

            _snowflake?.Stop();
            _snowflake = null;
            Loaded -= HandleLoaded;
            Unloaded -= HandleUnloaded;
        }
    }
}
