using AMFormsCST.Core.Interfaces.UserSettings;

namespace AMFormsCST.Desktop.Models.UserSettings;

public class UiSettings : IUiSettings
{
    public List<ISetting> Settings { get; set; }

    public UiSettings()
    {
        Settings =
        [
            new ThemeSetting(),
            new AlwaysOnTopSetting { IsEnabled = false },
            new NewTemplateSetting(),
            new CapitalizeFormCodeSetting()
        ];
    }
}