using AMFormsCST.Core.Interfaces.UserSettings;
using System.Collections.Generic;

namespace AMFormsCST.Desktop.Models.UserSettings;

public class UiSettings : IUiSettings
{
    public List<ISetting> Settings { get; set; }

    public UiSettings()
    {
        // Initialize with default UI settings defined by the Desktop project.
        Settings =
        [
            new ThemeSetting(),
            new AlwaysOnTopSetting { IsEnabled = false },
            new NewTemplateSetting(),
            new CapitalizeFormCodeSetting()
            // Add other UI-specific settings here
        ];
    }
}