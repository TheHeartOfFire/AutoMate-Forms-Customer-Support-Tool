using AMFormsCST.Desktop.Views.Pages;
using AMFormsCST.Desktop.ViewModels.Pages;
using Moq;
using Xunit;
using AMFormsCST.Core.Interfaces;
using AMFormsCST.Desktop.Services;
using AMFormsCST.Core.Interfaces.Utils;
using AMFormsCST.Core.Interfaces.CodeBlocks;
using AMFormsCST.Core.Types.CodeBlocks;
using AMFormsCST.Core.Interfaces.UserSettings;
using AMFormsCST.Test.Helpers;

[Collection("STA Tests")]
public class DashboardPageTests
{
    [WpfFact]
    public void Constructor_SetsViewModelAndDataContext()
    {
        // Arrange
        var codeBaseMock = new Mock<ICodeBase>();
        codeBaseMock.SetupGet(cb => cb.Name).Returns("TestName");
        codeBaseMock.SetupGet(cb => cb.Description).Returns("TestDescription");
        codeBaseMock.Setup(cb => cb.GetCode()).Returns("TestCode");
        codeBaseMock.SetupGet(cb => cb.Inputs).Returns(new List<CodeInput>());

        var codeBlocksMock = new Mock<ICodeBlocks>();
        codeBlocksMock.Setup(cb => cb.GetBlocks()).Returns(new List<ICodeBase> { codeBaseMock.Object });

        var userSettingsMock = new Mock<IUserSettings>();
        userSettingsMock.SetupGet(us => us.ExtSeparator).Returns("|");

        var settingsMock = new Mock<ISettings>();
        settingsMock.SetupGet(s => s.UserSettings).Returns(userSettingsMock.Object);

        var supportToolMock = new Mock<ISupportTool>();
        supportToolMock.SetupGet(st => st.CodeBlocks).Returns(codeBlocksMock.Object);
        supportToolMock.SetupGet(st => st.Settings).Returns(settingsMock.Object);

        var dialogServiceMock = new Mock<IDialogService>();
        var fileSystemMock = new Mock<IFileSystem>();

        var vm = new DashboardViewModel(
            supportToolMock.Object,
            dialogServiceMock.Object,
            fileSystemMock.Object
        );

        // Act
        var page = new DashboardPage(vm);

        // Assert
        Assert.Equal(vm, page.ViewModel);
        Assert.Equal(vm, page.DataContext);
    }
}