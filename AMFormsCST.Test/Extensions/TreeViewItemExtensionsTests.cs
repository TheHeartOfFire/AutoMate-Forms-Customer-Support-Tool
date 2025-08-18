using AMFormsCST.Desktop.Extensions;
using AMFormsCST.Test.Helpers;
using System.Windows.Controls;
using Xunit;
using TreeViewItem = Wpf.Ui.Controls.TreeViewItem;

[Collection("STA Tests")]
public class TreeViewItemExtensionsTests
{
    [WpfFact]
    public void ExpandAll_SetsIsExpanded_True_ForAllItems()
    {
        // Arrange
        var root = new TreeViewItem();
        var child1 = new TreeViewItem();
        var child2 = new TreeViewItem();
        var grandchild = new TreeViewItem();
        child1.Items.Add(grandchild);
        root.Items.Add(child1);
        root.Items.Add(child2);

        // Act
        root.ExpandAll();

        // Assert
        Assert.True(root.IsExpanded);
        Assert.True(child1.IsExpanded);
        Assert.True(child2.IsExpanded);
        Assert.True(grandchild.IsExpanded);
    }

    [WpfFact]
    public void CollapseAll_SetsIsExpanded_False_ForAllItems()
    {
        // Arrange
        var root = new TreeViewItem { IsExpanded = true };
        var child1 = new TreeViewItem { IsExpanded = true };
        var child2 = new TreeViewItem { IsExpanded = true };
        var grandchild = new TreeViewItem { IsExpanded = true };
        child1.Items.Add(grandchild);
        root.Items.Add(child1);
        root.Items.Add(child2);

        // Act
        root.CollapseAll();

        // Assert
        Assert.False(root.IsExpanded);
        Assert.False(child1.IsExpanded);
        Assert.False(child2.IsExpanded);
        Assert.False(grandchild.IsExpanded);
    }

    [WpfFact]
    public void Clone_CreatesDeepCopyWithSameHeaderTagAndExpansion()
    {
        // Arrange
        var root = new TreeViewItem { Header = "Root", Tag = 1, IsExpanded = true };
        var child1 = new TreeViewItem { Header = "Child1", Tag = 2, IsExpanded = false };
        var child2 = new TreeViewItem { Header = "Child2", Tag = 3, IsExpanded = true };
        root.Items.Add(child1);
        root.Items.Add(child2);

        // Act
        TreeViewItem clone = root.Clone();

        // Assert
        Assert.NotSame(root, clone);
        Assert.Equal(root.Header, clone.Header);
        Assert.Equal(root.Tag, clone.Tag);
        Assert.Equal(root.IsExpanded, clone.IsExpanded);
        Assert.Equal(root.Items.Count, clone.Items.Count);

        var cloneChild1 = (TreeViewItem)clone.Items[0];
        var cloneChild2 = (TreeViewItem)clone.Items[1];
        Assert.Equal(child1.Header, cloneChild1.Header);
        Assert.Equal(child1.Tag, cloneChild1.Tag);
        Assert.Equal(child1.IsExpanded, cloneChild1.IsExpanded);
        Assert.Equal(child2.Header, cloneChild2.Header);
        Assert.Equal(child2.Tag, cloneChild2.Tag);
        Assert.Equal(child2.IsExpanded, cloneChild2.IsExpanded);
    }
}