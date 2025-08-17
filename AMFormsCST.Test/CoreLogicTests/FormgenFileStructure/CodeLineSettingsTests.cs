using AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure;
using System.IO;
using System.Text;
using System.Xml;
using Xunit;
using CodeType = AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure.CodeLineSettings.CodeType;
using AMFormsCST.Test.CoreLogicTests.FormgenFileStructure;

namespace AMFormsCST.Test.CoreLogicTests.FormgenFileStructure;

public class CodeLineSettingsTests
{
    // Always alias the live sample data provider
    private static readonly IEnumerable<object[]> FormgenFilePaths = FormgenTestDataHelper.FormgenFilePaths;

    [Theory]
    [MemberData(nameof(FormgenFilePaths), MemberType = typeof(FormgenTestDataHelper))]
    public void XmlConstructor_ParsesSettingsFromSampleFiles(string formgenFilePath)
    {
        var dotFormgen = FormgenTestDataHelper.LoadDotFormgen(formgenFilePath);

        foreach (var codeLine in dotFormgen.CodeLines)
        {
            var settings = codeLine.Settings;
            Assert.NotNull(settings);

            // Basic checks for parsed values
            Assert.True(settings.Order >= 0);
            Assert.False(string.IsNullOrWhiteSpace(settings.Variable));
            Assert.True(settings.Type == CodeType.INIT || settings.Type == CodeType.PROMPT || settings.Type == CodeType.POST);
        }
    }

    [Fact]
    public void ParameterlessConstructor_InitializesWithDefaults()
    {
        var settings = new CodeLineSettings();

        Assert.Equal(0, settings.Order);
        Assert.Equal(CodeType.PROMPT, settings.Type);
        Assert.Null(settings.Variable);
    }

    [Fact]
    public void CloneConstructor_CopiesAndUpdatesPropertiesCorrectly()
    {
        var originalSettings = new CodeLineSettings
        {
            Order = 1,
            Type = CodeType.INIT,
            Variable = "OldVar"
        };
        var newName = "NewVar";
        var newIndex = 10;

        var clonedSettings = new CodeLineSettings(originalSettings, newName, newIndex);

        Assert.Equal(newIndex, clonedSettings.Order);
        Assert.Equal(newName, clonedSettings.Variable);
        Assert.Equal(originalSettings.Type, clonedSettings.Type);
    }

    [Fact]
    public void XmlConstructor_ParsesAttributesCorrectly()
    {
        var doc = new XmlDocument();
        var element = doc.CreateElement("codeLine");
        element.SetAttribute("order", "5");
        element.SetAttribute("type", "POST");
        element.SetAttribute("destVariable", "MyPostVar");
        var attributes = element.Attributes;

        var settings = new CodeLineSettings(attributes);

        Assert.Equal(5, settings.Order);
        Assert.Equal(CodeType.POST, settings.Type);
        Assert.Equal("MyPostVar", settings.Variable);
    }

    [Theory]
    [InlineData("INIT", CodeType.INIT)]
    [InlineData("PROMPT", CodeType.PROMPT)]
    [InlineData("POST", CodeType.POST)]
    [InlineData("INVALID", CodeType.PROMPT)]
    public void GetCodeType_FromString_ReturnsCorrectEnum(string typeString, CodeType expectedType)
    {
        var result = CodeLineSettings.GetCodeType(typeString);
        Assert.Equal(expectedType, result);
    }

    [Theory]
    [InlineData(CodeType.INIT, "INIT")]
    [InlineData(CodeType.PROMPT, "PROMPT")]
    [InlineData(CodeType.POST, "POST")]
    [InlineData((CodeType)99, "PROMPT")]
    public void GetCodeType_FromEnum_ReturnsCorrectString(CodeType typeEnum, string expectedString)
    {
        var result = CodeLineSettings.GetCodeType(typeEnum);
        Assert.Equal(expectedString, result);
    }

    [Fact]
    public void GenerateXml_WritesCorrectAttributes()
    {
        var settings = new CodeLineSettings
        {
            Order = 7,
            Type = CodeType.INIT,
            Variable = "InitVar"
        };
        var sb = new StringBuilder();
        using var writer = XmlWriter.Create(sb, new XmlWriterSettings { OmitXmlDeclaration = true });
        writer.WriteStartElement("dummy");

        settings.GenerateXml(writer);
        writer.WriteEndElement();
        writer.Flush();
        var resultXml = sb.ToString();

        var expected = "<dummy order=\"7\" type=\"INIT\" destVariable=\"InitVar\" />";
        Assert.Equal(expected, resultXml);
    }
}