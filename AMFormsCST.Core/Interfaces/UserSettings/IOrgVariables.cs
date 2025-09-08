using AMFormsCST.Core.Interfaces.BestPractices;
using AMFormsCST.Core.Interfaces.Utils;
using AMFormsCST.Core.Types.UserSettings;
using System.Text.Json.Serialization;

namespace AMFormsCST.Core.Interfaces.UserSettings;

[JsonDerivedType(typeof(AutomateFormsOrgVariables), typeDiscriminator: "orgVars")]
public interface IOrgVariables : ISetting
{
    Dictionary<string, string> LooseVariables { get; set; }
    List<ITextTemplateVariable> Variables { get; }

    void InstantiateVariables(IBestPracticeEnforcer enforcer, INotebook notebook);
}
