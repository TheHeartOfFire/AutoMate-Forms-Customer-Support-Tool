using AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure;
using System.IO;
using System.Text;
using System.Xml;
using Assert = Xunit.Assert;

namespace AMFormsCST.Test.CoreLogicTests.FormgenFileStructure;

public class FormPageTests
{
    [Fact]
    public void ParameterlessConstructor_InitializesPropertiesCorrectly()
    {
        // Arrange & Act
        var page = new FormPage();

        // Assert
        Assert.NotNull(page.Settings);
        Assert.Empty(page.Fields);
    }

    [Fact]
    public void XmlConstructor_ParsesNodeCorrectly()
    {
        // Arrange
        var xml = @"
            <pages pageNumber=""1"" defaultPoints=""10"" leftMargin=""0"" rightMargin=""0"" topMargin=""0"" bottomMargin=""0"">
                <fields>
                    <entry>
                        <key>1</key>
                        <value uniqueId=""1""><expression>F1</expression></value>
                    </entry>
                    <entry>
                        <key>2</key>
                        <value uniqueId=""2""><expression>F2</expression></value>
                    </entry>
                </fields>
            </pages>";
        var doc = new XmlDocument();
        doc.LoadXml(xml);
        var node = doc.DocumentElement!;

        // Act
        var page = new FormPage(node);

        // Assert
        Assert.NotNull(page.Settings);
        Assert.Equal(1, page.Settings.PageNumber);
        Assert.Equal(10, page.Settings.DefaultFontSize);
        Assert.Equal(2, page.Fields.Count);
        Assert.Equal("F1", page.Fields[0].Expression);
    }

    [Fact]
    public void GenerateXml_WritesCorrectXml()
    {
        // Arrange
        var page = new FormPage
        {
            Settings = { PageNumber = 2, DefaultFontSize = 12 }
        };
        page.Fields.Add(new FormField { Settings = { ID = 100 }, Expression = "FieldA" });
        page.Fields.Add(new FormField { Settings = { ID = 101 }, Expression = "FieldB" });

        var sb = new StringBuilder();
        using var writer = XmlWriter.Create(sb, new XmlWriterSettings { OmitXmlDeclaration = true });

        // Act
        page.GenerateXml(writer);
        writer.Flush();
        var resultXml = sb.ToString();

        // Assert
        var expectedStart = "<pages pageNumber=\"2\" defaultPoints=\"12\"";
        var expectedFields = "<fields><entry><key>100</key>";
        var expectedEnd = "</fields></pages>";

        Assert.StartsWith(expectedStart, resultXml);
        Assert.Contains(expectedFields, resultXml);
        Assert.EndsWith(expectedEnd, resultXml);
    }
}