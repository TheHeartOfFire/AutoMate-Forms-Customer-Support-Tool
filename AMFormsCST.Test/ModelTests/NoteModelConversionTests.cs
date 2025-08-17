using AMFormsCST.Desktop.Models;
using CoreNote = AMFormsCST.Core.Types.Notebook.Note;
using Assert = Xunit.Assert;

namespace AMFormsCST.Test.ModelTests;

public class NoteModelConversionTests
{
    private const string TestExtSeparator = "x";

    [Fact]
    public void ImplicitConversion_CorrectlyMapsAllProperties()
    {
        // Arrange
        var noteModel = new NoteModel(TestExtSeparator)
        {
            CaseNumber = "CS12345",
            Notes = "This is a test note.",
        };

        // Populate the selected dealer and its company
        var dealer = noteModel.SelectedDealer!;
        dealer.ServerCode = "SVR1";
        dealer.Name = "Test Dealership";
        dealer.Companies[0].CompanyCode = "C1";
        dealer.Companies[0].Name = "Company One"; // Make it non-blank to add a new one
        dealer.Companies.Add(new Company { CompanyCode = "C2", Name = "Company Two" });

        // Populate the selected contact
        var contact = noteModel.SelectedContact!;
        contact.Name = "John Doe";
        contact.Email = "john.doe@example.com";
        contact.Phone = "555-1234";
        contact.PhoneExtension = "101";

        // Populate the selected form and its deal
        var form = noteModel.SelectedForm!;
        form.Name = "MyForm.frp";
        form.SelectedTestDeal!.DealNumber = "DEAL99";

        // Act
        // Perform the implicit conversion
        CoreNote coreNote = noteModel;

        // Assert
        Assert.NotNull(coreNote);
        Assert.Equal(noteModel.Id, coreNote.Id);
        Assert.Equal("CS12345", coreNote.CaseText);
        Assert.Equal("This is a test note.", coreNote.NotesText);
        Assert.Equal("SVR1", coreNote.ServerId);
        Assert.Equal("Test Dealership", coreNote.Dealership);
        Assert.Equal("C1, C2", coreNote.Companies); // Note the expected comma separation
        Assert.Equal("John Doe", coreNote.ContactName);
        Assert.Equal("john.doe@example.com", coreNote.Email);
        Assert.Equal("555-1234", coreNote.Phone);
        Assert.Equal("101", coreNote.PhoneExt);
        Assert.Equal("MyForm.frp", coreNote.FormsText);
        Assert.Equal("DEAL99", coreNote.DealText);
    }

    [Fact]
    public void ImplicitConversion_WithNullNoteModel_ReturnsNewCoreNote()
    {
        // Arrange
        NoteModel? noteModel = null;

        // Act
        CoreNote coreNote = noteModel;

        // Assert
        Assert.NotNull(coreNote);
        // Verify it's a new object with default (null/empty) values
        Assert.NotEqual(System.Guid.Empty, coreNote.Id);
        Assert.Null(coreNote.CaseText);
    }

    [Fact]
    public void ImplicitConversion_WithBlankNoteModel_MapsEmptyAndNullValues()
    {
        // Arrange
        var noteModel = new NoteModel(TestExtSeparator); // A completely blank note

        // Act
        CoreNote coreNote = noteModel;

        // Assert
        Assert.NotNull(coreNote);
        Assert.Equal(string.Empty, coreNote.CaseText);
        Assert.Null(coreNote.NotesText);
        Assert.Equal(string.Empty, coreNote.ServerId);
        Assert.Equal(string.Empty, coreNote.Dealership);
        Assert.Equal(string.Empty, coreNote.Companies); // Should be empty string, not null
        Assert.Equal(string.Empty, coreNote.ContactName);
        Assert.Equal(string.Empty, coreNote.Email);
        Assert.Equal(string.Empty, coreNote.Phone);
        Assert.Equal(string.Empty, coreNote.PhoneExt);
        Assert.Equal(string.Empty, coreNote.FormsText);
        Assert.Equal(string.Empty, coreNote.DealText);
    }
}