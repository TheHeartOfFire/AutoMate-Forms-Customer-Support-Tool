using System;
using System.Globalization;
using System.Windows.Data;

namespace AMFormsCST.Desktop.Converters;

public class EnumToBooleanConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null || parameter == null)
            return false;

        string enumValue = value.ToString();
        string targetValue = parameter.ToString();

        return enumValue.Equals(targetValue, StringComparison.OrdinalIgnoreCase);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is false || parameter == null)
            return Binding.DoNothing;

        return Enum.Parse(targetType, parameter.ToString());
    }
}