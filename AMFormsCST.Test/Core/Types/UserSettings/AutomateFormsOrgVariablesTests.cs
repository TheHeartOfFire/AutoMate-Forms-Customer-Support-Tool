using AMFormsCST.Core.Helpers;
using AMFormsCST.Core.Interfaces.BestPractices;
using AMFormsCST.Core.Interfaces.Notebook;
using AMFormsCST.Core.Interfaces.Utils;
using AMFormsCST.Core.Types.Notebook;
using AMFormsCST.Core.Types.UserSettings;
using Moq;
using System.Linq;
using Xunit;

namespace AMFormsCST.Test.Core.Types.UserSettings;
public class AutomateFormsOrgVariablesTests
{
    [Fact]
    public void Constructor_WithNullDependencies_InitializesLooseVariablesAndEmptyVariables()
    {
        // Arrange & Act
        var orgVars = new AutomateFormsOrgVariables(null, null);

        // Assert
        Assert.NotNull(orgVars.LooseVariables);
        Assert.NotNull(orgVars.Variables);
        Assert.Empty(orgVars.Variables);
        Assert.Null(orgVars.Enforcer);
        Assert.Null(orgVars.Notebook);

        // Check default loose variables
        Assert.Contains("AMMailingAddress", orgVars.LooseVariables.Keys);
        Assert.Contains("AMStreetAddress", orgVars.LooseVariables.Keys);
        Assert.Contains("AMCityStateZip", orgVars.LooseVariables.Keys);
    }

    [Fact]
    public void Constructor_WithDependencies_RegistersVariables()
    {
        // Arrange
        var mockEnforcer = new Mock<IBestPracticeEnforcer>();
        var mockNotebook = new Mock<INotebook>();
        var mockNote = new Mock<INote>();
        var mockDealer = new Mock<IDealer>();

        mockDealer.SetupGet(d => d.ServerCode).Returns("SVR1");
        mockDealer.SetupGet(d => d.Companies).Returns(new SelectableList<ICompany>()); // Setup nested collection
        
        var dealerList = new SelectableList<IDealer> { mockDealer.Object };
        dealerList.SelectedItem = mockDealer.Object;

        mockNote.SetupGet(n => n.Dealers).Returns(dealerList);
        mockNote.SetupGet(n => n.Contacts).Returns(new SelectableList<IContact>());
        mockNote.SetupGet(n => n.Forms).Returns(new SelectableList<IForm>());

        var notesList = new SelectableList<INote> { mockNote.Object };
        notesList.SelectedItem = mockNote.Object;
        mockNotebook.SetupGet(n => n.Notes).Returns(notesList);

        // Act
        var orgVars = new AutomateFormsOrgVariables(mockEnforcer.Object, mockNotebook.Object);

        // Assert
        Assert.Same(mockEnforcer.Object, orgVars.Enforcer);
        Assert.Same(mockNotebook.Object, orgVars.Notebook);
        Assert.NotEmpty(orgVars.Variables);

        var serverIdVar = orgVars.Variables.Find(v => v.Name.Equals("serverid", System.StringComparison.OrdinalIgnoreCase));
        Assert.NotNull(serverIdVar);
        Assert.Equal("SVR1", serverIdVar.GetValue());
    }

    [Fact]
    public void InstantiateVariables_UpdatesDependenciesAndVariables()
    {
        // Arrange
        var mockEnforcer1 = new Mock<IBestPracticeEnforcer>();
        var mockNotebook1 = new Mock<INotebook>();
        var mockNote1 = new Mock<INote>();
        var mockDealer1 = new Mock<IDealer>();
        mockDealer1.SetupGet(d => d.ServerCode).Returns("A");
        mockDealer1.SetupGet(d => d.Companies).Returns(new SelectableList<ICompany>());
        var dealerList1 = new SelectableList<IDealer> { mockDealer1.Object };
        dealerList1.SelectedItem = mockDealer1.Object;
        mockNote1.SetupGet(n => n.Dealers).Returns(dealerList1);
        mockNote1.SetupGet(n => n.Contacts).Returns(new SelectableList<IContact>());
        mockNote1.SetupGet(n => n.Forms).Returns(new SelectableList<IForm>());
        var notesList1 = new SelectableList<INote> { mockNote1.Object };
        notesList1.SelectedItem = mockNote1.Object;
        mockNotebook1.SetupGet(n => n.Notes).Returns(notesList1);

        var orgVars = new AutomateFormsOrgVariables(mockEnforcer1.Object, mockNotebook1.Object);

        var mockEnforcer2 = new Mock<IBestPracticeEnforcer>();
        var mockNotebook2 = new Mock<INotebook>();
        var mockNote2 = new Mock<INote>();
        var mockDealer2 = new Mock<IDealer>();
        mockDealer2.SetupGet(d => d.ServerCode).Returns("B");
        mockDealer2.SetupGet(d => d.Companies).Returns(new SelectableList<ICompany>());
        var dealerList2 = new SelectableList<IDealer> { mockDealer2.Object };
        dealerList2.SelectedItem = mockDealer2.Object;
        mockNote2.SetupGet(n => n.Dealers).Returns(dealerList2);
        mockNote2.SetupGet(n => n.Contacts).Returns(new SelectableList<IContact>());
        mockNote2.SetupGet(n => n.Forms).Returns(new SelectableList<IForm>());
        var notesList2 = new SelectableList<INote> { mockNote2.Object };
        notesList2.SelectedItem = mockNote2.Object;
        mockNotebook2.SetupGet(n => n.Notes).Returns(notesList2);

        // Act
        orgVars.InstantiateVariables(mockEnforcer2.Object, mockNotebook2.Object);

        // Assert
        Assert.Same(mockEnforcer2.Object, orgVars.Enforcer);
        Assert.Same(mockNotebook2.Object, orgVars.Notebook);
        var serverIdVar = orgVars.Variables.Find(v => v.Name.Equals("serverid", System.StringComparison.OrdinalIgnoreCase));
        Assert.NotNull(serverIdVar);
        Assert.Equal("B", serverIdVar.GetValue());
    }

    [Fact]
    public void RegisterVariables_FirstNameVariable_ReturnsFirstWordOfContactName()
    {
        // Arrange
        var mockEnforcer = new Mock<IBestPracticeEnforcer>();
        var mockNotebook = new Mock<INotebook>();
        var mockNote = new Mock<INote>();
        var mockContact = new Mock<IContact>();

        mockContact.SetupGet(c => c.Name).Returns("John Doe");
        var contactList = new SelectableList<IContact> { mockContact.Object };
        contactList.SelectedItem = mockContact.Object;
        mockNote.SetupGet(n => n.Dealers).Returns(new SelectableList<IDealer>());
        mockNote.SetupGet(n => n.Contacts).Returns(contactList);
        mockNote.SetupGet(n => n.Forms).Returns(new SelectableList<IForm>());
        var notesList = new SelectableList<INote> { mockNote.Object };
        notesList.SelectedItem = mockNote.Object;
        mockNotebook.SetupGet(n => n.Notes).Returns(notesList);

        var orgVars = new AutomateFormsOrgVariables(mockEnforcer.Object, mockNotebook.Object);

        // Act
        var firstNameVar = orgVars.Variables.Find(v => v.Name.Equals("firstname", System.StringComparison.OrdinalIgnoreCase));
        var value = firstNameVar?.GetValue();

        // Assert
        Assert.Equal("John", value);
    }

    [Fact]
    public void RegisterVariables_FirstNameVariable_ReturnsFullNameIfNoSpace()
    {
        // Arrange
        var mockEnforcer = new Mock<IBestPracticeEnforcer>();
        var mockNotebook = new Mock<INotebook>();
        var mockNote = new Mock<INote>();
        var mockContact = new Mock<IContact>();

        mockContact.SetupGet(c => c.Name).Returns("SingleName");
        var contactList = new SelectableList<IContact> { mockContact.Object };
        contactList.SelectedItem = mockContact.Object;
        mockNote.SetupGet(n => n.Dealers).Returns(new SelectableList<IDealer>());
        mockNote.SetupGet(n => n.Contacts).Returns(contactList);
        mockNote.SetupGet(n => n.Forms).Returns(new SelectableList<IForm>());
        var notesList = new SelectableList<INote> { mockNote.Object };
        notesList.SelectedItem = mockNote.Object;
        mockNotebook.SetupGet(n => n.Notes).Returns(notesList);

        var orgVars = new AutomateFormsOrgVariables(mockEnforcer.Object, mockNotebook.Object);

        // Act
        var firstNameVar = orgVars.Variables.Find(v => v.Name.Equals("firstname", System.StringComparison.OrdinalIgnoreCase));
        var value = firstNameVar?.GetValue();

        // Assert
        Assert.Equal("SingleName", value);
    }

    [Fact]
    public void RegisterVariables_FormNameGeneratorVariable_CallsEnforcer()
    {
        // Arrange
        var mockEnforcer = new Mock<IBestPracticeEnforcer>();
        mockEnforcer.Setup(e => e.GetFormName()).Returns("FormName123");
        var mockNotebook = new Mock<INotebook>();
        var mockNote = new Mock<INote>();
        mockNote.SetupGet(n => n.Dealers).Returns(new SelectableList<IDealer>());
        mockNote.SetupGet(n => n.Contacts).Returns(new SelectableList<IContact>());
        mockNote.SetupGet(n => n.Forms).Returns(new SelectableList<IForm>());
        var notesList = new SelectableList<INote> { mockNote.Object };
        notesList.SelectedItem = mockNote.Object;
        mockNotebook.SetupGet(n => n.Notes).Returns(notesList);

        var orgVars = new AutomateFormsOrgVariables(mockEnforcer.Object, mockNotebook.Object);

        // Act
        var formNameVar = orgVars.Variables.Find(v => v.Name.Equals("formname", System.StringComparison.OrdinalIgnoreCase));
        var value = formNameVar?.GetValue();

        // Assert
        Assert.Equal("FormName123", value);
        mockEnforcer.Verify(e => e.GetFormName(), Times.Once);
    }
}