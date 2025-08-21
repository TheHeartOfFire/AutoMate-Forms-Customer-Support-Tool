using AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure;
using AMFormsCST.Desktop.Models.FormgenUtilities.Grouping;
using System.Collections.Generic;
using Xunit;
using static AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure.CodeLineSettings;

namespace AMFormsCST.Test.Desktop.Models.FormgenUtilities.Grouping;
public class CodeLineGroupTests
{
    [Fact]
    public void Constructor_SetsTypeAndCodeLinesProperties()
    {
        // Arrange
        IEnumerable<CodeLine> codeLines = new List<CodeLine> { new CodeLine(), new CodeLine() };
        var type = CodeType.INIT;

        // Act
        var group = new CodeLineGroup(type, codeLines);

        // Assert
        Assert.Equal(type, group.Type);
        Assert.Same(codeLines, group.CodeLines);
        Assert.Equal(2, ((ICollection<CodeLine>)group.CodeLines).Count);
    }

    [Fact]
    public void CodeLinesProperty_IsEnumerable()
    {
        // Arrange
        var codeLines = new List<CodeLine> { new CodeLine(), new CodeLine() };
        var group = new CodeLineGroup(CodeType.PROMPT, codeLines);

        // Act
        var enumerated = new List<CodeLine>();
        foreach (var line in group.CodeLines)
            enumerated.Add(line);

        // Assert
        Assert.Equal(codeLines, enumerated);
    }
}