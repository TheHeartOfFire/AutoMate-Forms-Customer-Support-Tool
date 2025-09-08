using System.Globalization;
using System.Windows.Data;

namespace AMFormsCST.Desktop.Converters;

public class EnumValuesConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => 
        (value is Type enumType && enumType.IsEnum) ?  Enum.GetValues(enumType) : Array.Empty<object>();
    

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => Binding.DoNothing;
}