using AMFormsCST.Desktop.Models.FormgenUtilities;
using AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure;
using System.Collections.Generic;
using Xunit;

public class FormPropertiesTests
{
    private DotFormgen CreateDotFormgen()
    {
        return new DotFormgen
        {
            Title = "Test Title",
            TradePrompt = true,
            FormType = DotFormgen.Format.Pdf,
            SalesPersonPrompt = false,
            Username = "tester",
            BillingName = "Bill Name",
            Category = DotFormgen.FormCategory.Gap,
            Settings = new DotFormgenSettings { UUID = "uuid-123", TotalPages = 5 }
        };
    }

    [Fact]
    public void Properties_ReflectDotFormgenValues()
    {
        // Arrange
        var dotFormgen = CreateDotFormgen();
        var props = new FormProperties(dotFormgen);

        // Act & Assert
        Assert.Equal(dotFormgen.Title, props.Title);
        Assert.Equal(dotFormgen.TradePrompt, props.TradePrompt);
        Assert.Equal(dotFormgen.FormType, props.FormType);
        Assert.Equal(dotFormgen.SalesPersonPrompt, props.SalesPersonPrompt);
        Assert.Equal(dotFormgen.Username, props.Username);
        Assert.Equal(dotFormgen.BillingName, props.BillingName);
        Assert.Equal(dotFormgen.Category, props.Category);
    }

    [Fact]
    public void Setters_UpdateDotFormgenValues()
    {
        // Arrange
        var dotFormgen = CreateDotFormgen();
        var props = new FormProperties(dotFormgen);

        // Act
        props.Title = "New Title";
        props.TradePrompt = false;
        props.FormType = DotFormgen.Format.Impact;
        props.SalesPersonPrompt = true;
        props.Username = "newuser";
        props.BillingName = "New Bill";
        props.Category = DotFormgen.FormCategory.Retail;

        // Assert
        Assert.Equal("New Title", dotFormgen.Title);
        Assert.False(dotFormgen.TradePrompt);
        Assert.Equal(DotFormgen.Format.Impact, dotFormgen.FormType);
        Assert.True(dotFormgen.SalesPersonPrompt);
        Assert.Equal("newuser", dotFormgen.Username);
        Assert.Equal("New Bill", dotFormgen.BillingName);
        Assert.Equal(DotFormgen.FormCategory.Retail, dotFormgen.Category);
    }

    [Theory]
    [InlineData(DotFormgen.Format.Pdf, "Pdf")]
    [InlineData(DotFormgen.Format.LegacyLaser, "Legacy Laser")]
    [InlineData(DotFormgen.Format.Impact, "Impact")]
    [InlineData((DotFormgen.Format)999, "Unknown")]
    public void GetFormType_ReturnsExpectedString(DotFormgen.Format format, string expected)
    {
        // Arrange
        var dotFormgen = CreateDotFormgen();
        dotFormgen.FormType = format;
        var props = new FormProperties(dotFormgen);

        // Act
        var result = props.GetFormType();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(DotFormgen.FormCategory.Gap, "Gap")]
    [InlineData(DotFormgen.FormCategory.Maintenance, "Maintenance")]
    [InlineData(DotFormgen.FormCategory.Other, "Other")]
    [InlineData((DotFormgen.FormCategory)999, "Unknown")]
    public void GetCategory_ReturnsExpectedString(DotFormgen.FormCategory category, string expected)
    {
        // Arrange
        var dotFormgen = CreateDotFormgen();
        dotFormgen.Category = category;
        var props = new FormProperties(dotFormgen);

        // Act
        var result = props.GetCategory();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetDisplayProperties_ReturnsExpectedProperties()
    {
        // Arrange
        var dotFormgen = CreateDotFormgen();
        var props = new FormProperties(dotFormgen);

        // Act
        var displayProps = props.GetDisplayProperties();

        // Assert
        Assert.Contains(displayProps, dp => dp.Name == "Title:" && dp.Value == dotFormgen.Title);
        Assert.Contains(displayProps, dp => dp.Name == "Form Type:" && dp.Value == dotFormgen.FormType.ToString());
        Assert.Contains(displayProps, dp => dp.Name == "Category:" && dp.Value == dotFormgen.Category.ToString());
        Assert.Contains(displayProps, dp => dp.Name == "Username:" && dp.Value == dotFormgen.Username);
        Assert.Contains(displayProps, dp => dp.Name == "Billing Name:" && dp.Value == dotFormgen.BillingName);
        Assert.Contains(displayProps, dp => dp.Name == "Trade Prompt:" && dp.Value == dotFormgen.TradePrompt.ToString());
        Assert.Contains(displayProps, dp => dp.Name == "Salesperson Prompt:" && dp.Value == dotFormgen.SalesPersonPrompt.ToString());
        Assert.Contains(displayProps, dp => dp.Name == "UUID:" && dp.Value == dotFormgen.Settings.UUID);
        Assert.Contains(displayProps, dp => dp.Name == "Total Pages:" && dp.Value == dotFormgen.Settings.TotalPages.ToString());
    }
}