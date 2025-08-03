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
public class PromptDataProperties : IFormgenFileProperties
{
    public PromptDataProperties() { }
    public PromptDataProperties(PromptData promptData)
    {
        Settings = new PromptDataSettings(promptData.Settings);
        Message = promptData.Message ?? string.Empty;
        Choices = promptData.Choices;
    }

    public IFormgenFileSettings Settings { get; set; } = new PromptDataSettings();
    public string Message { get; set; } = string.Empty;
    public List<string> Choices { get; set; } = [];

    public UIElement GetUIElements() => BasicStats.GetSettingsAndPropertiesUIElements(this);
}
