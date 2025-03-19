using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows;
using Wpf.Ui.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using AMFormsCST.Desktop.Views.Pages;

namespace AMFormsCST.Desktop.ViewModels;
public partial class MainWindowViewModel : ViewModel
{
    [ObservableProperty]
    private string _applicationTitle = "AutoMate Forms Customer Support Tool";

    [ObservableProperty]
    private ObservableCollection<object> _menuItems =
    [
        new NavigationViewItem("Home", SymbolRegular.Home24, typeof(DashboardPage)),
        new NavigationViewItem()
        {
            Content = "Tools",
            Icon = new SymbolIcon { Symbol = SymbolRegular.DesktopToolbox20 },
            //MenuItemsSource = new object[]
            //{
            //    new NavigationViewItem("Formgen Utilities", SymbolRegular.TextFont24, typeof(TypographyPage)),
            //    new NavigationViewItem("Form Name Generator", SymbolRegular.Diversity24, typeof(IconsPage)),
            //    new NavigationViewItem("Templates", SymbolRegular.Color24, typeof(ColorsPage)),
            //},
        }
    ];

    [ObservableProperty]
    private ObservableCollection<object> _footerMenuItems =
    [
        //new NavigationViewItem("Settings", SymbolRegular.Settings24, typeof(SettingsPage)),
    ];

    [ObservableProperty]
    private ObservableCollection<Wpf.Ui.Controls.MenuItem> _trayMenuItems =
    [
        new Wpf.Ui.Controls.MenuItem { Header = "Home", Tag = "tray_home" },
        new Wpf.Ui.Controls.MenuItem { Header = "Close", Tag = "tray_close" },
    ];
}
