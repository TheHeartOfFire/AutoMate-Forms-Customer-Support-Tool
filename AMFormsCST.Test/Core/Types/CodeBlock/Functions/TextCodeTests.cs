using AMFormsCST.Core.Types.CodeBlocks.Functions;
using Assert = Xunit.Assert;

namespace AMFormsCST.Test.Core.Types.CodeBlock.Functions;

public class TextCodeTests
{
    [Fact]
    public void Constructor_InitializesPropertiesAndInputsCorrectly()
    {
        // Arrange & Act
        var codeBlock = new TextCode();

        // Assert
        Assert.Equal("Text", codeBlock.Name);
        Assert.Equal("TEXT", codeBlock.Prefix);
        Assert.Equal("Convert a Date or Numeric field to a Text field", codeBlock.Description);
        Assert.Single(codeBlock.Inputs);
        Assert.Equal("Value", codeBlock.Inputs[0].Description);
    }

    [Fact]
    public void GetCode_WithInputSet_ReturnsCorrectString()
    {
        // Arrange
        var codeBlock = new TextCode();
        codeBlock.SetInputValue(0, "F123"); // Value
        var expected = "TEXT( F123 )";

        // Act
        var result = codeBlock.GetCode();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetCode_WithNoInputSet_ReturnsStringWithEmptyParentheses()
    {
        // Arrange
        var codeBlock = new TextCode();
        // No input is set, so it should default to an empty string.
        var expected = "TEXT( Value )";

        // Act
        var result = codeBlock.GetCode();

        // Assert
        Assert.Equal(expected, result);
    }
}