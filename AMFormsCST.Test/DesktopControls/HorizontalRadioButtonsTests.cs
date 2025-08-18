using AMFormsCST.Desktop.Controls;
using AMFormsCST.Test.Helpers;
using Moq;
using System.Collections;
using System.Windows.Input;
using Xunit;

[Collection("STA Tests")]
public class HorizontalRadioButtonsTests
{
    [WpfFact]
    public void ItemsSource_Property_SetAndGet_Works()
    {
        // Arrange
        var control = new HorizontalRadioButtons();
        var items = new[] { "A", "B", "C" };

        // Act
        control.ItemsSource = items;

        // Assert
        Assert.Same(items, control.ItemsSource);
    }

    [WpfFact]
    public void RadioButtonCommand_Property_SetAndGet_Works()
    {
        // Arrange
        var control = new HorizontalRadioButtons();
        var commandMock = new Mock<ICommand>();

        // Act
        control.RadioButtonCommand = commandMock.Object;

        // Assert
        Assert.Same(commandMock.Object, control.RadioButtonCommand);
    }

    [WpfFact]
    public void CommandParameterPath_Property_SetAndGet_Works()
    {
        // Arrange
        var control = new HorizontalRadioButtons();

        // Act
        control.CommandParameterPath = "Id";

        // Assert
        Assert.Equal("Id", control.CommandParameterPath);
    }

    [WpfFact]
    public void ContentBindingPath_Property_SetAndGet_Works()
    {
        // Arrange
        var control = new HorizontalRadioButtons();

        // Act
        control.ContentBindingPath = "Name";

        // Assert
        Assert.Equal("Name", control.ContentBindingPath);
    }

    [WpfFact]
    public void ContentFallbackValue_Property_SetAndGet_Works()
    {
        // Arrange
        var control = new HorizontalRadioButtons();

        // Act
        control.ContentFallbackValue = "Fallback";

        // Assert
        Assert.Equal("Fallback", control.ContentFallbackValue);
    }

    [WpfFact]
    public void GroupName_Property_SetAndGet_Works()
    {
        // Arrange
        var control = new HorizontalRadioButtons();

        // Act
        control.GroupName = "Group1";

        // Assert
        Assert.Equal("Group1", control.GroupName);
    }

    [WpfFact]
    public void RefreshTrigger_Property_SetAndGet_Works()
    {
        // Arrange
        var control = new HorizontalRadioButtons();
        var trigger = new object();

        // Act
        control.RefreshTrigger = trigger;

        // Assert
        Assert.Same(trigger, control.RefreshTrigger);
    }

    [WpfFact]
    public void DeleteCommand_Property_SetAndGet_Works()
    {
        // Arrange
        var control = new HorizontalRadioButtons();
        var commandMock = new Mock<ICommand>();

        // Act
        control.DeleteCommand = commandMock.Object;

        // Assert
        Assert.Same(commandMock.Object, control.DeleteCommand);
    }
}