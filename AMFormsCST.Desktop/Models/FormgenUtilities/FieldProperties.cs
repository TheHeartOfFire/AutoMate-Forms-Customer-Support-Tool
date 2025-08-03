using AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure;
using AMFormsCST.Desktop.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure.FormField;

namespace AMFormsCST.Desktop.Models.FormgenUtilities;
public class FieldProperties : IFormgenFileProperties
{
    public FieldProperties(FormField field)
    {
        Settings = new FieldSettings(field.Settings);
        Expression = field.Expression ?? string.Empty;
        FormattingOption = field.FormattingOption;
        SampleData = field.SampleData ?? string.Empty;
    }

    public string Expression { get; set; } = string.Empty;
    public FormatOption FormattingOption { get; set; }
    public string SampleData { get; set; } = string.Empty;
    public IFormgenFileSettings Settings { get; set; } = new FieldSettings();

    public UIElement GetUIElements() => BasicStats.GetSettingsAndPropertiesUIElements(this);
}
