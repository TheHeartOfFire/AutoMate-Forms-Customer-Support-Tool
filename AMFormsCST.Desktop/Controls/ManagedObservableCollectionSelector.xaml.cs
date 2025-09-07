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
/// Interaction logic for ManagedObservableCollectionSelector.xaml
/// </summary>
public partial class ManagedObservableCollectionSelector : UserControl
{
    public ManagedObservableCollectionSelector()
    {
        InitializeComponent();
    }

    // Dependency Property for ItemsSource
    public static readonly DependencyProperty ItemsSourceProperty =
        DependencyProperty.Register(
            "ItemsSource",
            typeof(IEnumerable), // Or a more specific collection type if you know it
            typeof(ManagedObservableCollectionSelector),
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
            typeof(ManagedObservableCollectionSelector),
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
            typeof(ManagedObservableCollectionSelector),
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
                typeof(ManagedObservableCollectionSelector),
                new PropertyMetadata(string.Empty)); // Default to empty string

    public string ContentBindingPath
    {
        get { return (string)GetValue(ContentBindingPathProperty); }
        set { SetValue(ContentBindingPathProperty, value); }
    }

    public static readonly DependencyProperty ContentFallbackValueProperty =
        DependencyProperty.Register(
            "ContentFallbackValue",
            typeof(string),
            typeof(ManagedObservableCollectionSelector),
            new PropertyMetadata(string.Empty)); // Default to empty string

    public string ContentFallbackValue
    {
        get { return (string)GetValue(ContentFallbackValueProperty); }
        set { SetValue(ContentFallbackValueProperty, value); }
    }

    public static readonly DependencyProperty GroupNameProperty =
        DependencyProperty.Register(
            nameof(GroupName),      // The name of the property
            typeof(string),         // The type of the property
            typeof(ManagedObservableCollectionSelector), // The type of the owner class
            new PropertyMetadata(string.Empty)); // Default value, or you can use null


    public string GroupName
    {
        get { return (string)GetValue(GroupNameProperty); }
        set { SetValue(GroupNameProperty, value); }
    }

    public static readonly DependencyProperty RefreshTriggerProperty =
        DependencyProperty.Register(
            "RefreshTrigger",
            typeof(object),
            typeof(ManagedObservableCollectionSelector),
            new PropertyMetadata(null));

    public object RefreshTrigger
    {
        get { return GetValue(RefreshTriggerProperty); }
        set { SetValue(RefreshTriggerProperty, value); }
    }
    public static readonly DependencyProperty DeleteCommandProperty =
    DependencyProperty.Register(nameof(DeleteCommand), typeof(ICommand), typeof(ManagedObservableCollectionSelector), new PropertyMetadata(null));

    public ICommand DeleteCommand
    {
        get { return (ICommand)GetValue(DeleteCommandProperty); }
        set { SetValue(DeleteCommandProperty, value); }
    }
}