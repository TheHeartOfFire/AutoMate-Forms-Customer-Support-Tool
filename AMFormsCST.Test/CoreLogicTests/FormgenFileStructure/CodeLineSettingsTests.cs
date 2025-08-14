using AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure;
using System.IO;
using System.Text;
using System.Xml;
using Xunit;
using CodeType = AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure.CodeLineSettings.CodeType;

namespace AMFormsCST.Test.CoreLogicTests.FormgenFileStructure;

public class CodeLineSettingsTests
{
    [Fact]
    public void ParameterlessConstructor_InitializesWithDefaults()
    {
        // Arrange & Act
        var settings = new CodeLineSettings();

        // Assert
        Assert.Equal(0, settings.Order);
        Assert.Equal(CodeType.PROMPT, settings.Type); // Based on the default case in GetCodeType
        Assert.Null(settings.Variable);
    }

    [Fact]
    public void CloneConstructor_CopiesAndUpdatesPropertiesCorrectly()
    {
        // Arrange
        var originalSettings = new CodeLineSettings
        {
            Order = 1,
            Type = CodeType.INIT,
            Variable = "OldVar"
        };
        var newName = "NewVar";
        var newIndex = 10;

        // Act
        var clonedSettings = new CodeLineSettings(originalSettings, newName, newIndex);

        // Assert
        Assert.Equal(newIndex, clonedSettings.Order);
        Assert.Equal(newName, clonedSettings.Variable);
        Assert.Equal(originalSettings.Type, clonedSettings.Type); // Type should be copied
    }

    [Fact]
    public void XmlConstructor_ParsesAttributesCorrectly()
    {
        // Arrange
        var doc = new XmlDocument();
        var element = doc.CreateElement("codeLine");
        element.SetAttribute("order", "5");
        element.SetAttribute("type", "POST");
        element.SetAttribute("destVariable", "MyPostVar");
        var attributes = element.Attributes;

        // Act
        var settings = new CodeLineSettings(attributes);

        // Assert
        Assert.Equal(5, settings.Order);
        Assert.Equal(CodeType.POST, settings.Type);
        Assert.Equal("MyPostVar", settings.Variable);
    }

    [Theory]
    [InlineData("INIT", CodeType.INIT)]
    [InlineData("PROMPT", CodeType.PROMPT)]
    [InlineData("POST", CodeType.POST)]
    [InlineData("INVALID", CodeType.PROMPT)] // Default case
    public void GetCodeType_FromString_ReturnsCorrectEnum(string typeString, CodeType expectedType)
    {
        // Act
        var result = CodeLineSettings.GetCodeType(typeString);

        // Assert
        Assert.Equal(expectedType, result);
    }

    [Theory]
    [InlineData(CodeType.INIT, "INIT")]
    [InlineData(CodeType.PROMPT, "PROMPT")]
    [InlineData(CodeType.POST, "POST")]
    [InlineData((CodeType)99, "PROMPT")] // Default case
    public void GetCodeType_FromEnum_ReturnsCorrectString(CodeType typeEnum, string expectedString)
    {
        // Act
        var result = CodeLineSettings.GetCodeType(typeEnum);

        // Assert
        Assert.Equal(expectedString, result);
    }

    [Fact]
    public void GenerateXml_WritesCorrectAttributes()
    {
        // Arrange
        var settings = new CodeLineSettings
        {
            Order = 7,
            Type = CodeType.INIT,
            Variable = "InitVar"
        };
        var sb = new StringBuilder();
        using var writer = XmlWriter.Create(sb, new XmlWriterSettings { OmitXmlDeclaration = true });
        writer.WriteStartElement("dummy");

        // Act
        settings.GenerateXml(writer);
        writer.WriteEndElement();
        writer.Flush();
        var resultXml = sb.ToString();

        // Assert
        var expected = "<dummy order=\"7\" type=\"INIT\" destVariable=\"InitVar\" />";
        Assert.Equal(expected, resultXml);
    }
}