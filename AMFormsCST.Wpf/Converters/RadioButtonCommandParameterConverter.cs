using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace AMFormsCST.Desktop.Converters;
public class RadioButtonCommandParameterConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        // Ensure you have enough values and they are not DependencyProperty.UnsetValue
        if (values == null || values.Length < 2 ||
            values[0] == DependencyProperty.UnsetValue ||
            values[1] == DependencyProperty.UnsetValue)
        {
            return null; // Or handle as an error condition
        }

        // 'values[0]' will be the actual data item (e.g., your Company object)
        object dataItem = values[0];
        // 'values[1]' will be the string path (e.g., "DataContext" or "Id")
        string commandParameterPath = values[1] as string;

        if (dataItem == null || string.IsNullOrEmpty(commandParameterPath))
        {
            return null;
        }

        if (commandParameterPath.Equals("DataContext", StringComparison.OrdinalIgnoreCase))
        {
            // If the path is "DataContext", return the entire item
            return dataItem;
        }
        else
        {
            // Otherwise, use reflection to get the specified property's value
            PropertyInfo prop = dataItem.GetType().GetProperty(commandParameterPath);
            if (prop != null)
            {
                return prop.GetValue(dataItem);
            }
        }
        return null; // Return null if the property is not found or path is invalid
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

