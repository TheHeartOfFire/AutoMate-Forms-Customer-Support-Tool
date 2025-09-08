using AMFormsCST.Core.Types.UserSettings;
using AMFormsCST.Core.Interfaces.UserSettings;
using Moq;
using Xunit;

namespace AMFormsCST.Test.Core.Types.UserSettings;
public class UserSettingsTests
{
    [Fact]
    public void Constructor_SetsOrganizationProperty()
    {
        // Arrange
        var mockOrgVars = new Mock<IOrgVariables>();

        // Act
        var settings = new AMFormsCST.Core.Types.UserSettings.UserSettings(mockOrgVars.Object);

        // Assert
        Assert.Same(mockOrgVars.Object, settings.Organization);
    }

    [Fact]
    public void DefaultProperties_AreInitializedCorrectly()
    {
        // Arrange
        var mockOrgVars = new Mock<IOrgVariables>();

        // Act
        var settings = new AMFormsCST.Core.Types.UserSettings.UserSettings(mockOrgVars.Object);

        // Assert
        Assert.Equal(string.Empty, settings.Name);
        Assert.Equal(string.Empty, settings.ApplicationFilesPath);
        Assert.Equal("x", settings.ExtSeparator);
    }

    [Fact]
    public void Properties_CanBeSetAndGet()
    {
        // Arrange
        var mockOrgVars1 = new Mock<IOrgVariables>();
        var mockOrgVars2 = new Mock<IOrgVariables>();
        var settings = new AMFormsCST.Core.Types.UserSettings.UserSettings(mockOrgVars1.Object);

        // Act
        settings.Organization = mockOrgVars2.Object;
        settings.Name = "Test User";
        settings.ApplicationFilesPath = @"C:\TestPath";
        settings.ExtSeparator = "-";

        // Assert
        Assert.Same(mockOrgVars2.Object, settings.Organization);
        Assert.Equal("Test User", settings.Name);
        Assert.Equal(@"C:\TestPath", settings.ApplicationFilesPath);
        Assert.Equal("-", settings.ExtSeparator);
    }
}