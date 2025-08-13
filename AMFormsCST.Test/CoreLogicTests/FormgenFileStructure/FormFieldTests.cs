using AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure;
using System.IO;
using System.Text;
using System.Xml;
using Assert = Xunit.Assert;
using FormatOption = AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure.FormField.FormatOption;

namespace AMFormsCST.Test.CoreLogicTests.FormgenFileStructure;

public class FormFieldTests
{
    [Fact]
    public void ParameterlessConstructor_InitializesProperties()
    {
        // Arrange & Act
        var field = new FormField();

        // Assert
        Assert.NotNull(field.Settings);
        Assert.Null(field.Expression);
        Assert.Null(field.SampleData);
        Assert.Equal(FormatOption.None, field.FormattingOption);
    }

    [Fact]
    public void XmlConstructor_ParsesNodeCorrectly()
    {
        // Arrange
        var xml = @"
            <entry>
                <key>123</key>
                <value uniqueId=""123"" formFieldType=""TEXT"" legacyCol=""0"" legacyLine=""0"" x=""0"" y=""0"" w=""0"" h=""0"" manualSize=""false"" fontPoints=""10"" boldFont=""false"" shrinkFontToFit=""false"" pictureLeft=""0"" pictureRight=""0"" displayPartialField=""false"" startChar=""0"" endChar=""0"" perCharDeltaPts=""0"" alignment=""Left"">
                    <expression>F1+F2</expression>
                    <sampleData>Test Data</sampleData>
                    <formatOption>NAIfBlank</formatOption>
                </value>
            </entry>";
        var doc = new XmlDocument();
        doc.LoadXml(xml);
        var node = doc.DocumentElement!;

        // Act
        var field = new FormField(node);

        // Assert
        Assert.NotNull(field.Settings);
        Assert.Equal(123, field.Settings.ID);
        Assert.Equal("F1+F2", field.Expression);
        Assert.Equal("Test Data", field.SampleData);
        Assert.Equal(FormatOption.NAIfBlank, field.FormattingOption);
    }

    [Theory]
    [InlineData("None", FormatOption.None)]
    [InlineData("NAIfBlank", FormatOption.NAIfBlank)]
    [InlineData("EmptyZeroPrintsNothing", FormatOption.EmptyZeroPrintsNothing)]
    [InlineData("InvalidOption", FormatOption.None)] // Default case
    public void GetFormatOption_FromString_ReturnsCorrectEnum(string optionString, FormatOption expectedOption)
    {
        // Act
        var result = FormField.GetFormatOption(optionString);

        // Assert
        Assert.Equal(expectedOption, result);
    }

    [Theory]
    [InlineData(FormatOption.None, "None")]
    [InlineData(FormatOption.NAIfBlank, "NAIfBlank")]
    [InlineData(FormatOption.NumbersAsWords, "NumberAsWords")]
    [InlineData((FormatOption)99, "None")] // Default case
    public void GetFormatOption_FromEnum_ReturnsCorrectString(FormatOption optionEnum, string expectedString)
    {
        // Act
        var result = FormField.GetFormatOption(optionEnum);

        // Assert
        Assert.Equal(expectedString, result);
    }

    [Fact]
    public void GenerateXml_WritesCorrectXml()
    {
        // Arrange
        var field = new FormField
        {
            Settings = { ID = 456 },
            Expression = "MyExpression",
            SampleData = "MySample",
            FormattingOption = FormatOption.EmptyFieldPrints0
        };
        var sb = new StringBuilder();
        using var writer = XmlWriter.Create(sb, new XmlWriterSettings { OmitXmlDeclaration = true });

        // Act
        field.GenerateXml(writer);
        writer.Flush();
        var resultXml = sb.ToString();

        // Assert
        // Note: The method generates the content *inside* an <entry> tag.
        var expectedStart = "<key>456</key><value";
        var expectedEnd = "><expression>MyExpression</expression><sampleData>MySample</sampleData><formatOption>BlankPrintsZero</formatOption></value>";
        Assert.StartsWith(expectedStart, resultXml);
        Assert.EndsWith(expectedEnd, resultXml);
    }
}