using AMFormsCST.Desktop.Views.Pages.Tools;
using AMFormsCST.Desktop.ViewModels.Pages.Tools;
using Moq;
using System.Windows;
using Xunit;
using AMFormsCST.Core.Interfaces;
using AMFormsCST.Test.Helpers;

namespace AMFormsCST.Test.Desktop.Views.Pages.Tools;
[Collection("STA Tests")]
public class FormNameGeneratorPageTests
{
    [WpfFact]
    public void Constructor_SetsViewModelAndDataContext()
    {
        // Arrange
        var mockSupportTool = new Mock<ISupportTool>();
        var vm = new FormNameGeneratorViewModel(mockSupportTool.Object);

        // Act
        var page = new FormNameGeneratorPage(vm);

        // Assert
        Assert.Equal(vm, page.ViewModel);
        Assert.Equal(vm, page.DataContext);
    }
}