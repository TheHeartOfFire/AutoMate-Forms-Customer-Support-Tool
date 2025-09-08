using AMFormsCST.Core.Types.UserSettings;
using System.Text.Json.Serialization;

namespace AMFormsCST.Core.Interfaces.UserSettings;

[JsonDerivedType(typeof(Types.UserSettings.UserSettings), typeDiscriminator: "userSettings")]
[JsonDerivedType(typeof(AutomateFormsOrgVariables), typeDiscriminator: "orgVars")]
public interface ISetting
{
}
