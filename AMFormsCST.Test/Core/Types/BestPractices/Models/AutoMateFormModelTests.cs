using AMFormsCST.Core.Types.BestPractices.Models;
using Assert = Xunit.Assert;

namespace AMFormsCST.Test.Core.Types.BestPractices.Models;

public class AutoMateFormModelTests
{
    [Fact]
    public void Constructor_InitializesWithDefaultValues()
    {
        // Arrange & Act
        var model = new AutoMateFormModel();

        // Assert
        Assert.Equal(string.Empty, model.Name);
        Assert.Equal(string.Empty, model.Code);
        Assert.Equal(string.Empty, model.RevisionDate);
        Assert.Equal(string.Empty, model.Company);
        Assert.Equal(string.Empty, model.Bank);
        Assert.Equal(string.Empty, model.Manufacturer);
        Assert.Equal(string.Empty, model.Dealership);
        Assert.Equal(string.Empty, model.State);
        Assert.False(model.IsCustom);
        Assert.False(model.IsLAW);
        Assert.Equal(AutoMateFormModel.SoldTrade.None, model.VehicleType);
        Assert.Equal(AutoMateFormModel.FormFormat.LegacyImpact, model.Format);
    }

    [Fact]
    public void Properties_CanBeSetAndGetCorrectly()
    {
        // Arrange
        var model = new AutoMateFormModel();

        // Act
        model.Name = "Test Form";
        model.Code = "T123";
        model.RevisionDate = "01-01-24";
        model.Company = "Test Company";
        model.IsCustom = true;
        model.VehicleType = AutoMateFormModel.SoldTrade.Sold;
        model.Format = AutoMateFormModel.FormFormat.Pdf;

        // Assert
        Assert.Equal("Test Form", model.Name);
        Assert.Equal("T123", model.Code);
        Assert.Equal("01-01-24", model.RevisionDate);
        Assert.Equal("Test Company", model.Company);
        Assert.True(model.IsCustom);
        Assert.Equal(AutoMateFormModel.SoldTrade.Sold, model.VehicleType);
        Assert.Equal(AutoMateFormModel.FormFormat.Pdf, model.Format);
    }
}