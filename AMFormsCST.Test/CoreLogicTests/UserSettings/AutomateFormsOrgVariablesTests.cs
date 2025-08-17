using AMFormsCST.Core.Interfaces.BestPractices;
using AMFormsCST.Core.Interfaces.Notebook;
using AMFormsCST.Core.Interfaces.Utils;
using AMFormsCST.Core.Types.BestPractices.TextTemplates.Models;
using AMFormsCST.Core.Types.Notebook;
using AMFormsCST.Core.Types.UserSettings;
using Moq;
using System.Collections.Generic;
using Xunit;

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
        Assert.True(orgVars.LooseVariables.ContainsKey("AMMailingAddress"));
        Assert.True(orgVars.LooseVariables.ContainsKey("AMStreetAddress"));
        Assert.True(orgVars.LooseVariables.ContainsKey("AMCityStateZip"));
        Assert.True(orgVars.LooseVariables.ContainsKey("AMCity"));
        Assert.True(orgVars.LooseVariables.ContainsKey("AMState"));
        Assert.True(orgVars.LooseVariables.ContainsKey("AMZip"));
    }

    [Fact]
    public void Constructor_WithDependencies_RegistersVariables()
    {
        // Arrange
        var mockEnforcer = new Mock<IBestPracticeEnforcer>();
        var mockNotebook = new Mock<INotebook>();
        var mockNote = new Note();
        mockNotebook.Setup(n => n.CurrentNote).Returns(mockNote);

        // Act
        var orgVars = new AutomateFormsOrgVariables(mockEnforcer.Object, mockNotebook.Object);

        // Assert
        Assert.Same(mockEnforcer.Object, orgVars.Enforcer);
        Assert.Same(mockNotebook.Object, orgVars.Notebook);
        Assert.NotNull(orgVars.Variables);
        Assert.NotEmpty(orgVars.Variables);

        // Check that at least one variable returns a string (e.g., Notes:ServerID)
        var serverIdVar = orgVars.Variables.Find(v => v.Name == "serverid");
        Assert.NotNull(serverIdVar);
        Assert.Equal(string.Empty, serverIdVar.GetValue());
    }

    [Fact]
    public void InstantiateVariables_UpdatesDependenciesAndVariables()
    {
        // Arrange
        var mockEnforcer1 = new Mock<IBestPracticeEnforcer>();
        var mockNotebook1 = new Mock<INotebook>();
        var mockNote1 = new Note { ServerId = "A" };
        mockNotebook1.Setup(n => n.CurrentNote).Returns(mockNote1);

        var orgVars = new AutomateFormsOrgVariables(mockEnforcer1.Object, mockNotebook1.Object);

        var mockEnforcer2 = new Mock<IBestPracticeEnforcer>();
        var mockNotebook2 = new Mock<INotebook>();
        var mockNote2 = new Note { ServerId = "B" };
        mockNotebook2.Setup(n => n.CurrentNote).Returns(mockNote2);

        // Act
        orgVars.InstantiateVariables(mockEnforcer2.Object, mockNotebook2.Object);

        // Assert
        Assert.Same(mockEnforcer2.Object, orgVars.Enforcer);
        Assert.Same(mockNotebook2.Object, orgVars.Notebook);
        var serverIdVar = orgVars.Variables.Find(v => v.Name == "serverid");
        Assert.NotNull(serverIdVar);
        Assert.Equal("B", serverIdVar.GetValue());
    }

    [Fact]
    public void RegisterVariables_FirstNameVariable_ReturnsFirstWordOfContactName()
    {
        // Arrange
        var mockEnforcer = new Mock<IBestPracticeEnforcer>();
        var mockNotebook = new Mock<INotebook>();
        var note = new Note { ContactName = "John Doe" };
        mockNotebook.Setup(n => n.CurrentNote).Returns(note);

        var orgVars = new AutomateFormsOrgVariables(mockEnforcer.Object, mockNotebook.Object);

        // Act
        var firstNameVar = orgVars.Variables.Find(v => v.Name == "firstname");
        var value = firstNameVar.GetValue();

        // Assert
        Assert.Equal("John", value);
    }

    [Fact]
    public void RegisterVariables_FirstNameVariable_ReturnsEmptyIfNoSpace()
    {
        // Arrange
        var mockEnforcer = new Mock<IBestPracticeEnforcer>();
        var mockNotebook = new Mock<INotebook>();
        var note = new Note { ContactName = "SingleName" };
        mockNotebook.Setup(n => n.CurrentNote).Returns(note);

        var orgVars = new AutomateFormsOrgVariables(mockEnforcer.Object, mockNotebook.Object);

        // Act
        var firstNameVar = orgVars.Variables.Find(v => v.Name == "firstname");
        var value = firstNameVar.GetValue();

        // Assert
        Assert.Equal(string.Empty, value);
    }

    [Fact]
    public void RegisterVariables_FormNameGeneratorVariable_CallsEnforcer()
    {
        // Arrange
        var mockEnforcer = new Mock<IBestPracticeEnforcer>();
        mockEnforcer.Setup(e => e.GetFormName()).Returns("FormName123");
        var mockNotebook = new Mock<INotebook>();
        var note = new Note();
        mockNotebook.Setup(n => n.CurrentNote).Returns(note);

        var orgVars = new AutomateFormsOrgVariables(mockEnforcer.Object, mockNotebook.Object);

        // Act
        var formNameVar = orgVars.Variables.Find(v => v.Name == "formname");
        var value = formNameVar.GetValue();

        // Assert
        Assert.Equal("FormName123", value);
        mockEnforcer.Verify(e => e.GetFormName(), Times.Once);
    }
}