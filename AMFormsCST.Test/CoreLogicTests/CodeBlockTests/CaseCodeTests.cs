using AMFormsCST.Core.Types.CodeBlocks.Functions;
using Assert = Xunit.Assert;

namespace AMFormsCST.Test.CoreLogicTests.CodeBlockTests;

public class CaseCodeTests
{
    [Fact]
    public void Constructor_InitializesPropertiesAndInputsCorrectly()
    {
        // Arrange & Act
        var codeBlock = new CaseCode();

        // Assert
        Assert.Equal("Case Statement", codeBlock.Name);
        Assert.Equal("CASE", codeBlock.Prefix);
        Assert.Equal(6, codeBlock.DefaultArgCount);
        Assert.Equal(2, codeBlock.ArgIncrement);
        Assert.Equal(6, codeBlock.Inputs.Count);
        Assert.Equal("Comparison", codeBlock.Inputs[0].Description);
        Assert.Equal("Case", codeBlock.Inputs[1].Description);
        Assert.Equal("Result", codeBlock.Inputs[2].Description);
        Assert.Equal("Default", codeBlock.Inputs[5].Description);
    }

    [Fact]
    public void GetCode_WithDefaultInputs_ReturnsCorrectString()
    {
        // Arrange
        var codeBlock = new CaseCode();
        codeBlock.SetInputValue(0, "F100");
        codeBlock.SetInputValue(1, "1");
        codeBlock.SetInputValue(2, "'A'");
        codeBlock.SetInputValue(3, "2");
        codeBlock.SetInputValue(4, "'B'");
        codeBlock.SetInputValue(5, "'C'");
        var expected = "CASE(F100, 1, 'A', 2, 'B', 'C')";

        // Act
        var result = codeBlock.GetCode();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void AddExtraInputs_AddsCorrectNumberOfInputs()
    {
        // Arrange
        var codeBlock = new CaseCode(); // Starts with 6 inputs
        
        // Act
        codeBlock.AddExtraInputs(2); // Add 2 pairs (4 inputs)

        // Assert
        Assert.Equal(10, codeBlock.Inputs.Count);
        Assert.Equal("Case", codeBlock.Inputs[5].Description);
        Assert.Equal("Result", codeBlock.Inputs[6].Description);
        Assert.Equal("Default", codeBlock.Inputs[9].Description); // Verify Default is still last
    }

    [Fact]
    public void RemoveExtraInputs_RemovesCorrectNumberOfInputs()
    {
        // Arrange
        var codeBlock = new CaseCode();
        codeBlock.AddExtraInputs(3); // Total of 12 inputs
        
        // Act
        codeBlock.RemoveExtraInputs(2); // Remove 2 pairs (4 inputs)

        // Assert
        Assert.Equal(8, codeBlock.Inputs.Count);
    }

    [Fact]
    public void RemoveExtraInputs_DoesNotRemoveBelowDefaultCount()
    {
        // Arrange
        var codeBlock = new CaseCode(); // Starts with 6 inputs

        // Act
        codeBlock.RemoveExtraInputs(1); // Attempt to remove
        
        // Assert
        Assert.Equal(6, codeBlock.Inputs.Count);
    }

    [Fact]
    public void GetCode_AfterAddingInputs_ReturnsCorrectString()
    {
        // Arrange
        var codeBlock = new CaseCode();
        codeBlock.AddExtraInputs(1); // Add one "Case"/"Result" pair
        codeBlock.SetInputValue(0, "F100");
        codeBlock.SetInputValue(1, "1");
        codeBlock.SetInputValue(2, "'A'");
        codeBlock.SetInputValue(3, "2");
        codeBlock.SetInputValue(4, "'B'");
        codeBlock.SetInputValue(5, "3"); // New Case
        codeBlock.SetInputValue(6, "'C'"); // New Result
        codeBlock.SetInputValue(7, "'D'"); // Default
        var expected = "CASE(F100, 1, 'A', 2, 'B', 3, 'C', 'D')";

        // Act
        var result = codeBlock.GetCode();

        // Assert
        Assert.Equal(expected, result);
    }
}