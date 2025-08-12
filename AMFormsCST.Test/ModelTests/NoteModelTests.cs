using AMFormsCST.Desktop.Models;

namespace AMFormsCST.Test.ModelTests;

public class NoteModelTests
{
    private const string TestExtSeparator = "x";

    [Fact]
    public void Constructor_InitializesCollections_WithOneBlankItemEach()
    {
        // Arrange & Act
        var note = new NoteModel(TestExtSeparator);

        // Assert
        // Verify each collection is created and contains a single, blank item.
        Assert.Single(note.Dealers);
        Assert.True(note.Dealers[0].IsBlank);

        Assert.Single(note.Contacts);
        Assert.True(note.Contacts[0].IsBlank);

        Assert.Single(note.Forms);
        Assert.True(note.Forms[0].IsBlank);
    }

    [Fact]
    public void Constructor_SelectsDefaultBlankItems()
    {
        // Arrange & Act
        var note = new NoteModel(TestExtSeparator);

        // Assert
        // Verify that the initially created blank items are set as the selected items.
        Assert.NotNull(note.SelectedDealer);
        Assert.Same(note.Dealers[0], note.SelectedDealer);
        Assert.True(note.SelectedDealer.IsSelected);

        Assert.NotNull(note.SelectedContact);
        Assert.Same(note.Contacts[0], note.SelectedContact);
        Assert.True(note.SelectedContact.IsSelected);

        Assert.NotNull(note.SelectedForm);
        Assert.Same(note.Forms[0], note.SelectedForm);
        Assert.True(note.SelectedForm.IsSelected);
    }

    [Fact]
    public void IsBlank_IsTrue_ForNewModel()
    {
        // Arrange
        var note = new NoteModel(TestExtSeparator);

        // Assert
        Assert.True(note.IsBlank);
    }

    [Theory]
    [InlineData("12345", null)]
    [InlineData(null, "Some notes")]
    public void IsBlank_IsFalse_WhenTopLevelPropertiesAreSet(string? caseNumber, string? notes)
    {
        // Arrange
        var note = new NoteModel(TestExtSeparator);

        // Act
        note.CaseNumber = caseNumber;
        note.Notes = notes;

        // Assert
        Assert.False(note.IsBlank);
    }

    [Fact]
    public void IsBlank_IsFalse_WhenChildItemBecomesNonBlank()
    {
        // Arrange
        var note = new NoteModel(TestExtSeparator);
        Assert.True(note.IsBlank); // Pre-condition check

        // Act
        // Make a child item (a dealer) non-blank.
        note.Dealers[0].Name = "Test Dealer";

        // Assert
        // The parent note should now report that it is not blank.
        Assert.False(note.IsBlank);
    }

    [Fact]
    public void SelectDealer_UpdatesSelectedDealer_AndSelectionState()
    {
        // Arrange
        var note = new NoteModel(TestExtSeparator);
        var dealer1 = note.Dealers[0];
        dealer1.Name = "Dealer 1";
        note.Dealers.Add(new()); // Extra Dealer handled by containing VM when NoteModel's IsBlank state changes due to Dealer IsBlank state changing.
        var dealer2 = note.Dealers[1];

        // Act
        note.SelectDealer(dealer2);

        // Assert
        Assert.Same(dealer2, note.SelectedDealer);
        Assert.True(dealer2.IsSelected);
        Assert.False(dealer1.IsSelected);
    }

    [Fact]
    public void ImplicitConversion_ToCoreNote_MapsPropertiesCorrectly()
    {
        // Arrange
        var noteModel = new NoteModel(TestExtSeparator);
        noteModel.CaseNumber = "98765";
        noteModel.Notes = "Test case notes.";

        noteModel.SelectedDealer!.ServerCode = "SVR1";
        noteModel.SelectedDealer.Name = "Test Dealer";
        noteModel.SelectedDealer.Companies[0].CompanyCode = "C1";
        noteModel.SelectedDealer.Companies[0].Name = "Company 1"; // Make non-blank to add another
        noteModel.SelectedDealer.Companies.Add(new Company { CompanyCode = "C2" });

        noteModel.SelectedContact!.Name = "John Doe";
        noteModel.SelectedContact.Email = "john.doe@email.com";
        noteModel.SelectedContact.Phone = "555-1234";
        noteModel.SelectedContact.PhoneExtension = "101";

        noteModel.SelectedForm!.Name = "MyForm.frp";
        noteModel.SelectedForm.SelectedTestDeal!.DealNumber = "Deal123";

        // Act
        Core.Types.Notebook.Note coreNote = noteModel;

        // Assert
        Assert.Equal(noteModel.Id, coreNote.Id);
        Assert.Equal("98765", coreNote.CaseText);
        Assert.Equal("Test case notes.", coreNote.NotesText);
        Assert.Equal("SVR1", coreNote.ServerId);
        Assert.Equal("Test Dealer", coreNote.Dealership);
        Assert.Equal("C1, C2", coreNote.Companies); // Note the implicit string.Join
        Assert.Equal("John Doe", coreNote.ContactName);
        Assert.Equal("john.doe@email.com", coreNote.Email);
        Assert.Equal("555-1234", coreNote.Phone);
        Assert.Equal("101", coreNote.PhoneExt);
        Assert.Equal("MyForm.frp", coreNote.FormsText);
        Assert.Equal("Deal123", coreNote.DealText);
    }
}