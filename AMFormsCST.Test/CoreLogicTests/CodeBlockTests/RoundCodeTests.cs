using AMFormsCST.Core.Types.CodeBlocks.Functions;
using Assert = Xunit.Assert;

namespace AMFormsCST.Test.CoreLogicTests.CodeBlockTests;

public class RoundCodeTests
{
    [Fact]
    public void Constructor_InitializesPropertiesAndInputsCorrectly()
    {
        // Arrange & Act
        var codeBlock = new RoundCode();

        // Assert
        Assert.Equal("Round", codeBlock.Name);
        Assert.Equal("ROUND", codeBlock.Prefix);
        Assert.Equal("Round a number to a specified number of decimal places. Uses nearest rounding. i.e. 0.5 rounds to 1.0, 0.4 rounds to 0.0", codeBlock.Description);
        Assert.Equal(2, codeBlock.Inputs.Count);
        Assert.Equal("Number", codeBlock.Inputs[0].Description);
        Assert.Equal("Decimal Places", codeBlock.Inputs[1].Description);
    }

    [Fact]
    public void GetCode_WithAllInputsSet_ReturnsCorrectString()
    {
        // Arrange
        var codeBlock = new RoundCode();
        codeBlock.SetInputValue(0, "F123"); // Number
        codeBlock.SetInputValue(1, "2");    // Decimal Places
        var expected = "ROUND( F123, 2 )";

        // Act
        var result = codeBlock.GetCode();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetCode_WithNoInputsSet_ReturnsStringWithEmptyValues()
    {
        // Arrange
        var codeBlock = new RoundCode();
        // No inputs are set, so they should default to empty strings.
        var expected = "ROUND( Number, Decimal Places )";

        // Act
        var result = codeBlock.GetCode();

        // Assert
        Assert.Equal(expected, result);
    }
}