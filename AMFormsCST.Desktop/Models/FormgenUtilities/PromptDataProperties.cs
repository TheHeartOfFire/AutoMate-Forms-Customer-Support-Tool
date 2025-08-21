using AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure;
using AMFormsCST.Desktop.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using static AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure.PromptDataSettings;
using System.Reflection;

namespace AMFormsCST.Desktop.Models.FormgenUtilities;

public partial class PromptDataProperties : ObservableObject, IFormgenFileProperties
{
    private readonly PromptData _corePromptData;

    public IFormgenFileSettings Settings { get; set; }

    public string? Message
    {
        get => _corePromptData.Message;
        set => SetProperty(_corePromptData.Message, value, _corePromptData, (p, v) => p.Message = v);
    }

    public List<string> Choices
    {
        get => _corePromptData.Choices;
        set => SetProperty(_corePromptData.Choices, value, _corePromptData, (p, v) => p.Choices = v);
    }

    public PromptDataProperties(PromptData promptData)
    {
        _corePromptData = promptData;
        if (promptData.Settings is not null)
        {
            Settings = new PromptDataSettings(promptData.Settings);
        }
    }

    public IEnumerable<DisplayProperty> GetDisplayProperties()
    {
        var promptType = typeof(PromptData);

        // Editable property: Message
        var messageProp = promptType.GetProperty(nameof(PromptData.Message));
        if (messageProp != null)
            yield return new DisplayProperty(_corePromptData, messageProp);

        // Editable property: Choices (if any)
        var choicesProp = promptType.GetProperty(nameof(PromptData.Choices));
        if (choicesProp != null && _corePromptData.Choices.Count != 0)
            yield return new DisplayProperty(_corePromptData, choicesProp);

        // Editable properties from PromptDataSettings
        if (Settings is PromptDataSettings promptSettings)
        {
            var settingsType = typeof(PromptDataSettings);

            var typeProp = settingsType.GetProperty(nameof(PromptDataSettings.Type));
            if (typeProp != null)
                yield return new DisplayProperty(promptSettings, typeProp);

            var lengthProp = settingsType.GetProperty(nameof(PromptDataSettings.Length));
            if (lengthProp != null)
                yield return new DisplayProperty(promptSettings, lengthProp);

            var requiredProp = settingsType.GetProperty(nameof(PromptDataSettings.Required));
            if (requiredProp != null)
                yield return new DisplayProperty(promptSettings, requiredProp);
        }
    }
}
