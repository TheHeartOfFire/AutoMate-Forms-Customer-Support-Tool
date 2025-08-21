using System;
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

    public override DataTemplate? SelectTemplate(object item, DependencyObject container)
    {
        if (item is not DisplayProperty prop)
            return base.SelectTemplate(item, container);

        if (prop.IsReadOnly)
            return ReadOnlyTemplate;

        if (prop.Type == typeof(string))
            return StringTemplate;
        if (prop.Type == typeof(int))
            return IntTemplate;
        if (prop.Type == typeof(bool))
            return BoolTemplate;
        if (prop.Type.IsEnum)
            return EnumTemplate;
        if (prop.Type == typeof(List<string>))
            return ListStringTemplate;

        return ReadOnlyTemplate;
    }
}