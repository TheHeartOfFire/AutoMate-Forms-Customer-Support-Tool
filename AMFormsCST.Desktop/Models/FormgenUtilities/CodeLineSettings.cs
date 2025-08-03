using AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure;
using AMFormsCST.Desktop.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using static AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure.CodeLineSettings;

namespace AMFormsCST.Desktop.Models.FormgenUtilities;

public partial class CodeLineSettings : ObservableObject, IFormgenFileSettings
{
    private readonly Core.Types.FormgenUtils.FormgenFileStructure.CodeLineSettings _coreSettings;

    public CodeLineSettings(Core.Types.FormgenUtils.FormgenFileStructure.CodeLineSettings settings)
    {
        _coreSettings = settings;
    }

    public int Order
    {
        get => _coreSettings.Order;
        set => SetProperty(_coreSettings.Order, value, _coreSettings, (s, v) => s.Order = v);
    }

    public CodeType Type
    {
        get => _coreSettings.Type;
        set => SetProperty(_coreSettings.Type, value, _coreSettings, (s, v) => s.Type = v);
    }

    public string? Variable
    {
        get => _coreSettings.Variable;
        set => SetProperty(_coreSettings.Variable, value, _coreSettings, (s, v) => s.Variable = v);
    }

    public string GetCodeType() => Type switch
        {
            CodeType.INIT => "INIT",
            CodeType.PROMPT => "PROMPT",
            CodeType.POST => "POST",
            _ => "PROMPT", 
        };
    
}
