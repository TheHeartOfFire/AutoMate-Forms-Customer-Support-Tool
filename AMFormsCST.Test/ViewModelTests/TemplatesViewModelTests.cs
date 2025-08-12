using AMFormsCST.Core.Interfaces;
using AMFormsCST.Core.Interfaces.BestPractices;
using AMFormsCST.Core.Interfaces.UserSettings;
using AMFormsCST.Core.Interfaces.Utils;
using AMFormsCST.Core.Types.BestPractices.TextTemplates.Models;
using AMFormsCST.Desktop.ViewModels.Pages.Tools;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace AMFormsCST.Test.ViewModelTests;

public class TemplatesViewModelTests
{
    private readonly Mock<ISupportTool> _mockSupportTool;
    private readonly Mock<IBestPracticeEnforcer> _mockEnforcer;
    private readonly Mock<ISettings> _mockSettings;
    private readonly Mock<IUserSettings> _mockUserSettings;
    private readonly Mock<IOrgVariables> _mockOrgVariables;
    private readonly List<TextTemplate> _templateList;

    public TemplatesViewModelTests()
    {
        // 1. Mock all required dependencies
        _mockSupportTool = new Mock<ISupportTool>();
        _mockEnforcer = new Mock<IBestPracticeEnforcer>();
        _mockSettings = new Mock<ISettings>();
        _mockUserSettings = new Mock<IUserSettings>();
        _mockOrgVariables = new Mock<IOrgVariables>();

        // 2. Create a list of templates to be returned by the mock enforcer
        _templateList =
        [
            new TextTemplate("Template 1", "Desc 1", "Text 1"),
            new TextTemplate("Template 2", "Desc 2", "Text 2")
        ];

        // 3. Configure the full dependency chain required by the view models
        _mockOrgVariables.Setup(ov => ov.Variables).Returns([]); // Return an empty list for the variables
        _mockUserSettings.Setup(us => us.Organization).Returns(_mockOrgVariables.Object);
        _mockSettings.Setup(s => s.UserSettings).Returns(_mockUserSettings.Object);
        _mockSupportTool.Setup(st => st.Settings).Returns(_mockSettings.Object);
        _mockEnforcer.Setup(e => e.Templates).Returns(_templateList);
        _mockSupportTool.Setup(st => st.Enforcer).Returns(_mockEnforcer.Object);
    }

    [Fact]
    public void Constructor_LoadsTemplatesFromEnforcer()
    {
        // Arrange & Act
        var viewModel = new TemplatesViewModel(_mockSupportTool.Object);

        // Assert
        Assert.Equal(2, viewModel.Templates.Count);
        // The constructor creates a new, blank item for the initial selection.
        Assert.NotNull(viewModel.SelectedTemplate);
        Assert.True(string.IsNullOrEmpty(viewModel.SelectedTemplate.Template.Name));
    }

    [Fact]
    public void RemoveTemplateCommand_RemovesTemplateFromEnforcer_AndRefreshesList()
    {
        // Arrange
        var viewModel = new TemplatesViewModel(_mockSupportTool.Object);
        var templateToRemove = viewModel.Templates.First();
        var modelToRemove = templateToRemove.Template;

        // Act
        viewModel.RemoveTemplateCommand.Execute(templateToRemove);

        // Assert
        // Verify the enforcer's RemoveTemplate method was called with the correct model
        _mockEnforcer.Verify(e => e.RemoveTemplate(modelToRemove), Times.Once);
        
        // The view model re-queries the list. To simulate this, we can remove the item from our source list
        // and then verify the mock was called to get the templates again.
        _mockEnforcer.Verify(e => e.Templates, Times.Exactly(2)); // Once in constructor, once after remove
    }

    [Fact]
    public void SelectTemplateCommand_ChangesSelectedTemplate()
    {
        // Arrange
        var viewModel = new TemplatesViewModel(_mockSupportTool.Object);
        var templateToSelect = viewModel.Templates[1];

        // Act
        viewModel.SelectTemplateCommand.Execute(templateToSelect);

        // Assert
        Assert.Same(templateToSelect, viewModel.SelectedTemplate);
    }
}