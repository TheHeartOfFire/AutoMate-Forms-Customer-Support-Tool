using AMFormsCST.Core.Types;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AMFormsCST.Core.Interfaces.UserSettings;

[JsonDerivedType(typeof(Settings), typeDiscriminator: "settings")]
public interface ISettings
{
    List<ISetting> AllSettings { get; }
    IUserSettings UserSettings { get; set; }
    IUiSettings UiSettings { get; }
}