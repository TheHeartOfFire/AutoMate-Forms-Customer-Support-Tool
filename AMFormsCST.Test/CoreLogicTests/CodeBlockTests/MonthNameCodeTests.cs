using AMFormsCST.Core.Types.CodeBlocks;
using AMFormsCST.Core.Types.CodeBlocks.Formulae;
using AMFormsCST.Core.Types.CodeBlocks.Functions;
using Assert = Xunit.Assert;

namespace AMFormsCST.Test.CoreLogicTests.CodeBlockTests;

public class MonthNameCodeTests
{
    [Fact]
    public void Constructor_InitializesPropertiesAndInputsCorrectly()
    {
        // Arrange & Act
        var codeBlock = new MonthNameCode();

        // Assert
        Assert.Equal("Month Names", codeBlock.Name);
        Assert.Equal("Get the name of the specified month. 1 = January, 2 = February, etc.", codeBlock.Description);
        Assert.Single(codeBlock.Inputs);
        Assert.Equal("Date Field", codeBlock.Inputs[0].Description);
    }

    [Fact]
    public void GetCode_WithInputSet_ReturnsCorrectCaseStatement()
    {
        // Arrange
        var codeBlock = new MonthNameCode();
        codeBlock.SetInputValue(0, "MONTH( F123 )");
        var expected = "CASE( MONTH( F123 ), 1, 'JANUARY', 2, 'FEBRUARY', 3, 'MARCH', " +
            "4, 'APRIL', 5, 'MAY', 6, 'JUNE', 7, 'JULY', 8, 'AUGUST', 9, 'SEPTEMBER', " +
            "10, 'OCTOBER', 11, 'NOVEMBER', 12, 'DECEMBER', 'Invalid Month' )";

        // Act
        var result = codeBlock.GetCode();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetCode_WithNoInputSet_ReturnsCaseStatementWithEmptyFirstArg()
    {
        // Arrange
        var codeBlock = new MonthNameCode();
        // No input is set, so it should default to an empty string.
        var expected = "CASE( , 1, 'JANUARY', 2, 'FEBRUARY', 3, 'MARCH', " +
            "4, 'APRIL', 5, 'MAY', 6, 'JUNE', 7, 'JULY', 8, 'AUGUST', 9, 'SEPTEMBER', " +
            "10, 'OCTOBER', 11, 'NOVEMBER', 12, 'DECEMBER', 'Invalid Month' )";

        // Act
        var result = codeBlock.GetCode();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetCode_WithCodeBaseAsInput_TreatsInputAsEmptyString()
    {
        // Arrange
        var codeBlock = new MonthNameCode();
        var monthCode = new MonthCode(); // A simple CodeBase object
        monthCode.SetInputValue(0, "F123");

        codeBlock.SetInputValue(0, monthCode); // Set input as a CodeBase

        // Because MonthNameCode's GetCode casts inputs using 'as string', the monthCode object
        // will be treated as null and then converted to an empty string.
        var expected = "CASE(, 1, 'JANUARY', 2, 'FEBRUARY', 3, 'MARCH', 4, 'APRIL', 5, 'MAY', 6, 'JUNE', 7, 'JULY', 8, 'AUGUST', 9, 'SEPTEMBER', 10, 'OCTOBER', 11, 'NOVEMBER', 12, 'DECEMBER', 'Invalid Month')";

        // Act
        var result = codeBlock.GetCode();

        // Assert
        Assert.Equal(expected, result);
    }
}