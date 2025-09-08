using AMFormsCST.Desktop.ViewModels.Pages.Tools.CodeSnippets;
using AMFormsCST.Core.Types.CodeBlocks;
using Moq;
using System.Collections.ObjectModel;
using Xunit;
using AMFormsCST.Core.Interfaces.CodeBlocks;

namespace AMFormsCST.Test.Desktop.ViewModels.Pages.Tools.CodeSnippets;
public class CodeSnippetItemViewModelTests
{
    [Fact]
    public void Constructor_InitializesPropertiesAndInputs()
    {
        // Arrange
        var codeInput = new CodeInput("val", "input desc", 1);

        var codeBaseMock = new Mock<ICodeBase>();
        codeBaseMock.SetupGet(cb => cb.Name).Returns("TestName");
        codeBaseMock.SetupGet(cb => cb.Description).Returns("TestDesc");
        codeBaseMock.Setup(cb => cb.GetCode()).Returns("output code");
        codeBaseMock.SetupGet(cb => cb.Inputs).Returns(new List<CodeInput> { codeInput });

        // Act
        var vm = new CodeSnippetItemViewModel(codeBaseMock.Object);

        // Assert
        Assert.Equal("TestName", vm.Name);
        Assert.Equal("TestDesc", vm.Description);
        Assert.Equal("output code", vm.Output);
        Assert.Single(vm.Inputs);
        Assert.IsType<CodeInputViewModel>(vm.Inputs[0]);
        Assert.Equal(codeBaseMock.Object, vm.CodeBase);
        Assert.False(vm.IsSelected);
        Assert.NotEqual(default, vm.Id);
    }

    [Fact]
    public void Select_SetsIsSelectedTrue()
    {
        // Arrange
        var codeBaseMock = new Mock<ICodeBase>();
        codeBaseMock.SetupGet(cb => cb.Inputs).Returns([]);
        var vm = new CodeSnippetItemViewModel(codeBaseMock.Object);

        // Act
        vm.Select();

        // Assert
        Assert.True(vm.IsSelected);
    }

    [Fact]
    public void Deselect_SetsIsSelectedFalse()
    {
        // Arrange
        var codeBaseMock = new Mock<ICodeBase>();
        codeBaseMock.SetupGet(cb => cb.Inputs).Returns([]);
        var vm = new CodeSnippetItemViewModel(codeBaseMock.Object);
        vm.IsSelected = true;

        // Act
        vm.Deselect();

        // Assert
        Assert.False(vm.IsSelected);
    }

    [Fact]
    public void InputChanged_UpdatesOutput()
    {
        // Arrange
        var codeBaseMock = new Mock<ICodeBase>();
        codeBaseMock.SetupGet(cb => cb.Inputs).Returns([]);
        codeBaseMock.SetupSequence(cb => cb.GetCode())
            .Returns("initial")
            .Returns("changed");

        var vm = new CodeSnippetItemViewModel(codeBaseMock.Object);
        vm.Output = "initial";

        // Act
        vm.InputChanged();

        // Assert
        Assert.Equal("changed", vm.Output);
    }
}