using AMFormsCST.Desktop.Interfaces;
using AMFormsCST.Desktop.Models;
using Moq;
using System.Linq;
using Xunit;
using CoreForm = AMFormsCST.Core.Types.Notebook.Form;

namespace AMFormsCST.Test.Desktop.Models.Notebook;

public class FormModelTests
{
    [Fact]
    public void Constructor_InitializesWithOneBlankTestDeal_AndSelectsIt()
    {
        // Arrange & Act
        var form = new Form(null);

        // Assert
        Assert.Single(form.TestDeals);
        Assert.True(form.TestDeals[0].IsBlank);

        Assert.NotNull(form.SelectedTestDeal);
        Assert.Same(form.TestDeals[0], form.SelectedTestDeal);
        Assert.Equal(IManagedObservableCollectionItem.CollectionMemberState.Selected, form.SelectedTestDeal.State);
    }

    [Fact]
    public void IsBlank_IsTrue_ForNewModel()
    {
        // Arrange
        var form = new Form(null);

        // Assert
        Assert.True(form.IsBlank);
    }

    [Theory]
    [InlineData("MyForm.frp", "")]
    [InlineData("", "Some form notes")]
    [InlineData("MyForm.frp", "Some form notes")]
    public void IsBlank_IsFalse_WhenPropertiesAreSet(string name, string notes)
    {
        // Arrange
        var form = new Form(null);

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
        var form = new Form(null);
        Assert.True(form.IsBlank); // Pre-condition check

        // Act
        form.TestDeals[0].DealNumber = "Deal123";

        // Assert
        Assert.False(form.IsBlank);
    }

    [Fact]
    public void SelectingAnItem_UpdatesSelectedTestDeal_AndItemState()
    {
        // Arrange
        var form = new Form(null);
        var deal1 = form.TestDeals[0];
        deal1.DealNumber = "Deal 1"; // Makes it non-blank, collection adds a new blank item
        var deal2 = form.TestDeals.Last(d => d.IsBlank);

        // Act
        deal2.Select();

        // Assert
        Assert.Same(deal2, form.SelectedTestDeal);
        Assert.Equal(IManagedObservableCollectionItem.CollectionMemberState.Selected, deal2.State);
        Assert.Equal(IManagedObservableCollectionItem.CollectionMemberState.NotSelected, deal1.State);
    }

    [Fact]
    public void ImplicitConversion_ToCoreForm_MapsPropertiesCorrectly()
    {
        // Arrange
        var formModel = new Form(null)
        {
            Name = "F1",
            Notes = "N1",
            Notable = true
        };
        // The constructor already adds a blank TestDeal. We add a second, non-blank one.
        var testDeal = new TestDeal(null) { DealNumber = "D1", Purpose = "P1" };
        formModel.TestDeals.Add(testDeal);

        // Act
        CoreForm coreForm = formModel;

        // Assert
        Assert.Equal(formModel.Id, coreForm.Id);
        Assert.Equal("F1", coreForm.Name);
        Assert.Equal("N1", coreForm.Notes);
        Assert.True(coreForm.Notable);

        // The test should account for the blank item that is part of the collection.
        Assert.Equal(2, coreForm.TestDeals.Count);

        // Find the non-blank item to verify its properties
        var coreTestDeal = coreForm.TestDeals.FirstOrDefault(td => td.DealNumber == "D1");
        Assert.NotNull(coreTestDeal);
        Assert.Equal("P1", coreTestDeal.Purpose);
    }

    [Fact]
    public void UpdateCore_UpdatesCoreTypeAndNotifiesParent()
    {
        // Arrange
        var noteModel = new NoteModel("x", null);
        var form = new Form(null) { Parent = noteModel };
        var coreForm = new CoreForm();
        form.CoreType = coreForm;

        // Fully establish the parent-child relationship
        noteModel.Forms.Add(form);

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
        form.Name = "New Form Name"; // This change should trigger the notification chain.

        // Assert
        // 1. Verify the underlying CoreType was updated.
        Assert.Equal("New Form Name", coreForm.Name);

        // 2. Verify the notification bubbled up to the NoteModel.
        Assert.True(wasNotified, "The NoteModel's PropertyChanged event was not raised as expected.");
    }
}