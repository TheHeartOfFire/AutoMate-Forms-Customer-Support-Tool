using AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure;
using AMFormsCST.Desktop.Models.FormgenUtilities;
using AMFormsCST.Desktop.Models.FormgenUtilities.Grouping;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using CodeLineSettings = AMFormsCST.Desktop.Models.FormgenUtilities.CodeLineSettings;
using static AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure.CodeLineSettings;

namespace AMFormsCST.Test.Desktop.Models.FormgenUtilities;

public class CodeLinePropertiesTests
{
    private CodeLine MakeCodeLine(CodeType type, string? variable = null, string? expression = null, PromptData? promptData = null)
    {
        var settings = new AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure.CodeLineSettings
        {
            Type = type,
            Variable = variable
        };
        return new CodeLine { Settings = settings, Expression = expression, PromptData = promptData };
    }

    [Fact]
    public void Constructor_InitializesSettingsAndPromptData()
    {
        // Arrange
        var promptData = new PromptData { Message = "Choose", Choices = new List<string> { "A", "B" } };
        var codeLine = MakeCodeLine(CodeType.PROMPT, "F1", "expr", promptData);

        // Act
        var props = new CodeLineProperties(codeLine);

        // Assert
        Assert.NotNull(props.Settings);
        Assert.IsType<CodeLineSettings>(props.Settings);
        Assert.NotNull(props.PromptData);
        Assert.Equal("Choose", props.PromptData.Message);
    }

    [Fact]
    public void Expression_Property_GetsAndSetsUnderlyingCodeLine()
    {
        // Arrange
        var codeLine = MakeCodeLine(CodeType.POST, "F2", "original");
        var props = new CodeLineProperties(codeLine);

        // Act
        props.Expression = "updated";

        // Assert
        Assert.Equal("updated", props.Expression);
        Assert.Equal("updated", codeLine.Expression);
    }

    [Fact]
    public void GetDisplayProperties_ReturnsExpectedProperties()
    {
        // Arrange
        var promptData = new PromptData { Message = "Prompt!", Choices = new List<string> { "X", "Y" } };
        var codeLine = MakeCodeLine(CodeType.INIT, "Var1", "Expr1", promptData);
        codeLine.Settings.Order = 42;
        var props = new CodeLineProperties(codeLine);

        // Act
        var displayProps = props.GetDisplayProperties().ToList();

        // Assert
        Assert.Contains(displayProps, dp => dp.Name == "Expression" && (string?)dp.Value == "Expr1");
        Assert.Contains(displayProps, dp => dp.Name == "Order" && (int)dp.Value == 42);
        Assert.Contains(displayProps, dp => dp.Name == "Type" && (CodeType)dp.Value == CodeType.INIT);
        Assert.Contains(displayProps, dp => dp.Name == "Variable" && (string?)dp.Value == "Var1");
        Assert.Contains(displayProps, dp => dp.Name == "Message" && (string?)dp.Value == "Prompt!");
        Assert.Contains(displayProps, dp => dp.Name == "Choices" && ((List<string>)dp.Value).SequenceEqual(new[] { "X", "Y" }));
    }

    [Fact]
    public void GetDisplayProperties_HandlesMissingPromptDataAndExpression()
    {
        // Arrange
        var codeLine = MakeCodeLine(CodeType.POST, "Var2", null, null);
        codeLine.Settings.Order = 7;
        var props = new CodeLineProperties(codeLine);

        // Act
        var displayProps = props.GetDisplayProperties().ToList();

        // Assert
        Assert.DoesNotContain(displayProps, dp => dp.Name == "Message");
        Assert.DoesNotContain(displayProps, dp => dp.Name == "Choices");
        Assert.Contains(displayProps, dp => dp.Name == "Order" && (int)dp.Value == 7);
        Assert.Contains(displayProps, dp => dp.Name == "Type" && (CodeType)dp.Value == CodeType.POST);
        Assert.Contains(displayProps, dp => dp.Name == "Variable" && (string?)dp.Value == "Var2");
    }
}