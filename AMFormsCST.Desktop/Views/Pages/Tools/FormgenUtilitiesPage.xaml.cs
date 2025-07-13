using AMFormsCST.Core.Interfaces.Utils;
using AMFormsCST.Core.Utils;
using AMFormsCST.Desktop.ControlsLookup;
using AMFormsCST.Desktop.Extensions;
using AMFormsCST.Desktop.Models.FormgenUtilities;
using AMFormsCST.Desktop.ViewModels.Pages.Tools;
using Microsoft.Win32;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
            var dialog = FormgenUtilitiesViewModel.OpenWindow;

            if (dialog.ShowDialog() == false) return; 
            ProgressRing.Visibility = Visibility.Visible;

            ViewModel.FormgenUtils.OpenFile(dialog.FileName);

            LoadTreeView();
            ToggleVisibility(dialog.FileName[..^7]);

            RenameFormgenFileTextBox.Text = ViewModel.FormgenUtils.ParsedFormgenFile?.Title;
            ViewModel.Uuid = ViewModel.FormgenUtils?.ParsedFormgenFile?.Settings.UUID ?? string.Empty;
            OpenButtonIcon.FontSize = 20;
            PropertiesStackPanel.Children.Clear();

            ProgressRing.Visibility = Visibility.Collapsed;
        }

        private void RegenerateUUIDButtonClick(object sender, RoutedEventArgs e)
        {
            if(FormgenUtilitiesViewModel.Confirm(
                "Are you sure you want to change the UUID for this form. Formgen will no longer recognize this as the same form.",
                "Update UUID") is System.Windows.MessageBoxResult.No) return;

            ViewModel.RegenerateUUID();
        }


        private void FormgenFileTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (FormgenFileTreeView.SelectedItem is not TreeItemNodeViewModel item) return;

            PropertiesStackPanel.Children.Clear();

            PropertiesStackPanel.Children.Add(item.Properties.GetUIElements());

        }

        private void LoadTreeView()
        {
            if (FormgenFileTreeView.ItemsSource is null)
                FormgenFileTreeView.Items.Clear();

            var rootNode = new TreeItemNodeViewModel();
            rootNode.Initialize(ViewModel.FormgenUtils.ParsedFormgenFile!);
            FormgenFileTreeView.ItemsSource = new[] { rootNode };
        }

        private void ToggleVisibility(string path)
        {   
            var extension = "pdf";

            if (ViewModel.FormgenUtils.ParsedFormgenFile?.FormType
                is Format.LegacyImpact or Format.LegacyLaser)
                extension = "jpg";

            if (File.Exists(path + extension))
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
            FileViewer.Visibility = Visibility.Visible;

            RenameControlsStackPanel.Visibility = Visibility.Visible;
            UUIDStackPanel.Visibility = Visibility.Visible;
            RenameStackPanel.Visibility = Visibility.Visible;
            RegenerateUUIDButton.Visibility = Visibility.Visible;
            SaveButton.Visibility = Visibility.Visible;

        }
        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {
            var dialog = FormgenUtilitiesViewModel.SaveWindow;

            if (dialog.ShowDialog() == false) return;

            FormgenUtilitiesViewModel.Confirm($"You are about to save your changes to {RenameFormgenFileTextBox.Text}. " +
                $"A backup will be created of your original file in case you change your mind. Do you wish to proceed?",
               "Save");

            ViewModel.SaveFormgenFile(dialog.FolderName, 
                !RenameFormgenFileTextBox.Text.Equals(ViewModel.FormgenUtils?.ParsedFormgenFile?.Title) ? RenameFormgenFileTextBox.Text : null,
                RenameImageCheckBox.IsChecked is true && ImageFoundSuccessIcon.Visibility is Visibility.Visible);
        }

        private void RestoreButtonClick(object sender, RoutedEventArgs e)
        {
            var dialog = FormgenUtilitiesViewModel.SaveWindow;

            if (dialog.ShowDialog() == false) return;

            ViewModel.SaveFormgenFile(dialog.FolderName);
        }
    }
}
