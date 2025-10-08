using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace AMFormsCST.Desktop.Helpers;

public static class RichTextBoxHelper
{
    public static readonly DependencyProperty DocumentProperty =
        DependencyProperty.RegisterAttached(
            "Document",
            typeof(FlowDocument),
            typeof(RichTextBoxHelper),
            new FrameworkPropertyMetadata(null, OnDocumentChanged));

    public static FlowDocument GetDocument(DependencyObject dp)
    {
        return (FlowDocument)dp.GetValue(DocumentProperty);
    }

    public static void SetDocument(DependencyObject dp, FlowDocument value)
    {
        dp.SetValue(DocumentProperty, value);
    }

    private static void OnDocumentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is RichTextBox rtb)
        {
            rtb.Document = e.NewValue as FlowDocument ?? new FlowDocument();
        }
    }

    public static readonly DependencyProperty TextChangedCommandProperty =
        DependencyProperty.RegisterAttached("TextChangedCommand", typeof(ICommand), typeof(RichTextBoxHelper), new PropertyMetadata(null, OnTextChangedCommandChanged));

    public static ICommand GetTextChangedCommand(DependencyObject obj)
    {
        return (ICommand)obj.GetValue(TextChangedCommandProperty);
    }

    public static void SetTextChangedCommand(DependencyObject obj, ICommand value)
    {
        obj.SetValue(TextChangedCommandProperty, value);
    }

    private static void OnTextChangedCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is RichTextBox rtb)
        {
            rtb.TextChanged -= RichTextBox_TextChanged; // Unsubscribe to prevent multiple subscriptions
            if (e.NewValue is ICommand)
            {
                rtb.TextChanged += RichTextBox_TextChanged;
            }
        }
    }

    private static void RichTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (sender is RichTextBox rtb)
        {
            ICommand command = GetTextChangedCommand(rtb);
            if (command?.CanExecute(null) == true)
            {
                command.Execute(null);
            }
        }
    }
}
