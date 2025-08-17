using AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure;
using AMFormsCST.Desktop.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace AMFormsCST.Desktop.Models.FormgenUtilities;

/// <summary>
/// A wrapper class that prepares a CodeLine object's data to be displayed as UI properties.
/// </summary>
public partial class CodeLineProperties : ObservableObject, IFormgenFileProperties
{
    private readonly CodeLine _coreCodeLine;

    /// <summary>
    /// The settings associated with the CodeLine (e.g., Order, Type, Variable).
    /// </summary>
    public IFormgenFileSettings? Settings { get; set; }

    /// <summary>
    /// The nested prompt data associated with the CodeLine, wrapped for UI display.
    /// </summary>
    public PromptDataProperties? PromptData { get; set; }

    /// <summary>
    /// The actual code expression for this line.
    /// </summary>
    public string? Expression
    {
        get => _coreCodeLine.Expression;
        set => SetProperty(_coreCodeLine.Expression, value, _coreCodeLine, (c, v) => c.Expression = v);
    }

    public CodeLineProperties(CodeLine codeLine)
    {
        _coreCodeLine = codeLine;

        if (codeLine.Settings is not null)
        {
            Settings = new CodeLineSettings(codeLine.Settings);
        }
        if (codeLine.PromptData is not null)
        {
            PromptData = new PromptDataProperties(codeLine.PromptData);
        }
    }

    /// <summary>
    /// Generates a collection of display-ready properties for this CodeLine.
    /// </summary>
    public IEnumerable<DisplayProperty> GetDisplayProperties()
    {
        if (!string.IsNullOrEmpty(Expression))
        {
            yield return new DisplayProperty("Expression:", Expression);
        }

        if (Settings is CodeLineSettings codeLineSettings)
        {
            yield return new DisplayProperty("Order:", codeLineSettings.Order.ToString());
            yield return new DisplayProperty("Type:", codeLineSettings.Type.ToString());
            yield return new DisplayProperty("Variable:", codeLineSettings.Variable);
        }

        if (PromptData is not null)
        {
            yield return new DisplayProperty("Prompt Message:", PromptData.Message);
            if (PromptData.Choices.Any())
            {
                yield return new DisplayProperty("Choices:", string.Join(", ", PromptData.Choices));
            }
        }
    }
}
