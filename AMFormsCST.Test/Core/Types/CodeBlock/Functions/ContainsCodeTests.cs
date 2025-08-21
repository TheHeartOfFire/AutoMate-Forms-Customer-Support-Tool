using AMFormsCST.Core.Types.CodeBlocks.Functions;
using AMFormsCST.Core.Types.CodeBlocks;
using Moq;
using Xunit;

namespace AMFormsCST.Test.Core.Types.CodeBlock.Functions;

public class ContainsCodeTests
{
    [Fact]
    public void Constructor_InitializesPropertiesCorrectly()
    {
        // Act
        var containsCode = new ContainsCode();

        // Assert
        Assert.Equal("Contains", containsCode.Name);
        Assert.Equal("CONTAINS", containsCode.Prefix);
        Assert.Equal("Returns TRUE if TextB appears anywhere within TextA", containsCode.Description);
        Assert.Equal(2, containsCode.Inputs.Count);
        Assert.Equal("TextA", containsCode.Inputs[0].Description);
        Assert.Equal("TextB", containsCode.Inputs[1].Description);
    }

    [Fact]
    public void Inputs_AreAddedCorrectly()
    {
        // Act
        var containsCode = new ContainsCode();

        // Assert
        Assert.Collection(containsCode.Inputs,
            input => Assert.Equal("TextA", input.Description),
            input => Assert.Equal("TextB", input.Description));
    }

    [Fact]
    public void GetCode_ReturnsExpectedFormat()
    {
        // Arrange
        var containsCode = new ContainsCode();
        containsCode.Inputs[0].SetValue("Hello World");
        containsCode.Inputs[1].SetValue("World");

        // Act
        var code = containsCode.GetCode();

        // Assert
        Assert.Equal("CONTAINS( Hello World, World )", code);
    }

    [Fact]
    public void GetCode_WithCodeBaseInputs_ReturnsExpectedFormat()
    {
        // Arrange
        var mockCodeBaseA = new Mock<CodeBase>();
        mockCodeBaseA.Setup(cb => cb.GetCode()).Returns("A");
        var mockCodeBaseB = new Mock<CodeBase>();
        mockCodeBaseB.Setup(cb => cb.GetCode()).Returns("B");

        var containsCode = new ContainsCode();
        containsCode.Inputs[0].SetValue(mockCodeBaseA.Object);
        containsCode.Inputs[1].SetValue(mockCodeBaseB.Object);

        // Act
        var code = containsCode.GetCode();

        // Assert
        Assert.Equal("CONTAINS( A, B )", code);
    }
}