using AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure;
using AMFormsCST.Desktop.Interfaces;
using System.Windows;

namespace AMFormsCST.Desktop.Models.FormgenUtilities;

public class CodeLineStats : IFormgenFileProperties
{
    public IFormgenFileSettings Settings { get; set; }
    public PromptDataProperties? PromptData { get; set; }

    public CodeLineStats(CodeLine codeLine)
    {
        Settings = new CodeLineSettings(codeLine.Settings);
        if (codeLine.PromptData is not null)
        {
            PromptData = new PromptDataProperties(codeLine.PromptData);
        }
    }

    public UIElement GetUIElements()
    {
        return BasicStats.GetSettingsAndPropertiesUIElements(this);
    }
}
