using AMFormsCST.Desktop.Views.Pages.Tools;
using AMFormsCST.Desktop.ViewModels.Pages.Tools;
using Moq;
using System.Collections.Generic;
using Xunit;
using AMFormsCST.Core.Interfaces;
using AMFormsCST.Core.Interfaces.Utils;
using AMFormsCST.Core.Types.BestPractices.TextTemplates.Models;
using AMFormsCST.Test.Helpers;

[Collection("STA Tests")]
public class TemplatesPageTests
{
    [WpfFact]
    public void Constructor_SetsViewModelAndDataContext()
    {
        // Arrange
        var supportToolMock = new Mock<ISupportTool>();

        // Setup Enforcer.Templates to return an empty list or test data
        var enforcerMock = new Mock<IBestPracticeEnforcer>();
        var fileSystemMock = new Mock<IFileSystem>();
        enforcerMock.SetupGet(e => e.Templates).Returns(new List<TextTemplate>());
        supportToolMock.SetupGet(s => s.Enforcer).Returns(enforcerMock.Object);

        var vm = new TemplatesViewModel(supportToolMock.Object, fileSystemMock.Object);

        // Act
        var page = new TemplatesPage(vm);

        // Assert
        Assert.Equal(vm, page.ViewModel);
        Assert.Equal(vm, page.DataContext);
    }
}