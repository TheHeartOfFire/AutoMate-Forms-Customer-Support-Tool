using AMFormsCST.Core.Types.CodeBlocks;
using AMFormsCST.Core.Types.CodeBlocks.Formulae;
using AMFormsCST.Core.Types.CodeBlocks.Functions;
using Assert = Xunit.Assert;

namespace AMFormsCST.Test.CoreLogicTests.CodeBlockTests;

public class CityStateZIPCodeTests
{
    [Fact]
    public void Constructor_InitializesPropertiesAndInputsCorrectly()
    {
        // Arrange & Act
        var codeBlock = new CityStateZIPCode();

        // Assert
        Assert.Equal("City, State ZIP", codeBlock.Name);
        Assert.Equal("Nicely format a city, state, and ZIP code.", codeBlock.Description);
        Assert.Equal(3, codeBlock.Inputs.Count);
        Assert.Equal("City", codeBlock.Inputs[0].Description);
        Assert.Equal("State", codeBlock.Inputs[1].Description);
        Assert.Equal("ZIP", codeBlock.Inputs[2].Description);
    }

    [Fact]
    public void GetCode_WithAllStringInputs_ReturnsCorrectlyNestedSeplistString()
    {
        // Arrange
        var codeBlock = new CityStateZIPCode();
        codeBlock.SetInputValue(0, "F100"); // City
        codeBlock.SetInputValue(1, "F101"); // State
        codeBlock.SetInputValue(2, "F102"); // ZIP
        var expected = "SEPLIST( ' ', SEPLIST( ', ', F100, F101 ), F102 )";

        // Act
        var result = codeBlock.GetCode();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetCode_WithUnsetStringInput_HandlesGracefully()
    {
        // Arrange
        var codeBlock = new CityStateZIPCode();
        codeBlock.SetInputValue(0, "F100"); // City
        // State is intentionally left with its default value
        codeBlock.SetInputValue(2, "F102"); // ZIP
        var expected = "SEPLIST( ' ', SEPLIST( ', ', F100, Item ), F102 )";

        // Act
        var result = codeBlock.GetCode();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetCode_WithDefaultInputs_ReturnsStringWithEmptyValues()
    {
        // Arrange
        var codeBlock = new CityStateZIPCode();
        // No inputs are set, so they should use their default empty string values.
        var expected = "SEPLIST( ' ', SEPLIST( ', ', Item, Item ), Item )";

        // Act
        var result = codeBlock.GetCode();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetCode_WithCodeBaseAsInput_TreatsInputAsEmptyString()
    {
        // Arrange
        var codeBlock = new CityStateZIPCode();
        var cityCodeBlock = new TextCode(); // A simple CodeBase object
        cityCodeBlock.SetInputValue(0, "MyCity");

        codeBlock.SetInputValue(0, cityCodeBlock); // Set City input as a CodeBase
        codeBlock.SetInputValue(1, "TX");          // State
        codeBlock.SetInputValue(2, "12345");       // ZIP

        // Because CityStateZIPCode's GetCode casts inputs using 'as string', the cityCodeBlock
        // will be treated as null and then converted to an empty string.
        var expected = "SEPLIST( ' ', SEPLIST( ', ', Item, TX ), 12345 )";

        // Act
        var result = codeBlock.GetCode();

        // Assert
        Assert.Equal(expected, result);
    }
}