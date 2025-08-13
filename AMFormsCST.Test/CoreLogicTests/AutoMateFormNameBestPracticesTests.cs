using AMFormsCST.Core.Types.BestPractices.Models;
using AMFormsCST.Core.Types.BestPractices.Practices;
using Assert = Xunit.Assert;

namespace AMFormsCST.Test.CoreLogicTests;

public class AutoMateFormNameBestPracticesTests
{
    [Fact]
    public void Generate_WithOnlyName_ReturnsNameWithTrailingSpace()
    {
        // Arrange
        var model = new AutoMateFormModel { Name = "Retail Installment Contract" };
        var practice = new AutoMateFormNameBestPractices(model);

        // Act
        var result = practice.Generate();

        // Assert
        Assert.Equal("Retail Installment Contract ", result);
    }

    [Fact]
    public void Generate_WithAllProperties_PdfFormat_ReturnsCorrectlyFormattedString()
    {
        // Arrange
        var model = new AutoMateFormModel
        {
            Format = AutoMateFormModel.FormFormat.Pdf,
            State = "TX",
            IsLAW = true,
            Company = "R&R",
            Bank = "Ally",
            Name = "5052",
            Code = "5052A",
            RevisionDate = "1-1-24",
            Manufacturer = "Ford",
            Dealership = "Best",
            VehicleType = AutoMateFormModel.SoldTrade.Sold,
            IsCustom = true
        };
        var practice = new AutoMateFormNameBestPractices(model);
        var expected = "TX LAW R&RAlly 5052 [LAW 5052A (1-1-24)] (FordBest) (SOLD) - Custom";

        // Act
        var result = practice.Generate();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Generate_WithAllProperties_LegacyImpactFormat_ReturnsCorrectlyFormattedString()
    {
        // Arrange
        var model = new AutoMateFormModel
        {
            Format = AutoMateFormModel.FormFormat.LegacyImpact,
            State = "TX",
            IsLAW = true,
            Company = "R&R",
            Bank = "Ally",
            Name = "5052",
            Code = "5052A",
            RevisionDate = "1-1-24",
            Manufacturer = "Ford",
            Dealership = "Best",
            VehicleType = AutoMateFormModel.SoldTrade.Trade,
            IsCustom = true
        };
        var practice = new AutoMateFormNameBestPractices(model);
        var expected = "TX LAW R&RAlly 5052 (LAW 5052A [1-1-24]) (FordBest) (TRADE) - Custom";

        // Act
        var result = practice.Generate();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Generate_WithOnlyCode_EncapsulatesCorrectly()
    {
        // Arrange
        var model = new AutoMateFormModel { Name = "Test Form", Code = "T123" };
        var practice = new AutoMateFormNameBestPractices(model);

        // Assert
        Assert.Equal("Test Form [T123 ]", practice.Generate());
    }

    [Fact]
    public void Generate_WithOnlyRevisionDate_EncapsulatesCorrectly()
    {
        // Arrange
        var model = new AutoMateFormModel { Name = "Test Form", RevisionDate = "1-1-24" };
        var practice = new AutoMateFormNameBestPractices(model);

        // Assert
        Assert.Equal("Test Form [1-1-24]", practice.Generate());
    }

    [Fact]
    public void Generate_WithSlashInDate_ReplacesWithDash()
    {
        // Arrange
        var model = new AutoMateFormModel { RevisionDate = "01/01/24" };
        var practice = new AutoMateFormNameBestPractices(model);

        // Assert
        Assert.Equal("[01-01-24]", practice.Generate());
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
}