using System.Globalization;
using System.Windows.Data;

namespace AMFormsCST.Desktop.Converters
{
    public class IntToDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int i)
                return (double)i;
            if (value is double d)
                return d;
            return 0d;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double d)
                return (int)d;
            if (value is int i)
                return i;
            return 0;
        }
    }
}