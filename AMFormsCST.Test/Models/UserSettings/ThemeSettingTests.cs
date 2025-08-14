using AMFormsCST.Desktop.Models.UserSettings;
using Wpf.Ui.Appearance;
using Xunit;

public class ThemeSettingTests
{
    [Fact]
    public void Default_Theme_IsDark()
    {
        // Act
        var setting = new ThemeSetting();

        // Assert
        Assert.Equal(ApplicationTheme.Dark, setting.Theme);
    }

    [Fact]
    public void Theme_CanBeSetAndRetrieved()
    {
        // Arrange
        var setting = new ThemeSetting();

        // Act
        setting.Theme = ApplicationTheme.Light;

        // Assert
        Assert.Equal(ApplicationTheme.Light, setting.Theme);

        // Act
        setting.Theme = ApplicationTheme.Dark;

        // Assert
        Assert.Equal(ApplicationTheme.Dark, setting.Theme);
    }
}