using AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure;
using AMFormsCST.Desktop.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure.CodeLineSettings;

namespace AMFormsCST.Desktop.Models.FormgenUtilities;
public class CodeLineSettings : IFormgenFileSettings
{
    public CodeLineSettings() { }
    public CodeLineSettings(Core.Types.FormgenUtils.FormgenFileStructure.CodeLineSettings? settings)
    {
        if (settings is null) return;

        Order = settings.Order;
        Type = settings.Type;
        Variable = settings.Variable;
    }

    public int Order { get; set; }
    public CodeType Type { get; set; }
    public string? Variable { get; set; }

    public string GetCodeType() => Type switch
        {
            CodeType.INIT => "INIT",
            CodeType.PROMPT => "PROMPT",
            CodeType.POST => "POST",
            _ => "PROMPT", 
        };
    
}
