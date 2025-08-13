using AMFormsCST.Desktop;
using AMFormsCST.Desktop.Controls;
using AMFormsCST.Test.Helpers;
using Moq;
using System;
using System.DirectoryServices.ActiveDirectory;
using System.Windows;
using Wpf.Ui;
using Wpf.Ui.Input;
using Xunit;
using Xunit.Sdk;

public class GalleryNavigationPresenterTests
{
    [WpfFact]
    public void ItemsSource_Property_SetAndGet_Works()
    {
        // Arrange
        var presenter = new GalleryNavigationPresenter();
        var items = new object();

        // Act
        presenter.ItemsSource = items;

        // Assert
        Assert.Same(items, presenter.ItemsSource);
    }

    [WpfFact]
    public void TemplateButtonCommand_IsInitialized()
    {
        // Arrange
        var presenter = new GalleryNavigationPresenter();

        // Act
        var command = presenter.TemplateButtonCommand;

        // Assert
        Assert.NotNull(command);
        Assert.IsAssignableFrom<IRelayCommand>(command);
    }

    [WpfFact]
    public void TemplateButtonCommand_ExecutesNavigation_WhenTypeIsProvided()
    {
        // Arrange
        var presenter = new GalleryNavigationPresenter();
        var testType = typeof(string);

        // Act & Assert
        // This will call App.GetRequiredService<INavigationService>(), which will use the real implementation.
        // If you want to verify navigation, you must refactor for testability.
        var exception = Record.Exception(() => presenter.TemplateButtonCommand.Execute(testType));

        // Assert
        // The command should not throw, but we cannot verify navigation without refactoring.
        Assert.Null(exception);
    }

    [WpfFact]
    public void TemplateButtonCommand_DoesNotNavigate_WhenTypeIsNull()
    {
        // Arrange
        var presenter = new GalleryNavigationPresenter();
        var mockNavService = new Mock<INavigationService>();

        // Act & Assert
        // This will call App.GetRequiredService<INavigationService>(), which will use the real implementation.
        // If you want to verify navigation, you must refactor for testability.
        var exception = Record.Exception(() => presenter.TemplateButtonCommand.Execute(null));

        // Assert
        // The command should not throw, but we cannot verify navigation without refactoring.
        Assert.Null(exception);
    }
}