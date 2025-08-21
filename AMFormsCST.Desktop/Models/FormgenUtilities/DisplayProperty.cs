using AMFormsCST.Desktop.Converters;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Wpf.Ui.Controls;
using System.Reflection;

namespace AMFormsCST.Desktop.Models.FormgenUtilities;

public class DisplayProperty : INotifyPropertyChanged
{
    public string Name { get; }
    public Type Type { get; }
    public bool IsReadOnly { get; }
    private object? _value;
    private readonly object? _source;
    private readonly PropertyInfo? _propertyInfo;

    public event PropertyChangedEventHandler? PropertyChanged;

    // For editable properties
    public DisplayProperty(object source, PropertyInfo propertyInfo, bool isReadOnly = false)
    {
        Name = propertyInfo.Name;
        Type = propertyInfo.PropertyType;
        IsReadOnly = isReadOnly || !propertyInfo.CanWrite;
        _source = source;
        _propertyInfo = propertyInfo;
        _value = propertyInfo.GetValue(source);
    }

    // For summary/statistic properties
    public DisplayProperty(string name, object? value, Type type, bool isReadOnly = true)
    {
        Name = name;
        Type = type;
        IsReadOnly = isReadOnly;
        _value = value;
        _source = null;
        _propertyInfo = null;
    }

    public object? Value
    {
        get => _value;
        set
        {
            if (!Equals(_value, value))
            {
                _value = value;
                // Use the setter method if available
                if (!IsReadOnly && _source != null && _propertyInfo != null)
                {
                    var setMethod = _propertyInfo.GetSetMethod();
                    if (setMethod != null)
                    {
                        setMethod.Invoke(_source, new[] { value });
                    }
                    else
                    {
                        _propertyInfo.SetValue(_source, value);
                    }
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
            }
        }
    }
}