using AMFormsCST.Core.Interfaces.Utils;
using AMFormsCST.Core.Utils;
using AMFormsCST.Desktop.ControlsLookup;
using AMFormsCST.Desktop.ViewModels.Pages.Tools;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace AMFormsCST.Desktop.Views.Pages.Tools
{
    /// <summary>
    /// Interaction logic for FormgenUtilitiesPage.xaml
    /// </summary>
    [GalleryPage("Here are a collection of utilities for manipulating .formgen files.", SymbolRegular.DocumentTextToolbox24)]
    public partial class FormgenUtilitiesPage : Page
    {
        private readonly INavigationService _navigationService;
        public FormgenUtilitiesViewModel ViewModel { get; }

        public FormgenUtilitiesPage(
            FormgenUtilitiesViewModel viewModel,
            INavigationService navigationService
        )
        {
            ViewModel = viewModel;
            DataContext = ViewModel;
            _navigationService = navigationService;

            InitializeComponent();

            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            // We use the dispatcher to ensure this code runs after the UI has been fully composed.
            Application.Current.Dispatcher.BeginInvoke(
                System.Windows.Threading.DispatcherPriority.Loaded,
                new Action(() =>
                {
                    if (_navigationService.GetNavigationControl() is not Wpf.Ui.Controls.NavigationView navigationView)
                    {
                        return;
                    }

                    navigationView.IsBackButtonVisible = Wpf.Ui.Controls.NavigationViewBackButtonVisible.Visible;

                    // Traverse the visual tree upwards to find the parent ScrollViewer.
                    // This is more robust than relying on template part names.
                    var parentScrollViewer = FindParent<ScrollViewer>(this);
                    if (parentScrollViewer != null)
                    {
                        // Disable its scrolling to allow the page's internal ScrollViewers to take over.
                        parentScrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
                    }
                })
            );
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            // Traverse the visual tree upwards to find the parent ScrollViewer.
            var parentScrollViewer = FindParent<ScrollViewer>(this);
            if (parentScrollViewer != null)
            {
                // Re-enable its scrolling for other pages that may need it.
                parentScrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            }

            Loaded -= OnLoaded;
            Unloaded -= OnUnloaded;
        }

        /// <summary>
        /// Finds a parent of a given item on the visual tree.
        /// </summary>
        /// <typeparam name="T">The type of the queried item.</typeparam>
        /// <param name="child">A direct or indirect child of the queried item.</param>
        /// <returns>The first parent item that matches the submitted type parameter. 
        /// If not found, a null reference is returned.</returns>
        private static T? FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            if (parentObject == null)
                return null;

            if (parentObject is T parent)
                return parent;
            
            return FindParent<T>(parentObject);
        }

        private void Debug_ToggleImageFoundButtonClick(object sender, RoutedEventArgs e)
        {
            if (ViewModel.IsImageFound.HasValue)
            {
                ViewModel.IsImageFound = !ViewModel.IsImageFound;
            }
        }
    }
}
