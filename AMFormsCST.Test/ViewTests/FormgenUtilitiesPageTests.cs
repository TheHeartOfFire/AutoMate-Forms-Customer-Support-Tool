using AMFormsCST.Desktop.Views.Pages.Tools;
using AMFormsCST.Test.Helpers;
using System.Windows.Controls;
using Xunit;

namespace AMFormsCST.Test.ViewTests;

public class FormgenUtilitiesPageTests
{
    [WpfFact]
    public void FindParent_WhenParentExists_ReturnsParent()
    {
        // Arrange
        var grid = new Grid();
        var stackPanel = new StackPanel();
        var textBlock = new TextBlock();

        // Build a simple visual tree: grid -> stackPanel -> textBlock
        grid.Children.Add(stackPanel);
        stackPanel.Children.Add(textBlock);

        // Act
        var result = FormgenUtilitiesPage.FindParent<Grid>(textBlock);

        // Assert
        Assert.NotNull(result);
        Assert.Same(grid, result);
    }

    [WpfFact]
    public void FindParent_WhenParentDoesNotExist_ReturnsNull()
    {
        // Arrange
        var stackPanel = new StackPanel();
        var textBlock = new TextBlock();

        // Build a simple visual tree: stackPanel -> textBlock
        stackPanel.Children.Add(textBlock);

        // Act
        // Attempt to find a Grid, which is not in the parent hierarchy.
        var result = FormgenUtilitiesPage.FindParent<Grid>(textBlock);

        // Assert
        Assert.Null(result);
    }

    [WpfFact]
    public void FindParent_WhenCalledOnRoot_ReturnsNull()
    {
        // Arrange
        var grid = new Grid(); // The root of our tree

        // Act
        var result = FormgenUtilitiesPage.FindParent<StackPanel>(grid);

        // Assert
        Assert.Null(result);
    }
}