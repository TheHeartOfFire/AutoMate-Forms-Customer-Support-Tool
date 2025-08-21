using AMFormsCST.Desktop.ControlsLookup;
using System.Linq;
using Xunit;

namespace AMFormsCST.Test.Desktop.ControlsLookup;
public class ControlPagesTests
{
    [Fact]
    public void All_ReturnsGalleryPagesWithCorrectProperties()
    {
        // Act
        var pages = ControlPages.All().ToList();

        // Assert
        Assert.All(pages, page =>
        {
            Assert.False(string.IsNullOrEmpty(page.Name));
            Assert.NotNull(page.Description);
            Assert.NotNull(page.PageType);
            Assert.EndsWith("Page", page.PageType.Name);
        });
    }

    [Fact]
    public void FromNamespace_FiltersPagesByNamespace()
    {
        // Arrange
        var targetNamespace = "AMFormsCST.Desktop.Views.Pages.Tools";

        // Act
        var pages = ControlPages.FromNamespace(targetNamespace).ToList();

        // Assert
        Assert.All(pages, page =>
        {
            Assert.StartsWith(targetNamespace, page.PageType.Namespace);
        });
    }
}