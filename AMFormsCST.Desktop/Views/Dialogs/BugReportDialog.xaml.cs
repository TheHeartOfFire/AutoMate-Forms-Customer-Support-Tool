using AMFormsCST.Desktop.ViewModels.Dialogs;
using System.Windows.Controls;
using System.Windows.Input;
using Wpf.Ui.Controls;

namespace AMFormsCST.Desktop.Views.Dialogs
{
    public partial class BugReportDialog : FluentWindow
    {
        public BugReportDialog()
        {
            DataContext = new BugReportDialogViewModel();
            InitializeComponent();
        }
        private void CustomTitleBarArea_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
    }
}