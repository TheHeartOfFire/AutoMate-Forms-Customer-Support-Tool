using AMFormsCST.Core.Interfaces.CodeBlocks;
using AMFormsCST.Core.Types.CodeBlocks;
using AMFormsCST.Desktop.ViewModels.Pages.Tools.CodeSnippets;
using Moq;
using Xunit;

namespace AMFormsCST.Test.Desktop.ViewModels.Pages.Tools.CodeSnippets;
public class CodeInputViewModelTests
{
    [Fact]
    public void Constructor_InitializesProperties()
    {
        // Arrange
        var codeInput = new CodeInput("init", "desc", 42);
        var codeBaseStub = new Mock<ICodeBase>();
        codeBaseStub.SetupGet(cb => cb.Inputs).Returns(new List<CodeInput>());
        var parent = new CodeSnippetItemViewModel(codeBaseStub.Object);

        // Act
        var vm = new CodeInputViewModel(codeInput, parent);

        // Assert
        Assert.Equal("desc", vm.Description);
        Assert.Equal(42, vm.Index);
        Assert.Equal("init", vm.BindableValue);
    }

    [Fact]
    public void BindableValue_Setter_UpdatesModelAndNotifiesParent()
    {
        // Arrange
        var codeInput = new CodeInput("old", "desc", 1);
        var codeBaseStub = new Mock<ICodeBase>();
        codeBaseStub.SetupGet(cb => cb.Inputs).Returns(new List<CodeInput>());
        codeBaseStub.Setup(cb => cb.GetCode()).Returns("output");
        var parent = new CodeSnippetItemViewModel(codeBaseStub.Object);

        var vm = new CodeInputViewModel(codeInput, parent);

        // Act
        vm.BindableValue = "new value";

        // Assert
        Assert.Equal("new value", codeInput.Value);
        Assert.Equal("new value", vm.BindableValue);
        // Optionally, check that Output property is updated if relevant
        Assert.Equal("output", parent.Output);
    }
}