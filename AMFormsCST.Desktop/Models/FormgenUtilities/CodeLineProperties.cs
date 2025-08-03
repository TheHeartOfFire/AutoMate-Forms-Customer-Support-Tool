using AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure;
using AMFormsCST.Desktop.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AMFormsCST.Desktop.Models.FormgenUtilities;
public class CodeLineProperties : IFormgenFileProperties
{
    public CodeLineProperties() { }
    public CodeLineProperties(CodeLine codeLine)
    {
        Settings = new CodeLineSettings(codeLine.Settings);
        Expression = codeLine.Expression ?? string.Empty;
        if (codeLine.PromptData is not null)
            PromptData = new(codeLine.PromptData);
    }

    public IFormgenFileSettings Settings { get; set; } = new CodeLineSettings();
    public string Expression { get; set; } = string.Empty;
    public PromptDataProperties PromptData { get; set; } = new();

    public UIElement GetUIElements() => BasicStats.GetSettingsAndPropertiesUIElements(this);
}
