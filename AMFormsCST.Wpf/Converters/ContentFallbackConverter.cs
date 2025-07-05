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
public class ContentFallbackConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        // values[0] will be the data item (e.g., your Company object)
        // values[1] will be the ContentBindingPath string
        // values[2] will be the ContentFallbackValue string

        if (values == null || values.Length < 3 ||
            values[0] == DependencyProperty.UnsetValue ||
            values[1] == DependencyProperty.UnsetValue ||
            values[2] == DependencyProperty.UnsetValue)
        {
            return null; // Or some default if inputs are not ready
        }

        object dataItem = values[0];
        string bindingPath = values[1] as string;
        string fallbackValue = values[2] as string;

        if (dataItem == null || string.IsNullOrEmpty(bindingPath))
        {
            return fallbackValue; // If no item or no path, use fallback
        }

        // Get the value based on the binding path
        object contentValue = null;
        if (bindingPath.Equals(".", StringComparison.OrdinalIgnoreCase)) // If path is ".", return the whole item
        {
            contentValue = dataItem;
        }
        else
        {
            PropertyInfo prop = dataItem.GetType().GetProperty(bindingPath);
            if (prop != null)
            {
                contentValue = prop.GetValue(dataItem);
            }
        }

        // If contentValue is null or empty, use the fallback
        if (contentValue == null || (contentValue is string s && string.IsNullOrEmpty(s)))
        {
            return fallbackValue;
        }

        return contentValue; // Otherwise, use the actual content value
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException(); // One-way binding
    }
}
