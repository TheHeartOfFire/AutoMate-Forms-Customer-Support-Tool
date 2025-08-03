using AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure;
using AMFormsCST.Desktop.Interfaces;
using System.Windows;
using static AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure.FormField;

namespace AMFormsCST.Desktop.Models.FormgenUtilities;

public class FieldStats : IFormgenFileProperties
{
    public IFormgenFileSettings Settings { get; set; }
    public string? Expression { get; set; }
    public string? SampleData { get; set; }
    public FormatOption FormattingOption { get; set; }

    public FieldStats(FormField field)
    {
        // Assuming a FieldSettings wrapper class exists, following the established pattern.
        Settings = new FieldSettings(field.Settings);
        Expression = field.Expression;
        SampleData = field.SampleData;
        FormattingOption = field.FormattingOption;
    }

    public UIElement GetUIElements()
    {
        return BasicStats.GetSettingsAndPropertiesUIElements(this);
    }
}
