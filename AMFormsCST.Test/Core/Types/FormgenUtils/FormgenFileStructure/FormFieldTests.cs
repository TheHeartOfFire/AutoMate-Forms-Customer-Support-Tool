using AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure;
using System.IO;
using System.Text;
using System.Xml;
using Xunit;
using FormatOption = AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure.FormField.FormatOption;

namespace AMFormsCST.Test.Core.Types.FormgenUtils.FormgenFileStructure;

public class FormFieldTests
{
    // Always alias the live sample data provider
    private static readonly IEnumerable<object[]> FormgenFilePaths = FormgenTestDataHelper.FormgenFilePaths;

    [Theory]
    [MemberData(nameof(FormgenFilePaths), MemberType = typeof(FormgenTestDataHelper))]
    public void XmlConstructor_ParsesFormFieldsFromSampleFiles(string formgenFilePath)
    {
        var dotFormgen = FormgenTestDataHelper.LoadDotFormgen(formgenFilePath);

        foreach (var page in dotFormgen.Pages)
        {
            foreach (var field in page.Fields)
            {
                Assert.NotNull(field.Settings);
                // Expression and SampleData may be null, but should not throw
                // FormattingOption should be a valid enum value
                Assert.True(Enum.IsDefined(typeof(FormatOption), field.FormattingOption));
            }
        }
    }

    [Fact]
    public void ParameterlessConstructor_InitializesProperties()
    {
        var field = new FormField();

        Assert.NotNull(field.Settings);
        Assert.Null(field.Expression);
        Assert.Null(field.SampleData);
        Assert.Equal(FormatOption.None, field.FormattingOption);
    }

    [Fact]
    public void XmlConstructor_ParsesNodeCorrectly()
    {
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

        var field = new FormField(node);

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
    [InlineData("InvalidOption", FormatOption.None)]
    public void GetFormatOption_FromString_ReturnsCorrectEnum(string optionString, FormatOption expectedOption)
    {
        var result = FormField.GetFormatOption(optionString);
        Assert.Equal(expectedOption, result);
    }

    [Theory]
    [InlineData(FormatOption.None, "None")]
    [InlineData(FormatOption.NAIfBlank, "NAIfBlank")]
    [InlineData(FormatOption.NumbersAsWords, "NumberAsWords")]
    [InlineData((FormatOption)99, "None")]
    public void GetFormatOption_FromEnum_ReturnsCorrectString(FormatOption optionEnum, string expectedString)
    {
        var result = FormField.GetFormatOption(optionEnum);
        Assert.Equal(expectedString, result);
    }
}