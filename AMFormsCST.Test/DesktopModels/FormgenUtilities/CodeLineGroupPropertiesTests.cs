using AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure;
using AMFormsCST.Desktop.Models.FormgenUtilities.Grouping;
using AMFormsCST.Desktop.Models.FormgenUtilities;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using CodeLineSettings = AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure.CodeLineSettings;
using static AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure.CodeLineSettings;

public class CodeLineGroupPropertiesTests
{
    private CodeLine MakeCodeLine(CodeType type, string? variable = null)
    {
        var settings = new CodeLineSettings
        {
            Type = type,
            Variable = variable
        };
        return new CodeLine { Settings = settings };
    }

    [Fact]
    public void GetDisplayProperties_ReturnsCorrectGroupTypeAndLineCount()
    {
        // Arrange
        var codeLines = new List<CodeLine>
        {
            MakeCodeLine(CodeType.PROMPT, "F1"),
            MakeCodeLine(CodeType.PROMPT, "F0"),
            MakeCodeLine(CodeType.PROMPT, "F2"),
            MakeCodeLine(CodeType.POST, "F0"),
            MakeCodeLine(CodeType.POST, "F3"),
        };
        var group = new CodeLineGroup(CodeType.PROMPT, codeLines);
        var props = new CodeLineGroupProperties(group);

        // Act
        var displayProps = props.GetDisplayProperties().ToList();

        // Assert
        Assert.Equal("Group Type:", displayProps[0].Name);
        Assert.Equal(CodeType.PROMPT.ToString(), displayProps[0].Value);

        Assert.Equal("Line Count:", displayProps[1].Name);
        // Only lines where variable != "F0" (so 3: F1, F2, F3)
        Assert.Equal("3", displayProps[1].Value);
    }

    [Fact]
    public void GetDisplayProperties_HandlesEmptyGroup()
    {
        // Arrange
        var group = new CodeLineGroup(CodeType.POST, new List<CodeLine>());
        var props = new CodeLineGroupProperties(group);

        // Act
        var displayProps = props.GetDisplayProperties().ToList();

        // Assert
        Assert.Equal("Group Type:", displayProps[0].Name);
        Assert.Equal(CodeType.POST.ToString(), displayProps[0].Value);
        Assert.Equal("Line Count:", displayProps[1].Name);
        Assert.Equal("0", displayProps[1].Value);
    }
}