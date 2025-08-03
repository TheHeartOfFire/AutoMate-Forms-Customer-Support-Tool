using AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure;
using AMFormsCST.Desktop.Interfaces;
using System.Windows;

namespace AMFormsCST.Desktop.Models.FormgenUtilities;

/// <summary>
/// A wrapper class that prepares a CodeLine object's data to be displayed as UI properties.
/// </summary>
public class CodeLineProperties : IFormgenFileProperties
{
    /// <summary>
    /// The settings associated with the CodeLine (e.g., Order, Type, Variable).
    /// </summary>
    public IFormgenFileSettings Settings { get; set; }

    /// <summary>
    /// The nested prompt data associated with the CodeLine, wrapped for UI display.
    /// </summary>
    public PromptDataProperties? PromptData { get; set; }

    public CodeLineProperties(CodeLine codeLine)
    {
        Settings = new CodeLineSettings(codeLine.Settings);
        if (codeLine.PromptData is not null)
        {
            PromptData = new PromptDataProperties(codeLine.PromptData);
        }
    }

    /// <summary>
    /// Generates the UI elements for this CodeLine's properties.
    /// </summary>
    public UIElement GetUIElements()
    {
        // This static method from BasicStats will build the UI for us.
        return BasicStats.GetSettingsAndPropertiesUIElements(this);
    }
}
