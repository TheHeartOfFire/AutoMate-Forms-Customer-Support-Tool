using AMFormsCST.Core.Interfaces;
using System.Windows;
using System.Windows.Controls;

namespace AMFormsCST.Desktop.Models.FormgenUtilities;

public class DisplayPropertyTemplateSelector : DataTemplateSelector
{
    public DataTemplate? StringTemplate { get; set; }
    public DataTemplate? IntTemplate { get; set; }
    public DataTemplate? BoolTemplate { get; set; }
    public DataTemplate? EnumTemplate { get; set; }
    public DataTemplate? ListStringTemplate { get; set; }
    public DataTemplate? ReadOnlyTemplate { get; set; }
    public ILogService? Logger { get; set; }

    public override DataTemplate? SelectTemplate(object item, DependencyObject container)
    {
        if (item is not DisplayProperty prop)
        {
            Logger?.LogWarning("DisplayPropertyTemplateSelector: Item is not a DisplayProperty.");
            return base.SelectTemplate(item, container);
        }

        if (prop.IsReadOnly)
        {
            Logger?.LogDebug($"DisplayPropertyTemplateSelector: '{prop.Name}' is read-only.");
            return ReadOnlyTemplate;
        }

        if (prop.Type == typeof(string))
        {
            Logger?.LogDebug($"DisplayPropertyTemplateSelector: '{prop.Name}' uses StringTemplate.");
            return StringTemplate;
        }
        if (prop.Type == typeof(int))
        {
            Logger?.LogDebug($"DisplayPropertyTemplateSelector: '{prop.Name}' uses IntTemplate.");
            return IntTemplate;
        }
        if (prop.Type == typeof(bool))
        {
            Logger?.LogDebug($"DisplayPropertyTemplateSelector: '{prop.Name}' uses BoolTemplate.");
            return BoolTemplate;
        }
        if (prop.Type.IsEnum)
        {
            Logger?.LogDebug($"DisplayPropertyTemplateSelector: '{prop.Name}' uses EnumTemplate.");
            return EnumTemplate;
        }
        if (prop.Type == typeof(List<string>))
        {
            Logger?.LogDebug($"DisplayPropertyTemplateSelector: '{prop.Name}' uses ListStringTemplate.");
            return ListStringTemplate;
        }

        Logger?.LogDebug($"DisplayPropertyTemplateSelector: '{prop.Name}' uses ReadOnlyTemplate (fallback).");
        return ReadOnlyTemplate;
    }
}