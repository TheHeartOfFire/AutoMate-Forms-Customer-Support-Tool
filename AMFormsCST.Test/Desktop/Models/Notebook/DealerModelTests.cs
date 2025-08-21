using AMFormsCST.Desktop.Models;
using Assert = Xunit.Assert;

namespace AMFormsCST.Test.Desktop.Models.Notebook;

public class DealerModelTests
{
    [Fact]
    public void Constructor_InitializesWithOneBlankCompany_AndSelectsIt()
    {
        // Arrange & Act
        var dealer = new Dealer();

        // Assert
        // Verify the Companies collection is created and contains a single, blank item.
        Assert.Single(dealer.Companies);
        Assert.True(dealer.Companies[0].IsBlank);

        // Verify that the initially created blank company is set as the selected company.
        Assert.NotNull(dealer.SelectedCompany);
        Assert.Same(dealer.Companies[0], dealer.SelectedCompany);
        Assert.True(dealer.SelectedCompany.IsSelected);
    }

    [Fact]
    public void IsBlank_IsTrue_ForNewModel()
    {
        // Arrange
        var dealer = new Dealer();

        // Assert
        Assert.True(dealer.IsBlank);
    }

    [Theory]
    [InlineData("Test Dealer", null)]
    [InlineData(null, "SVR1")]
    public void IsBlank_IsFalse_WhenTopLevelPropertiesAreSet(string? name, string? serverCode)
    {
        // Arrange
        var dealer = new Dealer();

        // Act
        dealer.Name = name;
        dealer.ServerCode = serverCode;

        // Assert
        Assert.False(dealer.IsBlank);
    }

    [Fact]
    public void IsBlank_IsFalse_WhenChildCompanyBecomesNonBlank()
    {
        // Arrange
        var dealer = new Dealer();
        Assert.True(dealer.IsBlank); // Pre-condition check

        // Act
        // Make the child company non-blank.
        dealer.Companies[0].Name = "Test Company";

        // Assert
        // The parent dealer should now report that it is not blank.
        Assert.False(dealer.IsBlank);
    }

    [Fact]
    public void SelectCompany_UpdatesSelectedCompany_AndSelectionState()
    {
        // Arrange
        var dealer = new Dealer();
        var company1 = dealer.Companies[0];
        company1.Name = "Company 1"; // Making this non-blank will trigger the parent NoteModel (in a real scenario) to add a new blank one. For this test, we add it manually.
        var company2 = new Company();
        dealer.Companies.Add(company2);

        // Act
        dealer.SelectCompany(company2);

        // Assert
        Assert.Same(company2, dealer.SelectedCompany);
        Assert.True(company2.IsSelected);
        Assert.False(company1.IsSelected);
    }

    [Fact]
    public void SelectAndDeselect_UpdateIsSelectedProperty()
    {
        // Arrange
        var dealer = new Dealer();
        Assert.False(dealer.IsSelected); // Initial state

        // Act: Select
        dealer.Select();

        // Assert
        Assert.True(dealer.IsSelected);

        // Act: Deselect
        dealer.Deselect();

        // Assert
        Assert.False(dealer.IsSelected);
    }
}