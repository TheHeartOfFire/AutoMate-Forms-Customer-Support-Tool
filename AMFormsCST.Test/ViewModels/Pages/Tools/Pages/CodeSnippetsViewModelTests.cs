using AMFormsCST.Desktop.ViewModels.Pages.Tools.CodeSnippets;
using AMFormsCST.Core.Interfaces.CodeBlocks;
using Moq;
using System.Collections.ObjectModel;
using Xunit;
using AMFormsCST.Core.Interfaces.Utils;
using AMFormsCST.Desktop.ViewModels.Pages.Tools;
using AMFormsCST.Core.Interfaces;

public class CodeSnippetsViewModelTests
{
    [Fact]
    public void Constructor_InitializesCodeSnippetsAndSelectsFirst()
    {
        // Arrange
        var codeBaseMock1 = new Mock<ICodeBase>();
        codeBaseMock1.SetupGet(cb => cb.Name).Returns("A");
        codeBaseMock1.Setup(cb => cb.GetCode()).Returns("codeA");
        codeBaseMock1.SetupGet(cb => cb.Inputs).Returns([]);

        var codeBaseMock2 = new Mock<ICodeBase>();
        codeBaseMock2.SetupGet(cb => cb.Name).Returns("B");
        codeBaseMock2.Setup(cb => cb.GetCode()).Returns("codeB");
        codeBaseMock2.SetupGet(cb => cb.Inputs).Returns([]);

        var codeBlocksMock = new Mock<ICodeBlocks>();
        codeBlocksMock.Setup(cb => cb.GetBlocks()).Returns(new[] { codeBaseMock2.Object, codeBaseMock1.Object }); // Unordered

        var supportToolMock = new Mock<ISupportTool>();
        supportToolMock.SetupGet(st => st.CodeBlocks).Returns(codeBlocksMock.Object);

        // Act
        var vm = new CodeSnippetsViewModel(supportToolMock.Object);

        // Assert
        Assert.Equal(2, vm.CodeSnippets.Count);
        Assert.Equal("A", vm.CodeSnippets[0].Name); // Ordered by Name
        Assert.Equal("B", vm.CodeSnippets[1].Name);
        Assert.Equal(vm.CodeSnippets[0], vm.SelectedCodeSnippet);
        Assert.True(vm.SelectedCodeSnippet.IsSelected);
    }

    [Fact]
    public void SelectCodeSnippetCommand_ChangesSelectionAndIsSelected()
    {
        // Arrange
        var codeBaseMock1 = new Mock<ICodeBase>();
        codeBaseMock1.SetupGet(cb => cb.Name).Returns("A");
        codeBaseMock1.Setup(cb => cb.GetCode()).Returns("codeA");
        codeBaseMock1.SetupGet(cb => cb.Inputs).Returns([]);

        var codeBaseMock2 = new Mock<ICodeBase>();
        codeBaseMock2.SetupGet(cb => cb.Name).Returns("B");
        codeBaseMock2.Setup(cb => cb.GetCode()).Returns("codeB");
        codeBaseMock2.SetupGet(cb => cb.Inputs).Returns([]);

        var codeBlocksMock = new Mock<ICodeBlocks>();
        codeBlocksMock.Setup(cb => cb.GetBlocks()).Returns(new[] { codeBaseMock1.Object, codeBaseMock2.Object });

        var supportToolMock = new Mock<ISupportTool>();
        supportToolMock.SetupGet(st => st.CodeBlocks).Returns(codeBlocksMock.Object);

        var vm = new CodeSnippetsViewModel(supportToolMock.Object);
        var first = vm.CodeSnippets[0];
        var second = vm.CodeSnippets[1];

        // Act
        vm.SelectCodeSnippetCommand.Execute(second);

        // Assert
        Assert.Equal(second, vm.SelectedCodeSnippet);
        Assert.True(second.IsSelected);
        Assert.False(first.IsSelected);
    }

    [Fact]
    public void SelectCodeSnippetCommand_DoesNothingIfNullOrAlreadySelected()
    {
        // Arrange
        var codeBaseMock = new Mock<ICodeBase>();
        codeBaseMock.SetupGet(cb => cb.Name).Returns("A");
        codeBaseMock.Setup(cb => cb.GetCode()).Returns("codeA");
        codeBaseMock.SetupGet(cb => cb.Inputs).Returns([]);

        var codeBlocksMock = new Mock<ICodeBlocks>();
        codeBlocksMock.Setup(cb => cb.GetBlocks()).Returns(new[] { codeBaseMock.Object });

        var supportToolMock = new Mock<ISupportTool>();
        supportToolMock.SetupGet(st => st.CodeBlocks).Returns(codeBlocksMock.Object);

        var vm = new CodeSnippetsViewModel(supportToolMock.Object);
        var selected = vm.SelectedCodeSnippet;

        // Act
        vm.SelectCodeSnippetCommand.Execute(null);
        vm.SelectCodeSnippetCommand.Execute(selected);

        // Assert
        Assert.Equal(selected, vm.SelectedCodeSnippet);
        Assert.True(selected.IsSelected);
    }

    [Fact]
    public void CopyOutput_CopiesSelectedCodeSnippetOutput()
    {
        // Arrange
        var codeBaseMock = new Mock<ICodeBase>();
        codeBaseMock.SetupGet(cb => cb.Name).Returns("A");
        codeBaseMock.Setup(cb => cb.GetCode()).Returns("output");
        codeBaseMock.SetupGet(cb => cb.Inputs).Returns([]);

        var codeBlocksMock = new Mock<ICodeBlocks>();
        codeBlocksMock.Setup(cb => cb.GetBlocks()).Returns(new[] { codeBaseMock.Object });

        var supportToolMock = new Mock<ISupportTool>();
        supportToolMock.SetupGet(st => st.CodeBlocks).Returns(codeBlocksMock.Object);

        var vm = new CodeSnippetsViewModel(supportToolMock.Object);
        var selected = vm.SelectedCodeSnippet;
        selected.Output = "output";

        // Act & Assert
        var exception = Record.Exception(() => vm.CopyOutput());
        Assert.Null(exception); // Ensure no exception is thrown
    }

    [Fact]
    public void CopyOutput_DoesNothingIfNoSelectedOrEmptyOutput()
    {
        // Arrange
        var codeBaseMock = new Mock<ICodeBase>();
        codeBaseMock.SetupGet(cb => cb.Name).Returns("A");
        codeBaseMock.Setup(cb => cb.GetCode()).Returns("");
        codeBaseMock.SetupGet(cb => cb.Inputs).Returns([]);

        var codeBlocksMock = new Mock<ICodeBlocks>();
        codeBlocksMock.Setup(cb => cb.GetBlocks()).Returns(new[] { codeBaseMock.Object });

        var supportToolMock = new Mock<ISupportTool>();
        supportToolMock.SetupGet(st => st.CodeBlocks).Returns(codeBlocksMock.Object);

        var vm = new CodeSnippetsViewModel(supportToolMock.Object);
        var selected = vm.SelectedCodeSnippet;
        selected.Output = "";

        // Act & Assert
        var exception = Record.Exception(() => vm.CopyOutput());
        Assert.Null(exception); // Ensure no exception is thrown
    }
}