using AMFormsCST.Desktop.Models;

namespace AMFormsCST.Test.ModelTests;

public class TestDealModelTests
{
    [Fact]
    public void IsBlank_IsTrue_ForNewModel()
    {
        // Arrange
        var deal = new TestDeal();

        // Assert
        Assert.True(deal.IsBlank);
    }

    [Theory]
    [InlineData("Deal123", null)]
    [InlineData(null, "Test Purpose")]
    public void IsBlank_IsFalse_WhenPropertiesAreSet(string? dealNumber, string? purpose)
    {
        // Arrange
        var deal = new TestDeal();

        // Act
        deal.DealNumber = dealNumber;
        deal.Purpose = purpose;

        // Assert
        Assert.False(deal.IsBlank);
    }

    [Fact]
    public void SelectAndDeselect_UpdateIsSelectedProperty()
    {
        // Arrange
        var deal = new TestDeal();
        Assert.False(deal.IsSelected); // Initial state

        // Act: Select
        deal.Select();

        // Assert
        Assert.True(deal.IsSelected);

        // Act: Deselect
        deal.Deselect();

        // Assert
        Assert.False(deal.IsSelected);
    }
}