using AMFormsCST.Desktop.ViewModels.Dialogs;
using System.Windows;
using Xunit;
using Moq;
using AMFormsCST.Test.Helpers;

namespace AMFormsCST.Test.Desktop.ViewModels.Dialogs;
public class NewTemplateDialogViewModelTests
{
    [Fact]
    public void Constructor_InitializesPropertiesToEmpty()
    {
        // Act
        var vm = new NewTemplateDialogViewModel();

        // Assert
        Assert.Equal(string.Empty, vm.TemplateName);
        Assert.Equal(string.Empty, vm.TemplateDescription);
        Assert.Equal(string.Empty, vm.TemplateContent);
    }

    [Fact]
    public void Properties_CanBeSetAndRetrieved()
    {
        // Arrange
        var vm = new NewTemplateDialogViewModel();

        // Act
        vm.TemplateName = "Name";
        vm.TemplateDescription = "Desc";
        vm.TemplateContent = "Content";

        // Assert
        Assert.Equal("Name", vm.TemplateName);
        Assert.Equal("Desc", vm.TemplateDescription);
        Assert.Equal("Content", vm.TemplateContent);
    }
}