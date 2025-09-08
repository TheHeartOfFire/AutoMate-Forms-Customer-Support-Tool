using AMFormsCST.Core.Types.CodeBlocks;
using Assert = Xunit.Assert;

namespace AMFormsCST.Test.Core.Types.CodeBlockTests;

public class CodeInputTests
{
    // A minimal concrete implementation of CodeBase for testing purposes.
    private class TestCodeBlock : CodeBase { }

    [Fact]
    public void StringConstructor_InitializesPropertiesCorrectly()
    {
        // Arrange
        var value = "TestValue";
        var description = "A test input";
        var index = 1;

        // Act
        var codeInput = new CodeInput(value, description, index);

        // Assert
        Assert.Equal(value, codeInput.Value);
        Assert.IsType<string>(codeInput.Value);
        Assert.Equal(description, codeInput.Description);
        Assert.Equal(index, codeInput.Index);
    }

    [Fact]
    public void CodeBaseConstructor_InitializesPropertiesCorrectly()
    {
        // Arrange
        var value = new TestCodeBlock();
        var description = "A nested code block";
        var index = 2;

        // Act
        var codeInput = new CodeInput(value, description, index);

        // Assert
        Assert.Same(value, codeInput.Value);
        Assert.IsType<TestCodeBlock>(codeInput.Value);
        Assert.Equal(description, codeInput.Description);
        Assert.Equal(index, codeInput.Index);
    }

    [Fact]
    public void SetValue_WithString_UpdatesValueCorrectly()
    {
        // Arrange
        var codeInput = new CodeInput("Initial", "Desc", 0);
        var newValue = "UpdatedValue";

        // Act
        codeInput.SetValue(newValue);

        // Assert
        Assert.Equal(newValue, codeInput.Value);
        Assert.IsType<string>(codeInput.Value);
    }

    [Fact]
    public void SetValue_WithCodeBase_UpdatesValueCorrectly()
    {
        // Arrange
        var codeInput = new CodeInput("Initial", "Desc", 0);
        var newValue = new TestCodeBlock();

        // Act
        codeInput.SetValue(newValue);

        // Assert
        Assert.Same(newValue, codeInput.Value);
        Assert.IsType<TestCodeBlock>(codeInput.Value);
    }
}