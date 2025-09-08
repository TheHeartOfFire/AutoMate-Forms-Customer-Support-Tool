using AMFormsCST.Desktop.ControlsLookup;
using System;
using Wpf.Ui.Controls;
using Xunit;

namespace AMFormsCST.Test.Desktop.ControlsLookup;
public class GalleryPageTests
{
    [Fact]
    public void Constructor_SetsPropertiesCorrectly()
    {
        // Arrange
        var name = "Templates";
        var description = "Manage text templates";
        var icon = SymbolRegular.MailTemplate24;
        var pageType = typeof(object);

        // Act
        var page = new GalleryPage(name, description, icon, pageType);

        // Assert
        Assert.Equal(name, page.Name);
        Assert.Equal(description, page.Description);
        Assert.Equal(icon, page.Icon);
        Assert.Equal(pageType, page.PageType);
    }

    [Fact]
    public void Records_WithSameValues_AreEqual()
    {
        // Arrange
        var name = "Templates";
        var description = "Manage text templates";
        var icon = SymbolRegular.MailTemplate24;
        var pageType = typeof(object);

        var page1 = new GalleryPage(name, description, icon, pageType);
        var page2 = new GalleryPage(name, description, icon, pageType);

        // Assert
        Assert.Equal(page1, page2);
        Assert.True(page1 == page2);
        Assert.False(page1 != page2);
        Assert.Equal(page1.GetHashCode(), page2.GetHashCode());
    }

    [Fact]
    public void Records_WithDifferentValues_AreNotEqual()
    {
        // Arrange
        var page1 = new GalleryPage("Templates", "Desc1", SymbolRegular.MailTemplate24, typeof(object));
        var page2 = new GalleryPage("Other", "Desc2", SymbolRegular.Code20, typeof(string));

        // Assert
        Assert.NotEqual(page1, page2);
        Assert.True(page1 != page2);
        Assert.False(page1 == page2);
    }

    [Fact]
    public void Deconstruct_SetsOutVariables()
    {
        // Arrange
        var page = new GalleryPage("Templates", "Desc", SymbolRegular.MailTemplate24, typeof(object));

        // Act
        var (name, description, icon, pageType) = page;

        // Assert
        Assert.Equal(page.Name, name);
        Assert.Equal(page.Description, description);
        Assert.Equal(page.Icon, icon);
        Assert.Equal(page.PageType, pageType);
    }
}