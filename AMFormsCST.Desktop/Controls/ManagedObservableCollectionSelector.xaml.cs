using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

    public static readonly DependencyProperty ItemsSourceProperty =
        DependencyProperty.Register(
            "ItemsSource",
            typeof(IEnumerable), 
            typeof(ManagedObservableCollectionSelector),
            new PropertyMetadata(null));

    public IEnumerable ItemsSource
    {
        get { return (IEnumerable)GetValue(ItemsSourceProperty); }
        set { SetValue(ItemsSourceProperty, value); }
    }

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

    public static readonly DependencyProperty CommandParameterPathProperty =
        DependencyProperty.Register(
            "CommandParameterPath",
            typeof(string),
            typeof(ManagedObservableCollectionSelector),
            new PropertyMetadata("DataContext")); 

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
                new PropertyMetadata(string.Empty)); 

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
            new PropertyMetadata(string.Empty)); 

    public string ContentFallbackValue
    {
        get { return (string)GetValue(ContentFallbackValueProperty); }
        set { SetValue(ContentFallbackValueProperty, value); }
    }

    public static readonly DependencyProperty GroupNameProperty =
        DependencyProperty.Register(
            nameof(GroupName),      
            typeof(string),         
            typeof(ManagedObservableCollectionSelector), 
            new PropertyMetadata(string.Empty)); 


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