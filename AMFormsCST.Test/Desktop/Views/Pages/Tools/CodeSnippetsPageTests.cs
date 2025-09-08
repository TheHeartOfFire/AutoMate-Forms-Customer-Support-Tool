using AMFormsCST.Core.Interfaces;
using AMFormsCST.Core.Interfaces.CodeBlocks;
using AMFormsCST.Core.Interfaces.Utils;
using AMFormsCST.Desktop.ViewModels.Pages.Tools;
using AMFormsCST.Desktop.Views.Pages.Tools;
using AMFormsCST.Test.Helpers;
using Moq;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Xunit;

namespace AMFormsCST.Test.Desktop.Views.Pages.Tools;

[Collection("STA Tests")]
public class CodeSnippetsPageTests
{
    [WpfFact]
    public void Button_Click_DoesNotThrow()
    {
        // Arrange
        // 1. Mock the dependencies for the ViewModel.
        var supportToolMock = new Mock<ISupportTool>();
        var codeBlocksMock = new Mock<ICodeBlocks>();
        var logServiceMock = new Mock<ILogService>();

        // 2. Configure the mocks.
        supportToolMock.Setup(s => s.CodeBlocks).Returns(codeBlocksMock.Object);
        codeBlocksMock.Setup(c => c.GetBlocks()).Returns(new List<ICodeBase>());

        // 3. Create the ViewModel and Page instances.
        var viewModel = new CodeSnippetsViewModel(supportToolMock.Object, logServiceMock.Object);
        var page = new CodeSnippetsPage(viewModel);

        // 4. Force the page's template to be applied by hosting it in a window.
        // This is the crucial step to prevent the NullReferenceException.
        var window = new Window { Content = page };
        window.Show();

        // 5. Find the button control within the now-loaded visual tree by its content.
        var button = FindButtonByContent(page, "Copy");
        Assert.NotNull(button); // It's good practice to ensure the control was found.

        // Act
        // Now that the button is guaranteed to exist, we can safely raise the Click event.
        var exception = Record.Exception(() =>
        {
            button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        });

        // Assert
        Assert.Null(exception);

        // Cleanup
        window.Close();
    }

    /// <summary>
    /// Helper method to find a Button control by its Content property.
    /// </summary>
    private static Button? FindButtonByContent(DependencyObject parent, string content)
    {
        if (parent == null) return null;

        int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
        for (int i = 0; i < childrenCount; i++)
        {
            var child = VisualTreeHelper.GetChild(parent, i);

            if (child is Button button && button.Content is string buttonContent && buttonContent == content)
            {
                return button;
            }
            else
            {
                var foundButton = FindButtonByContent(child, content);
                if (foundButton != null)
                {
                    return foundButton;
                }
            }
        }
        return null;
    }

    /// <summary>
    /// Helper method to find a child control of a specific type and name in the visual tree.
    /// </summary>
    private static T? FindChild<T>(DependencyObject? parent, string childName) where T : DependencyObject
    {
        if (parent == null) return null;

        T? foundChild = null;
        int childrenCount = VisualTreeHelper.GetChildrenCount(parent);

        for (int i = 0; i < childrenCount; i++)
        {
            var child = VisualTreeHelper.GetChild(parent, i);
            if (child is not T childType)
            {
                foundChild = FindChild<T>(child, childName);
                if (foundChild != null) break;
            }
            else if (!string.IsNullOrEmpty(childName))
            {
                if (child is FrameworkElement frameworkElement && frameworkElement.Name == childName)
                {
                    foundChild = childType;
                    break;
                }
            }
            else
            {
                foundChild = childType;
                break;
            }
        }

        return foundChild;
    }
}