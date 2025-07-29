using AMFormsCST.Desktop.ViewModels.Dialogs;
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
using Wpf.Ui.Controls;

namespace AMFormsCST.Desktop.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for PageHostDialog.xaml
    /// </summary>
    public partial class PageHostDialog : FluentWindow
    {
        public PageHostDialog(Page contentPage)
        {
            InitializeComponent();

            DialogPageFrame.Navigate(contentPage);

            this.DataContext = new PageHostDialogViewModel(contentPage.DataContext);
        }
        public bool ConfirmSelected => ((PageHostDialogViewModel)DataContext).ConfirmSelected;

        private void CustomTitleBarArea_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
    }
}
