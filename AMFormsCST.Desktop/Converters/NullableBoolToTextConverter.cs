using System;
using System.Globalization;
using System.Windows.Data;

namespace AMFormsCST.Desktop.Converters;

public class NullableBoolToTextConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        // The switch expression correctly handles all cases for a nullable bool (true, false, null).
        // The previous 'if' check was incorrect for null values.
        return (bool?)value switch
        {
            true => "Image Found",
            false => "Image Not Found",
            null => "Image status not checked"
        };
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}