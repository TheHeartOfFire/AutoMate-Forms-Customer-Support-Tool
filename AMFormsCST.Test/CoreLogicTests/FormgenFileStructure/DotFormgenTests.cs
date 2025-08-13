using AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Assert = Xunit.Assert;
using CodeType = AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure.CodeLineSettings.CodeType;
using DotFormgenFormat = AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure.DotFormgen.Format;

namespace AMFormsCST.Test.CoreLogicTests.FormgenFileStructure;

public class DotFormgenTests
{
    [Fact]
    public void ParameterlessConstructor_InitializesPropertiesCorrectly()
    {
        // Arrange & Act
        var formgen = new DotFormgen();

        // Assert
        Assert.NotNull(formgen.Settings);
        Assert.Empty(formgen.Pages);
        Assert.Empty(formgen.CodeLines);
        Assert.Empty(formgen.States);
    }

    [Fact]
    public void XmlConstructor_ParsesAllPropertiesCorrectly()
    {
        // Arrange
        var xml = @"
            <formDef version=""1"" publishedUUID=""test-guid"" legacyImport=""false"" totalPages=""1"" defaultPoints=""10"" missingSourceJpeg=""false"" duplex=""false"" maxAccessoryLines=""0"" prePrintedLaserForm=""false"">
                <title>My Test Form</title>
                <tradePrompt>true</tradePrompt>
                <formPrintType>Pdf</formPrintType>
                <salespersonPrompt>false</salespersonPrompt>
                <username>jdoe</username>
                <billingName>John Doe</billingName>
                <codeLines order=""1"" type=""PROMPT"" destVariable=""Var1""><promptData/></codeLines>
                <formCategory>Retail</formCategory>
                <validStates>TX</validStates>
                <pages pageNumber=""1""><fields/></pages>
            </formDef>";
        var doc = new XmlDocument();
        doc.LoadXml(xml);
        var rootElement = doc.DocumentElement!;

        // Act
        var formgen = new DotFormgen(rootElement);

        // Assert
        Assert.Equal("My Test Form", formgen.Title);
        Assert.True(formgen.TradePrompt);
        Assert.Equal(DotFormgenFormat.Pdf, formgen.FormType);
        Assert.False(formgen.SalesPersonPrompt);
        Assert.Equal("jdoe", formgen.Username);
        Assert.Equal("John Doe", formgen.BillingName);
        Assert.Single(formgen.CodeLines);
        Assert.Equal(DotFormgen.FormCategory.Retail, formgen.Category);
        Assert.Single(formgen.States);
        Assert.Equal("TX", formgen.States[0]);
        Assert.Single(formgen.Pages);
    }

    [Fact]
    public void CountingMethods_ReturnCorrectCounts()
    {
        // Arrange
        var formgen = new DotFormgen();
        formgen.CodeLines.AddRange(new[]
        {
            new CodeLine { Settings = { Type = CodeType.INIT } },
            new CodeLine { Settings = { Type = CodeType.PROMPT } },
            new CodeLine { Settings = { Type = CodeType.PROMPT } },
            new CodeLine { Settings = { Type = CodeType.POST } }
        });
        var page1 = new FormPage();
        page1.Fields.Add(new FormField());
        var page2 = new FormPage();
        page2.Fields.Add(new FormField());
        page2.Fields.Add(new FormField());
        formgen.Pages.AddRange(new[] { page1, page2 });

        // Act & Assert
        Assert.Equal(1, formgen.InitCount());
        Assert.Equal(2, formgen.PromptCount());
        Assert.Equal(1, formgen.PostCount());
        Assert.Equal(3, formgen.FieldCount());
    }

    [Fact]
    public void GetPrompt_And_GetField_ReturnCorrectItems()
    {
        // Arrange
        var formgen = new DotFormgen();
        var prompt1 = new CodeLine { Settings = { Type = CodeType.PROMPT, Variable = "P1" } };
        var prompt2 = new CodeLine { Settings = { Type = CodeType.PROMPT, Variable = "P2" } };
        formgen.CodeLines.AddRange(new[] { new CodeLine { Settings = { Type = CodeType.INIT } }, prompt1, prompt2 });

        var field1 = new FormField { Expression = "F1" };
        var field2 = new FormField { Expression = "F2" };
        var page = new FormPage();
        page.Fields.AddRange(new[] { field1, field2 });
        formgen.Pages.Add(page);

        // Act & Assert
        Assert.Same(prompt2, formgen.GetPrompt(1));
        Assert.Same(field2, formgen.GetField(1));
    }

    [Fact]
    public void GenerateXML_ProducesCorrectStructure()
    {
        // Arrange
        var formgen = new DotFormgen
        {
            Settings = { UUID = "xml-guid", Version = 2 },
            Title = "Generated Form",
            FormType = DotFormgenFormat.LegacyImpact,
            Category = DotFormgen.FormCategory.Custom,
            States = { "CA" },
            CodeLines = { new CodeLine { Settings = { Order = 1, Type = CodeType.INIT, Variable = "Var1" }, Expression = "1+1" } }
        };

        // Act
        var xmlString = formgen.GenerateXML();
        var doc = new XmlDocument();
        doc.LoadXml(xmlString);

        // Assert
        var root = doc.DocumentElement!;
        Assert.Equal("formDef", root.Name);
        Assert.Equal("xml-guid", root.GetAttribute("publishedUUID"));
        Assert.Equal("Generated Form", root.SelectSingleNode("title")?.InnerText);
        Assert.Equal("LegacyImpact", root.SelectSingleNode("formPrintType")?.InnerText);
        Assert.Equal("Custom", root.SelectSingleNode("formCategory")?.InnerText);
        Assert.Equal("CA", root.SelectSingleNode("validStates")?.InnerText);
        Assert.NotNull(root.SelectSingleNode("codeLines"));
        Assert.Equal("1+1", root.SelectSingleNode("codeLines/expression")?.InnerText);
    }
}