using AMFormsCST.Core.Types;
using AMFormsCST.Core.Interfaces.UserSettings;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

public class SettingsTests
{
    [Fact]
    public void Constructor_SetsPropertiesCorrectly()
    {
        // Arrange
        var mockUserSettings = new Mock<IUserSettings>();
        var mockUiSettings = new Mock<IUiSettings>();

        // Act
        var settings = new Settings(mockUserSettings.Object, mockUiSettings.Object);

        // Assert
        Assert.Same(mockUserSettings.Object, settings.UserSettings);
        Assert.Same(mockUiSettings.Object, settings.UiSettings);
    }

    [Fact]
    public void Constructor_ThrowsArgumentNullException_WhenUserSettingsIsNull()
    {
        // Arrange
        var mockUiSettings = new Mock<IUiSettings>();

        // Act & Assert
        var ex = Assert.Throws<ArgumentNullException>(() => new Settings(null, mockUiSettings.Object));
        Assert.Contains("userSettings", ex.Message);
    }

    [Fact]
    public void Constructor_ThrowsArgumentNullException_WhenUiSettingsIsNull()
    {
        // Arrange
        var mockUserSettings = new Mock<IUserSettings>();

        // Act & Assert
        var ex = Assert.Throws<ArgumentNullException>(() => new Settings(mockUserSettings.Object, null));
        Assert.Contains("uiSettings", ex.Message);
    }

    [Fact]
    public void UserSettings_Property_CanBeSetAndGet()
    {
        // Arrange
        var mockUserSettings1 = new Mock<IUserSettings>();
        var mockUserSettings2 = new Mock<IUserSettings>();
        var mockUiSettings = new Mock<IUiSettings>();
        var settings = new Settings(mockUserSettings1.Object, mockUiSettings.Object);

        // Act
        settings.UserSettings = mockUserSettings2.Object;

        // Assert
        Assert.Same(mockUserSettings2.Object, settings.UserSettings);
    }

    [Fact]
    public void UiSettings_Property_CanBeGet()
    {
        // Arrange
        var mockUserSettings = new Mock<IUserSettings>();
        var mockUiSettings = new Mock<IUiSettings>();
        var settings = new Settings(mockUserSettings.Object, mockUiSettings.Object);

        // Act & Assert
        Assert.Same(mockUiSettings.Object, settings.UiSettings);
    }

    [Fact]
    public void AllSettings_ReturnsListWithUserAndUiSettings()
    {
        // Arrange
        var mockUserSettings = new Mock<IUserSettings>();
        var mockUiSettings = new Mock<IUiSettings>();
        var settings = new Settings(mockUserSettings.Object, mockUiSettings.Object);

        // Act
        var allSettings = settings.AllSettings;

        // Assert
        Assert.Equal(2, allSettings.Count);
        Assert.Contains(mockUserSettings.Object, allSettings);
        Assert.Contains(mockUiSettings.Object, allSettings);
    }
}