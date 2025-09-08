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
using System.Collections.Generic;
using AMFormsCST.Core.Interfaces.Notebook;
using AMFormsCST.Core.Helpers;

namespace AMFormsCST.Test.Desktop.Views.Pages;
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

        var notebookMock = new Mock<INotebook>();
        notebookMock.Setup(n => n.Notes).Returns(new SelectableList<INote>());

        var supportToolMock = new Mock<ISupportTool>();
        supportToolMock.SetupGet(st => st.CodeBlocks).Returns(codeBlocksMock.Object);
        supportToolMock.SetupGet(st => st.Settings).Returns(settingsMock.Object);
        supportToolMock.SetupGet(st => st.Notebook).Returns(notebookMock.Object);

        var dialogServiceMock = new Mock<IDialogService>();
        var fileSystemMock = new Mock<IFileSystem>();
        var debounceServiceMock = new Mock<IDebounceService>();
        var logServiceMock = new Mock<ILogService>(); // Mock the ILogService

        var vm = new DashboardViewModel(
            supportToolMock.Object,
            dialogServiceMock.Object,
            fileSystemMock.Object,
            debounceServiceMock.Object,
            logServiceMock.Object // Pass the mocked logger
        );

        // Act
        var page = new DashboardPage(vm);

        // Assert
        Assert.Equal(vm, page.ViewModel);
        Assert.Equal(vm, page.DataContext);
    }
}