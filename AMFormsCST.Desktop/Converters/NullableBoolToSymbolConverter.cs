using System;
using System.Globalization;
using System.Windows.Data;
using Wpf.Ui.Controls;

namespace AMFormsCST.Desktop.Converters;

public class NullableBoolToSymbolConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not bool)
        {
            return SymbolRegular.Question24;
        }

        return (bool?)value switch
        {
            true => SymbolRegular.Checkmark24,
            false => SymbolRegular.Dismiss24,
            null => SymbolRegular.Question24
        };
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}