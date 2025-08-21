using AMFormsCST.Desktop.Models.FormgenUtilities;
using AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AMFormsCST.Test.Desktop.Models.FormgenUtilities;
public class PagePropertiesTests
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

    private FormPage CreateFormPage(FormPageSettings? settings = null)
    {
        return new FormPage { Settings = settings };
    }

    [Fact]
    public void Constructor_InitializesSettings()
    {
        // Arrange
        var pageSettings = CreatePageSettings(2, 14, 11, 21, 6, 16);
        var page = CreateFormPage(pageSettings);

        // Act
        var props = new PageProperties(page);

        // Assert
        Assert.NotNull(props.Settings);
        Assert.IsType<PageSettings>(props.Settings);
        var s = (PageSettings)props.Settings;
        Assert.Equal(2, s.PageNumber);
        Assert.Equal(14, s.DefaultFontSize);
        Assert.Equal(11, s.LeftPrinterMargin);
        Assert.Equal(21, s.RightPrinterMargin);
        Assert.Equal(6, s.TopPrinterMargin);
        Assert.Equal(16, s.BottomPrinterMargin);
    }

    [Fact]
    public void GetDisplayProperties_ReturnsExpectedProperties()
    {
        // Arrange
        var pageSettings = CreatePageSettings(3, 18, 12, 22, 7, 17);
        var page = CreateFormPage(pageSettings);
        var props = new PageProperties(page);

        // Act
        var displayProps = props.GetDisplayProperties().ToList();

        // Assert
        Assert.Contains(displayProps, dp => dp.Name == "PageNumber" && Equals(dp.Value, 3));
        Assert.Contains(displayProps, dp => dp.Name == "DefaultFontSize" && Equals(dp.Value, 18));
        Assert.Contains(displayProps, dp => dp.Name == "LeftPrinterMargin" && Equals(dp.Value, 12));
        Assert.Contains(displayProps, dp => dp.Name == "RightPrinterMargin" && Equals(dp.Value, 22));
        Assert.Contains(displayProps, dp => dp.Name == "TopPrinterMargin" && Equals(dp.Value, 7));
        Assert.Contains(displayProps, dp => dp.Name == "BottomPrinterMargin" && Equals(dp.Value, 17));
    }

    [Fact]
    public void GetDisplayProperties_HandlesNullSettings()
    {
        // Arrange
        var page = CreateFormPage(null);
        var props = new PageProperties(page);

        // Act
        var displayProps = props.GetDisplayProperties().ToList();

        // Assert
        Assert.Empty(displayProps);
    }
}