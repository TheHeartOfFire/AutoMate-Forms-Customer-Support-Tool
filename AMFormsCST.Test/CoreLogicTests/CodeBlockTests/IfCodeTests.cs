using AMFormsCST.Core.Types.CodeBlocks.Functions;
using Assert = Xunit.Assert;

namespace AMFormsCST.Test.CoreLogicTests.CodeBlockTests;

public class IfCodeTests
{
    [Fact]
    public void Constructor_InitializesPropertiesAndInputsCorrectly()
    {
        // Arrange & Act
        var codeBlock = new IfCode();

        // Assert
        Assert.Equal("If Statement", codeBlock.Name);
        Assert.Equal("IF", codeBlock.Prefix);
        Assert.Equal("If the condition is true, return the true value, otherwise return the false value.", codeBlock.Description);
        Assert.Equal(3, codeBlock.Inputs.Count);
        Assert.Equal("Condition", codeBlock.Inputs[0].Description);
        Assert.Equal("ResultIfTrue", codeBlock.Inputs[1].Description);
        Assert.Equal("ResultIfFalse", codeBlock.Inputs[2].Description);
    }

    [Fact]
    public void GetCode_WithAllInputsSet_ReturnsCorrectString()
    {
        // Arrange
        var codeBlock = new IfCode();
        codeBlock.SetInputValue(0, "F100 > 5");   // Condition
        codeBlock.SetInputValue(1, "'Yes'");      // ResultIfTrue
        codeBlock.SetInputValue(2, "'No'");       // ResultIfFalse
        var expected = "IF(F100 > 5, 'Yes', 'No')";

        // Act
        var result = codeBlock.GetCode();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetCode_WithNoInputsSet_ReturnsStringWithEmptyValues()
    {
        // Arrange
        var codeBlock = new IfCode();
        // No inputs are set, so they should default to empty strings.
        var expected = "IF(, , )";

        // Act
        var result = codeBlock.GetCode();

        // Assert
        Assert.Equal(expected, result);
    }
}