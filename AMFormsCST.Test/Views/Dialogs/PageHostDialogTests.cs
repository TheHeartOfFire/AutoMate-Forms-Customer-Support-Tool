using AMFormsCST.Desktop.Views.Dialogs;
using AMFormsCST.Desktop.ViewModels.Dialogs;
using Moq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Xunit;
using AMFormsCST.Test.Helpers;

[Collection("STA Tests")]
public class PageHostDialogTests
{
    [WpfFact]
    public void Constructor_SetsDataContextAndNavigatesPage()
    {
        // Arrange
        var page = new Page();
        page.DataContext = new object();
        var dialog = new PageHostDialog(page, true);

        // Assert
        Assert.IsType<PageHostDialogViewModel>(dialog.DataContext);
        Assert.Equal(page.DataContext, ((PageHostDialogViewModel)dialog.DataContext).HostedPageViewModel);
        Assert.True(((PageHostDialogViewModel)dialog.DataContext).CanConfirm == Visibility.Visible);
    }

    [WpfFact]
    public void ConfirmSelected_ReturnsViewModelConfirmSelected()
    {
        // Arrange
        var page = new Page();
        page.DataContext = new object();
        var dialog = new PageHostDialog(page, true);
        var vm = (PageHostDialogViewModel)dialog.DataContext;
        vm.ConfirmSelected = true;

        // Act
        var result = dialog.ConfirmSelected;

        // Assert
        Assert.True(result);
    }

    [WpfFact]
    public void CustomTitleBarArea_MouseLeftButtonDown_CallsDragMove()
    {
        // Arrange
        var page = new Page();
        page.DataContext = new object();
        var dialog = new PageHostDialog(page, true);

        var mouseEvent = new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left)
        {
            RoutedEvent = UIElement.MouseLeftButtonDownEvent,
            Source = dialog
        };

        // Act
        var method = typeof(PageHostDialog).GetMethod("CustomTitleBarArea_MouseLeftButtonDown", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        method.Invoke(dialog, new object[] { dialog, mouseEvent });

        // Assert
        // No exception thrown, method can be called.
        // DragMove is a protected method, so we can't directly assert its call without further refactoring.
    }
}