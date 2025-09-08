using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Data;

namespace AMFormsCST.Desktop.Converters;
public class RadioButtonCommandParameterConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values == null || values.Length < 2 ||
            values[0] == DependencyProperty.UnsetValue ||
            values[1] == DependencyProperty.UnsetValue)
        {
            return null; 
        }

        object dataItem = values[0];
        string commandParameterPath = values[1] as string;

        if (dataItem == null || string.IsNullOrEmpty(commandParameterPath))
        {
            return null;
        }

        if (commandParameterPath.Equals("DataContext", StringComparison.OrdinalIgnoreCase))
        {
            return dataItem;
        }
        else
        {
            PropertyInfo prop = dataItem.GetType().GetProperty(commandParameterPath);
            if (prop != null)
            {
                return prop.GetValue(dataItem);
            }
        }
        return null; 
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
    
}

