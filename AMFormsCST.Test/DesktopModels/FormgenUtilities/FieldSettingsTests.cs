using AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure;
using AMFormsCST.Desktop.Models.FormgenUtilities;
using System.Drawing;
using Xunit;
using static AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure.FormFieldSettings;

public class FieldSettingsTests
{
    private FormFieldSettings CreateSampleFormFieldSettings()
    {
        return new FormFieldSettings
        {
            DecimalPlaces = 2,
            DisplayPartial = true,
            EndIndex = 10,
            FontSize = 14,
            FontAlignment = FormFieldSettings.Alignment.CENTER,
            ID = 123,
            ImpactPosition = new Point(5, 6),
            Kearning = 3,
            LaserRect = new Rectangle(1, 2, 3, 4),
            Length = 20,
            ManualSize = true,
            Bold = true,
            ShrinkToFit = false,
            StartIndex = 1,
            Type = FormFieldSettings.FieldType.NUMERIC
        };
    }

    [Fact]
    public void Constructor_InitializesPropertiesFromFormFieldSettings()
    {
        // Arrange
        var formFieldSettings = CreateSampleFormFieldSettings();

        // Act
        var fieldSettings = new FieldSettings(formFieldSettings);

        // Assert
        Assert.Equal(formFieldSettings.DecimalPlaces, fieldSettings.DecimalPlaces);
        Assert.Equal(formFieldSettings.DisplayPartial, fieldSettings.DisplayPartial);
        Assert.Equal(formFieldSettings.EndIndex, fieldSettings.EndIndex);
        Assert.Equal(formFieldSettings.FontSize, fieldSettings.FontSize);
        Assert.Equal(formFieldSettings.FontAlignment, fieldSettings.FontAlignment);
        Assert.Equal(formFieldSettings.ID, fieldSettings.ID);
        Assert.Equal(formFieldSettings.ImpactPosition, fieldSettings.ImpactPosition);
        Assert.Equal(formFieldSettings.Kearning, fieldSettings.Kerning);
        Assert.Equal(formFieldSettings.LaserRect, fieldSettings.LaserRect);
        Assert.Equal(formFieldSettings.Length, fieldSettings.Length);
        Assert.Equal(formFieldSettings.ManualSize, fieldSettings.ManualSize);
        Assert.Equal(formFieldSettings.Bold, fieldSettings.Bold);
        Assert.Equal(formFieldSettings.ShrinkToFit, fieldSettings.ShrinkToFit);
        Assert.Equal(formFieldSettings.StartIndex, fieldSettings.StartIndex);
        Assert.Equal(formFieldSettings.Type, fieldSettings.Type);
    }

    [Fact]
    public void Constructor_InitializesDefaults_WhenSettingsIsNull()
    {
        // Act
        var fieldSettings = new FieldSettings(null);

        // Assert
        Assert.Equal(0, fieldSettings.DecimalPlaces);
        Assert.False(fieldSettings.DisplayPartial);
        Assert.Equal(0, fieldSettings.EndIndex);
        Assert.Equal(0, fieldSettings.FontSize);
        Assert.Equal(0, fieldSettings.ID);
        Assert.Equal(new Point(), fieldSettings.ImpactPosition);
        Assert.Equal(0, fieldSettings.Kerning);
        Assert.Equal(new Rectangle(), fieldSettings.LaserRect);
        Assert.Equal(0, fieldSettings.Length);
        Assert.False(fieldSettings.ManualSize);
        Assert.False(fieldSettings.Bold);
        Assert.False(fieldSettings.ShrinkToFit);
        Assert.Equal(0, fieldSettings.StartIndex);
        Assert.Equal(FieldType.TEXT, fieldSettings.Type);
        Assert.Equal(Alignment.LEFT, fieldSettings.FontAlignment);
    }

    [Theory]
    [InlineData(Alignment.LEFT, "LEFT")]
    [InlineData(Alignment.CENTER, "CENTER")]
    [InlineData(Alignment.RIGHT, "RIGHT")]
    public void GetFontAlignment_ReturnsExpectedString(Alignment alignment, string expected)
    {
        // Arrange
        var fieldSettings = new FieldSettings(null) { FontAlignment = alignment };

        // Act
        var result = fieldSettings.GetFontAlignment();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(FieldType.TEXT, "TEXT")]
    [InlineData(FieldType.NUMERIC, "NUMERIC")]
    [InlineData(FieldType.SIGNATURE, "SIGNATURE")]
    [InlineData(FieldType.INITIALS, "INITIALS")]
    public void GetFieldType_ReturnsExpectedString(FieldType type, string expected)
    {
        // Arrange
        var fieldSettings = new FieldSettings(null) { Type = type };

        // Act
        var result = fieldSettings.GetFieldType();

        // Assert
        Assert.Equal(expected, result);
    }
}