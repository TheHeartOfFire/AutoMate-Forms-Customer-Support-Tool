using AMFormsCST.Desktop.Views.Pages;
using AMFormsCST.Desktop.ViewModels.Pages;
using Moq;
using Xunit;
using AMFormsCST.Desktop.Services;
using AMFormsCST.Core.Interfaces;
using System.Collections.Generic;
using AMFormsCST.Core.Interfaces.UserSettings;
using AMFormsCST.Test.Helpers;

public class SettingsPageTests
{
    [WpfFact]
    public void Constructor_SetsViewModelAndDataContext()
    {
        // Arrange
        var updateManagerServiceMock = new Mock<IUpdateManagerService>();
        var supportToolMock = new Mock<ISupportTool>();

        // Setup nested settings structure
        var uiSettings = new Mock<IUiSettings>();
        uiSettings.SetupGet(u => u.Settings).Returns(new List<ISetting>());

        var userSettings = new Mock<IUserSettings>();
        userSettings.SetupGet(u => u.ExtSeparator).Returns(string.Empty);

        var settings = new Mock<ISettings>();
        settings.SetupGet(s => s.UiSettings).Returns(uiSettings.Object);
        settings.SetupGet(s => s.UserSettings).Returns(userSettings.Object);

        supportToolMock.SetupGet(s => s.Settings).Returns(settings.Object);

        var vm = new SettingsViewModel(updateManagerServiceMock.Object, supportToolMock.Object);

        // Act
        var page = new SettingsPage(vm);

        // Assert
        Assert.Equal(vm, page.ViewModel);
        Assert.Equal(vm, page.DataContext);
    }
}