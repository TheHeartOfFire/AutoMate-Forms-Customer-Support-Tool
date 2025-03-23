using AMFormsCST.Core.Interfaces.Utils;
using AMFormsCST.Desktop.ControlsLookup;
using AMFormsCST.Desktop.ViewModels.Pages.Tools;
using Microsoft.Win32;
using System.IO;
using System.Reflection;
using System.Windows;
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
            DebugStackPanel.Visibility = Visibility.Visible;
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
            PopulateTreeView(ViewModel.FormgenUtils);
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

        private void PopulateTreeView(IFormgenUtils utils)
        {
            FormgenFileTreeView.Items.Clear();

            var header = new TreeViewItem() { Header = utils.ParsedFormgenFile?.Title };

            var codelines = new TreeViewItem() { Header = "Code Lines" };

            var init = new TreeViewItem() { Header = "Init" };
            var prompts = new TreeViewItem() { Header = "Prompts" };
            var postPrompts = new TreeViewItem() { Header = "Post Prompts" };

            foreach (var line in utils.ParsedFormgenFile!.CodeLines)
            {
                switch(line.Settings?.Type)
                {
                    case CodeType.INIT:
                        init.Items.Add(new TreeViewItem() { 
                            Header = $"{line.Settings.Order.ToString() ?? string.Empty}: " +
                            $"{line.Settings.Variable ?? string.Empty }" });
                        break;
                    case CodeType.PROMPT:
                        prompts.Items.Add(new TreeViewItem() { 
                            Header = $"{line.Settings.Order.ToString() ?? string.Empty}: " +
                            $"{line.Settings.Variable ?? string.Empty}"
                        });
                        break;
                    case CodeType.POST:
                        postPrompts.Items.Add(new TreeViewItem() { 
                            Header = $"{line.Settings.Order.ToString() ?? string.Empty}: " +
                            $"{line.Settings.Variable ?? string.Empty}"
                        });
                        break;
                }

            }
            init.IsExpanded = true;
            prompts.IsExpanded = true;
            postPrompts.IsExpanded = true;

            codelines.Items.Add(init);
            codelines.Items.Add(prompts);
            codelines.Items.Add(postPrompts);
            codelines.IsExpanded = true;

            var fields = new TreeViewItem() { Header = "Fields" };

            foreach(var page in utils.ParsedFormgenFile.Pages)
                foreach (var field in page.Fields)
                    fields.Items.Add(new TreeViewItem() { Header = field.Expression });

            fields.IsExpanded = true;

            header.Items.Add(codelines);
            header.Items.Add(fields);
            header.IsExpanded = true;

            FormgenFileTreeView.Items.Add(header);
        }

        private void RegenerateUUIDButtonClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
