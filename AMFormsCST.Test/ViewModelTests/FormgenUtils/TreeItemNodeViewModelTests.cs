using AMFormsCST.Core.Interfaces;
using AMFormsCST.Core.Interfaces.Utils;
using AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure;
using AMFormsCST.Desktop.Interfaces;
using AMFormsCST.Desktop.Models.FormgenUtilities.Grouping;
using AMFormsCST.Desktop.ViewModels.Pages.Tools;
using AMFormsCST.Desktop.ViewModels.Pages.Tools.FormgenUtils;
using Moq;
using System.Linq;
using Wpf.Ui.Controls;
using Assert = Xunit.Assert;
using AMFormsCST.Desktop.Services;

namespace AMFormsCST.Test.ViewModelTests;

public class TreeItemNodeViewModelTests
{
    private readonly Mock<FormgenUtilitiesViewModel> _mockParentViewModel;

    public TreeItemNodeViewModelTests()
    {
        // To mock a concrete class with constructor arguments, we must provide
        // valid (non-null) objects for its dependencies.
        var mockSupportTool = new Mock<ISupportTool>();
        var mockDialogService = new Mock<IDialogService>();
        var mockFileSystem = new Mock<IFileSystem>();

        // Now, create the mock of the ViewModel, passing the mocked dependencies.
        _mockParentViewModel = new Mock<FormgenUtilitiesViewModel>(
            mockSupportTool.Object,
            mockDialogService.Object,
            mockFileSystem.Object);
    }

    [Fact]
    public void Constructor_ForDotFormgen_SetsCorrectName()
    {
        // Arrange
        var formgenFile = new DotFormgen { Title = "My Test Form" };

        // Act
        var node = new TreeItemNodeViewModel(formgenFile, _mockParentViewModel.Object);

        // Assert
        Assert.Equal("My Test Form", node.Header);
    }

    [Fact]
    public void Constructor_ForPageGroup_CreatesChildPageNodes()
    {
        // Arrange
        var pages = new[] { new FormPage(), new FormPage() };
        var pageGroup = new PageGroup(pages);

        // Act
        var node = new TreeItemNodeViewModel(pageGroup, _mockParentViewModel.Object);

        // Assert
        Assert.Equal("Pages", node.Header);
        Assert.Equal(2, node.Children.Count);
        Assert.All(node.Children, child => Assert.IsType<FormPage>(child.Data));
    }

    [Fact]
    public void Constructor_ForCodeLineCollection_CreatesChildCodeLineNodes()
    {
        // Arrange
        var codeLines = new[] { new CodeLine(), new CodeLine() };
        var codeLineCollection = new CodeLineCollection(codeLines);

        // Act
        var node = new TreeItemNodeViewModel(codeLineCollection, _mockParentViewModel.Object);

        // Assert
        // all 3 groups get generated when prompts are added.
        // These dummy code lines are defaulting to init,
        // so we need to check the init group for these code Lines
        Assert.Equal("Code Lines", node.Header);
        Assert.Equal(2, node.Children[0].Children.Count); 
        Assert.All(node.Children[0].Children, child => Assert.IsType<CodeLine>(child.Data));
    }

    [Fact]
    public void Constructor_ForFormField_SetsCorrectName()
    {
        // Arrange
        var field = new FormField { Expression = "Test Form Field" };

        // Act
        var node = new TreeItemNodeViewModel(field, _mockParentViewModel.Object);

        // Assert
        Assert.Equal("Test Form Field", node.Header);
        Assert.Empty(node.Children); // Fields should have no children
    }

    [Fact]
    public void IsSelected_SetToTrue_UpdatesParentViewModelSelectedNode()
    {
        // Arrange
        var formgenFile = new DotFormgen();
        var node = new TreeItemNodeViewModel(formgenFile, _mockParentViewModel.Object);

        // Act
        node.IsSelected = true;

        // Assert
        // Verify that setting IsSelected on the node sets the parent VM's SelectedNode property.
        _mockParentViewModel.VerifySet(vm => vm.SelectedNode = node, Times.Once());
    }
}