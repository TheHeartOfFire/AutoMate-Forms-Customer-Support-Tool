using AMFormsCST.Core.Interfaces.UserSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFormsCST.Core.Types;
public class Settings(IUserSettings userSettings) : ISettings
{
    public List<ISetting> AllSettings
    {
        get
        {
            return [UserSettings, .. UiSettings];
        }
    }
    public IUserSettings UserSettings { get; set; } = userSettings ?? throw new ArgumentNullException(nameof(userSettings), "User settings cannot be null.");
    public List<ISetting> UiSettings { get; set; } = [];

}
