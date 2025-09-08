using AMFormsCST.Desktop.Models;
using Xunit;
using CoreTestDeal = AMFormsCST.Core.Types.Notebook.TestDeal;

namespace AMFormsCST.Test.Desktop.Models.Notebook;

public class TestDealModelTests
{
    [Fact]
    public void IsBlank_IsTrue_ForNewModel()
    {
        // Arrange
        var deal = new TestDeal(null);

        // Assert
        Assert.True(deal.IsBlank);
    }

    [Theory]
    [InlineData("Deal123", "")]
    [InlineData("", "Test Purpose")]
    [InlineData("Deal123", "Test Purpose")]
    public void IsBlank_IsFalse_WhenPropertiesAreSet(string dealNumber, string purpose)
    {
        // Arrange
        var deal = new TestDeal(null);

        // Act
        deal.DealNumber = dealNumber;
        deal.Purpose = purpose;

        // Assert
        Assert.False(deal.IsBlank);
    }

    [Fact]
    public void Constructor_FromCoreType_InitializesProperties()
    {
        // Arrange
        var coreDeal = new CoreTestDeal { DealNumber = "D1", Purpose = "P1" };

        // Act
        var deal = new TestDeal(coreDeal, null);

        // Assert
        Assert.Equal("D1", deal.DealNumber);
        Assert.Equal("P1", deal.Purpose);
        Assert.Same(coreDeal, deal.CoreType);
    }

    [Fact]
    public void ImplicitConversion_ToCoreTestDeal_MapsPropertiesCorrectly()
    {
        // Arrange
        var dealModel = new TestDeal(null) { DealNumber = "D1", Purpose = "P1" };

        // Act
        CoreTestDeal coreDeal = dealModel;

        // Assert
        Assert.Equal(dealModel.Id, coreDeal.Id);
        Assert.Equal("D1", coreDeal.DealNumber);
        Assert.Equal("P1", coreDeal.Purpose);
    }

    [Fact]
    public void UpdateCore_UpdatesCoreTypeAndNotifiesGrandparent()
    {
        // Arrange
        var noteModel = new NoteModel("x", null);
        var form = new Form(null) { Parent = noteModel };
        var testDeal = new TestDeal(null) { Parent = form };
        var coreTestDeal = new CoreTestDeal();
        testDeal.CoreType = coreTestDeal;

        // Fully establish the parent-child relationships
        noteModel.Forms.Add(form);
        form.TestDeals.Add(testDeal);

        bool wasNotified = false;
        noteModel.PropertyChanged += (sender, args) =>
        {
            // The notification bubbles up as a generic "all properties changed" signal.
            if (string.IsNullOrEmpty(args.PropertyName))
            {
                wasNotified = true;
            }
        };

        // Act
        testDeal.DealNumber = "New Deal Number"; // This change should trigger the notification chain.

        // Assert
        // 1. Verify the underlying CoreType was updated.
        Assert.Equal("New Deal Number", coreTestDeal.DealNumber);

        // 2. Verify the notification bubbled up to the NoteModel.
        Assert.True(wasNotified, "The NoteModel's PropertyChanged event was not raised as expected.");
    }
}