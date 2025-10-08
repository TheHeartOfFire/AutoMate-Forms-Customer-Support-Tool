    using AMFormsCST.Core.Types.BestPractices.TextTemplates.Models;
using AMFormsCST.Desktop.Models.Templates;
using Moq;
using System;
using System.Collections.Generic;
using System.Windows.Documents;
using Xunit;

namespace AMFormsCST.Test.Desktop.Models.Templates;
public class DeprecatedTemplateTests
{
    [Fact]
    public void Constructor_SetsPropertiesCorrectly()
    {
        var defaults = new List<string> { "A", "B" };
        var template = new DeprecatedTemplate("Name", "Text", 2, defaults, DeprecatedTemplate.TemplateType.PublishComments);

        Assert.Equal("Name", template.Name);
        Assert.Equal("Text", template.Text);
        Assert.Equal((ushort)2, template.Variables);
        Assert.Equal(defaults, template.VariableDefaults);
        Assert.Equal(DeprecatedTemplate.TemplateType.PublishComments, template.Type);
    }

    [Theory]
    [InlineData("Case# {0} Deal# {1}", new[] { "123", "456" }, 2, "Case# 123 Deal# 456")]
    [InlineData("Hello {0}", new[] { "World" }, 1, "Hello World")]
    [InlineData("No variables", new string[0], 0, "No variables")]
    public void ExplicitConversion_FormatsTextCorrectly(string text, string[] defaults, ushort variables, string expected)
    {
        var template = new DeprecatedTemplate("Test", text, variables, new List<string>(defaults), DeprecatedTemplate.TemplateType.Other);
        var converted = (TextTemplate)template;

        Assert.Equal(expected + "\r\n", TextTemplate.GetFlowDocumentPlainText(converted.Text));
        Assert.Equal("Test", converted.Name);
        Assert.Equal("[Converted]", converted.Description);
        Assert.Equal(TextTemplate.TemplateType.Other, converted.Type);
    }

    [Fact]
    public void ExplicitConversion_HandlesFormatExceptionGracefully()
    {
        // Not enough variables for the format string
        var template = new DeprecatedTemplate("Test", "Value: {0} {1}", 1, new List<string> { "A" }, DeprecatedTemplate.TemplateType.Other);
        var converted = (TextTemplate)template;

        // Should fallback to raw text
        Assert.Equal("Value: {0} {1}\r\n", TextTemplate.GetFlowDocumentPlainText(converted.Text));
    }

    [Fact]
    public void ConvertType_MapsEnumsCorrectly()
    {
        Assert.Equal(TextTemplate.TemplateType.PublishComments, GetConvertedType(DeprecatedTemplate.TemplateType.PublishComments));
        Assert.Equal(TextTemplate.TemplateType.InternalComments, GetConvertedType(DeprecatedTemplate.TemplateType.InternalComments));
        Assert.Equal(TextTemplate.TemplateType.ClosureComments, GetConvertedType(DeprecatedTemplate.TemplateType.ClosureComments));
        Assert.Equal(TextTemplate.TemplateType.Other, GetConvertedType(DeprecatedTemplate.TemplateType.Other));
    }

    private static TextTemplate.TemplateType GetConvertedType(DeprecatedTemplate.TemplateType type)
    {
        // Use reflection to access the private static method
        var method = typeof(DeprecatedTemplate).GetMethod("ConvertType", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        return (TextTemplate.TemplateType)method.Invoke(null, new object[] { type });
    }
}