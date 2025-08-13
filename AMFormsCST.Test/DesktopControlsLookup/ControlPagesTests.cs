using AMFormsCST.Desktop.ControlsLookup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

// Dummy attribute and type for testing
public class TestGalleryPageAttribute : Attribute
{
    public string Description { get; }
    public object Icon { get; }
    public TestGalleryPageAttribute(string description, object icon)
    {
        Description = description;
        Icon = icon;
    }
}

public class DummyPage { }
public class DummyGalleryPage : DummyPage
{
    public static string Namespace => typeof(DummyGalleryPage).Namespace;
}

public class ControlPagesTests
{
    [Fact]
    public void All_ReturnsGalleryPagesWithCorrectProperties()
    {
        // Arrange
        // Create a dummy type with the GalleryPageAttribute
        var dummyType = typeof(DummyGalleryPage);
        var attr = new GalleryPageAttribute("Test Description", Wpf.Ui.Controls.SymbolRegular.Checkmark24);

        // Use reflection to add the attribute to the type (not possible at runtime, so this is illustrative)
        // In practice, use a real type decorated with [GalleryPageAttribute] in your test assembly.

        // Act
        var pages = ControlPages.All().ToList();

        // Assert
        Assert.All(pages, page =>
        {
            Assert.False(string.IsNullOrEmpty(page.Name));
            Assert.NotNull(page.Description);
            Assert.NotNull(page.PageType);
            Assert.EndsWith("Gallery", page.PageType.Name); // Should match the suffix logic
        });
    }

    [Fact]
    public void FromNamespace_FiltersPagesByNamespace()
    {
        // Arrange
        var targetNamespace = typeof(DummyGalleryPage).Namespace;

        // Act
        var pages = ControlPages.FromNamespace(targetNamespace).ToList();

        // Assert
        Assert.All(pages, page =>
        {
            Assert.StartsWith(targetNamespace, page.PageType.Namespace);
        });
    }
}