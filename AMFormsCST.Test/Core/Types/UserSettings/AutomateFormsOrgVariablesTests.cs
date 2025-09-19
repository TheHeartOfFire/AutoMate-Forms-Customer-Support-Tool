using AMFormsCST.Core.Helpers;
using AMFormsCST.Core.Interfaces;
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
    public void Constructor_WithNullSupportToolFactory_InitializesLooseVariablesAndEmptyVariables()
    {
        // Arrange & Act
        var orgVars = new AutomateFormsOrgVariables(() => null);

        // Assert
        Assert.NotNull(orgVars.LooseVariables);
        Assert.NotNull(orgVars.Variables);
        Assert.Empty(orgVars.Variables);

        // Check default loose variables
        Assert.Contains("AMMailingAddress", orgVars.LooseVariables.Keys);
        Assert.Contains("AMStreetAddress", orgVars.LooseVariables.Keys);
        Assert.Contains("AMCityStateZip", orgVars.LooseVariables.Keys);
    }

    [Fact]
    public void Constructor_WithDependencies_RegistersVariables()
    {
        // Arrange
        var mockSupportTool = new Mock<ISupportTool>();
        var mockNotebook = new Mock<INotebook>();
        var mockNote = new Mock<INote>();
        var mockDealer = new Mock<IDealer>();

        mockDealer.SetupGet(d => d.ServerCode).Returns("SVR1");
        mockDealer.SetupGet(d => d.Companies).Returns(new SelectableList<ICompany>());

        var dealerList = new SelectableList<IDealer> { mockDealer.Object };
        dealerList.SelectedItem = mockDealer.Object;

        mockNote.SetupGet(n => n.Dealers).Returns(dealerList);
        mockNote.SetupGet(n => n.Contacts).Returns(new SelectableList<IContact>());
        mockNote.SetupGet(n => n.Forms).Returns(new SelectableList<IForm>());

        var notesList = new SelectableList<INote> { mockNote.Object };
        notesList.SelectedItem = mockNote.Object;
        mockNotebook.SetupGet(n => n.Notes).Returns(notesList);
        mockSupportTool.SetupGet(st => st.Notebook).Returns(mockNotebook.Object);

        // Act
        var orgVars = new AutomateFormsOrgVariables(() => mockSupportTool.Object);

        // Assert
        Assert.NotEmpty(orgVars.Variables);

        var serverIdVar = orgVars.Variables.Find(v => v.Name.Equals("serverid", System.StringComparison.OrdinalIgnoreCase));
        Assert.NotNull(serverIdVar);
        Assert.Equal("SVR1", serverIdVar.GetValue());
    }

    [Fact]
    public void InstantiateVariables_UpdatesDependenciesAndVariables()
    {
        // Arrange
        var mockSupportTool1 = new Mock<ISupportTool>();
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
        mockSupportTool1.SetupGet(st => st.Notebook).Returns(mockNotebook1.Object);

        var orgVars = new AutomateFormsOrgVariables(() => mockSupportTool1.Object);
        // Access variables to initialize Lazy
        var initialValue = orgVars.Variables.Find(v => v.Name.Equals("serverid", System.StringComparison.OrdinalIgnoreCase))?.GetValue();
        Assert.Equal("A", initialValue);

        var mockSupportTool2 = new Mock<ISupportTool>();
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
        mockSupportTool2.SetupGet(st => st.Notebook).Returns(mockNotebook2.Object);

        // Act
        orgVars.InstantiateVariables(mockSupportTool2.Object);

        // Assert
        var serverIdVar = orgVars.Variables.Find(v => v.Name.Equals("serverid", System.StringComparison.OrdinalIgnoreCase));
        Assert.NotNull(serverIdVar);
        Assert.Equal("B", serverIdVar.GetValue());
    }

    [Fact]
    public void RegisterVariables_FirstNameVariable_ReturnsFirstWordOfContactName()
    {
        // Arrange
        var mockSupportTool = new Mock<ISupportTool>();
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
        mockSupportTool.SetupGet(st => st.Notebook).Returns(mockNotebook.Object);

        var orgVars = new AutomateFormsOrgVariables(() => mockSupportTool.Object);

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
        var mockSupportTool = new Mock<ISupportTool>();
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
        mockSupportTool.SetupGet(st => st.Notebook).Returns(mockNotebook.Object);

        var orgVars = new AutomateFormsOrgVariables(() => mockSupportTool.Object);

        // Act
        var firstNameVar = orgVars.Variables.Find(v => v.Name.Equals("firstname", System.StringComparison.OrdinalIgnoreCase));
        var value = firstNameVar?.GetValue();

        // Assert
        Assert.Equal("SingleName", value);
    }
}