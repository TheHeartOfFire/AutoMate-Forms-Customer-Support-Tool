using AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure;
using AMFormsCST.Desktop.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using static AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure.PromptDataSettings;

namespace AMFormsCST.Desktop.Models.FormgenUtilities;

public partial class PromptDataSettings : ObservableObject, IFormgenFileSettings
{
    private readonly Core.Types.FormgenUtils.FormgenFileStructure.PromptDataSettings _coreSettings;

    public PromptDataSettings(Core.Types.FormgenUtils.FormgenFileStructure.PromptDataSettings settings)
    {
        _coreSettings = settings;
    }

    public PromptType Type
    {
        get => _coreSettings.Type;
        set => SetProperty(_coreSettings.Type, value, _coreSettings, (s, v) => s.Type = v);
    }

    public bool IsExpression
    {
        get => _coreSettings.IsExpression;
        set => SetProperty(_coreSettings.IsExpression, value, _coreSettings, (s, v) => s.IsExpression = v);
    }

    public bool Required
    {
        get => _coreSettings.Required;
        set => SetProperty(_coreSettings.Required, value, _coreSettings, (s, v) => s.Required = v);
    }

    public int Length
    {
        get => _coreSettings.Length;
        set => SetProperty(_coreSettings.Length, value, _coreSettings, (s, v) => s.Length = v);
    }

    public int DecimalPlaces
    {
        get => _coreSettings.DecimalPlaces;
        set => SetProperty(_coreSettings.DecimalPlaces, value, _coreSettings, (s, v) => s.DecimalPlaces = v);
    }

    public string Delimiter
    {
        get => _coreSettings.Delimiter;
        set => SetProperty(_coreSettings.Delimiter, value, _coreSettings, (s, v) => s.Delimiter = v);
    }

    public bool AllowNegative
    {
        get => _coreSettings.AllowNegative;
        set => SetProperty(_coreSettings.AllowNegative, value, _coreSettings, (s, v) => s.AllowNegative = v);
    }

    public bool ForceUpperCase
    {
        get => _coreSettings.ForceUpperCase;
        set => SetProperty(_coreSettings.ForceUpperCase, value, _coreSettings, (s, v) => s.ForceUpperCase = v);
    }

    public bool MakeBuyerVars
    {
        get => _coreSettings.MakeBuyerVars;
        set => SetProperty(_coreSettings.MakeBuyerVars, value, _coreSettings, (s, v) => s.MakeBuyerVars = v);
    }

    public bool IncludeNoneAsOption
    {
        get => _coreSettings.IncludeNoneAsOption;
        set => SetProperty(_coreSettings.IncludeNoneAsOption, value, _coreSettings, (s, v) => s.IncludeNoneAsOption = v);
    }
}
