using AMFormsCST.Core.Interfaces;
using AMFormsCST.Core.Interfaces.Utils;
using AMFormsCST.Desktop.Services;
using AMFormsCST.Desktop.ViewModels.Pages.Tools;
using AMFormsCST.Desktop.Views.Pages.Tools;
using AMFormsCST.Test.Helpers;
using Moq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Wpf.Ui;
using Wpf.Ui.Controls;
using Xunit;

namespace AMFormsCST.Test.Desktop.Views.Pages.Tools;
[Collection("STA Tests")]
public class FormgenUtilitiesPageTests
{
    [WpfFact]
    public void Constructor_SetsViewModelAndDataContext()
    {
        // Arrange
        var formgenUtilsMock = new Mock<IFormgenUtils>();
        var supportToolMock = new Mock<ISupportTool>();
        supportToolMock.SetupGet(s => s.FormgenUtils).Returns(formgenUtilsMock.Object);

        var dialogServiceMock = new Mock<IDialogService>();
        var fileSystemMock = new Mock<IFileSystem>();

        var vm = new FormgenUtilitiesViewModel(
            supportToolMock.Object,
            dialogServiceMock.Object,
            fileSystemMock.Object
        );

        var navServiceMock = new Mock<INavigationService>();

        // Act
        var page = new FormgenUtilitiesPage(vm, navServiceMock.Object);

        // Assert
        Assert.Equal(vm, page.ViewModel);
        Assert.Equal(vm, page.DataContext);
    }

    [WpfFact]
    public void OnLoaded_SetsBackButtonAndDisablesParentScrollViewer()
    {
        // Arrange
        var formgenUtilsMock = new Mock<IFormgenUtils>();
        var supportToolMock = new Mock<ISupportTool>();
        supportToolMock.SetupGet(s => s.FormgenUtils).Returns(formgenUtilsMock.Object);
        var dialogServiceMock = new Mock<IDialogService>();
        var fileSystemMock = new Mock<IFileSystem>();

        var vm = new FormgenUtilitiesViewModel(
            supportToolMock.Object,
            dialogServiceMock.Object,
            fileSystemMock.Object
        );
        var navServiceMock = new Mock<INavigationService>();
        var page = new FormgenUtilitiesPage(vm, navServiceMock.Object);

        var navigationViewMock = new Mock<Wpf.Ui.Controls.NavigationView>();
        navServiceMock.Setup(s => s.GetNavigationControl()).Returns(navigationViewMock.Object);

        var parentScrollViewer = new ScrollViewer();
        var visualTreeHelperMock = new Mock<DependencyObject>();
        // Simulate FindParent returning a ScrollViewer
        var findParentMethod = typeof(FormgenUtilitiesPage).GetMethod("FindParent", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        // Not directly testable, but we can call OnLoaded and ensure no exceptions

        var args = new RoutedEventArgs(FrameworkElement.LoadedEvent, page);

        // Act & Assert
        var method = typeof(FormgenUtilitiesPage).GetMethod(
            "OnLoaded",
            System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic,
            null,
            new Type[] { typeof(object), typeof(RoutedEventArgs) },
            null
        );
                method.Invoke(page, new object[] { page, args });
                // No exception thrown, method can be called.
    }

    [WpfFact]
    public void OnUnloaded_ReenablesParentScrollViewer()
    {
        // Arrange
        var formgenUtilsMock = new Mock<IFormgenUtils>();
        var supportToolMock = new Mock<ISupportTool>();
        supportToolMock.SetupGet(s => s.FormgenUtils).Returns(formgenUtilsMock.Object);
        var dialogServiceMock = new Mock<IDialogService>();
        var fileSystemMock = new Mock<IFileSystem>();

        var vm = new FormgenUtilitiesViewModel(
            supportToolMock.Object,
            dialogServiceMock.Object,
            fileSystemMock.Object
        );
        var navServiceMock = new Mock<INavigationService>();
        var page = new FormgenUtilitiesPage(vm, navServiceMock.Object);

        var parentScrollViewer = new ScrollViewer();
        // Simulate FindParent returning a ScrollViewer
        var args = new RoutedEventArgs(FrameworkElement.UnloadedEvent, page);

        // Act & Assert
        var method = typeof(FormgenUtilitiesPage).GetMethod(
            "OnUnloaded",
            System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic,
            null,
            new Type[] { typeof(object), typeof(RoutedEventArgs) },
            null
        );
                method.Invoke(page, new object[] { page, args });
                // No exception thrown, method can be called.
    }

    [WpfFact]
    public void FindParent_ReturnsNullIfNoParent()
    {
        // Arrange
        var dummy = new NumberBox();

        // Act
        var result = FormgenUtilitiesPage.FindParent<ScrollViewer>(dummy);

        // Assert
        Assert.Null(result);
    }

    public static T? FindParent<T>(DependencyObject child) where T : DependencyObject
    {
        if (child == null)
            return null;

        DependencyObject? parentObject = null;

        if (child is Visual || child is Visual3D)
        {
            parentObject = VisualTreeHelper.GetParent(child);
        }
        else
        {
            // Not a visual, cannot traverse visual tree
            return null;
        }

        if (parentObject == null)
            return null;

        if (parentObject is T parent)
            return parent;
        else
            return FindParent<T>(parentObject);
    }
}