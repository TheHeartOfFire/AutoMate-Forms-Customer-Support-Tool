using AMFormsCST.Core.Interfaces;
using AMFormsCST.Core.Interfaces.BestPractices;
using AMFormsCST.Core.Interfaces.Utils;
using AMFormsCST.Core.Types.BestPractices.Models;
using AMFormsCST.Desktop.Models.FormNameGenerator;
using AMFormsCST.Desktop.ViewModels.Pages.Tools;
using Moq;
using Assert = Xunit.Assert;

namespace AMFormsCST.Test.ViewModelTests;

public class FormNameGeneratorViewModelTests
{
    private readonly Mock<ISupportTool> _mockSupportTool;
    private readonly Mock<IBestPracticeEnforcer> _mockEnforcer;
    private readonly Mock<IFormNameBestPractice> _mockFormNamePractice;

    public FormNameGeneratorViewModelTests()
    {
        // 1. Mock the full dependency chain required by the view model and its models.
        _mockSupportTool = new Mock<ISupportTool>();
        _mockEnforcer = new Mock<IBestPracticeEnforcer>();
        _mockFormNamePractice = new Mock<IFormNameBestPractice>();

        // 2. Configure the mocks to return each other, simulating the real object graph.
        _mockEnforcer.Setup(e => e.FormNameBestPractice).Returns(_mockFormNamePractice.Object);
        _mockSupportTool.Setup(st => st.Enforcer).Returns(_mockEnforcer.Object);
    }

    [Fact]
    public void Constructor_InitializesWithCorrectDefaults()
    {
        // Arrange & Act
        var viewModel = new FormNameGeneratorViewModel(_mockSupportTool.Object);

        // Assert
        Assert.NotNull(viewModel.Form);
        Assert.True(viewModel.IsPdfSelected); // Default state from constructor
        Assert.False(viewModel.IsImpactSelected);
        Assert.Contains(Form.Tag.Pdf, viewModel.Form.Tags); // Check the underlying model state
    }

    [Fact]
    public void TogglingIsImpactSelected_UpdatesFormTags()
    {
        // Arrange
        var viewModel = new FormNameGeneratorViewModel(_mockSupportTool.Object);

        // Act
        viewModel.IsImpactSelected = true;

        // Assert
        Assert.Contains(Form.Tag.Impact, viewModel.Form.Tags);

        // Act again to toggle off
        viewModel.IsImpactSelected = false;

        // Assert
        Assert.DoesNotContain(Form.Tag.Impact, viewModel.Form.Tags);
    }

    [Fact]
    public void TogglingIsSoldSelected_ManagesTradeAndFormTags()
    {
        // Arrange
        var viewModel = new FormNameGeneratorViewModel(_mockSupportTool.Object);
        viewModel.IsTradeSelected = true; // Start with Trade selected
        Assert.Contains(Form.Tag.Trade, viewModel.Form.Tags);

        // Act
        viewModel.IsSoldSelected = true;

        // Assert
        Assert.True(viewModel.IsSoldSelected);
        Assert.False(viewModel.IsTradeSelected); // Should be set to false by the viewmodel logic
        Assert.Contains(Form.Tag.Sold, viewModel.Form.Tags);
        Assert.DoesNotContain(Form.Tag.Trade, viewModel.Form.Tags);
    }

    [Fact]
    public void TogglingIsTradeSelected_ManagesSoldAndFormTags()
    {
        // Arrange
        var viewModel = new FormNameGeneratorViewModel(_mockSupportTool.Object);
        viewModel.IsSoldSelected = true; // Start with Sold selected
        Assert.Contains(Form.Tag.Sold, viewModel.Form.Tags);

        // Act
        viewModel.IsTradeSelected = true;

        // Assert
        Assert.True(viewModel.IsTradeSelected);
        Assert.False(viewModel.IsSoldSelected); // Should be set to false by the viewmodel logic
        Assert.Contains(Form.Tag.Trade, viewModel.Form.Tags);
        Assert.DoesNotContain(Form.Tag.Sold, viewModel.Form.Tags);
    }

    [Fact]
    public void ClearFormCommand_ResetsViewModelAndModelProperties()
    {
        // Arrange
        var viewModel = new FormNameGeneratorViewModel(_mockSupportTool.Object)
        {
            IsImpactSelected = true,
            IsSoldSelected = true
        };
        viewModel.Form.State = "TX";
        viewModel.Form.Title = "Initial Title";

        // Act
        viewModel.ClearFormCommand.Execute(null);

        // Assert
        // Verify ViewModel properties are reset to their default states
        Assert.True(viewModel.IsPdfSelected);
        Assert.False(viewModel.IsImpactSelected);
        Assert.False(viewModel.IsSoldSelected);
        Assert.False(viewModel.IsTradeSelected);
        Assert.False(viewModel.IsLawSelected);
        Assert.False(viewModel.IsCustomSelected);
        Assert.False(viewModel.IsVehicleMerchandisingSelected);

        // Verify underlying Form model properties are cleared
        Assert.True(string.IsNullOrEmpty(viewModel.Form.State));
        Assert.True(string.IsNullOrEmpty(viewModel.Form.Title));
        
        // Verify tags are reset to default (only Pdf)
        Assert.Single(viewModel.Form.Tags);
        Assert.Contains(Form.Tag.Pdf, viewModel.Form.Tags);
    }
}