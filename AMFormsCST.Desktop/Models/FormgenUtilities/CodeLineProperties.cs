using AMFormsCST.Core.Interfaces;
using AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure;
using AMFormsCST.Desktop.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using static AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure.CodeLineSettings;

namespace AMFormsCST.Desktop.Models.FormgenUtilities;

/// <summary>
/// A wrapper class that prepares a CodeLine object's data to be displayed as UI properties.
/// </summary>
public partial class CodeLineProperties : ObservableObject, IFormgenFileProperties
{
    private readonly CodeLine _coreCodeLine;
    private readonly ILogService? _logger;

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
        set
        {
            SetProperty(_coreCodeLine.Expression, value, _coreCodeLine, (c, v) => c.Expression = v);
            _logger?.LogInfo($"CodeLine Expression changed: {value}");
        }
    }

    public CodeLineProperties(CodeLine codeLine, ILogService? logger = null)
    {
        _coreCodeLine = codeLine;
        _logger = logger;

        if (codeLine.Settings is not null)
        {
            Settings = new CodeLineSettings(codeLine.Settings);
            _logger?.LogInfo($"CodeLineProperties Settings initialized: Order={codeLine.Settings.Order}, Type={codeLine.Settings.Type}, Variable={codeLine.Settings.Variable}");
        }
        if (codeLine.PromptData is not null)
        {
            PromptData = new PromptDataProperties(codeLine.PromptData);
            _logger?.LogInfo("CodeLineProperties PromptData initialized.");
        }
        _logger?.LogInfo("CodeLineProperties initialized.");
    }

    /// <summary>
    /// Generates a collection of display-ready properties for this CodeLine.
    /// </summary>
    public IEnumerable<DisplayProperty> GetDisplayProperties()
    {
        // Editable property: Expression
        var exprProp = typeof(CodeLine).GetProperty(nameof(CodeLine.Expression));
        if (exprProp != null)
            yield return new DisplayProperty(_coreCodeLine, exprProp);

        // Editable properties from Settings (Order, Type, Variable)
        if (Settings is CodeLineSettings codeLineSettings)
        {
            var settingsType = typeof(CodeLineSettings);
            var orderProp = settingsType.GetProperty(nameof(CodeLineSettings.Order));
            if (orderProp != null)
                yield return new DisplayProperty(codeLineSettings, orderProp);

            var typeProp = settingsType.GetProperty(nameof(CodeLineSettings.Type));
            if (typeProp != null)
                yield return new DisplayProperty(codeLineSettings, typeProp);

            var variableProp = settingsType.GetProperty(nameof(CodeLineSettings.Variable));
            if (variableProp != null)
                yield return new DisplayProperty(codeLineSettings, variableProp);
        }

        // Editable properties from PromptData (Message, Choices)
        if (PromptData is not null)
        {
            var promptType = typeof(PromptDataProperties);
            var messageProp = promptType.GetProperty(nameof(PromptDataProperties.Message));
            if (messageProp != null)
                yield return new DisplayProperty(PromptData, messageProp);

            var choicesProp = promptType.GetProperty(nameof(PromptDataProperties.Choices));
            if (choicesProp != null && PromptData.Choices.Any())
                yield return new DisplayProperty(PromptData, choicesProp);
        }
    }
}
