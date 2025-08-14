using AMFormsCST.Desktop.Models.FormgenUtilities;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using PromptData = AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure.PromptData;
using CorePromptSettings = AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure.PromptDataSettings;


public class PromptDataPropertiesTests
{
    private PromptData CreatePromptData(
        string? message = "Test Message",
        List<string>? choices = null,
        CorePromptSettings? settings = null)
    {
        return new PromptData
        {
            Message = message,
            Choices = choices ?? new List<string> { "A", "B" },
            Settings = settings ?? new CorePromptSettings
            {
                Type = CorePromptSettings.PromptType.RadioButtons,
                Length = 5,
                Required = true
            }
        };
    }

    [Fact]
    public void Constructor_InitializesSettingsAndCorePromptData()
    {
        // Arrange
        var promptData = CreatePromptData("Hello", new List<string> { "X", "Y" });

        // Act
        var props = new PromptDataProperties(promptData);

        // Assert
        Assert.NotNull(props.Settings);
        Assert.Equal(promptData.Settings.Length, ((PromptDataSettings)props.Settings).Length);
        Assert.Equal(promptData.Message, props.Message);
        Assert.Equal(promptData.Choices, props.Choices);
    }

    [Fact]
    public void Message_Property_GetsAndSetsUnderlyingPromptData()
    {
        // Arrange
        var promptData = CreatePromptData("Initial");
        var props = new PromptDataProperties(promptData);

        // Act
        props.Message = "Updated";

        // Assert
        Assert.Equal("Updated", props.Message);
        Assert.Equal("Updated", promptData.Message);
    }

    [Fact]
    public void Choices_Property_GetsAndSetsUnderlyingPromptData()
    {
        // Arrange
        var promptData = CreatePromptData("Msg", new List<string> { "A" });
        var props = new PromptDataProperties(promptData);

        // Act
        props.Choices = new List<string> { "B", "C" };

        // Assert
        Assert.Equal(new List<string> { "B", "C" }, props.Choices);
        Assert.Equal(new List<string> { "B", "C" }, promptData.Choices);
    }

    [Fact]
    public void GetDisplayProperties_ReturnsExpectedProperties()
    {
        // Arrange
        var settings = new CorePromptSettings
        {
            Type = CorePromptSettings.PromptType.CheckBox,
            Length = 10,
            Required = false
        };
        var promptData = CreatePromptData("Prompt!", new List<string> { "Yes", "No" }, settings);
        var props = new PromptDataProperties(promptData);

        // Act
        var displayProps = props.GetDisplayProperties().ToList();

        // Assert
        Assert.Contains(displayProps, dp => dp.Name == "Message:" && dp.Value == "Prompt!");
        Assert.Contains(displayProps, dp => dp.Name == "Choices:" && dp.Value == "Yes, No");
        Assert.Contains(displayProps, dp => dp.Name == "Type:" && dp.Value == settings.Type.ToString());
        Assert.Contains(displayProps, dp => dp.Name == "Length:" && dp.Value == "10");
        Assert.Contains(displayProps, dp => dp.Name == "Required:" && dp.Value == "False");
    }

    [Fact]
    public void GetDisplayProperties_HandlesNoChoices()
    {
        // Arrange
        var promptData = CreatePromptData("Prompt!", new List<string>(), new CorePromptSettings { Type = CorePromptSettings.PromptType.Text, Length = 1, Required = true });
        var props = new PromptDataProperties(promptData);

        // Act
        var displayProps = props.GetDisplayProperties().ToList();

        // Assert
        Assert.Contains(displayProps, dp => dp.Name == "Message:" && dp.Value == "Prompt!");
        Assert.DoesNotContain(displayProps, dp => dp.Name == "Choices:");
    }
}