using AMFormsCST.Core.Types.CodeBlocks.Functions;
using Assert = Xunit.Assert;

namespace AMFormsCST.Test.CoreLogicTests.CodeBlockTests;

public class YearCodeTests
{
    [Fact]
    public void Constructor_InitializesPropertiesAndInputsCorrectly()
    {
        // Arrange & Act
        var codeBlock = new YearCode();

        // Assert
        Assert.Equal("Year", codeBlock.Name);
        Assert.Equal("YEAR", codeBlock.Prefix);
        Assert.Equal("Extract the numeric year from a date", codeBlock.Description);
        Assert.Single(codeBlock.Inputs);
        Assert.Equal("Date Field", codeBlock.Inputs[0].Description);
    }

    [Fact]
    public void GetCode_WithInputSet_ReturnsCorrectString()
    {
        // Arrange
        var codeBlock = new YearCode();
        codeBlock.SetInputValue(0, "F123"); // Date Field
        var expected = "YEAR( F123 )";

        // Act
        var result = codeBlock.GetCode();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetCode_WithNoInputSet_ReturnsStringWithEmptyParentheses()
    {
        // Arrange
        var codeBlock = new YearCode();
        // No input is set, so it should default to an empty string.
        var expected = "YEAR( Date Field )";

        // Act
        var result = codeBlock.GetCode();

        // Assert
        Assert.Equal(expected, result);
    }
} 