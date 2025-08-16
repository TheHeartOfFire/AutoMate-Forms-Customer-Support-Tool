using AMFormsCST.Core.Types.CodeBlocks.Formulae;
using Assert = Xunit.Assert;

namespace AMFormsCST.Test.CoreLogicTests.CodeBlockTests;

public class FuelDropdownDefaultCodeTests
{
    [Fact]
    public void Constructor_InitializesPropertiesCorrectly()
    {
        // Arrange & Act
        var codeBlock = new FuelDropdownDefaultCode();
        var expectedDescription = "This code goes in the Init screen. " +
                                  "This serves as the default value for a dropdown named \"FUELTYPE\" with \"Gas\", \"Diesel\", \"Propane\", \"Hybrid\", \"Electric\", and \"Other\" as its options.";

        // Assert
        Assert.Equal("Fuel Dropdown Default", codeBlock.Name);
        Assert.Equal(expectedDescription, codeBlock.Description);
        Assert.Empty(codeBlock.Inputs); // This code block should have no inputs.
    }

    [Fact]
    public void GetCode_ReturnsCorrectHardcodedFormula()
    {
        // Arrange
        var codeBlock = new FuelDropdownDefaultCode();
        var expected = "FUELTYPE = IF( CONTAINS( F776, 'GAS' ), 'Gas', " +
            "IF( CONTAINS( F776, 'DIESEL' ), 'Diesel', " +
            "IF( CONTAINS( F776, 'PROPANE' ), 'Propane', " +
            "IF( CONTAINS( F776, 'HYBRID' ), 'Hybrid', " +
            "IF( CONTAINS( F776, 'ELECTRIC' ), 'Electric', " +
            "IF( CONTAINS( F776, 'OTHER' ), 'Other', ' ' ))))))";

        // Act
        var result = codeBlock.GetCode();

        // Assert
        Assert.Equal(expected, result);
    }
}