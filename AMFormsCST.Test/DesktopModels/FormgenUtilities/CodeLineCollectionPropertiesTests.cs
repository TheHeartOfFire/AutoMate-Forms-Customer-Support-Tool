using AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure;
using AMFormsCST.Desktop.Models.FormgenUtilities;
using AMFormsCST.Desktop.Models.FormgenUtilities.Grouping;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using static AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure.CodeLineSettings;

public class CodeLineCollectionPropertiesTests
{
    private CodeLine MakeCodeLine(CodeType type, string? variable = null)
    {
        var settings = new AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure.CodeLineSettings
        {
            Order = 0,
            Type = type,
            Variable = variable
        };
        return new CodeLine { Settings = settings };
    }

    [Fact]
    public void GetDisplayProperties_ReturnsCorrectCounts()
    {
        // Arrange
        var codeLines = new List<CodeLine>
        {
            MakeCodeLine(CodeType.INIT),
            MakeCodeLine(CodeType.PROMPT, "F1"),
            MakeCodeLine(CodeType.PROMPT, "F0"),
            MakeCodeLine(CodeType.POST),
            MakeCodeLine(CodeType.POST),
        };
        var collection = new CodeLineCollection(codeLines);
        var props = new CodeLineCollectionProperties(collection);

        // Act
        var displayProps = props.GetDisplayProperties().ToList();

        // Assert
        Assert.Equal("Total Code Lines:", displayProps[0].Name);
        Assert.Equal("5", displayProps[0].Value);

        Assert.Equal("INIT Count:", displayProps[1].Name);
        Assert.Equal("1", displayProps[1].Value);

        Assert.Equal("PROMPT Count:", displayProps[2].Name);
        // Only PROMPTs where variable != "F0" (so only one)
        Assert.Equal("1", displayProps[2].Value);

        Assert.Equal("POST Count:", displayProps[3].Name);
        Assert.Equal("2", displayProps[3].Value);
    }

    [Fact]
    public void GetDisplayProperties_HandlesEmptyCollection()
    {
        // Arrange
        var collection = new CodeLineCollection(new List<CodeLine>());
        var props = new CodeLineCollectionProperties(collection);

        // Act
        var displayProps = props.GetDisplayProperties().ToList();

        // Assert
        Assert.All(displayProps, dp => Assert.Equal("0", dp.Value));
    }
}
