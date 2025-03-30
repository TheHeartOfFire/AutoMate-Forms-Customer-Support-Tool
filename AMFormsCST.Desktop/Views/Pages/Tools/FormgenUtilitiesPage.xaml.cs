using AMFormsCST.Core.Interfaces.Utils;
using AMFormsCST.Desktop.Controls.FormgenUtilities;
using AMFormsCST.Desktop.ControlsLookup;
using AMFormsCST.Desktop.Extensions;
using AMFormsCST.Desktop.ViewModels.Pages.Tools;
using Microsoft.Win32;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using Wpf.Ui.Abstractions.Controls;
using Wpf.Ui.Controls;
using static AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure.CodeLineSettings;
using static AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure.DotFormgen;
using static System.Environment;

namespace AMFormsCST.Desktop.Views.Pages.Tools
{
    /// <summary>
    /// Interaction logic for FormgenUtilitiesPage.xaml
    /// </summary>
    [GalleryPage("Formgen Utilities", SymbolRegular.DocumentTextToolbox24)]
    public partial class FormgenUtilitiesPage : INavigableView<FormgenUtilitiesViewModel>
    {
        public FormgenUtilitiesViewModel ViewModel { get; }

        public FormgenUtilitiesPage(FormgenUtilitiesViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
#if DEBUG
            ViewModel.ToggleDebugMode(true);
#endif
        }

        private void Debug_ToggleImageFoundButtonClick(object sender, RoutedEventArgs e)
        {
            if(ImageFoundFailedIcon.Visibility == Visibility.Visible)
            {
                ImageFoundFailedIcon.Visibility = Visibility.Collapsed;
                ImageFoundSuccessIcon.Visibility = Visibility.Visible;
            }
            else
            {
                ImageFoundFailedIcon.Visibility = Visibility.Visible;
                ImageFoundSuccessIcon.Visibility = Visibility.Collapsed;
            }
        }

        private void OpenButtonClick(object sender, RoutedEventArgs e)
        {
            var path = GetFolderPath(SpecialFolder.MyComputer);
#if DEBUG
            var entryAssembly = Assembly.GetEntryAssembly()?.Location;
            var dir = Path.GetDirectoryName(entryAssembly);
            if (dir is null) return;
            dir = dir[..dir.LastIndexOf('\\')]
                [..dir.LastIndexOf('\\')]
                [..dir.LastIndexOf('\\')];
            path = dir +
                "\\SampleData\\Formgen Sample Data";

#endif

            var dialog = new OpenFileDialog
            {
                InitialDirectory = path,
                Filter = "Formgen Files (*.formgen)|*.formgen"
            };

            if (dialog.ShowDialog() == false) return; // TODO: this needs to collaps most of the page if no file was found
            ProgressRing.Visibility = Visibility.Visible;
            ViewModel.FormgenUtils.OpenFile(dialog.FileName);
            if (FormgenFileTreeView.ItemsSource is null)
                FormgenFileTreeView.Items.Clear();
            var rootNode = new TreeItemNodeViewModel();
            rootNode.Initialize(ViewModel.FormgenUtils.ParsedFormgenFile!);
            FormgenFileTreeView.ItemsSource = new[] { rootNode };

            RenameFormgenFileTextBox.Text = ViewModel.FormgenUtils.ParsedFormgenFile?.Title;

            var extension = "pdf";

            if(ViewModel.FormgenUtils.ParsedFormgenFile?.FormType is Format.LegacyImpact or Format.LegacyLaser)
                extension = "jpg";

            if (File.Exists(dialog.FileName[..^7] + extension))
            {
                ImageFoundSuccessIcon.Visibility = Visibility.Visible;
                ImageFoundFailedIcon.Visibility = Visibility.Collapsed;

                RenameImageCheckBox.IsChecked = true;
            }
            else
            {
                ImageFoundSuccessIcon.Visibility = Visibility.Collapsed;
                ImageFoundFailedIcon.Visibility = Visibility.Visible;

                RenameImageCheckBox.IsChecked = false;
            }
            RegenerateUUIDTextBox.Text = ViewModel.FormgenUtils.ParsedFormgenFile?.Settings.UUID;

            RenameControlsStackPanel.Visibility = Visibility.Visible;
            UUIDStackPanel.Visibility = Visibility.Visible;
            RenameStackPanel.Visibility = Visibility.Visible;
            RegenerateUUIDButton.Visibility = Visibility.Visible;
            OpenButtonIcon.FontSize = 20;

            ProgressRing.Visibility = Visibility.Collapsed;
        }

        private void RegenerateUUIDButtonClick(object sender, RoutedEventArgs e)
        {

        }


        private void FormgenFileTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (FormgenFileTreeView.SelectedItem is not TreeItemNodeViewModel item) return;

            var properties = item.Properties as FormProperties;

            PropertiesStackPanel.Children.Clear();
            PropertiesStackPanel.Children.Add(properties);
        }
    }
}
