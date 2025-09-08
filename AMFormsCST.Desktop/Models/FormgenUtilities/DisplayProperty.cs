using System.ComponentModel;
using System.Reflection;
using AMFormsCST.Core.Interfaces;

namespace AMFormsCST.Desktop.Models.FormgenUtilities;

public class DisplayProperty : INotifyPropertyChanged
{
    public string Name { get; }
    public Type Type { get; }
    public bool IsReadOnly { get; }
    private object? _value;
    private readonly object? _source;
    private readonly PropertyInfo? _propertyInfo;
    private readonly ILogService? _logger;

    public event PropertyChangedEventHandler? PropertyChanged;

    public DisplayProperty(object source, PropertyInfo propertyInfo, bool isReadOnly = false, ILogService? logger = null)
    {
        Name = propertyInfo.Name;
        Type = propertyInfo.PropertyType;
        IsReadOnly = isReadOnly || !propertyInfo.CanWrite;
        _source = source;
        _propertyInfo = propertyInfo;
        _value = propertyInfo.GetValue(source);
        _logger = logger;
        _logger?.LogInfo($"DisplayProperty created for '{Name}' (Type: {Type.Name}, ReadOnly: {IsReadOnly})");
    }

    public DisplayProperty(string name, object? value, Type type, bool isReadOnly = true, ILogService? logger = null)
    {
        Name = name;
        Type = type;
        IsReadOnly = isReadOnly;
        _value = value;
        _source = null;
        _propertyInfo = null;
        _logger = logger;
        _logger?.LogInfo($"DisplayProperty summary created for '{Name}' (Type: {Type.Name}, ReadOnly: {IsReadOnly})");
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
                      _logger?.LogInfo($"DisplayProperty '{Name}' value changed: {value}");
                  }
                  PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
              }
          }
      }
  }