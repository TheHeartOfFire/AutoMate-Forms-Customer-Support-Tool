using AMFormsCST.Desktop.Controls;
using AMFormsCST.Test.Helpers;
using System.Windows;
using System.Windows.Controls;
using Xunit;
using Xunit.Sdk;

public class CopyButtonTests
{
    [WpfFact]
    public void TextToCopy_Property_SetAndGet_Works()
    {
        // Arrange
        var button = new CopyButton();

        // Act
        button.TextToCopy = "Hello World";

        // Assert
        Assert.Equal("Hello World", button.TextToCopy);
    }

    [WpfFact]
    public void CopyButton_OnClick_CopiesTextToClipboard()
    {
        // Arrange
        var button = new CopyButton();
        var testText = "Copy this text!";
        button.TextToCopy = testText;

        // Act
        var routedEventArgs = new RoutedEventArgs(Button.ClickEvent);
        button.GetType().GetMethod("CopyButton_OnClick", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
            .Invoke(button, new object[] { button, routedEventArgs });

        // Assert
        Assert.Equal(testText, Clipboard.GetText());
    }

    [WpfFact]
    public void CopyButton_OnClick_DoesNothing_WhenTextIsNullOrEmpty()
    {
        // Arrange
        var button = new CopyButton();
        Clipboard.Clear();

        // Act
        var routedEventArgs = new RoutedEventArgs(Button.ClickEvent);
        button.TextToCopy = null;
        button.GetType().GetMethod("CopyButton_OnClick", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
            .Invoke(button, new object[] { button, routedEventArgs });

        // Assert
        Assert.True(string.IsNullOrEmpty(Clipboard.GetText()));

        // Act with empty string
        button.TextToCopy = "";
        button.GetType().GetMethod("CopyButton_OnClick", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
            .Invoke(button, new object[] { button, routedEventArgs });

        // Assert
        Assert.True(string.IsNullOrEmpty(Clipboard.GetText()));
    }
}