using System.Windows;
using System.Windows.Documents;
using Wpf.Ui.Controls;

namespace AMFormsCST.Desktop.Helpers
{
    public static class RichTextBoxHelper
    {
        public static readonly DependencyProperty DocumentProperty =
            DependencyProperty.RegisterAttached(
                "Document",
                typeof(FlowDocument),
                typeof(RichTextBoxHelper),
                new FrameworkPropertyMetadata(
                    null,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnDocumentChanged));

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
    }
}