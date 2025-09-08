using AMFormsCST.Desktop.Models.FormgenUtilities;
using Xunit;
using CorePromptSettings = AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure.PromptDataSettings;

namespace AMFormsCST.Test.Desktop.Models.FormgenUtilities;
public class PromptDataSettingsTests
{
    private CorePromptSettings CreateCoreSettings()
    {
        return new CorePromptSettings
        {
            Type = CorePromptSettings.PromptType.RadioButtons,
            IsExpression = true,
            Required = false,
            Length = 10,
            DecimalPlaces = 2,
            Delimiter = ",",
            AllowNegative = true,
            ForceUpperCase = false,
            MakeBuyerVars = true,
            IncludeNoneAsOption = false
        };
    }

    [Fact]
    public void Constructor_InitializesBackingSettings()
    {
        // Arrange
        var coreSettings = CreateCoreSettings();

        // Act
        var settings = new PromptDataSettings(coreSettings);

        // Assert
        Assert.Equal(coreSettings.Type, settings.Type);
        Assert.Equal(coreSettings.IsExpression, settings.IsExpression);
        Assert.Equal(coreSettings.Required, settings.Required);
        Assert.Equal(coreSettings.Length, settings.Length);
        Assert.Equal(coreSettings.DecimalPlaces, settings.DecimalPlaces);
        Assert.Equal(coreSettings.Delimiter, settings.Delimiter);
        Assert.Equal(coreSettings.AllowNegative, settings.AllowNegative);
        Assert.Equal(coreSettings.ForceUpperCase, settings.ForceUpperCase);
        Assert.Equal(coreSettings.MakeBuyerVars, settings.MakeBuyerVars);
        Assert.Equal(coreSettings.IncludeNoneAsOption, settings.IncludeNoneAsOption);
    }

    [Fact]
    public void Setters_UpdateBackingSettings()
    {
        // Arrange
        var coreSettings = CreateCoreSettings();
        var settings = new PromptDataSettings(coreSettings)
        {
            // Act
            Type = CorePromptSettings.PromptType.CheckBox,
            IsExpression = false,
            Required = true,
            Length = 99,
            DecimalPlaces = 5,
            Delimiter = "|",
            AllowNegative = false,
            ForceUpperCase = true,
            MakeBuyerVars = false,
            IncludeNoneAsOption = true
        };

        // Assert
        Assert.Equal(CorePromptSettings.PromptType.CheckBox, coreSettings.Type);
        Assert.False(coreSettings.IsExpression);
        Assert.True(coreSettings.Required);
        Assert.Equal(99, coreSettings.Length);
        Assert.Equal(5, coreSettings.DecimalPlaces);
        Assert.Equal("|", coreSettings.Delimiter);
        Assert.False(coreSettings.AllowNegative);
        Assert.True(coreSettings.ForceUpperCase);
        Assert.False(coreSettings.MakeBuyerVars);
        Assert.True(coreSettings.IncludeNoneAsOption);
    }
}