using AMFormsCST.Desktop.Models;

namespace AMFormsCST.Test.ModelTests;

public class FormModelTests
{
    [Fact]
    public void Constructor_InitializesWithOneBlankTestDeal_AndSelectsIt()
    {
        // Arrange & Act
        var form = new Form();

        // Assert
        // Verify the TestDeals collection is created and contains a single, blank item.
        Assert.Single(form.TestDeals);
        Assert.True(form.TestDeals[0].IsBlank);

        // Verify that the initially created blank deal is set as the selected deal.
        Assert.NotNull(form.SelectedTestDeal);
        Assert.Same(form.TestDeals[0], form.SelectedTestDeal);
        Assert.True(form.SelectedTestDeal.IsSelected);
    }

    [Fact]
    public void IsBlank_IsTrue_ForNewModel()
    {
        // Arrange
        var form = new Form();

        // Assert
        Assert.True(form.IsBlank);
    }

    [Theory]
    [InlineData("MyForm.frp", null)]
    [InlineData(null, "Some form notes")]
    public void IsBlank_IsFalse_WhenPropertiesAreSet(string? name, string? notes)
    {
        // Arrange
        var form = new Form();

        // Act
        form.Name = name;
        form.Notes = notes;

        // Assert
        Assert.False(form.IsBlank);
    }

    [Fact]
    public void IsBlank_IsFalse_WhenChildTestDealBecomesNonBlank()
    {
        // Arrange
        var form = new Form();
        Assert.True(form.IsBlank); // Pre-condition check

        // Act
        // Make the child TestDeal non-blank.
        form.TestDeals[0].DealNumber = "Deal123";

        // Assert
        // The parent form should now report that it is not blank.
        Assert.False(form.IsBlank);
    }

    [Fact]
    public void SelectTestDeal_UpdatesSelectedTestDeal_AndSelectionState()
    {
        // Arrange
        var form = new Form();
        var deal1 = form.TestDeals[0];
        deal1.DealNumber = "Deal 1"; // Making this non-blank will trigger the parent NoteModel (in a real scenario) to add a new blank one. For this test, we add it manually.
        var deal2 = new TestDeal();
        form.TestDeals.Add(deal2);

        // Act
        form.SelectTestDeal(deal2);

        // Assert
        Assert.Same(deal2, form.SelectedTestDeal);
        Assert.True(deal2.IsSelected);
        Assert.False(deal1.IsSelected);
    }

    [Fact]
    public void SelectAndDeselect_UpdateIsSelectedProperty()
    {
        // Arrange
        var form = new Form();
        Assert.False(form.IsSelected); // Initial state

        // Act: Select
        form.Select();

        // Assert
        Assert.True(form.IsSelected);

        // Act: Deselect
        form.Deselect();

        // Assert
        Assert.False(form.IsSelected);
    }
}