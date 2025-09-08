using AMFormsCST.Core.Types.CodeBlocks.Formulae;
using Assert = Xunit.Assert;

namespace AMFormsCST.Test.Core.Types.CodeBlock.Formulae;

public class NumToTextCodeTests
{
    [Fact]
    public void Constructor_InitializesPropertiesAndInputsCorrectly()
    {
        // Arrange & Act
        var codeBlock = new NumToTextCode();
        var expectedDescription = "Converts a number to text. " +
                                  "This is needed because just using the NUM() function produces strange results in FormGen. " +
                                  "Can also be used to left justify numbers";

        // Assert
        Assert.Equal("Number to Text", codeBlock.Name);
        Assert.Equal(expectedDescription, codeBlock.Description);
        Assert.Equal(2, codeBlock.Inputs.Count);
        Assert.Equal("Number", codeBlock.Inputs[0].Description);
        Assert.Equal("Decimal Places", codeBlock.Inputs[1].Description);
    }

    [Fact]
    public void GetCode_WithAllInputsSet_ReturnsCorrectlyNestedString()
    {
        // Arrange
        var codeBlock = new NumToTextCode();
        codeBlock.SetInputValue(0, "F123"); // Number
        codeBlock.SetInputValue(1, "2");    // Decimal Places
        var expected = "TEXT( ROUND( F123, 2 ))";

        // Act
        var result = codeBlock.GetCode();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetCode_WithNoInputsSet_ReturnsStringWithEmptyFunctions()
    {
        // Arrange
        var codeBlock = new NumToTextCode();
        // No inputs are set, so they should default to empty strings.
        var expected = "TEXT( ROUND( Number, Decimal Places ))";

        // Act
        var result = codeBlock.GetCode();

        // Assert
        Assert.Equal(expected, result);
    }
}