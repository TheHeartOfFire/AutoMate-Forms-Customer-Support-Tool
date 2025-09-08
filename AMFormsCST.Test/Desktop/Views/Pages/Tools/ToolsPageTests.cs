using AMFormsCST.Core.Interfaces;
using AMFormsCST.Desktop.ViewModels.Pages.Tools;
using AMFormsCST.Desktop.Views.Pages.Tools;
using AMFormsCST.Test.Helpers;
using Moq;
using Wpf.Ui;
using Xunit;

namespace AMFormsCST.Test.Desktop.Views.Pages.Tools;

[Collection("STA Tests")]
public class ToolsPageTests
{
    [WpfFact]
    public void Constructor_SetsViewModelAndDataContext()
    {
        // Arrange
        // 1. Mock the dependencies needed by the ToolsViewModel.
        var mockNavService = new Mock<INavigationService>();

        // 2. Instantiate the ToolsViewModel with its required dependencies.
        // The ILogService is optional and not needed for this test.
        var viewModel = new ToolsViewModel();

        // Act
        // 3. Create the ToolsPage with the real ViewModel instance.
        var page = new ToolsPage(viewModel, mockNavService.Object);

        // Assert
        // 4. Verify that the page's ViewModel and DataContext are correctly set.
        Assert.Equal(viewModel, page.ViewModel);
        Assert.Equal(page, page.DataContext);
    }
}