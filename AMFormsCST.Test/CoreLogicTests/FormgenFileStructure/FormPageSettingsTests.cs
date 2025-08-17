using AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure;
using System.IO;
using System.Text;
using System.Xml;
using Xunit;
using AMFormsCST.Test.CoreLogicTests.FormgenFileStructure;

namespace AMFormsCST.Test.CoreLogicTests.FormgenFileStructure;

public class FormPageSettingsTests
{
    // Always alias the live sample data provider
    private static readonly IEnumerable<object[]> FormgenFilePaths = FormgenTestDataHelper.FormgenFilePaths;

    [Theory]
    [MemberData(nameof(FormgenFilePaths), MemberType = typeof(FormgenTestDataHelper))]
    public void XmlConstructor_ParsesSettingsFromSampleFiles(string formgenFilePath)
    {
        var dotFormgen = FormgenTestDataHelper.LoadDotFormgen(formgenFilePath);

        foreach (var page in dotFormgen.Pages)
        {
            var settings = page.Settings;
            Assert.NotNull(settings);

            // Basic checks for parsed values
            Assert.True(settings.PageNumber >= 0);
            Assert.True(settings.DefaultFontSize >= 0);
            Assert.True(settings.LeftPrinterMargin >= 0);
            Assert.True(settings.RightPrinterMargin >= 0);
            Assert.True(settings.TopPrinterMargin >= 0);
            Assert.True(settings.BottomPrinterMargin >= 0);
        }
    }

    [Fact]
    public void ParameterlessConstructor_InitializesWithDefaults()
    {
        // Arrange & Act
        var settings = new FormPageSettings();

        // Assert
        Assert.Equal(0, settings.PageNumber);
        Assert.Equal(0, settings.DefaultFontSize);
        Assert.Equal(0, settings.LeftPrinterMargin);
        Assert.Equal(0, settings.RightPrinterMargin);
        Assert.Equal(0, settings.TopPrinterMargin);
        Assert.Equal(0, settings.BottomPrinterMargin);
    }

    [Fact]
    public void XmlConstructor_ParsesAttributesCorrectly()
    {
        // Arrange
        var doc = new XmlDocument();
        var element = doc.CreateElement("pages");
        element.SetAttribute("pageNumber", "2");
        element.SetAttribute("defaultPoints", "12");
        element.SetAttribute("leftPrinterMargin", "10");
        element.SetAttribute("rightPrinterMargin", "10");
        element.SetAttribute("topPrinterMargin", "15");
        element.SetAttribute("bottomPrinterMargin", "15");
        var attributes = element.Attributes;

        // Act
        var settings = new FormPageSettings(attributes);

        // Assert
        Assert.Equal(2, settings.PageNumber);
        Assert.Equal(12, settings.DefaultFontSize);
        Assert.Equal(10, settings.LeftPrinterMargin);
        Assert.Equal(10, settings.RightPrinterMargin);
        Assert.Equal(15, settings.TopPrinterMargin);
        Assert.Equal(15, settings.BottomPrinterMargin);
    }

    [Fact]
    public void GenerateXml_WritesCorrectAttributes()
    {
        // Arrange
        var settings = new FormPageSettings
        {
            PageNumber = 3,
            DefaultFontSize = 9,
            LeftPrinterMargin = 5,
            RightPrinterMargin = 5,
            TopPrinterMargin = 10,
            BottomPrinterMargin = 10
        };
        var sb = new StringBuilder();
        // Use a dummy element to write attributes onto
        using var writer = XmlWriter.Create(sb, new XmlWriterSettings { OmitXmlDeclaration = true });
        writer.WriteStartElement("dummy");

        // Act
        settings.GenerateXml(writer);
        writer.WriteEndElement();
        writer.Flush();
        var resultXml = sb.ToString();

        // Assert
        var expected = "<dummy pageNumber=\"3\" defaultPoints=\"9\" leftPrinterMargin=\"5\" rightPrinterMargin=\"5\" topPrinterMargin=\"10\" bottomPrinterMargin=\"10\" />";
        Assert.Equal(expected, resultXml);
    }
}