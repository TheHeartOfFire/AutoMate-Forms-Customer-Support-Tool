using AMFormsCST.Core.Interfaces.UserSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AMFormsCST.Core.Types;
public class Settings(IUserSettings userSettings, IUiSettings uiSettings) : ISettings
{
    [JsonIgnore] // This property is for convenience and should not be serialized
    public List<ISetting> AllSettings
    {
        get
        {
            return [UserSettings, UiSettings];
        }
    }

    [JsonInclude]
    public IUserSettings UserSettings { get; set; } = userSettings ?? throw new ArgumentNullException(nameof(userSettings), "User settings cannot be null.");

    [JsonInclude]
    public IUiSettings UiSettings { get; set; } = uiSettings ?? throw new ArgumentNullException(nameof(uiSettings), "UI settings cannot be null.");
}
