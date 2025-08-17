using AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure;
using System.IO;
using System.Text;
using System.Xml;
using Assert = Xunit.Assert;
using AMFormsCST.Test.CoreLogicTests.FormgenFileStructure; // Ensure this using is present

namespace AMFormsCST.Test.CoreLogicTests.FormgenFileStructure;

public class DotFormgenSettingsTests
{
    // Always alias the live sample data provider
    private static readonly IEnumerable<object[]> FormgenFilePaths = FormgenTestDataHelper.FormgenFilePaths;
    [Fact]
    public void ParameterlessConstructor_InitializesWithDefaults()
    {
        // Arrange & Act
        var settings = new DotFormgenSettings();

        // Assert
        Assert.Equal(0, settings.Version);
        Assert.Equal(string.Empty, settings.UUID);
        Assert.False(settings.LegacyImport);
        Assert.Equal(0, settings.TotalPages);
        Assert.Equal(0, settings.DefaultFontSize);
        Assert.False(settings.MissingSourceJpeg);
        Assert.False(settings.Duplex);
        Assert.Equal(0, settings.MaxAccessoryLines);
        Assert.False(settings.PreprintedLaserForm);
    }

    [Fact]
    public void XmlConstructor_ParsesAttributesCorrectly()
    {
        // Arrange
        var doc = new XmlDocument();
        var element = doc.CreateElement("formDef");
        element.SetAttribute("version", "2");
        element.SetAttribute("publishedUUID", "test-guid");
        element.SetAttribute("legacyImport", "true");
        element.SetAttribute("totalPages", "5");
        element.SetAttribute("defaultPoints", "12");
        element.SetAttribute("missingSourceJpeg", "true");
        element.SetAttribute("duplex", "true");
        element.SetAttribute("maxAccessoryLines", "10");
        element.SetAttribute("prePrintedLaserForm", "true");
        var attributes = element.Attributes;

        // Act
        var settings = new DotFormgenSettings(attributes);

        // Assert
        Assert.Equal(2, settings.Version);
        Assert.Equal("test-guid", settings.UUID);
        Assert.True(settings.LegacyImport);
        Assert.Equal(5, settings.TotalPages);
        Assert.Equal(12, settings.DefaultFontSize);
        Assert.True(settings.MissingSourceJpeg);
        Assert.True(settings.Duplex);
        Assert.Equal(10, settings.MaxAccessoryLines);
        Assert.True(settings.PreprintedLaserForm);
    }

    [Fact]
    public void GenerateXML_WritesCorrectAttributes()
    {
        // Arrange
        var settings = new DotFormgenSettings
        {
            Version = 3,
            UUID = "generate-guid",
            LegacyImport = false,
            TotalPages = 1,
            DefaultFontSize = 9,
            MissingSourceJpeg = false,
            Duplex = false,
            MaxAccessoryLines = 0,
            PreprintedLaserForm = false
        };
        var sb = new StringBuilder();
        // Use a dummy element to write attributes onto
        using var writer = XmlWriter.Create(sb, new XmlWriterSettings { OmitXmlDeclaration = true });
        writer.WriteStartElement("dummy");

        // Act
        settings.GenerateXML(writer);
        writer.WriteEndElement();
        writer.Flush();
        var resultXml = sb.ToString();

        // Assert
        var expected = "<dummy version=\"3\" publishedUUID=\"generate-guid\" legacyImport=\"false\" totalPages=\"1\" defaultPoints=\"9\" missingSourceJpeg=\"false\" duplex=\"false\" maxAccessoryLines=\"0\" prePrintedLaserForm=\"false\" />";
        Assert.Equal(expected, resultXml);
    }

    [Theory]
    [MemberData(nameof(FormgenFilePaths), MemberType = typeof(FormgenTestDataHelper))]
    public void XmlConstructor_ParsesAttributesFromSampleFiles(string formgenFilePath)
    {
        var dotFormgen = FormgenTestDataHelper.LoadDotFormgen(formgenFilePath);
        var settings = dotFormgen.Settings;

        // Basic assertions to ensure parsing works
        Assert.True(settings.Version > 0);
        Assert.False(string.IsNullOrWhiteSpace(settings.UUID));
        Assert.True(settings.TotalPages > 0);
        // You can add more assertions based on what you expect in your sample data
    }
}