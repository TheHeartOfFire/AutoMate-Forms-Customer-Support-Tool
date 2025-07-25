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
using AMFormsCST.Desktop.Views.Pages.Tools;

namespace AMFormsCST.Desktop.ViewModels;
public partial class MainWindowViewModel : ViewModel
{
    [ObservableProperty]
    private string _applicationTitle = string.Empty;

    [ObservableProperty]
    private ObservableCollection<object> _menuItems =
    [
        new NavigationViewItem("Home", SymbolRegular.Home24, typeof(DashboardPage)),
        new NavigationViewItem()
        {
            Content = "Tools",
            Icon = new SymbolIcon { Symbol = SymbolRegular.DesktopToolbox20 },
            TargetPageType = typeof(ToolsPage),
            MenuItemsSource = new object[]
            {
                new NavigationViewItem("Formgen Utilities", SymbolRegular.DocumentTextToolbox24, typeof(FormgenUtilitiesPage)),
                new NavigationViewItem("Form Name Generator", SymbolRegular.TextT12, typeof(FormNameGeneratorPage)),
                new NavigationViewItem("Code Snippets", SymbolRegular.Code20, typeof(CodeSnippetsPage)),
                //new NavigationViewItem("Templates", SymbolRegular.Color24, typeof(ColorsPage)),
            },
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
