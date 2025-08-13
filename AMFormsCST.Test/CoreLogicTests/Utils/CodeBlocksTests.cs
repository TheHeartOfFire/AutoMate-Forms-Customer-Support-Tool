using AMFormsCST.Core.Utils;
using AMFormsCST.Core.Interfaces.CodeBlocks;
using AMFormsCST.Core.Types.CodeBlocks.Functions;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

public class CodeBlocksTests
{
    [Fact]
    public void Constructor_InitializesDefaultBlocks()
    {
        // Act
        var codeBlocks = new CodeBlocks();

        // Assert
        var blocks = codeBlocks.GetBlocks();
        // The default constructor adds 11 default blocks
        Assert.True(blocks.Count >= 11);

        // Check for some known default block types
        Assert.Contains(blocks, b => b is CaseCode);
        Assert.Contains(blocks, b => b is IfCode);
        Assert.Contains(blocks, b => b is SeplistCode);
    }

    [Fact]
    public void CustomBlocks_CanBeAddedAndReturnedByGetBlocks()
    {
        // Arrange
        var codeBlocks = new CodeBlocks();
        var mockBlock = new Mock<ICodeBase>();
        mockBlock.SetupGet(b => b.Name).Returns("CustomBlock");

        // Act
        codeBlocks.CustomBlocks.Add(mockBlock.Object);
        var blocks = codeBlocks.GetBlocks();

        // Assert
        Assert.Contains(mockBlock.Object, blocks);
    }

    [Fact]
    public void GetBlocks_ReturnsCombinedDefaultAndCustomBlocks()
    {
        // Arrange
        var codeBlocks = new CodeBlocks();
        var initialCount = codeBlocks.GetBlocks().Count;
        var mockBlock1 = new Mock<ICodeBase>();
        var mockBlock2 = new Mock<ICodeBase>();
        codeBlocks.CustomBlocks.Add(mockBlock1.Object);
        codeBlocks.CustomBlocks.Add(mockBlock2.Object);

        // Act
        var blocks = codeBlocks.GetBlocks();

        // Assert
        Assert.Equal(initialCount + 2, blocks.Count);
        Assert.Contains(mockBlock1.Object, blocks);
        Assert.Contains(mockBlock2.Object, blocks);
    }

    [Fact]
    public void CustomBlocks_Property_IsMutable()
    {
        // Arrange
        var codeBlocks = new CodeBlocks();
        var mockBlock = new Mock<ICodeBase>();

        // Act
        codeBlocks.CustomBlocks = new List<ICodeBase> { mockBlock.Object };

        // Assert
        Assert.Single(codeBlocks.CustomBlocks);
        Assert.Same(mockBlock.Object, codeBlocks.CustomBlocks[0]);
    }
}