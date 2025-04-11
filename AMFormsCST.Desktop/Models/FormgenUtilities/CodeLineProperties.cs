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
    public IFormgenFileSettings Settings { get; set; } = new CodeLineSettings();
    public string Expression { get; set; } = string.Empty;
    public PromptDataProperties PromptData { get; set; } = new();

    public UIElement GetUIElements() => BasicStats.GetSettingsAndPropertiesUIElements(this);
}
