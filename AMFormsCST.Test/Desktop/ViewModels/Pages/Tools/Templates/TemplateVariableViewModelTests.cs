using AMFormsCST.Desktop.ViewModels.Pages.Tools.Templates;
using AMFormsCST.Core.Interfaces.BestPractices;
using Moq;
using Xunit;

namespace AMFormsCST.Test.Desktop.ViewModels.Pages.Tools.Templates;

public class TemplateVariableViewModelTests
{
    [Fact]
    public void Constructor_SetsVariableProperty()
    {
        // Arrange
        var mockVariable = new Mock<ITextTemplateVariable>();
        mockVariable.SetupGet(v => v.ProperName).Returns("DefaultName");

        // Act
        var vm = new TemplateVariableViewModel(mockVariable.Object);

        // Assert
        Assert.Equal(mockVariable.Object, vm.Variable);
        Assert.Equal("DefaultName", vm.DefaultValue);
    }

    [Fact]
    public void Value_Property_DefaultsToEmptyString()
    {
        // Arrange
        var mockVariable = new Mock<ITextTemplateVariable>();
        var vm = new TemplateVariableViewModel(mockVariable.Object);

        // Assert
        Assert.Equal(string.Empty, vm.Value);
    }

    [Fact]
    public void Value_Property_Setter_RaisesPropertyChanged()
    {
        // Arrange
        var mockVariable = new Mock<ITextTemplateVariable>();
        var vm = new TemplateVariableViewModel(mockVariable.Object);
        bool propertyChangedRaised = false;

        vm.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(vm.Value))
                propertyChangedRaised = true;
        };

        // Act
        vm.Value = "NewValue";

        // Assert
        Assert.True(propertyChangedRaised);
        Assert.Equal("NewValue", vm.Value);
    }

    [Fact]
    public void Variable_Property_Setter_RaisesPropertyChanged()
    {
        // Arrange
        var mockVariable1 = new Mock<ITextTemplateVariable>();
        var mockVariable2 = new Mock<ITextTemplateVariable>();
        var vm = new TemplateVariableViewModel(mockVariable1.Object);
        bool propertyChangedRaised = false;

        vm.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(vm.Variable))
                propertyChangedRaised = true;
        };

        // Act
        vm.Variable = mockVariable2.Object;

        // Assert
        Assert.True(propertyChangedRaised);
        Assert.Equal(mockVariable2.Object, vm.Variable);
    }

    [Fact]
    public void DefaultValue_ReturnsProperName()
    {
        // Arrange
        var mockVariable = new Mock<ITextTemplateVariable>();
        mockVariable.SetupGet(v => v.ProperName).Returns("ProperNameValue");
        var vm = new TemplateVariableViewModel(mockVariable.Object);

        // Act
        var defaultValue = vm.DefaultValue;

        // Assert
        Assert.Equal("ProperNameValue", defaultValue);
    }
}