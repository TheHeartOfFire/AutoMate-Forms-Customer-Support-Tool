using AMFormsCST.Core.Types.CodeBlocks;
using System;
using System.Collections.Generic;
using Assert = Xunit.Assert;

namespace AMFormsCST.Test.CoreLogicTests.CodeBlockTests;

// A concrete implementation for testing the abstract CodeBase.
public class ConcreteCodeBase : CodeBase
{
    public ConcreteCodeBase(string prefix = "TEST")
    {
        Prefix = prefix;
    }
}

public class CodeBaseTests
{
    [Fact]
    public void AddInput_WithStringDescription_AddsInputCorrectly()
    {
        // Arrange
        var codeBlock = new ConcreteCodeBase();

        // Act
        codeBlock.AddInput("First Input");
        codeBlock.AddInput("Second Input");

        // Assert
        Assert.Equal(2, codeBlock.InputCount());
        Assert.Equal("First Input", codeBlock.Inputs[0].Description);
        Assert.Equal(0, codeBlock.Inputs[0].Index);
        Assert.Equal("Second Input", codeBlock.Inputs[1].Description);
        Assert.Equal(1, codeBlock.Inputs[1].Index);
    }

    [Fact]
    public void AddInput_AtIndex_InsertsAndShiftsIndices()
    {
        // Arrange
        var codeBlock = new ConcreteCodeBase();
        codeBlock.AddInput("Input A"); // Index 0
        codeBlock.AddInput("Input C"); // Index 1

        // Act
        codeBlock.AddInput(1, "Input B"); // Insert at index 1

        // Assert
        Assert.Equal(3, codeBlock.InputCount());
        Assert.Equal("Input A", codeBlock.Inputs.Find(i => i.Index == 0)?.Description);
        Assert.Equal("Input B", codeBlock.Inputs.Find(i => i.Index == 1)?.Description);
        Assert.Equal("Input C", codeBlock.Inputs.Find(i => i.Index == 2)?.Description);
    }

    [Fact]
    public void RemoveInput_RemovesAndShiftsIndices()
    {
        // Arrange
        var codeBlock = new ConcreteCodeBase();
        codeBlock.AddInput("Input A"); // Index 0
        codeBlock.AddInput("Input B"); // Index 1
        codeBlock.AddInput("Input C"); // Index 2

        // Act
        codeBlock.RemoveInput(1); // Remove "Input B"

        // Assert
        Assert.Equal(2, codeBlock.InputCount());
        Assert.Equal("Input A", codeBlock.Inputs.Find(i => i.Index == 0)?.Description);
        Assert.Equal("Input C", codeBlock.Inputs.Find(i => i.Index == 1)?.Description);
    }

    [Fact]
    public void SetInputValue_UpdatesCorrectInput()
    {
        // Arrange
        var codeBlock = new ConcreteCodeBase();
        codeBlock.AddInput("My Input");
        
        // Act
        codeBlock.SetInputValue(0, "NewValue");

        // Assert
        Assert.Equal("NewValue", codeBlock.GetInput(0));
    }

    [Fact]
    public void GetCode_WithSimpleValues_GeneratesCorrectString()
    {
        // Arrange
        var codeBlock = new ConcreteCodeBase();
        codeBlock.AddInput("Input A").SetInputValue(0, "ValA");
        codeBlock.AddInput("Input B").SetInputValue(1, "ValB");
        var expected = "TEST(ValA, ValB)";

        // Act
        var result = codeBlock.GetCode();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetCode_WithNestedCodeBase_GeneratesCorrectString()
    {
        // Arrange
        var outerBlock = new ConcreteCodeBase("OUTER");
        var innerBlock = new ConcreteCodeBase("INNER");
        innerBlock.AddInput("Inner").SetInputValue(0, "InnerVal");

        outerBlock.AddInput("Outer").SetInputValue(0, "OuterVal");
        outerBlock.AddInput(innerBlock, "Nested");

        var expected = "OUTER(OuterVal, INNER(InnerVal))";

        // Act
        var result = outerBlock.GetCode();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ImplicitStringOperator_ReturnsResultOfGetCode()
    {
        // Arrange
        var codeBlock = new ConcreteCodeBase();
        codeBlock.AddInput("Input").SetInputValue(0, "MyValue");
        string expected = "TEST(MyValue)";

        // Act
        string result = codeBlock;

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void PlusOperator_ConcatenatesCorrectly()
    {
        // Arrange
        var codeBlockA = new ConcreteCodeBase("A");
        codeBlockA.AddInput("Input").SetInputValue(0, "1");

        var codeBlockB = new ConcreteCodeBase("B");
        codeBlockB.AddInput("Input").SetInputValue(0, "2");

        // Act & Assert
        Assert.Equal("A(1) + B(2)", codeBlockA + " + " + codeBlockB);
        Assert.Equal("Prefix - A(1)", "Prefix - " + codeBlockA);
        Assert.Equal("A(1) - Suffix", codeBlockA + " - Suffix");
    }
}