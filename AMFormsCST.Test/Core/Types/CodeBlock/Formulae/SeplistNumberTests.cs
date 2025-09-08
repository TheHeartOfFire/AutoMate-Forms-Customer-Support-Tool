using AMFormsCST.Core.Types.CodeBlocks.Formulae;
using Assert = Xunit.Assert;

namespace AMFormsCST.Test.Core.Types.CodeBlock.Formulae;

public class SeplistNumberTests
{
    [Fact]
    public void Constructor_InitializesPropertiesAndInputsCorrectly()
    {
        // Arrange & Act
        var codeBlock = new SeplistNumber();

        // Assert
        Assert.Equal("Seplist Number", codeBlock.Name);
        Assert.Equal("Allow Numeric fields to be used in seplist.", codeBlock.Description);
        Assert.Equal(2, codeBlock.Inputs.Count);
        Assert.Equal("Numeric Field", codeBlock.Inputs[0].Description);
        Assert.Equal("Decimal Places", codeBlock.Inputs[1].Description);
    }

    [Fact]
    public void GetCode_WithAllInputsSet_ReturnsCorrectlyNestedString()
    {
        // Arrange
        var codeBlock = new SeplistNumber();
        codeBlock.SetInputValue(0, "F123"); // Numeric Field
        codeBlock.SetInputValue(1, "2");    // Decimal Places
        var expected = "IF( F123 != 0, ROUND( F123, 2 ), '' )";

        // Act
        var result = codeBlock.GetCode();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetCode_WithNoInputsSet_ReturnsStringWithEmptyFunctions()
    {
        // Arrange
        var codeBlock = new SeplistNumber();
        // No inputs are set, so they should default to empty strings.
        var expected = "IF( Numeric Field != 0, ROUND( Numeric Field, Decimal Places ), '' )";

        // Act
        var result = codeBlock.GetCode();

        // Assert
        Assert.Equal(expected, result);
    }
}