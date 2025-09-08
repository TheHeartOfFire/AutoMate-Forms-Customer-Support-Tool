using System.Windows;
using System.Windows.Controls;

namespace AMFormsCST.Desktop.Controls;

public partial class CopyButton : UserControl
{
    public CopyButton()
    {
        InitializeComponent();
    }

    public static readonly DependencyProperty TextToCopyProperty =
        DependencyProperty.Register(
            nameof(TextToCopy),
            typeof(object),
            typeof(CopyButton),
            new PropertyMetadata(null));

    public object TextToCopy
    {
        get => GetValue(TextToCopyProperty);
        set => SetValue(TextToCopyProperty, value);
    }

    private void CopyButton_OnClick(object sender, RoutedEventArgs e)
    {
        var text = TextToCopy?.ToString();

        if (string.IsNullOrEmpty(text))
        {
            return;
        }

        try
        {
            Clipboard.SetText(text);
        }
        catch
        {
        }
    }
}