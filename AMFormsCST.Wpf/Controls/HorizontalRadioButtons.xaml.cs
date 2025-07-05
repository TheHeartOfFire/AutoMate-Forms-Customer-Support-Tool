using System;
using System.Collections;
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

namespace AMFormsCST.Desktop.Controls;
/// <summary>
/// Interaction logic for HorizontalRadioButtons.xaml
/// </summary>
public partial class HorizontalRadioButtons : UserControl
{
    public HorizontalRadioButtons()
    {
        InitializeComponent();
    }

    // Dependency Property for ItemsSource
    public static readonly DependencyProperty ItemsSourceProperty =
        DependencyProperty.Register(
            "ItemsSource",
            typeof(IEnumerable), // Or a more specific collection type if you know it
            typeof(HorizontalRadioButtons),
            new PropertyMetadata(null));

    public IEnumerable ItemsSource
    {
        get { return (IEnumerable)GetValue(ItemsSourceProperty); }
        set { SetValue(ItemsSourceProperty, value); }
    }

    // Dependency Property for the Command
    public static readonly DependencyProperty RadioButtonCommandProperty =
        DependencyProperty.Register(
            "RadioButtonCommand",
            typeof(ICommand),
            typeof(HorizontalRadioButtons),
            new PropertyMetadata(null));

    public ICommand RadioButtonCommand
    {
        get { return (ICommand)GetValue(RadioButtonCommandProperty); }
        set { SetValue(RadioButtonCommandProperty, value); }
    }

    // Dependency Property for the CommandParameter Path (e.g., "Id", "DataContext")
    // This allows you to specify which property of the bound item to pass as the command parameter
    public static readonly DependencyProperty CommandParameterPathProperty =
        DependencyProperty.Register(
            "CommandParameterPath",
            typeof(string),
            typeof(HorizontalRadioButtons),
            new PropertyMetadata("DataContext")); // Default to "DataContext" if not specified

    public string CommandParameterPath
    {
        get { return (string)GetValue(CommandParameterPathProperty); }
        set { SetValue(CommandParameterPathProperty, value); }
    }
    public static readonly DependencyProperty ContentBindingPathProperty =
            DependencyProperty.Register(
                "ContentBindingPath",
                typeof(string),
                typeof(HorizontalRadioButtons),
                new PropertyMetadata(string.Empty)); // Default to empty string

    public string ContentBindingPath
    {
        get { return (string)GetValue(ContentBindingPathProperty); }
        set { SetValue(ContentBindingPathProperty, value); }
    }

    // NEW: Dependency Property for the content's FallbackValue
    public static readonly DependencyProperty ContentFallbackValueProperty =
        DependencyProperty.Register(
            "ContentFallbackValue",
            typeof(string),
            typeof(HorizontalRadioButtons),
            new PropertyMetadata(string.Empty)); // Default to empty string

    public string ContentFallbackValue
    {
        get { return (string)GetValue(ContentFallbackValueProperty); }
        set { SetValue(ContentFallbackValueProperty, value); }
    }
}

