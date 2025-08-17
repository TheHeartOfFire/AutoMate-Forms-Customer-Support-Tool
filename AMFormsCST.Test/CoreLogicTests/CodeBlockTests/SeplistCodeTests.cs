using AMFormsCST.Core.Types.CodeBlocks.Functions;
using Assert = Xunit.Assert;

namespace AMFormsCST.Test.CoreLogicTests.CodeBlockTests;

public class SeplistCodeTests
{
    [Fact]
    public void Constructor_InitializesPropertiesAndInputsCorrectly()
    {
        // Arrange & Act
        var codeBlock = new SeplistCode();

        // Assert
        Assert.Equal("Seplist", codeBlock.Name);
        Assert.Equal("SEPLIST", codeBlock.Prefix);
        Assert.Equal("Separate a list of items with the given separator.", codeBlock.Description);
        Assert.Equal(3, codeBlock.DefaultArgCount);
        Assert.Equal(1, codeBlock.ArgIncrement);
        Assert.Equal(3, codeBlock.Inputs.Count);
        Assert.Equal("Separator", codeBlock.Inputs[0].Description);
        Assert.Equal("Item", codeBlock.Inputs[1].Description);
        Assert.Equal("Item", codeBlock.Inputs[2].Description);
    }

    [Fact]
    public void GetCode_WithDefaultInputs_ReturnsCorrectString()
    {
        // Arrange
        var codeBlock = new SeplistCode();
        codeBlock.SetInputValue(0, "' '");
        codeBlock.SetInputValue(1, "F100");
        codeBlock.SetInputValue(2, "F101");
        var expected = "SEPLIST( ' ', F100, F101 )";

        // Act
        var result = codeBlock.GetCode();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void AddExtraInputs_AddsCorrectNumberOfInputs()
    {
        // Arrange
        var codeBlock = new SeplistCode(); // Starts with 3 inputs

        // Act
        codeBlock.AddExtraInputs(2); // Add 2 more "Item" inputs

        // Assert
        Assert.Equal(5, codeBlock.Inputs.Count);
        Assert.Equal("Item", codeBlock.Inputs[3].Description);
        Assert.Equal("Item", codeBlock.Inputs[4].Description);
    }

    [Fact]
    public void RemoveExtraInputs_RemovesCorrectNumberOfInputs()
    {
        // Arrange
        var codeBlock = new SeplistCode();
        codeBlock.AddExtraInputs(3); // Total of 6 inputs

        // Act
        codeBlock.RemoveExtraInputs(2); // Remove 2 inputs

        // Assert
        Assert.Equal(4, codeBlock.Inputs.Count);
    }

    [Fact]
    public void RemoveExtraInputs_DoesNotRemoveBelowDefaultCount()
    {
        // Arrange
        var codeBlock = new SeplistCode(); // Starts with 3 inputs

        // Act
        codeBlock.RemoveExtraInputs(1); // Attempt to remove

        // Assert
        Assert.Equal(3, codeBlock.Inputs.Count);
    }

    [Fact]
    public void GetCode_AfterAddingInputs_ReturnsCorrectString()
    {
        // Arrange
        var codeBlock = new SeplistCode();
        codeBlock.AddExtraInputs(2); // Total of 5 inputs
        codeBlock.SetInputValue(0, "'-'");
        codeBlock.SetInputValue(1, "A");
        codeBlock.SetInputValue(2, "B");
        codeBlock.SetInputValue(3, "C");
        codeBlock.SetInputValue(4, "D");
        var expected = "SEPLIST( '-', A, B, C, D )";

        // Act
        var result = codeBlock.GetCode();

        // Assert
        Assert.Equal(expected, result);
    }
}