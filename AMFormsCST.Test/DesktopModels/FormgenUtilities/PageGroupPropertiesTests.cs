using AMFormsCST.Desktop.Models.FormgenUtilities;
using AMFormsCST.Desktop.Models.FormgenUtilities.Grouping;
using AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure;
using System.Collections.Generic;
using System.Linq;
using Xunit;

public class PageGroupPropertiesTests
{
    private FormPage MakeFormPage()
    {
        return new FormPage();
    }

    [Fact]
    public void GetDisplayProperties_ReturnsTotalPagesCount()
    {
        // Arrange
        var pages = new List<FormPage> { MakeFormPage(), MakeFormPage(), MakeFormPage() };
        var group = new PageGroup(pages);
        var props = new PageGroupProperties(group);

        // Act
        var displayProps = props.GetDisplayProperties().ToList();

        // Assert
        Assert.Single(displayProps);
        Assert.Equal("Total Pages:", displayProps[0].Name);
        Assert.Equal("3", displayProps[0].Value);
    }

    [Fact]
    public void GetDisplayProperties_HandlesEmptyPages()
    {
        // Arrange
        var group = new PageGroup(new List<FormPage>());
        var props = new PageGroupProperties(group);

        // Act
        var displayProps = props.GetDisplayProperties().ToList();

        // Assert
        Assert.Single(displayProps);
        Assert.Equal("Total Pages:", displayProps[0].Name);
        Assert.Equal("0", displayProps[0].Value);
    }
}