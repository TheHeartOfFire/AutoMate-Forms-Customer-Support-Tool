using System.Globalization;
using System.Windows.Data;
using Wpf.Ui.Controls;

namespace AMFormsCST.Desktop.Converters;

public class NullableBoolToSymbolConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
    value is not bool ? SymbolRegular.Question24 : (bool?)value switch
    {
        true => SymbolRegular.Checkmark24,
        false => SymbolRegular.Dismiss24,
        null => SymbolRegular.Question24
    };
    

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}