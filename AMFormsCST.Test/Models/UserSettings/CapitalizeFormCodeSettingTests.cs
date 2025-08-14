using AMFormsCST.Desktop.Models.UserSettings;
using Xunit;

public class CapitalizeFormCodeSettingTests
{
    [Fact]
    public void Default_CapitalizeFormCode_IsTrue()
    {
        // Act
        var setting = new CapitalizeFormCodeSetting();

        // Assert
        Assert.True(setting.CapitalizeFormCode);
    }

    [Fact]
    public void CapitalizeFormCode_CanBeSetAndRetrieved()
    {
        // Arrange
        var setting = new CapitalizeFormCodeSetting();

        // Act
        setting.CapitalizeFormCode = false;

        // Assert
        Assert.False(setting.CapitalizeFormCode);

        // Act
        setting.CapitalizeFormCode = true;

        // Assert
        Assert.True(setting.CapitalizeFormCode);
    }
}