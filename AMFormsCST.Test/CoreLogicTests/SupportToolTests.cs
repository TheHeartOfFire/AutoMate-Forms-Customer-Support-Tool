using AMFormsCST.Core;
using AMFormsCST.Core.Interfaces;
using AMFormsCST.Core.Interfaces.BestPractices;
using AMFormsCST.Core.Interfaces.UserSettings;
using AMFormsCST.Core.Interfaces.Utils;
using AMFormsCST.Core.Types;
using AMFormsCST.Core.Types.BestPractices.TextTemplates.Models;
using AMFormsCST.Core.Types.UserSettings;
using AMFormsCST.Core.Utils;
using Moq;
using System.IO;
using Xunit;

public class SupportToolTests
{
    [Fact]
    public void Constructor_InitializesDependenciesAndSettings()
    {
        // Arrange
        var mockFileSystem = new Mock<IFileSystem>();
        var mockFormNameBestPractice = new Mock<IFormNameBestPractice>();
        var mockTemplateRepo = new Mock<ITemplateRepository>();
        var mockOrgVars = new Mock<IOrgVariables>();
        var mockUserSettings = new Mock<IUserSettings>();
        var mockUiSettings = new Mock<IUiSettings>();
        var mockSettings = new Mock<ISettings>();
        var mockNotebook = new Mock<INotebook>();

        mockUserSettings.SetupGet(u => u.Organization).Returns(mockOrgVars.Object);
        mockSettings.SetupGet(s => s.UserSettings).Returns(mockUserSettings.Object);
        mockSettings.SetupGet(s => s.UiSettings).Returns(mockUiSettings.Object);

        // Make sure InstantiateVariables can be called
        mockOrgVars.Setup(o => o.InstantiateVariables(It.IsAny<IBestPracticeEnforcer>(), It.IsAny<INotebook>()));

        // Ensure the settings file does not exist so IO.LoadSettings() returns null
        var settingsPathField = typeof(IO).GetField("_settingsPath", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
        var settingsPath = (string)settingsPathField.GetValue(null);
        if (File.Exists(settingsPath))
            File.Delete(settingsPath);

        // Act
        var tool = new SupportTool(
            mockFileSystem.Object,
            mockFormNameBestPractice.Object,
            mockSettings.Object,
            mockTemplateRepo.Object
        );

        // Assert
        Assert.NotNull(tool.Enforcer);
        Assert.NotNull(tool.CodeBlocks);
        Assert.NotNull(tool.FormgenUtils);
        Assert.NotNull(tool.Notebook);
        Assert.Same(mockSettings.Object, tool.Settings);

        // Organization.InstantiateVariables should be called with the tool's Enforcer and Notebook
        mockOrgVars.Verify(o => o.InstantiateVariables(tool.Enforcer, tool.Notebook), Times.Once);
    }

    [Fact]
    public void SaveAllSettings_CreatesOrUpdatesSettingsFile()
    {
        // Arrange
        var mockFileSystem = new Mock<IFileSystem>();
        var mockFormNameBestPractice = new Mock<IFormNameBestPractice>();
        var mockTemplateRepo = new Mock<ITemplateRepository>();
        var mockOrgVars = new Mock<IOrgVariables>();
        var mockUserSettings = new Mock<IUserSettings>();
        var mockUiSettings = new Mock<IUiSettings>();
        var mockSettings = new Mock<ISettings>();

        mockUserSettings.SetupGet(u => u.Organization).Returns(mockOrgVars.Object);
        mockSettings.SetupGet(s => s.UserSettings).Returns(mockUserSettings.Object);
        mockSettings.SetupGet(s => s.UiSettings).Returns(mockUiSettings.Object);

        // Ensure the settings file does not exist before the test
        var settingsPathField = typeof(IO).GetField("_settingsPath", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
        var settingsPath = (string)settingsPathField.GetValue(null);
        if (File.Exists(settingsPath))
            File.Delete(settingsPath);

        var tool = new SupportTool(
            mockFileSystem.Object,
            mockFormNameBestPractice.Object,
            mockSettings.Object,
            mockTemplateRepo.Object
        );

        // Act
        tool.SaveAllSettings();

        // Assert
        Assert.True(File.Exists(settingsPath));
    }
}