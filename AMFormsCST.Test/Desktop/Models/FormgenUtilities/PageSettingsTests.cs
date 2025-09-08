using AMFormsCST.Desktop.Models.FormgenUtilities;
using AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure;
using Xunit;

namespace AMFormsCST.Test.Desktop.Models.FormgenUtilities;
public class PageSettingsTests
{
    private FormPageSettings CreatePageSettings(
        int pageNumber = 1,
        int fontSize = 12,
        int left = 10,
        int right = 20,
        int top = 5,
        int bottom = 15)
    {
        return new FormPageSettings
        {
            PageNumber = pageNumber,
            DefaultFontSize = fontSize,
            LeftPrinterMargin = left,
            RightPrinterMargin = right,
            TopPrinterMargin = top,
            BottomPrinterMargin = bottom
        };
    }

    [Fact]
    public void Constructor_InitializesBackingSettings()
    {
        // Arrange
        var coreSettings = CreatePageSettings(2, 14, 11, 21, 6, 16);

        // Act
        var settings = new PageSettings(coreSettings);

        // Assert
        Assert.Equal(2, settings.PageNumber);
        Assert.Equal(14, settings.DefaultFontSize);
        Assert.Equal(11, settings.LeftPrinterMargin);
        Assert.Equal(21, settings.RightPrinterMargin);
        Assert.Equal(6, settings.TopPrinterMargin);
        Assert.Equal(16, settings.BottomPrinterMargin);
    }

    [Fact]
    public void Setters_UpdateBackingSettings()
    {
        // Arrange
        var coreSettings = CreatePageSettings();
        var settings = new PageSettings(coreSettings);

        // Act
        settings.PageNumber = 99;
        settings.DefaultFontSize = 88;
        settings.LeftPrinterMargin = 77;
        settings.RightPrinterMargin = 66;
        settings.TopPrinterMargin = 55;
        settings.BottomPrinterMargin = 44;

        // Assert
        Assert.Equal(99, coreSettings.PageNumber);
        Assert.Equal(88, coreSettings.DefaultFontSize);
        Assert.Equal(77, coreSettings.LeftPrinterMargin);
        Assert.Equal(66, coreSettings.RightPrinterMargin);
        Assert.Equal(55, coreSettings.TopPrinterMargin);
        Assert.Equal(44, coreSettings.BottomPrinterMargin);
    }
}