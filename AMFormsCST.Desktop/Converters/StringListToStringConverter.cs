using System.Globalization;
using System.Windows.Data;

namespace AMFormsCST.Desktop.Converters;

public class StringListToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => 
        value is List<string> list ? string.Join(Environment.NewLine, list) : string.Empty;
    

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        value is string str && !str.Equals(String.Empty) ? str.Split([Environment.NewLine], StringSplitOptions.None).ToList() : new List<string>();
    
}