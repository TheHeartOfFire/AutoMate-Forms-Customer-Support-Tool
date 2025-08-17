using AMFormsCST.Core.Types.CodeBlocks;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace AMFormsCST.Test.CoreLogicTests.CodeBlockTests;

public class CodeBaseTests
{
    [Fact]
    public void AddInput_WithStringDescription_AddsInputCorrectly()
    {
        var mock = new Mock<CodeBase> { CallBase = true };
        var codeBlock = mock.Object;

        codeBlock.AddInput("First Input");
        codeBlock.AddInput("Second Input");

        Assert.Equal(2, codeBlock.InputCount());
        Assert.Equal("First Input", codeBlock.Inputs[0].Description);
        Assert.Equal(0, codeBlock.Inputs[0].Index);
        Assert.Equal("Second Input", codeBlock.Inputs[1].Description);
        Assert.Equal(1, codeBlock.Inputs[1].Index);
    }

    [Fact]
    public void AddInput_AtIndex_InsertsAndShiftsIndices()
    {
        var mock = new Mock<CodeBase> { CallBase = true };
        var codeBlock = mock.Object;
        codeBlock.AddInput("Input A");
        codeBlock.AddInput("Input C");

        codeBlock.AddInput(1, "Input B");

        Assert.Equal(3, codeBlock.InputCount());
        Assert.Equal("Input A", codeBlock.Inputs.Find(i => i.Index == 0)?.Description);
        Assert.Equal("Input B", codeBlock.Inputs.Find(i => i.Index == 1)?.Description);
        Assert.Equal("Input C", codeBlock.Inputs.Find(i => i.Index == 2)?.Description);
    }

    [Fact]
    public void RemoveInput_RemovesAndShiftsIndices()
    {
        var mock = new Mock<CodeBase> { CallBase = true };
        var codeBlock = mock.Object;
        codeBlock.AddInput("Input A");
        codeBlock.AddInput("Input B");
        codeBlock.AddInput("Input C");

        codeBlock.RemoveInput(1);

        Assert.Equal(2, codeBlock.InputCount());
        Assert.Equal("Input A", codeBlock.Inputs.Find(i => i.Index == 0)?.Description);
        Assert.Equal("Input C", codeBlock.Inputs.Find(i => i.Index == 1)?.Description);
    }

    [Fact]
    public void SetInputValue_UpdatesCorrectInput()
    {
        var mock = new Mock<CodeBase> { CallBase = true };
        var codeBlock = mock.Object;
        codeBlock.AddInput("My Input");

        codeBlock.SetInputValue(0, "NewValue");

        Assert.Equal("NewValue", codeBlock.GetInput(0));
    }

    [Fact]
    public void GetCode_WithSimpleValues_GeneratesCorrectString()
    {
        var mock = new Mock<CodeBase> { CallBase = true };
        mock.Object.Prefix = "TEST";
        var codeBlock = mock.Object;
        codeBlock.AddInput("Input A").SetInputValue(0, "ValA");
        codeBlock.AddInput("Input B").SetInputValue(1, "ValB");
        var expected = "TEST( ValA, ValB )";

        var result = codeBlock.GetCode();

        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetCode_WithNestedCodeBase_GeneratesCorrectString()
    {
        var outerMock = new Mock<CodeBase> { CallBase = true };
        var outerBlock = outerMock.Object;
        outerBlock.Prefix = "OUTER";

        var innerMock = new Mock<CodeBase> { CallBase = true };
        var innerBlock = innerMock.Object;
        innerBlock.Prefix = "INNER";

        innerBlock.AddInput("Inner").SetInputValue(0, "InnerVal");

        outerBlock.AddInput("Outer").SetInputValue(0, "OuterVal");
        outerBlock.AddInput(innerBlock, "Nested");

        var expected = "OUTER( OuterVal, INNER( InnerVal ))";

        var result = outerBlock.GetCode();

        Assert.Equal(expected, result);
    }

    [Fact]
    public void ImplicitStringOperator_ReturnsResultOfGetCode()
    {
        var mock = new Mock<CodeBase> { CallBase = true };
        mock.Object.Prefix = "TEST";
        var codeBlock = mock.Object;
        codeBlock.AddInput("Input").SetInputValue(0, "MyValue");
        string expected = "TEST( MyValue )";

        string result = codeBlock;

        Assert.Equal(expected, result);
    }

    [Fact]
    public void PlusOperator_ConcatenatesCorrectly()
    {
        var mockA = new Mock<CodeBase> { CallBase = true };
        mockA.Object.Prefix = "A";  
        var codeBlockA = mockA.Object;
        codeBlockA.AddInput("Input").SetInputValue(0, "1");

        var mockB = new Mock<CodeBase> { CallBase = true };
        mockB.Object.Prefix = "B";
        var codeBlockB = mockB.Object;
        codeBlockB.AddInput("Input").SetInputValue(0, "2");

        Assert.Equal("A( 1 ) + B( 2 )", codeBlockA + " + " + codeBlockB);
        Assert.Equal("Prefix - A( 1 )", "Prefix - " + codeBlockA);
        Assert.Equal("A( 1 ) - Suffix", codeBlockA + " - Suffix");
    }
}