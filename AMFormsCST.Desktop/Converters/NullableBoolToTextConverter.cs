using System.Globalization;
using System.Windows.Data;

namespace AMFormsCST.Desktop.Converters;

public class NullableBoolToTextConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
     value is not bool ? "Image status not checked" : (bool?)value switch
        {
            true => "Image Found",
            false => "Image Not Found",
            null => "Image status not checked"
        };
    

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}