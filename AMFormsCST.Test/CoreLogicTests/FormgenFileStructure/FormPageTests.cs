using AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure;
using System.IO;
using System.Text;
using System.Xml;
using Xunit;
using AMFormsCST.Test.CoreLogicTests.FormgenFileStructure;

namespace AMFormsCST.Test.CoreLogicTests.FormgenFileStructure;

public class FormPageTests
{
    private static readonly IEnumerable<object[]> FormgenFilePaths = FormgenTestDataHelper.FormgenFilePaths;
    [Theory]
    [MemberData(nameof(FormgenFilePaths), MemberType = typeof(FormgenTestDataHelper))]
    public void XmlConstructor_ParsesFormPagesFromSampleFiles(string formgenFilePath)
    {
        var dotFormgen = FormgenTestDataHelper.LoadDotFormgen(formgenFilePath);

        foreach (var page in dotFormgen.Pages)
        {
            Assert.NotNull(page.Settings);
            Assert.True(page.Settings.PageNumber > 0);
            Assert.True(page.Fields.Count >= 0);

            // Optionally, check that each field has a non-null expression
            foreach (var field in page.Fields)
            {
                Assert.NotNull(field.Expression);
            }
        }
    }

    [Fact]
    public void ParameterlessConstructor_InitializesPropertiesCorrectly()
    {
        var page = new FormPage();

        Assert.NotNull(page.Settings);
        Assert.Empty(page.Fields);
    }


    [Fact]
    public void GenerateXml_WritesCorrectXml()
    {
        var page = new FormPage
        {
            Settings = { PageNumber = 2, DefaultFontSize = 12 }
        };
        page.Fields.Add(new FormField { Settings = { ID = 100 }, Expression = "FieldA" });
        page.Fields.Add(new FormField { Settings = { ID = 101 }, Expression = "FieldB" });

        var sb = new StringBuilder();
        using var writer = XmlWriter.Create(sb, new XmlWriterSettings { OmitXmlDeclaration = true });

        page.GenerateXml(writer);
        writer.Flush();
        var resultXml = sb.ToString();

        var expectedStart = "<pages pageNumber=\"2\" defaultPoints=\"12\"";
        var expectedFields = "<fields><entry><key>100</key>";
        var expectedEnd = "</fields></pages>";

        Assert.StartsWith(expectedStart, resultXml);
        Assert.Contains(expectedFields, resultXml);
        Assert.EndsWith(expectedEnd, resultXml);
    }
}