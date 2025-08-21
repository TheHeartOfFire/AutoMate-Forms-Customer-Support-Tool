using AMFormsCST.Core.Interfaces;
using AMFormsCST.Core.Interfaces.Notebook;
using AMFormsCST.Core.Interfaces.UserSettings;
using AMFormsCST.Core.Interfaces.Utils;
using AMFormsCST.Desktop.Models;
using AMFormsCST.Desktop.Services;
using AMFormsCST.Desktop.ViewModels.Pages;
using Moq;
using Assert = Xunit.Assert;

namespace AMFormsCST.Test.Desktop.ViewModels.Pages;

public class DashboardViewModelTests
{
    private readonly Mock<ISupportTool> _mockSupportTool;
    private readonly Mock<ISettings> _mockSettings;
    private readonly Mock<IUserSettings> _mockUserSettings;
    private readonly Mock<INotebook> _mockNotebook;
    private readonly Mock<IDialogService> _mockDialogService;
    private readonly Mock<IFileSystem> _mockFileSystem;

    public DashboardViewModelTests()
    {
        // 1. Setup the mock objects for the view model's dependencies.
        _mockSupportTool = new Mock<ISupportTool>();
        _mockSettings = new Mock<ISettings>();
        _mockUserSettings = new Mock<IUserSettings>();
        _mockNotebook = new Mock<INotebook>();
        _mockDialogService = new Mock<IDialogService>();
        _mockFileSystem = new Mock<IFileSystem>();

        // 2. Configure the mocks to return the necessary objects.
        // This setup mimics the real dependency graph.
        _mockUserSettings.Setup(us => us.ExtSeparator).Returns("x");
        _mockSettings.Setup(s => s.UserSettings).Returns(_mockUserSettings.Object);
        _mockSupportTool.Setup(st => st.Settings).Returns(_mockSettings.Object);
        _mockSupportTool.Setup(st => st.Notebook).Returns(_mockNotebook.Object);

        // The UpdateNotebook method requires a non-null list to clear.
        _mockNotebook.Setup(n => n.Notes).Returns([]);
    }

    [Fact]
    public void Constructor_InitializesWithOneBlankNote_AndSelectsIt()
    {
        // Arrange & Act: Create a new instance of the view model.
        var viewModel = new DashboardViewModel(_mockSupportTool.Object, _mockDialogService.Object, _mockFileSystem.Object);

        // Assert: Verify the initial state is correct.
        // It should start with a single, blank note.
        Assert.Single(viewModel.Notes);
        Assert.True(viewModel.Notes[0].IsBlank);

        // The first note should be automatically selected.
        Assert.NotNull(viewModel.SelectedNote);
        Assert.Same(viewModel.Notes[0], viewModel.SelectedNote);
        Assert.True(viewModel.SelectedNote.IsSelected);
    }

    [Fact]
    public void OnNoteClicked_ChangesSelectedNote_AndUpdatesSelectionState()
    {
        // Arrange
        var viewModel = new DashboardViewModel(_mockSupportTool.Object, _mockDialogService.Object, _mockFileSystem.Object);
        
        // Simulate user input on the first note, which will make it non-blank.
        var firstNote = viewModel.Notes[0];
        firstNote.CaseNumber = "12345";

        // The view model's logic automatically adds a new blank note when the previous one becomes non-blank.
        // We now have two notes to test the selection logic with.
        Assert.Equal(2, viewModel.Notes.Count);
        var secondNote = viewModel.Notes[1];
        
        // Pre-condition check: ensure the first note is still selected after adding a new one.
        Assert.Same(firstNote, viewModel.SelectedNote);

        // Act: Simulate the user clicking on the second note.
        viewModel.NoteClickedCommand.Execute(secondNote.Id);

        // Assert
        // The SelectedNote should now be the second note.
        Assert.Same(secondNote, viewModel.SelectedNote);
        
        // Verify the selection flags are correct on both notes.
        Assert.True(secondNote.IsSelected);
        Assert.False(firstNote.IsSelected);
    }

    [Fact]
    public void OnDeleteItemClicked_RemovesNote_AndSelectsNextAvailable()
    {
        // Arrange
        var viewModel = new DashboardViewModel(_mockSupportTool.Object, _mockDialogService.Object, _mockFileSystem.Object);
        var note1 = viewModel.Notes[0];
        note1.CaseNumber = "Note 1"; // Make it non-blank
        var note2 = viewModel.Notes[1];
        note2.CaseNumber = "Note 2"; // Make it non-blank
        var note3 = viewModel.Notes[2]; // This is the current blank note

        viewModel.NoteClickedCommand.Execute(note2.Id); // Select the middle note
        Assert.Same(note2, viewModel.SelectedNote);
        Assert.Equal(3, viewModel.Notes.Count);

        // Act: Simulate deleting the currently selected note (note2).
        viewModel.DeleteItemClickedCommand.Execute(note2);

        // Assert
        Assert.Equal(2, viewModel.Notes.Count);
        Assert.DoesNotContain(note2, viewModel.Notes);

        // The view model should automatically select the first note in the list as the new SelectedNote.
        Assert.Same(note1, viewModel.SelectedNote);
        Assert.True(note1.IsSelected);
    }

    [Fact]
    public void OnDealerClicked_ChangesSelectedDealer()
    {
        // Arrange
        var viewModel = new DashboardViewModel(_mockSupportTool.Object, _mockDialogService.Object, _mockFileSystem.Object);
        var dealer1 = viewModel.SelectedNote.Dealers[0];
        dealer1.Name = "Dealer 1"; // Make it non-blank, which adds a new blank dealer.
        var dealer2 = viewModel.SelectedNote.Dealers[1];

        // Act
        viewModel.DealerClickedCommand.Execute(dealer2);

        // Assert
        Assert.Same(dealer2, viewModel.SelectedNote.SelectedDealer);
        Assert.True(dealer2.IsSelected);
        Assert.False(dealer1.IsSelected);
    }

    [Fact]
    public void OnDeleteItemClicked_RemovesDealer_AndSelectsNextAvailable()
    {
        // Arrange
        var viewModel = new DashboardViewModel(_mockSupportTool.Object, _mockDialogService.Object, _mockFileSystem.Object);
        var dealer1 = viewModel.SelectedNote.Dealers[0];
        dealer1.Name = "Dealer 1";
        var dealer2 = viewModel.SelectedNote.Dealers[1];
        dealer2.Name = "Dealer 2";
        
        viewModel.DealerClickedCommand.Execute(dealer1); // Select the first dealer
        Assert.Equal(3, viewModel.SelectedNote.Dealers.Count); // dealer1, dealer2, blank

        // Act
        viewModel.DeleteItemClickedCommand.Execute(dealer1);

        // Assert
        Assert.Equal(2, viewModel.SelectedNote.Dealers.Count);
        Assert.DoesNotContain(dealer1, viewModel.SelectedNote.Dealers);
        Assert.Same(dealer2, viewModel.SelectedNote.SelectedDealer); // Should select the next one
        Assert.True(dealer2.IsSelected);
    }

    [Fact]
    public void ChangingNoteProperty_TriggersUpdateNotebook()
    {
        // Arrange
        var viewModel = new DashboardViewModel(_mockSupportTool.Object, _mockDialogService.Object, _mockFileSystem.Object);
        
        // Act: Change a property that is configured to trigger the update.
        viewModel.SelectedNote.CaseNumber = "54321";

        // Assert: Verify that the UpdateNotebook method was called.
        // It's called once at startup and once for the property change.
        _mockNotebook.Verify(n => n.AddNote(It.IsAny<INote>(), It.IsAny<bool>()), Times.Exactly(2));
        _mockNotebook.Verify(n => n.SelectNote(It.IsAny<INote>()), Times.Exactly(2));
    }

    [Fact]
    public void OnContactClicked_ChangesSelectedContact()
    {
        // Arrange
        var viewModel = new DashboardViewModel(_mockSupportTool.Object, _mockDialogService.Object, _mockFileSystem.Object);
        var contact1 = viewModel.SelectedNote.Contacts[0];
        contact1.Name = "Contact 1";
        var contact2 = viewModel.SelectedNote.Contacts[1];

        // Act
        viewModel.ContactClickedCommand.Execute(contact2);

        // Assert
        Assert.Same(contact2, viewModel.SelectedNote.SelectedContact);
        Assert.True(contact2.IsSelected);
        Assert.False(contact1.IsSelected);
    }

    [Fact]
    public void OnFormClicked_ChangesSelectedForm()
    {
        // Arrange
        var viewModel = new DashboardViewModel(_mockSupportTool.Object, _mockDialogService.Object, _mockFileSystem.Object);
        var form1 = viewModel.SelectedNote.Forms[0];
        form1.Name = "Form 1";
        var form2 = viewModel.SelectedNote.Forms[1];

        // Act
        viewModel.FormClickedCommand.Execute(form2);

        // Assert
        Assert.Same(form2, viewModel.SelectedNote.SelectedForm);
        Assert.True(form2.IsSelected);
        Assert.False(form1.IsSelected);
    }

    [Fact]
    public void OnDeleteItemClicked_RemovesContact_AndSelectsNextAvailable()
    {
        // Arrange
        var viewModel = new DashboardViewModel(_mockSupportTool.Object, _mockDialogService.Object, _mockFileSystem.Object);
        var contact1 = viewModel.SelectedNote.Contacts[0];
        contact1.Name = "Contact 1";
        var contact2 = viewModel.SelectedNote.Contacts[1];
        contact2.Name = "Contact 2";
        
        viewModel.ContactClickedCommand.Execute(contact1);
        Assert.Equal(3, viewModel.SelectedNote.Contacts.Count);

        // Act
        viewModel.DeleteItemClickedCommand.Execute(contact1);

        // Assert
        Assert.Equal(2, viewModel.SelectedNote.Contacts.Count);
        Assert.DoesNotContain(contact1, viewModel.SelectedNote.Contacts);
        Assert.Same(contact2, viewModel.SelectedNote.SelectedContact);
        Assert.True(contact2.IsSelected);
    }

    [Fact]
    public void TypingInBlankItem_CreatesNewBlankItem()
    {
        // Arrange
        var viewModel = new DashboardViewModel(_mockSupportTool.Object, _mockDialogService.Object, _mockFileSystem.Object);
        Assert.Single(viewModel.Notes); // Starts with one blank note

        // Act
        viewModel.SelectedNote.CaseNumber = "New Case"; // Simulate user typing

        // Assert
        Assert.Equal(2, viewModel.Notes.Count); // A new blank note should be added
        Assert.False(viewModel.Notes[0].IsBlank); // The first note is no longer blank
        Assert.True(viewModel.Notes[1].IsBlank); // The new note is blank
    }

    [Fact]
    public void OnCompanyClicked_ChangesSelectedCompany()
    {
        // Arrange
        var viewModel = new DashboardViewModel(_mockSupportTool.Object, _mockDialogService.Object, _mockFileSystem.Object);
        var dealer = viewModel.SelectedNote.Dealers[0];
        dealer.Name = "Test Dealer"; // Make dealer non-blank
        var company1 = dealer.Companies[0];
        company1.Name = "Company 1"; // Make company non-blank
        var company2 = dealer.Companies[1];

        // Act
        viewModel.CompanyClickedCommand.Execute(company2);

        // Assert
        Assert.Same(company2, viewModel.SelectedNote.SelectedDealer?.SelectedCompany);
        Assert.True(company2.IsSelected);
        Assert.False(company1.IsSelected);
    }

    [Fact]
    public void OnDeleteItemClicked_RemovesCompany_AndSelectsNextAvailable()
    {
        // Arrange
        var viewModel = new DashboardViewModel(_mockSupportTool.Object, _mockDialogService.Object, _mockFileSystem.Object);
        var dealer = viewModel.SelectedNote.Dealers[0];
        dealer.Name = "Test Dealer";

        var company1 = dealer.Companies[0];
        company1.Name = "Company 1";
        var company2 = dealer.Companies[1];
        company2.Name = "Company 2";
        
        viewModel.CompanyClickedCommand.Execute(company1);
        Assert.Equal(3, dealer.Companies.Count);

        // Act
        viewModel.DeleteItemClickedCommand.Execute(company1);

        // Assert
        Assert.Equal(2, dealer.Companies.Count);
        Assert.DoesNotContain(company1, dealer.Companies);
        Assert.Same(company2, dealer.SelectedCompany);
        Assert.True(company2.IsSelected);
    }

    [Fact]
    public void OnDeleteItemClicked_RemovesForm_AndSelectsNextAvailable()
    {
        // Arrange
        var viewModel = new DashboardViewModel(_mockSupportTool.Object, _mockDialogService.Object, _mockFileSystem.Object);
        var form1 = viewModel.SelectedNote.Forms[0];
        form1.Name = "Form 1";
        var form2 = viewModel.SelectedNote.Forms[1];
        form2.Name = "Form 2";
        
        viewModel.FormClickedCommand.Execute(form1);
        Assert.Equal(3, viewModel.SelectedNote.Forms.Count);

        // Act
        viewModel.DeleteItemClickedCommand.Execute(form1);

        // Assert
        Assert.Equal(2, viewModel.SelectedNote.Forms.Count);
        Assert.DoesNotContain(form1, viewModel.SelectedNote.Forms);
        Assert.Same(form2, viewModel.SelectedNote.SelectedForm);
        Assert.True(form2.IsSelected);
    }

    [Fact]
    public void OnDealClicked_ChangesSelectedTestDeal()
    {
        // Arrange
        var viewModel = new DashboardViewModel(_mockSupportTool.Object, _mockDialogService.Object, _mockFileSystem.Object);
        var form = viewModel.SelectedNote.Forms[0];
        form.Name = "Test Form";
        var deal1 = form.TestDeals[0];
        deal1.DealNumber = "Deal 1";
        var deal2 = form.TestDeals[1];

        // Act
        viewModel.DealClickedCommand.Execute(deal2);

        // Assert
        Assert.Same(deal2, form.SelectedTestDeal);
        Assert.True(deal2.IsSelected);
        Assert.False(deal1.IsSelected);
    }

    [Fact]
    public void OnDeleteItemClicked_WhenDeletingLastNonBlankNote_SelectsBlankNote()
    {
        // Arrange
        var viewModel = new DashboardViewModel(_mockSupportTool.Object, _mockDialogService.Object, _mockFileSystem.Object);
        var note1 = viewModel.Notes[0];
        note1.CaseNumber = "The only note";
        var blankNote = viewModel.Notes[1];

        viewModel.NoteClickedCommand.Execute(note1.Id);
        Assert.Same(note1, viewModel.SelectedNote);

        // Act
        viewModel.DeleteItemClickedCommand.Execute(note1);

        // Assert
        Assert.Single(viewModel.Notes);
        Assert.Same(blankNote, viewModel.SelectedNote);
        Assert.True(blankNote.IsSelected);
    }
}