using AMFormsCST.Desktop.Views.Pages.Tools;
using Moq;
using System.Windows;
using System.Windows.Controls;
using Xunit;
using AMFormsCST.Desktop.ViewModels.Pages.Tools;
using AMFormsCST.Test.Helpers;

public class CodeSnippetsPageTests
{
    [WpfFact]
    public void Constructor_SetsViewModelAndDataContext()
    {
        // Arrange
        var vmMock = new Mock<CodeSnippetsViewModel>();

        // Act
        var page = new CodeSnippetsPage(vmMock.Object);

        // Assert
        Assert.Equal(vmMock.Object, page.ViewModel);
        Assert.Equal(vmMock.Object, page.DataContext);
    }

    [WpfFact]
    public void Button_Click_DoesNotThrow()
    {
        // Arrange
        var vmMock = new Mock<CodeSnippetsViewModel>();
        var page = new CodeSnippetsPage(vmMock.Object);
        var button = new Button();
        var args = new RoutedEventArgs(Button.ClickEvent, button);

        // Act & Assert
        var method = typeof(CodeSnippetsPage).GetMethod("Button_Click", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        method.Invoke(page, new object[] { button, args });
        // No exception thrown, method can be called.
    }
}