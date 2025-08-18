using AMFormsCST.Desktop.Views.Pages.Tools;
using AMFormsCST.Desktop.ViewModels.Pages.Tools;
using AMFormsCST.Desktop.Services;
using Moq;
using System.Windows;
using Xunit;
using Wpf.Ui;
using Wpf.Ui.Controls;
using AMFormsCST.Test.Helpers;

[Collection("STA Tests")]
public class ToolsPageTests
{
    [WpfFact]
    public void Constructor_SetsViewModelAndDataContext()
    {
        // Arrange
        var vmMock = new Mock<ToolsViewModel>();
        var navServiceMock = new Mock<INavigationService>();

        // Act
        var page = new ToolsPage(vmMock.Object, navServiceMock.Object);

        // Assert
        Assert.Equal(vmMock.Object, page.ViewModel);
        Assert.Equal(page, page.DataContext);
    }
}