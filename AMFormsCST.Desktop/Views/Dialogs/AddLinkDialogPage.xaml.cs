using AMFormsCST.Desktop.ViewModels.Dialogs;
using System.Windows.Controls;

namespace AMFormsCST.Desktop.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for AddLinkDialogPage.xaml
    /// </summary>
    public partial class AddLinkDialogPage : Page
    {
        public AddLinkDialogViewModel ViewModel { get; }

        public AddLinkDialogPage(AddLinkDialogViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = ViewModel;
            InitializeComponent();
        }
    }
}