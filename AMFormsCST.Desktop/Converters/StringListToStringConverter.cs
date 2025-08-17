using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace AMFormsCST.Desktop.Converters;

public class StringListToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is List<string> list)
        {
            return string.Join(Environment.NewLine, list);
        }
        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string str && !str.Equals(String.Empty))
        {
            return str.Split([Environment.NewLine], StringSplitOptions.None).ToList();
        }
        return new List<string>();
    }
}