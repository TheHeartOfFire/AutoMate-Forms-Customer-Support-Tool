using AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure;
using AMFormsCST.Desktop.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

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

    public UIElement GetUIElements()
    {
        return BasicStats.GetSettingsAndPropertiesUIElements(this);
    }

    public IEnumerable<DisplayProperty> GetDisplayProperties()
    {
        yield return new DisplayProperty("Message:", Message);
        if (Choices.Any())
        {
            yield return new DisplayProperty("Choices:", string.Join(", ", Choices));
        }

        if (Settings is PromptDataSettings promptSettings)
        {
            yield return new DisplayProperty("Type:", promptSettings.Type.ToString());
            yield return new DisplayProperty("Length:", promptSettings.Length.ToString());
            yield return new DisplayProperty("Required:", promptSettings.Required.ToString());
        }
    }
}
