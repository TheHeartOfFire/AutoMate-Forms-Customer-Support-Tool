using AMFormsCST.Core.Interfaces;
using AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure;
using AMFormsCST.Desktop.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using static AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure.PromptDataSettings;

namespace AMFormsCST.Desktop.Models.FormgenUtilities;

public partial class PromptDataSettings : ObservableObject, IFormgenFileSettings
{
    private readonly Core.Types.FormgenUtils.FormgenFileStructure.PromptDataSettings _coreSettings;
    private readonly ILogService? _logger;

    public PromptDataSettings(Core.Types.FormgenUtils.FormgenFileStructure.PromptDataSettings settings, ILogService? logger = null)
    {
        _coreSettings = settings;
        _logger = logger;
        _logger?.LogInfo($"PromptDataSettings initialized: Type={settings.Type}, Length={settings.Length}, Required={settings.Required}");
    }

    public PromptType Type
    {
        get => _coreSettings.Type;
        set
        {
            SetProperty(_coreSettings.Type, value, _coreSettings, (s, v) => s.Type = v);
            _logger?.LogInfo($"PromptDataSettings Type changed: {value}");
        }
    }

    public bool IsExpression
    {
        get => _coreSettings.IsExpression;
        set
        {
            SetProperty(_coreSettings.IsExpression, value, _coreSettings, (s, v) => s.IsExpression = v);
            _logger?.LogInfo($"PromptDataSettings IsExpression changed: {value}");
        }
    }

    public bool Required
    {
        get => _coreSettings.Required;
        set
        {
            SetProperty(_coreSettings.Required, value, _coreSettings, (s, v) => s.Required = v);
            _logger?.LogInfo($"PromptDataSettings Required changed: {value}");
        }
    }

    public int Length
    {
        get => _coreSettings.Length;
        set
        {
            SetProperty(_coreSettings.Length, value, _coreSettings, (s, v) => s.Length = v);
            _logger?.LogInfo($"PromptDataSettings Length changed: {value}");
        }
    }

    public int DecimalPlaces
    {
        get => _coreSettings.DecimalPlaces;
        set
        {
            SetProperty(_coreSettings.DecimalPlaces, value, _coreSettings, (s, v) => s.DecimalPlaces = v);
            _logger?.LogInfo($"PromptDataSettings DecimalPlaces changed: {value}");
        }
    }

    public string Delimiter
    {
        get => _coreSettings.Delimiter;
        set
        {
            SetProperty(_coreSettings.Delimiter, value, _coreSettings, (s, v) => s.Delimiter = v);
            _logger?.LogInfo($"PromptDataSettings Delimiter changed: {value}");
        }
    }

    public bool AllowNegative
    {
        get => _coreSettings.AllowNegative;
        set
        {
            SetProperty(_coreSettings.AllowNegative, value, _coreSettings, (s, v) => s.AllowNegative = v);
            _logger?.LogInfo($"PromptDataSettings AllowNegative changed: {value}");
        }
    }

    public bool ForceUpperCase
    {
        get => _coreSettings.ForceUpperCase;
        set
        {
            SetProperty(_coreSettings.ForceUpperCase, value, _coreSettings, (s, v) => s.ForceUpperCase = v);
            _logger?.LogInfo($"PromptDataSettings ForceUpperCase changed: {value}");
        }
    }

    public bool MakeBuyerVars
    {
        get => _coreSettings.MakeBuyerVars;
        set
        {
            SetProperty(_coreSettings.MakeBuyerVars, value, _coreSettings, (s, v) => s.MakeBuyerVars = v);
            _logger?.LogInfo($"PromptDataSettings MakeBuyerVars changed: {value}");
        }
    }

    public bool IncludeNoneAsOption
    {
        get => _coreSettings.IncludeNoneAsOption;
        set
        {
            SetProperty(_coreSettings.IncludeNoneAsOption, value, _coreSettings, (s, v) => s.IncludeNoneAsOption = v);
            _logger?.LogInfo($"PromptDataSettings IncludeNoneAsOption changed: {value}");
        }
    }
}
