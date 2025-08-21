using AMFormsCST.Core.Types.CodeBlocks.Formulae;
using Assert = Xunit.Assert;

namespace AMFormsCST.Test.Core.Types.CodeBlock.Formulae;

public class DmvCalculationCodeTests
{
    [Fact]
    public void Constructor_InitializesPropertiesCorrectly()
    {
        // Arrange & Act
        var codeBlock = new DmvCalculationCode();
        var expectedDescription = "The sum of taxable costs in the deal. " +
                                  "Selling Price + Taxed Accessory Total Price + Taxed Fees Total Amount + Taxed GAP Amount + Taxed Service Contracts Total Amount - Non-Taxed Rebates Total Amount - Total Non-taxable Trade Allowance Amount";

        // Assert
        Assert.Equal("DMV Calculation", codeBlock.Name);
        Assert.Equal(expectedDescription, codeBlock.Description);
        Assert.Empty(codeBlock.Inputs); // This code block should have no inputs.
    }

    [Fact]
    public void GetCode_ReturnsCorrectHardcodedFormula()
    {
        // Arrange
        var codeBlock = new DmvCalculationCode();
        var expected = "F3 + F1953 + F1979 + F1997 + F1983 - F2008 - F2493";

        // Act
        var result = codeBlock.GetCode();

        // Assert
        Assert.Equal(expected, result);
    }
}