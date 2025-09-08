using AMFormsCST.Desktop.Models.FormNameGenerator;
using AMFormsCST.Core.Interfaces;
using AMFormsCST.Core.Interfaces.BestPractices;
using AMFormsCST.Core.Types.BestPractices.Models;
using AMFormsCST.Core.Types.BestPractices.TextTemplates.Models;
using AMFormsCST.Core.Types.BestPractices.Practices;
using Moq;
using Xunit;
using AMFormsCST.Core.Interfaces.Utils;

namespace AMFormsCST.Test.Desktop.Models.FormNameGenerator;
public class FormTests
{
    private ISupportTool CreateSupportToolMock()
    {
        var formModel = new AutoMateFormModel();
        var formNameBestPracticeMock = new Mock<IFormNameBestPractice>();
        formNameBestPracticeMock.SetupProperty(x => x.Model, formModel);
        formNameBestPracticeMock.Setup(x => x.Generate()).Returns("GeneratedFileName");

        var bestPracticeEnforcerMock = new Mock<IBestPracticeEnforcer>();
        bestPracticeEnforcerMock.SetupGet(x => x.FormNameBestPractice).Returns(formNameBestPracticeMock.Object);
        bestPracticeEnforcerMock.Setup(x => x.GetFormName()).Returns(() => formNameBestPracticeMock.Object.Generate());

        var supportToolMock = new Mock<ISupportTool>();
        supportToolMock.SetupProperty(x => x.Enforcer, bestPracticeEnforcerMock.Object);
        supportToolMock.SetupProperty(x => x.CodeBlocks);
        supportToolMock.SetupProperty(x => x.FormgenUtils);
        supportToolMock.SetupProperty(x => x.Notebook);
        supportToolMock.SetupProperty(x => x.Settings);

        return supportToolMock.Object;
    }

    [Fact]
    public void Constructor_InitializesTagsAndPdfTag()
    {
        // Arrange
        var supportTool = CreateSupportToolMock();

        // Act
        var form = new Form(supportTool);

        // Assert
        Assert.NotNull(form.Tags);
        Assert.Contains(Form.Tag.Pdf, form.Tags);
    }

    [Fact]
    public void AddTag_AddsTagAndHandlesMutualExclusion()
    {
        // Arrange
        var supportTool = CreateSupportToolMock();
        var form = new Form(supportTool);

        // Act
        form.AddTag(Form.Tag.Impact);
        form.AddTag(Form.Tag.Pdf); // Should remove Impact
        form.AddTag(Form.Tag.Sold);
        form.AddTag(Form.Tag.Trade); // Should remove Sold

        // Assert
        Assert.Contains(Form.Tag.Pdf, form.Tags);
        Assert.DoesNotContain(Form.Tag.Impact, form.Tags);
        Assert.Contains(Form.Tag.Trade, form.Tags);
        Assert.DoesNotContain(Form.Tag.Sold, form.Tags);
    }

    [Fact]
    public void RemoveTag_RemovesTag()
    {
        // Arrange
        var supportTool = CreateSupportToolMock();
        var form = new Form(supportTool);
        form.AddTag(Form.Tag.Custom);

        // Act
        form.RemoveTag(Form.Tag.Custom);

        // Assert
        Assert.DoesNotContain(Form.Tag.Custom, form.Tags);
    }

    [Fact]
    public void Clear_ResetsPropertiesAndTags()
    {
        // Arrange
        var supportTool = CreateSupportToolMock();
        var form = new Form(supportTool);
        form.Title = "Title";
        form.Code = "Code";
        form.RevisionDate = "Date";
        form.State = "State";
        form.Dealer = "Dealer";
        form.Oem = "OEM";
        form.Bank = "Bank";
        form.Provider = "Provider";
        form.AddTag(Form.Tag.Custom);

        // Act
        form.Clear();

        // Assert
        Assert.Equal(string.Empty, form.Title);
        Assert.Equal(string.Empty, form.Code);
        Assert.Equal(string.Empty, form.RevisionDate);
        Assert.Equal(string.Empty, form.State);
        Assert.Equal(string.Empty, form.Dealer);
        Assert.Equal(string.Empty, form.Oem);
        Assert.Equal(string.Empty, form.Bank);
        Assert.Equal(string.Empty, form.Provider);
        Assert.Single(form.Tags);
        Assert.Contains(Form.Tag.Pdf, form.Tags);
    }

    [Fact]
    public void FileName_ReflectsGetFileNameResult()
    {
        // Arrange
        var supportTool = CreateSupportToolMock();
        var form = new Form(supportTool)
        {
            Title = "TestTitle",
            Code = "TestCode",
            RevisionDate = "2024-06-01",
            State = "CA",
            Dealer = "TestDealer",
            Oem = "TestOEM",
            Bank = "TestBank"
        };
        form.AddTag(Form.Tag.Law);
        form.AddTag(Form.Tag.Custom);

        // Act
        var fileName = form.FileName;

        // Assert
        Assert.Equal("GeneratedFileName", fileName);
    }
}