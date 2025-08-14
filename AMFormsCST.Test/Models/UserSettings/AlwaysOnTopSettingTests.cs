using AMFormsCST.Desktop.Models.UserSettings;
using Xunit;

public class AlwaysOnTopSettingTests
{
    [Fact]
    public void Default_IsEnabled_IsFalse()
    {
        // Act
        var setting = new AlwaysOnTopSetting();

        // Assert
        Assert.False(setting.IsEnabled);
    }

    [Fact]
    public void IsEnabled_CanBeSetAndRetrieved()
    {
        // Arrange
        var setting = new AlwaysOnTopSetting();

        // Act
        setting.IsEnabled = true;

        // Assert
        Assert.True(setting.IsEnabled);

        // Act
        setting.IsEnabled = false;

        // Assert
        Assert.False(setting.IsEnabled);
    }
}