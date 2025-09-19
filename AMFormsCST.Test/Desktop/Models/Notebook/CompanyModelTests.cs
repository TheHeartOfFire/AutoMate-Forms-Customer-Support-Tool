using AMFormsCST.Desktop.Models;
using Moq;
using Xunit;
using CoreCompany = AMFormsCST.Core.Types.Notebook.Company;
using CoreNote = AMFormsCST.Core.Types.Notebook.Note;
using CoreDealer = AMFormsCST.Core.Types.Notebook.Dealer;

namespace AMFormsCST.Test.Desktop.Models.Notebook;

public class CompanyModelTests
{
    [Fact]
    public void IsBlank_IsTrue_ForNewModel()
    {
        // Arrange
        var company = new Company(null);

        // Assert
        Assert.True(company.IsBlank);
    }

    [Theory]
    [InlineData("Test Company", "")]
    [InlineData("", "C123")]
    [InlineData("Test Company", "C123")]
    public void IsBlank_IsFalse_WhenPropertiesAreSet(string name, string companyCode)
    {
        // Arrange
        var company = new Company(null);

        // Act
        company.Name = name;
        company.CompanyCode = companyCode;

        // Assert
        Assert.False(company.IsBlank);
    }

    [Fact]
    public void Constructor_FromCoreType_InitializesProperties()
    {
        // Arrange
        var coreCompany = new CoreCompany { Name = "C1", CompanyCode = "CC1", Notable = true };

        // Act
        var company = new Company(coreCompany, null);

        // Assert
        Assert.Equal("C1", company.Name);
        Assert.Equal("CC1", company.CompanyCode);
        Assert.True(company.Notable);
        Assert.Same(coreCompany, company.CoreType);
    }

    [Fact]
    public void ImplicitConversion_ToCoreCompany_MapsPropertiesCorrectly()
    {
        // Arrange
        var companyModel = new Company(null)
        {
            Name = "C1",
            CompanyCode = "CC1",
            Notable = true
        };

        // Act
        CoreCompany coreCompany = companyModel;

        // Assert
        Assert.Equal(companyModel.Id, coreCompany.Id);
        Assert.Equal("C1", coreCompany.Name);
        Assert.Equal("CC1", coreCompany.CompanyCode);
        Assert.True(coreCompany.Notable);
    }

    [Fact]
    public void UpdateCore_UpdatesCoreType()
    {
        // Arrange
        var noteModel = new NoteModel("x", null);
        var dealer = new Dealer(null) { Parent = noteModel };
        var company = new Company(null) { Parent = dealer };
        var coreCompany = new CoreCompany();
        company.CoreType = coreCompany;

        // Fully establish the parent-child relationships
        noteModel.Dealers.Add(dealer);
        dealer.Companies.Add(company);

        // Act
        company.Name = "New Company Name"; // This change should trigger the notification chain.

        // Assert
        // 1. Verify the underlying CoreType was updated.
        Assert.Equal("New Company Name", coreCompany.Name);

    }
}