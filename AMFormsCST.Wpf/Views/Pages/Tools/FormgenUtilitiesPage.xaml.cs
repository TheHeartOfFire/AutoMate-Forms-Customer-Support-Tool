using AMFormsCST.Core.Interfaces.Utils;
using AMFormsCST.Desktop.ControlsLookup;
using AMFormsCST.Desktop.ViewModels.Pages.Tools;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
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
using Wpf.Ui;
using Wpf.Ui.Abstractions.Controls;
using Wpf.Ui.Controls;
using static AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure.CodeLineSettings;

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
            var dialog = new OpenFileDialog();

            if (dialog.ShowDialog() == false) return;

            ViewModel.FormgenUtils.OpenFile(dialog.FileName);
            PopulateTreeView(ViewModel.FormgenUtils);

        }

        private void PopulateTreeView(IFormgenUtils utils)
        {
            FormgenFileTreeView.Items.Clear();

            var header = new Wpf.Ui.Controls.TreeViewItem() { Header = utils.FileName };

            var codelines = new Wpf.Ui.Controls.TreeViewItem() { Header = "Code Lines" };

            var init = new Wpf.Ui.Controls.TreeViewItem() { Header = "Init" };
            var prompts = new Wpf.Ui.Controls.TreeViewItem() { Header = "Prompts" };
            var postPrompts = new Wpf.Ui.Controls.TreeViewItem() { Header = "Post Prompts" };

            foreach (var line in utils.ParsedFormgenFile!.CodeLines)
            {
                switch(line.Settings?.Type)
                {
                    case CodeType.INIT:
                        init.Items.Add(new Wpf.Ui.Controls.TreeViewItem() { 
                            Header = line.PromptData?.Message ?? string.Empty });
                        break;
                    case CodeType.PROMPT:
                        prompts.Items.Add(new Wpf.Ui.Controls.TreeViewItem() { 
                            Header = line.PromptData?.Message ?? string.Empty });
                        break;
                    case CodeType.POST:
                        postPrompts.Items.Add(new Wpf.Ui.Controls.TreeViewItem() { 
                            Header = line.PromptData?.Message ?? string.Empty });
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

            var fields = new Wpf.Ui.Controls.TreeViewItem() { Header = "Fields" };

            foreach(var page in utils.ParsedFormgenFile.Pages)
                foreach (var field in page.Fields)
                    fields.Items.Add(new Wpf.Ui.Controls.TreeViewItem() { Header = field.Expression });

            fields.IsExpanded = true;

            header.Items.Add(codelines);
            header.Items.Add(fields);
            header.IsExpanded = true;

            FormgenFileTreeView.Items.Add(header);
        }
    }
}
