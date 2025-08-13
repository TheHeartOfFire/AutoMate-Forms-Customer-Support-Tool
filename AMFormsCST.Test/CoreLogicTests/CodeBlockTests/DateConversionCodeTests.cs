using AMFormsCST.Core.Types.CodeBlocks.Formulae;
using Assert = Xunit.Assert;

namespace AMFormsCST.Test.CoreLogicTests.CodeBlockTests;

public class DateConversionCodeTests
{
    [Fact]
    public void Constructor_InitializesPropertiesAndInputsCorrectly()
    {
        // Arrange & Act
        var codeBlock = new DateConversionCode();

        // Assert
        Assert.Equal("Date Conversion", codeBlock.Name);
        Assert.Equal("Converts a date in the format of MM/DD/YYYY to a date in the format of MM/DD/YY.", codeBlock.Description);
        Assert.Single(codeBlock.Inputs);
        Assert.Equal("Date Field", codeBlock.Inputs[0].Description);
    }

    [Fact]
    public void GetCode_WithInputSet_ReturnsCorrectlyNestedString()
    {
        // Arrange
        var codeBlock = new DateConversionCode();
        codeBlock.SetInputValue(0, "F123"); // Date Field
        var expected = "SEPLIST('/', MONTH(F123), DAY(F123), YEAR(F123) % 100)";

        // Act
        var result = codeBlock.GetCode();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetCode_WithNoInputSet_ReturnsStringWithEmptyFunctions()
    {
        // Arrange
        var codeBlock = new DateConversionCode();
        // No input is set, so it should default to an empty string.
        var expected = "SEPLIST('/', MONTH(), DAY(), YEAR() % 100)";

        // Act
        var result = codeBlock.GetCode();

        // Assert
        Assert.Equal(expected, result);
    }
}