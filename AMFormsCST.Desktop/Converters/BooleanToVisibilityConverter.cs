using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace AMFormsCST.Desktop.Converters;

[ValueConversion(typeof(bool?), typeof(Visibility))]
public class BooleanToVisibilityConverter : IValueConverter
{
    /// <summary>
    /// Gets or sets the visibility to use for a false value. Defaults to Collapsed.
    /// </summary>
    public Visibility FalseVisibility { get; set; } = Visibility.Collapsed;

    /// <summary>
    /// Gets or sets a value indicating whether the conversion should be inverted.
    /// </summary>
    public bool Invert { get; set; }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        bool isTrue = false;
        if (value is bool b)
        {
            isTrue = b;
        }

        if (Invert)
        {
            isTrue = !isTrue;
        }

        return isTrue ? Visibility.Visible : FalseVisibility;
    }

    public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException("BooleanToVisibilityConverter can only be used for one-way conversion.");
    }
}
