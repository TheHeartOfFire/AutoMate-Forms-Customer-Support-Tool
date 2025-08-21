using AMFormsCST.Core.Types.BestPractices.Models;
using AMFormsCST.Core.Types.BestPractices.Practices;
using Assert = Xunit.Assert;

namespace AMFormsCST.Test.Core.Types.BestPractices.Practices;

public class AutoMateFormNameBestPracticesTests
{
    private AutoMateFormModel model = new AutoMateFormModel
    {
        Format = AutoMateFormModel.FormFormat.Pdf,
        IsLAW = true,
        Name = "Retail Installment Contract",
        Code = "553-TX-ARB-eps",
        RevisionDate = "1-1-24"
    };

    [Fact]
    public void Generate_WithOnlyName_ReturnsName()
    {
        // Arrange
        model.Code = string.Empty;
        model.RevisionDate = string.Empty;
        model.IsLAW = false;
        var practice = new AutoMateFormNameBestPractices(model);

        // Act
        var result = practice.Generate();

        // Assert
        Assert.Equal("Retail Installment Contract", result);
    }

    [Fact]
    public void Generate_WithAllProperties_PdfFormat_ReturnsCorrectlyFormattedString()
    {
        var practice = new AutoMateFormNameBestPractices(model);
        var expected = "LAW Retail Installment Contract [LAW 553-TX-ARB-eps (1-1-24)]";

        // Act
        var result = practice.Generate();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Generate_WithAllProperties_LegacyImpactFormat_ReturnsCorrectlyFormattedString()
    {
        var practice = new AutoMateFormNameBestPractices(model);
        model.Format = AutoMateFormModel.FormFormat.LegacyImpact;
        var expected = "LAW Retail Installment Contract (LAW 553-TX-ARB-eps [1-1-24])";

        // Act
        var result = practice.Generate();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Generate_WithOnlyCode_EncapsulatesCorrectly()
    {
        // Arrange
        model.IsLAW = false;
        model.RevisionDate = string.Empty;
        var practice = new AutoMateFormNameBestPractices(model);

        // Assert
        Assert.Equal("Retail Installment Contract [553-TX-ARB-eps]", practice.Generate());
    }

    [Fact]
    public void Generate_WithOnlyRevisionDate_EncapsulatesCorrectly()
    {
        // Arrange
        model.IsLAW = false;
        model.Code = string.Empty;

        var practice = new AutoMateFormNameBestPractices(model);

        // Assert
        Assert.Equal("Retail Installment Contract [1-1-24]", practice.Generate());
    }

    [Fact]
    public void Generate_WithSlashInDate_ReplacesWithDash()
    {
        // Arrange
        var model = new AutoMateFormModel { RevisionDate = "01/01/24" };
        var practice = new AutoMateFormNameBestPractices(model);

        // Assert
        Assert.Equal(" (01-01-24)", practice.Generate());
    }

    [Fact]
    public void Generate_WithEmptyModel_ReturnsEmptyString()
    {
        // Arrange
        var model = new AutoMateFormModel();
        var practice = new AutoMateFormNameBestPractices(model);

        // Act
        var result = practice.Generate();

        // Assert
        Assert.Equal(string.Empty, result);
    }
    [Fact]
    public void Generate_WithCustomTag_ReturnsCorrectlyFormattedString()
    {
        // Arrange
        model.IsCustom = true;
        var practice = new AutoMateFormNameBestPractices(model);
        var expected = "LAW Retail Installment Contract [LAW 553-TX-ARB-eps (1-1-24)] -Custom";

        // Act
        var result = practice.Generate();

        // Assert
        Assert.Equal(expected, result);
    }
    [Fact]
    public void Generate_WithSoldTag_ReturnsCorrectlyFormattedString()
    {
        // Arrange
        model.VehicleType = AutoMateFormModel.SoldTrade.Sold;
        var practice = new AutoMateFormNameBestPractices(model);
        var expected = "LAW Retail Installment Contract [LAW 553-TX-ARB-eps (1-1-24)] (SOLD)";

        // Act
        var result = practice.Generate();

        // Assert
        Assert.Equal(expected, result);
    }
    [Fact]
    public void Generate_WithTradeTag_ReturnsCorrectlyFormattedString()
    {
        // Arrange
        model.VehicleType = AutoMateFormModel.SoldTrade.Trade;
        var practice = new AutoMateFormNameBestPractices(model);
        var expected = "LAW Retail Installment Contract [LAW 553-TX-ARB-eps (1-1-24)] (TRADE)";

        // Act
        var result = practice.Generate();

        // Assert
        Assert.Equal(expected, result);
    }
}