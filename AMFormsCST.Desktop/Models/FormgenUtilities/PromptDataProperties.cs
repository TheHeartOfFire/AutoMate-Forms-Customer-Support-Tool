using AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure;
using AMFormsCST.Desktop.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using static AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure.PromptDataSettings;
using System.Reflection;
using AMFormsCST.Core.Interfaces;

namespace AMFormsCST.Desktop.Models.FormgenUtilities;

public partial class PromptDataProperties : ObservableObject, IFormgenFileProperties
{
    private readonly PromptData _corePromptData;
    private readonly ILogService? _logger;

    public IFormgenFileSettings Settings { get; set; }

    public string? Message
    {
        get => _corePromptData.Message;
        set
        {
            SetProperty(_corePromptData.Message, value, _corePromptData, (p, v) => p.Message = v);
            _logger?.LogInfo($"PromptData Message changed: {value}");
        }
    }

    public List<string> Choices
    {
        get => _corePromptData.Choices;
        set
        {
            SetProperty(_corePromptData.Choices, value, _corePromptData, (p, v) => p.Choices = v);
            _logger?.LogInfo($"PromptData Choices changed: {string.Join(", ", value ?? [])}");
        }
    }

    public PromptDataProperties(PromptData promptData, ILogService? logger = null)
    {
        _corePromptData = promptData;
        _logger = logger;
        if (promptData.Settings is not null)
        {
            Settings = new PromptDataSettings(promptData.Settings, _logger);
            _logger?.LogInfo("PromptDataProperties Settings initialized.");
        }
        _logger?.LogInfo("PromptDataProperties initialized.");
    }

    public IEnumerable<DisplayProperty> GetDisplayProperties()
    {
        var promptType = typeof(PromptData);

        // Editable property: Message
        var messageProp = promptType.GetProperty(nameof(PromptData.Message));
        if (messageProp != null)
            yield return new DisplayProperty(_corePromptData, messageProp, false, _logger);

        // Editable property: Choices (if any)
        var choicesProp = promptType.GetProperty(nameof(PromptData.Choices));
        if (choicesProp != null && _corePromptData.Choices.Count != 0)
            yield return new DisplayProperty(_corePromptData, choicesProp, false, _logger);

        // Editable properties from PromptDataSettings
        if (Settings is PromptDataSettings promptSettings)
        {
            var settingsType = typeof(PromptDataSettings);

            var typeProp = settingsType.GetProperty(nameof(PromptDataSettings.Type));
            if (typeProp != null)
                yield return new DisplayProperty(promptSettings, typeProp, false, _logger);

            var lengthProp = settingsType.GetProperty(nameof(PromptDataSettings.Length));
            if (lengthProp != null)
                yield return new DisplayProperty(promptSettings, lengthProp, false, _logger);

            var requiredProp = settingsType.GetProperty(nameof(PromptDataSettings.Required));
            if (requiredProp != null)
                yield return new DisplayProperty(promptSettings, requiredProp, false, _logger);
        }
    }
}
