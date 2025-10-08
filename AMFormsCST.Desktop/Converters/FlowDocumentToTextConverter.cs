using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Documents;

namespace AMFormsCST.Desktop.Converters
{
    public class FlowDocumentToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is FlowDocument doc)
            {
                return new TextRange(doc.ContentStart, doc.ContentEnd).Text;
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var flowDocument = new FlowDocument();
            if (value is string text)
            {
                flowDocument.Blocks.Add(new Paragraph(new Run(text)));
            }
            return flowDocument;
        }
    }
}