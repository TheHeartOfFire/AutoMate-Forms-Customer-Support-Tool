using AMFormsCST.Desktop.ViewModels.Dialogs;
using System.Windows;
using Xunit;
using Moq;

public class PageHostDialogViewModelTests
{
    [Fact]
    public void Constructor_InitializesProperties()
    {
        // Act
        var hostedVm = new object();
        var vm = new PageHostDialogViewModel(hostedVm, canConfirm: true);

        // Assert
        Assert.Equal("Page Preview", vm.DialogTitle);
        Assert.False(vm.ConfirmSelected);
        Assert.Equal(Visibility.Visible, vm.CanConfirm);
        Assert.Equal(hostedVm, vm.HostedPageViewModel);
    }

    [Fact]
    public void Constructor_SetsCanConfirmCollapsed_WhenFalse()
    {
        // Act
        var hostedVm = new object();
        var vm = new PageHostDialogViewModel(hostedVm, canConfirm: false);

        // Assert
        Assert.Equal(Visibility.Collapsed, vm.CanConfirm);
    }

    [Fact]
    public void Properties_CanBeSetAndRetrieved()
    {
        // Arrange
        var hostedVm = new object();
        var vm = new PageHostDialogViewModel(hostedVm);

        // Act
        vm.DialogTitle = "Custom Title";
        vm.ConfirmSelected = true;
        vm.CanConfirm = Visibility.Visible;

        // Assert
        Assert.Equal("Custom Title", vm.DialogTitle);
        Assert.True(vm.ConfirmSelected);
        Assert.Equal(Visibility.Visible, vm.CanConfirm);
    }
}