using AMFormsCST.Core.Interfaces;
using AMFormsCST.Core.Interfaces.Utils;
using AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure;
using AMFormsCST.Desktop.Models.FormgenUtilities.Grouping;
using AMFormsCST.Desktop.Services;
using AMFormsCST.Desktop.ViewModels.Pages.Tools;
using AMFormsCST.Desktop.ViewModels.Pages.Tools.FormgenUtils;
using Moq;
using System.Linq;
using Xunit;

namespace AMFormsCST.Test.Desktop.ViewModels.Pages.Tools.FormgenUtils;

public class TreeItemNodeViewModelTests
{
    private readonly Mock<FormgenUtilitiesViewModel> _mockParentViewModel;

    public TreeItemNodeViewModelTests()
    {
        // Setup required dependencies for FormgenUtilitiesViewModel
        var mockSupportTool = new Mock<ISupportTool>();
        var mockDialogService = new Mock<IDialogService>();
        var mockFileSystem = new Mock<IFileSystem>();

        // Setup FormgenUtils property to avoid NullReferenceException in ViewModel constructor
        var mockFormgenUtils = new Mock<IFormgenUtils>();
        mockSupportTool.SetupGet(s => s.FormgenUtils).Returns(mockFormgenUtils.Object);

        _mockParentViewModel = new Mock<FormgenUtilitiesViewModel>(
            mockSupportTool.Object,
            mockDialogService.Object,
            mockFileSystem.Object);
        _mockParentViewModel.SetupProperty(vm => vm.SelectedNode);
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
        Assert.Equal("Code Lines", node.Header);
        // The children structure may depend on grouping logic; check for at least one child group
        Assert.True(node.Children.Count > 0);
        Assert.All(node.Children.SelectMany(g => g.Children), child => Assert.IsType<CodeLine>(child.Data));
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
        Assert.Equal(node, _mockParentViewModel.Object.SelectedNode);
    }
}