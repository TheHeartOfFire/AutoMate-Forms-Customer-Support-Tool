using AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure;
using AMFormsCST.Desktop.Models.FormgenUtilities.Grouping;
using System.Collections.Generic;
using Xunit;

public class PageGroupTests
{
    [Fact]
    public void Constructor_SetsPagesProperty()
    {
        // Arrange
        var pages = new List<FormPage> { new FormPage(), new FormPage() };

        // Act
        var group = new PageGroup(pages);

        // Assert
        Assert.Same(pages, group.Pages);
        Assert.Equal(2, ((ICollection<FormPage>)group.Pages).Count);
    }

    [Fact]
    public void PagesProperty_IsEnumerable()
    {
        // Arrange
        var pages = new List<FormPage> { new FormPage(), new FormPage() };
        var group = new PageGroup(pages);

        // Act
        var enumerated = new List<FormPage>();
        foreach (var page in group.Pages)
            enumerated.Add(page);

        // Assert
        Assert.Equal(pages, enumerated);
    }
}