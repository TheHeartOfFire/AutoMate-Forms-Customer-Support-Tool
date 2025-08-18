using AMFormsCST.Desktop.Views.Dialogs;
using AMFormsCST.Desktop.ViewModels.Dialogs;
using Moq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Xunit;
using AMFormsCST.Test.Helpers;

[Collection("STA Tests")]
public class NewTemplateDialogTests
{
    [WpfFact]
    public void Constructor_InitializesDataContextAndProperties()
    {
        // Act
        var dialog = new NewTemplateDialog("Name", "Desc", "Content");

        // Assert
        Assert.IsType<NewTemplateDialogViewModel>(dialog.DataContext);
        var vm = (NewTemplateDialogViewModel)dialog.DataContext;
        Assert.Equal("Name", vm.TemplateName);
        Assert.Equal("Desc", vm.TemplateDescription);
        Assert.Equal("Content", vm.TemplateContent);
        Assert.Equal("Name", dialog.TemplateName);
        Assert.Equal("Desc", dialog.TemplateDescription);
        Assert.Equal("Content", dialog.TemplateContent);
    }

    [WpfFact]
    public void Constructor_SetsConfirmButtonContentToUpdate_WhenAnyFieldIsNotEmpty()
    {
        // Act
        var dialog = new NewTemplateDialog("Name", "Desc", "Content");

        // Assert
        Assert.Equal("Update", dialog.ConfirmButton.Content);
    }

    [WpfFact]
    public void Constructor_SetsConfirmButtonContentToDefault_WhenAllFieldsAreEmpty()
    {
        // Act
        var dialog = new NewTemplateDialog();

        // Assert
        Assert.NotEqual("Update", dialog.ConfirmButton.Content);
    }

    [WpfFact]
    public void CustomTitleBarArea_MouseLeftButtonDown_CallsDragMove()
    {
        // Arrange
        var dialog = new NewTemplateDialog("Name", "Desc", "Content");
        var mouseEvent = new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left)
        {
            RoutedEvent = UIElement.MouseLeftButtonDownEvent,
            Source = dialog
        };

        // Act
        var method = typeof(NewTemplateDialog).GetMethod("CustomTitleBarArea_MouseLeftButtonDown", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        method.Invoke(dialog, new object[] { dialog, mouseEvent });

        // Assert
        // No exception thrown, method can be called.
    }
}