using AMFormsCST.Core.Types.UserSettings;
using System.Text.Json.Serialization;

namespace AMFormsCST.Core.Interfaces.UserSettings;

[JsonDerivedType(typeof(Types.UserSettings.UserSettings), typeDiscriminator: "userSettings")]
[JsonDerivedType(typeof(AutomateFormsOrgVariables), typeDiscriminator: "orgVars")]
// Note: Concrete UI setting types are defined in the Desktop project.
// We will handle them with a custom resolver in the IO class.
public interface ISetting
{
}
