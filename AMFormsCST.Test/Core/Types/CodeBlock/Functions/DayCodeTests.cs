using AMFormsCST.Core.Types.CodeBlocks.Functions;
using Assert = Xunit.Assert;

namespace AMFormsCST.Test.Core.Types.CodeBlock.Functions;

public class DayCodeTests
{
    [Fact]
    public void Constructor_InitializesPropertiesAndInputsCorrectly()
    {
        // Arrange & Act
        var codeBlock = new DayCode();

        // Assert
        Assert.Equal("Day", codeBlock.Name);
        Assert.Equal("DAY", codeBlock.Prefix);
        Assert.Equal("Extract the numeric day from a date", codeBlock.Description);
        Assert.Single(codeBlock.Inputs);
        Assert.Equal("Date Field", codeBlock.Inputs[0].Description);
    }

    [Fact]
    public void GetCode_WithInputSet_ReturnsCorrectString()
    {
        // Arrange
        var codeBlock = new DayCode();
        codeBlock.SetInputValue(0, "F123"); // Date Field
        var expected = "DAY( F123 )";

        // Act
        var result = codeBlock.GetCode();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetCode_WithNoInputSet_ReturnsStringWithEmptyParentheses()
    {
        // Arrange
        var codeBlock = new DayCode();
        // No input is set, so it should default to an empty string.
        var expected = "DAY( Date Field )";

        // Act
        var result = codeBlock.GetCode();

        // Assert
        Assert.Equal(expected, result);
    }
}