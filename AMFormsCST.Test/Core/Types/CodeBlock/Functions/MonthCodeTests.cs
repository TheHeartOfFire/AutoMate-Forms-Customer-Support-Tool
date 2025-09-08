using AMFormsCST.Core.Types.CodeBlocks.Functions;
using Assert = Xunit.Assert;

namespace AMFormsCST.Test.Core.Types.CodeBlock.Functions;

public class MonthCodeTests
{
    [Fact]
    public void Constructor_InitializesPropertiesAndInputsCorrectly()
    {
        // Arrange & Act
        var codeBlock = new MonthCode();

        // Assert
        Assert.Equal("Month", codeBlock.Name);
        Assert.Equal("MONTH", codeBlock.Prefix);
        Assert.Equal("Extract the numeric month from a date", codeBlock.Description);
        Assert.Single(codeBlock.Inputs);
        Assert.Equal("Date Field", codeBlock.Inputs[0].Description);
    }

    [Fact]
    public void GetCode_WithInputSet_ReturnsCorrectString()
    {
        // Arrange
        var codeBlock = new MonthCode();
        codeBlock.SetInputValue(0, "F123"); // Date Field
        var expected = "MONTH( F123 )";

        // Act
        var result = codeBlock.GetCode();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetCode_WithNoInputSet_ReturnsStringWithEmptyParentheses()
    {
        // Arrange
        var codeBlock = new MonthCode();
        // No input is set, so it should default to an empty string.
        var expected = "MONTH( Date Field )";

        // Act
        var result = codeBlock.GetCode();

        // Assert
        Assert.Equal(expected, result);
    }
}