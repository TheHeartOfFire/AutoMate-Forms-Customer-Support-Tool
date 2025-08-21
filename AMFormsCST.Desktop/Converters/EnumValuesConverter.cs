using System;
using System.Globalization;
using System.Windows.Data;

namespace AMFormsCST.Desktop.Converters;

public class EnumValuesConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Type enumType && enumType.IsEnum)
        {
            return Enum.GetValues(enumType);
        }
        return Array.Empty<object>();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        // Not needed for enum values in ComboBox
        return Binding.DoNothing;
    }
}