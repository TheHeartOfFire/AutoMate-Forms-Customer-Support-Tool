using AMFormsCST.Desktop.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure.PromptDataSettings;

namespace AMFormsCST.Desktop.Models.FormgenUtilities;
public class PromptDataSettings : IFormgenFileSettings
{
    public PromptDataSettings() { }
    public PromptDataSettings(Core.Types.FormgenUtils.FormgenFileStructure.PromptDataSettings? settings)
    {
        if (settings is null) return;

        Type = settings.Type;
        IsExpression = settings.IsExpression;
        Required = settings.Required;
        Length = settings.Length;
        DecimalPlaces = settings.DecimalPlaces;
        Delimiter = settings.Delimiter;
        AllowNegative = settings.AllowNegative;
        ForceUpperCase = settings.ForceUpperCase;
        MakeBuyerVars = settings.MakeBuyerVars;
        IncludeNoneAsOption = settings.IncludeNoneAsOption;
    }

    public PromptType Type { get; set; }
    public bool IsExpression { get; set; }
    public bool Required { get; set; }
    public int Length { get; set; }
    public int DecimalPlaces { get; set; }
    public string Delimiter { get; set; } = string.Empty; 
    public bool AllowNegative { get; set; }
    public bool ForceUpperCase { get; set; }
    public bool MakeBuyerVars { get; set; }
    public bool IncludeNoneAsOption { get; set; }

    public string GetPromptType() => Type switch
    {
        PromptType.CheckBox => "Checkbox",
        PromptType.Date => "Date",
        PromptType.Decimal => "Decimal",
        PromptType.Dropdown => "Dropdown",
        PromptType.Integer => "Integer",
        PromptType.Label => "Label",
        PromptType.LabelNumber => "Label Number",
        PromptType.Money => "Money",
        PromptType.RadioButtons => "Radio Buttons",
        PromptType.Separator => "Separator",
        PromptType.StateCode => "State Code",
        PromptType.Text => "Text",
        PromptType.VIN => "VIN",
        PromptType.YesNo => "Yes/No",
        PromptType.ZIP5 => "5 digit Zip",
        PromptType.ZIP10 => "10 digit Zip",
        PromptType.OneTwoThree => "Buyer, Co-Buyer, or Both (1,2,3)",
        PromptType.OneTwoThreeFour => "Buyer, Co-Buyer, Both, or Other (1,2,3,4)",
        _ => "Unknown"
    };
}
