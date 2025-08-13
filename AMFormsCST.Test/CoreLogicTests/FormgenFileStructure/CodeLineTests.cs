using AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure;
using System.IO;
using System.Text;
using System.Xml;
using Assert = Xunit.Assert;
using CodeType = AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure.CodeLineSettings.CodeType;

namespace AMFormsCST.Test.CoreLogicTests.FormgenFileStructure;

public class CodeLineTests
{
    [Fact]
    public void ParameterlessConstructor_InitializesProperties()
    {
        // Arrange & Act
        var codeLine = new CodeLine();

        // Assert
        Assert.NotNull(codeLine.Settings);
        Assert.NotNull(codeLine.PromptData);
        Assert.Null(codeLine.Expression);
    }

    [Fact]
    public void CloneConstructor_CopiesPropertiesAndUpdatesSettings()
    {
        // Arrange
        var originalCodeLine = new CodeLine
        {
            Settings = { Variable = "OldVar", Order = 1, Type = CodeType.PROMPT },
            PromptData = { Message = "Original Message" },
            Expression = "Original Expression"
        };
        var newName = "NewVar";
        var newIndex = 99;

        // Act
        var clonedCodeLine = new CodeLine(originalCodeLine, newName, newIndex);

        // Assert
        Assert.NotNull(clonedCodeLine.Settings);
        Assert.Equal(newName, clonedCodeLine.Settings.Variable);
        Assert.Equal(newIndex, clonedCodeLine.Settings.Order);
        Assert.Equal(originalCodeLine.Settings.Type, clonedCodeLine.Settings.Type);

        // Verify that Expression and PromptData are copied
        Assert.Equal(originalCodeLine.Expression, clonedCodeLine.Expression);
        Assert.Same(originalCodeLine.PromptData, clonedCodeLine.PromptData); // Reference is copied
    }

    [Fact]
    public void XmlConstructor_ForNonPromptType_ParsesExpression()
    {
        // Arrange
        var xml = "<codeLine order=\"1\" type=\"INIT\" destVariable=\"MyVar\"><expression>F1+F2</expression></codeLine>";
        var doc = new XmlDocument();
        doc.LoadXml(xml);
        var node = doc.DocumentElement!;

        // Act
        var codeLine = new CodeLine(node);

        // Assert
        Assert.NotNull(codeLine.Settings);
        Assert.Equal(CodeType.INIT, codeLine.Settings.Type);
        Assert.Equal("MyVar", codeLine.Settings.Variable);
        Assert.Equal("F1+F2", codeLine.Expression);
        Assert.Null(codeLine.PromptData); // Should be null for non-prompt types
    }

    [Fact]
    public void XmlConstructor_ForPromptType_ParsesPromptData()
    {
        // Arrange
        var xml = "<codeLine order=\"2\" type=\"PROMPT\" destVariable=\"Choice\"><promptData><message>Choose one</message><choices><string>A</string><string>B</string></choices></promptData></codeLine>";
        var doc = new XmlDocument();
        doc.LoadXml(xml);
        var node = doc.DocumentElement!;

        // Act
        var codeLine = new CodeLine(node);

        // Assert
        Assert.NotNull(codeLine.Settings);
        Assert.Equal(CodeType.PROMPT, codeLine.Settings.Type);
        Assert.NotNull(codeLine.PromptData);
        Assert.Equal("Choose one", codeLine.PromptData.Message);
        Assert.Equal(2, codeLine.PromptData.Choices.Count);
        Assert.Equal("A", codeLine.PromptData.Choices[0]);
        Assert.Null(codeLine.Expression); // Should be null for prompt types
    }

    [Fact]
    public void GenerateXml_ForNonPromptType_WritesCorrectXml()
    {
        // Arrange
        var codeLine = new CodeLine
        {
            Settings = { Order = 5, Type = CodeType.POST, Variable = "Result" },
            Expression = "F1 * 1.1"
        };
        var sb = new StringBuilder();
        using var writer = XmlWriter.Create(sb, new XmlWriterSettings { OmitXmlDeclaration = true });

        // Act
        codeLine.GenerateXml(writer);
        writer.Flush();
        var resultXml = sb.ToString();

        // Assert
        var expected = "<codeLines order=\"5\" type=\"POST\" destVariable=\"Result\"><expression>F1 * 1.1</expression></codeLines>";
        Assert.Equal(expected, resultXml);
    }

    [Fact]
    public void GenerateXml_ForPromptType_WritesCorrectXml()
    {
        // Arrange
        var codeLine = new CodeLine
        {
            Settings = { Order = 10, Type = CodeType.PROMPT, Variable = "UserChoice" },
            PromptData = { Message = "Select an option", Choices = { "Opt1", "Opt2" } }
        };
        var sb = new StringBuilder();
        using var writer = XmlWriter.Create(sb, new XmlWriterSettings { OmitXmlDeclaration = true });

        // Act
        codeLine.GenerateXml(writer);
        writer.Flush();
        var resultXml = sb.ToString();

        // Assert
        var expected = "<codeLines order=\"10\" type=\"PROMPT\" destVariable=\"UserChoice\"><promptData><message>Select an option</message><choices><string>Opt1</string><string>Opt2</string></choices></promptData></codeLines>";
        Assert.Equal(expected, resultXml);
    }
}