using AMFormsCST.Core.Interfaces.Utils;
using AMFormsCST.Core.Utils;
using AMFormsCST.Desktop.ControlsLookup;
using AMFormsCST.Desktop.ViewModels.Pages.Tools;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace AMFormsCST.Desktop.Views.Pages.Tools
{
    /// <summary>
    /// Interaction logic for FormgenUtilitiesPage.xaml
    /// </summary>
    [GalleryPage("Here are a collection of utilities for manipulating .formgen files.", SymbolRegular.DocumentTextToolbox24)]
    public partial class FormgenUtilitiesPage : Page
    {
        public FormgenUtilitiesViewModel ViewModel { get; }

        public FormgenUtilitiesPage(FormgenUtilitiesViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = ViewModel;

            InitializeComponent();
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
