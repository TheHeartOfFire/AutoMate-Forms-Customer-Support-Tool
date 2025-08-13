using AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Assert = Xunit.Assert;

namespace AMFormsCST.Test.CoreLogicTests.FormgenFileStructure;

public class PromptDataTests
{
    [Fact]
    public void XmlConstructor_ParsesMessageAndChoicesCorrectly()
    {
        // Arrange
        var xml = @"
            <promptData>
                <promptMessage>Choose an option</promptMessage>
                <choices>Option1</choices>
                <choices>Option2</choices>
            </promptData>";
        var doc = new XmlDocument();
        doc.LoadXml(xml);
        var node = doc.DocumentElement!;

        // Act
        var promptData = new PromptData(node);

        // Assert
        Assert.Equal("Choose an option", promptData.Message);
        Assert.Equal(2, promptData.Choices.Count);
        Assert.Contains("Option1", promptData.Choices);
        Assert.Contains("Option2", promptData.Choices);
    }

    [Fact]
    public void XmlConstructor_WithNoChoices_ParsesMessageOnly()
    {
        // Arrange
        var xml = @"
            <promptData>
                <promptMessage>Only a message</promptMessage>
            </promptData>";
        var doc = new XmlDocument();
        doc.LoadXml(xml);
        var node = doc.DocumentElement!;

        // Act
        var promptData = new PromptData(node);

        // Assert
        Assert.Equal("Only a message", promptData.Message);
        Assert.Empty(promptData.Choices);
    }

    [Fact]
    public void GenerateXml_WritesCorrectXml()
    {
        // Arrange
        var promptData = new PromptData
        {
            Message = "Pick one",
            Choices = new List<string> { "A", "B" }
        };
        var sb = new StringBuilder();
        using var writer = XmlWriter.Create(sb, new XmlWriterSettings { OmitXmlDeclaration = true });

        // Act
        promptData.GenerateXml(writer);
        writer.Flush();
        var resultXml = sb.ToString();

        // Assert
        var expected =
            "<promptData><promptMessage>Pick one</promptMessage><choices>A</choices><choices>B</choices></promptData>";
        Assert.Equal(expected, resultXml);
    }
}