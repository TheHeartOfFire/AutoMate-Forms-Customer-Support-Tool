using AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure;
using AMFormsCST.Desktop.Models.FormgenUtilities.Grouping;
using System.Collections.Generic;
using Xunit;

namespace AMFormsCST.Test.Desktop.Models.FormgenUtilities.Grouping;
public class CodeLineCollectionTests
{

    [Fact]
    public void Constructor_SetsCodeLinesProperty()
    {
        // Arrange
        var codeLines = new List<CodeLine> { new CodeLine(), new CodeLine() };

        // Act
        var collection = new CodeLineCollection(codeLines);

        // Assert
        Assert.Same(codeLines, collection.CodeLines);
        Assert.Equal(2, ((ICollection<CodeLine>)collection.CodeLines).Count);
    }

    [Fact]
    public void CodeLinesProperty_IsEnumerable()
    {
        // Arrange
        var codeLines = new List<CodeLine> { new CodeLine(), new CodeLine() };
        var collection = new CodeLineCollection(codeLines);

        // Act
        var enumerated = new List<CodeLine>();
        foreach (var line in collection.CodeLines)
            enumerated.Add(line);

        // Assert
        Assert.Equal(codeLines, enumerated);
    }
}