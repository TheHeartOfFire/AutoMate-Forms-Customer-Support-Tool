using AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure;
using System.Text;
using System.Xml;
using Xunit;
using PromptType = AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure.PromptDataSettings.PromptType;
using AMFormsCST.Test.CoreLogicTests.FormgenFileStructure;

namespace AMFormsCST.Test.CoreLogicTests.FormgenFileStructure;

public class PromptDataSettingsTests
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
            if (codeLine.PromptData?.Settings != null)
            {
                var settings = codeLine.PromptData.Settings;
                Assert.NotNull(settings);

                // Basic checks for parsed values
                Assert.True(Enum.IsDefined(typeof(PromptType), settings.Type));
                Assert.True(settings.Length >= 0);
                Assert.True(settings.DecimalPlaces >= 0);
                // Delimiter may be null or empty, but should not throw
            }
        }
    }

    [Fact]
    public void ParameterlessConstructor_InitializesWithDefaults()
    {
        var settings = new PromptDataSettings();

        Assert.Equal(PromptType.OneTwoThree, settings.Type);
        Assert.False(settings.IsExpression);
        Assert.False(settings.Required);
        Assert.Equal(0, settings.Length);
        Assert.Equal(0, settings.DecimalPlaces);
        Assert.Null(settings.Delimiter);
        Assert.False(settings.AllowNegative);
        Assert.False(settings.ForceUpperCase);
        Assert.False(settings.MakeBuyerVars);
        Assert.False(settings.IncludeNoneAsOption);
    }

    [Fact]
    public void XmlConstructor_ParsesAttributesCorrectly()
    {
        var doc = new XmlDocument();
        var element = doc.CreateElement("promptData");
        element.SetAttribute("type", "Dropdown");
        element.SetAttribute("promptIsExpression", "true");
        element.SetAttribute("required", "true");
        element.SetAttribute("leftSize", "5");
        element.SetAttribute("rightSize", "2");
        element.SetAttribute("choicesDelimiter", "|");
        element.SetAttribute("allowNegatives", "true");
        element.SetAttribute("forceUpperCase", "true");
        element.SetAttribute("makeBuyerVars", "true");
        element.SetAttribute("includeNoneAsOption", "true");
        var attributes = element.Attributes;

        var settings = new PromptDataSettings(attributes);

        Assert.Equal(PromptType.Dropdown, settings.Type);
        Assert.True(settings.IsExpression);
        Assert.True(settings.Required);
        Assert.Equal(5, settings.Length);
        Assert.Equal(2, settings.DecimalPlaces);
        Assert.Equal("|", settings.Delimiter);
        Assert.True(settings.AllowNegative);
        Assert.True(settings.ForceUpperCase);
        Assert.True(settings.MakeBuyerVars);
        Assert.True(settings.IncludeNoneAsOption);
    }

    [Theory]
    [InlineData("Dropdown", PromptType.Dropdown)]
    [InlineData("Money", PromptType.Money)]
    [InlineData("VIN", PromptType.VIN)]
    [InlineData("Zip5", PromptType.ZIP5)]
    [InlineData("INVALID", PromptType.Text)]
    public void GetPromptType_FromString_ReturnsCorrectEnum(string typeString, PromptType expectedType)
    {
        var result = PromptDataSettings.GetPromptType(typeString);
        Assert.Equal(expectedType, result);
    }

    [Theory]
    [InlineData(PromptType.Dropdown, "Dropdown")]
    [InlineData(PromptType.Money, "Money")]
    [InlineData(PromptType.VIN, "VIN")]
    [InlineData(PromptType.ZIP5, "Zip5")]
    [InlineData((PromptType)99, "Text")]
    public void GetPromptType_FromEnum_ReturnsCorrectString(PromptType typeEnum, string expectedString)
    {
        var result = PromptDataSettings.GetPromptType(typeEnum);
        Assert.Equal(expectedString, result);
    }

    [Theory]
    [InlineData(PromptType.OneTwoThree, "123")]
    [InlineData(PromptType.CheckBox, "CHK")]
    [InlineData(PromptType.Money, "$1.00")]
    [InlineData(PromptType.Text, "ABC")]
    [InlineData((PromptType)99, "ABC")]
    public void PromptDescriptor_ReturnsCorrectString(PromptType typeEnum, string expected)
    {
        var result = PromptDataSettings.PromptDescriptor(typeEnum);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GenerateXml_WritesCorrectAttributes()
    {
        var settings = new PromptDataSettings
        {
            Type = PromptType.RadioButtons,
            IsExpression = true,
            Required = true,
            Length = 8,
            DecimalPlaces = 3,
            Delimiter = ",",
            AllowNegative = true,
            ForceUpperCase = false,
            MakeBuyerVars = true,
            IncludeNoneAsOption = false
        };
        var sb = new StringBuilder();
        using var writer = XmlWriter.Create(sb, new XmlWriterSettings { OmitXmlDeclaration = true });
        writer.WriteStartElement("dummy");

        settings.GenerateXml(writer);
        writer.WriteEndElement();
        writer.Flush();
        var resultXml = sb.ToString();

        Assert.Contains("type=\"RadioButtons\"", resultXml);
        Assert.Contains("promptIsExpression=\"true\"", resultXml);
        Assert.Contains("required=\"true\"", resultXml);
        Assert.Contains("leftSize=\"8\"", resultXml);
        Assert.Contains("rightSize=\"3\"", resultXml);
        Assert.Contains("choicesDelimiter=\",\"", resultXml);
        Assert.Contains("allowNegatives=\"true\"", resultXml);
        Assert.Contains("forceUpperCase=\"false\"", resultXml);
        Assert.Contains("makeBuyerVars=\"true\"", resultXml);
        Assert.Contains("includeNoneAsOption=\"false\"", resultXml);
    }
}