using AMFormsCST.Core.Types.CodeBlocks.Formulae;
using Assert = Xunit.Assert;

namespace AMFormsCST.Test.Core.Types.CodeBlock.Formulae;

public class DayAndSuffixCodeTests
{
    [Fact]
    public void Constructor_InitializesPropertiesAndInputsCorrectly()
    {
        // Arrange & Act
        var codeBlock = new DayAndSuffixCode();

        // Assert
        Assert.Equal("Day and Suffix", codeBlock.Name);
        Assert.Equal("Returns the day of the month with the suffix. 11 = 11th, 12 = 12th, 13 = 13th, 21 = 21st, 22 = 22nd, etc.", codeBlock.Description);
        Assert.Single(codeBlock.Inputs);
        Assert.Equal("Numeric Day", codeBlock.Inputs[0].Description);
    }

    [Fact]
    public void GetCode_WithInputSet_ReturnsCorrectlyNestedString()
    {
        // Arrange
        var codeBlock = new DayAndSuffixCode();
        codeBlock.SetInputValue(0, "F123"); // Numeric Day field
        var expected = "TEXT( ROUND( F123, 0 )) + CASE( F123, 11, 'th', 12, 'th', 13, 'th', CASE( F123 % 10, 1, 'st', 2, 'nd', 3, 'rd', 'th' ))";

        // Act
        var result = codeBlock.GetCode();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetCode_WithNoInputSet_ReturnsStringWithEmptyValues()
    {
        // Arrange
        var codeBlock = new DayAndSuffixCode();
        // No input is set, so it should default to an empty string.
        var expected = "TEXT( ROUND( Numeric Day, 0 )) + CASE( Numeric Day, 11, 'th', 12, 'th', 13, 'th', CASE( Numeric Day % 10, 1, 'st', 2, 'nd', 3, 'rd', 'th' ))";

        // Act
        var result = codeBlock.GetCode();

        // Assert
        Assert.Equal(expected, result);
    }
}