using AMFormsCST.Desktop.Models.FormgenUtilities;
using AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure;
using Xunit;

public class FormSettingsTests
{
    private DotFormgenSettings CreateDotFormgenSettings()
    {
        return new DotFormgenSettings
        {
            Version = 2,
            UUID = "uuid-abc",
            LegacyImport = true,
            TotalPages = 7,
            DefaultFontSize = 12,
            MissingSourceJpeg = true,
            Duplex = false,
            MaxAccessoryLines = 3,
            PreprintedLaserForm = true
        };
    }

    [Fact]
    public void Constructor_InitializesBackingSettings()
    {
        // Arrange
        var coreSettings = CreateDotFormgenSettings();

        // Act
        var settings = new FormSettings(coreSettings);

        // Assert
        Assert.Equal(coreSettings.UUID, settings.PublishedUUID);
        Assert.Equal(coreSettings.TotalPages, settings.TotalPages);
        Assert.Equal(coreSettings.Version.ToString(), settings.Version);
        Assert.Equal(coreSettings.LegacyImport, settings.LegacyImport);
        Assert.Equal(coreSettings.DefaultFontSize, settings.DefaultPoints);
        Assert.Equal(coreSettings.MissingSourceJpeg, settings.MissingSourceJpeg);
        Assert.Equal(coreSettings.Duplex, settings.Duplex);
        Assert.Equal(coreSettings.MaxAccessoryLines, settings.MaxAccessoryLines);
        Assert.Equal(coreSettings.PreprintedLaserForm, settings.PrePrintedLaserForm);
    }

    [Fact]
    public void Setters_UpdateBackingSettings()
    {
        // Arrange
        var coreSettings = CreateDotFormgenSettings();
        var settings = new FormSettings(coreSettings);

        // Act
        settings.PublishedUUID = "uuid-new";
        settings.LegacyImport = false;
        settings.TotalPages = 99;
        settings.DefaultPoints = 22;
        settings.MissingSourceJpeg = false;
        settings.Duplex = true;
        settings.MaxAccessoryLines = 8;
        settings.PrePrintedLaserForm = false;

        // Assert
        Assert.Equal("uuid-new", coreSettings.UUID);
        Assert.False(coreSettings.LegacyImport);
        Assert.Equal(99, coreSettings.TotalPages);
        Assert.Equal(22, coreSettings.DefaultFontSize);
        Assert.False(coreSettings.MissingSourceJpeg);
        Assert.True(coreSettings.Duplex);
        Assert.Equal(8, coreSettings.MaxAccessoryLines);
        Assert.False(coreSettings.PreprintedLaserForm);
    }

    [Fact]
    public void Version_IsReadOnly()
    {
        // Arrange
        var coreSettings = CreateDotFormgenSettings();
        var settings = new FormSettings(coreSettings);

        // Act & Assert
        Assert.Equal(coreSettings.Version.ToString(), settings.Version);
        // No setter available, so this property is read-only
    }
}