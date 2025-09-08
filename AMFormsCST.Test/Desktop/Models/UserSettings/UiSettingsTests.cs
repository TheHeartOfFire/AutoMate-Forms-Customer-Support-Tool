using AMFormsCST.Desktop.Models.UserSettings;
using AMFormsCST.Core.Interfaces.UserSettings;
using Xunit;
namespace AMFormsCST.Test.Desktop.Models.UserSettings;
public class UiSettingsTests
{
    [Fact]
    public void Constructor_InitializesSettingsWithDefaults()
    {
        // Act
        var uiSettings = new UiSettings();

        // Assert
        Assert.NotNull(uiSettings.Settings);
        Assert.True(uiSettings.Settings.Count >= 4); // Should contain at least the 4 default settings

        Assert.Contains(uiSettings.Settings, s => s is ThemeSetting);
        Assert.Contains(uiSettings.Settings, s => s is AlwaysOnTopSetting);
        Assert.Contains(uiSettings.Settings, s => s is NewTemplateSetting);
        Assert.Contains(uiSettings.Settings, s => s is CapitalizeFormCodeSetting);
    }

    [Fact]
    public void Settings_CanBeModified()
    {
        // Arrange
        var uiSettings = new UiSettings();

        // Act
        var alwaysOnTop = uiSettings.Settings.Find(s => s is AlwaysOnTopSetting) as AlwaysOnTopSetting;
        alwaysOnTop!.IsEnabled = true;

        // Assert
        Assert.True(alwaysOnTop.IsEnabled);
    }
}