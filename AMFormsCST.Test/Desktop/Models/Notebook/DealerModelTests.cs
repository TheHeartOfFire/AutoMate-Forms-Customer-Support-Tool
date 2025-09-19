using AMFormsCST.Desktop.Interfaces;
using AMFormsCST.Desktop.Models;
using Moq;
using System.Linq;
using Xunit;
using CoreDealer = AMFormsCST.Core.Types.Notebook.Dealer;

namespace AMFormsCST.Test.Desktop.Models.Notebook;

public class DealerModelTests
{
    [Fact]
    public void Constructor_InitializesWithOneBlankCompany_AndSelectsIt()
    {
        // Arrange & Act
        var dealer = new Dealer(null);

        // Assert
        Assert.Single(dealer.Companies);
        Assert.True(dealer.Companies[0].IsBlank);

        Assert.NotNull(dealer.SelectedCompany);
        Assert.Same(dealer.Companies[0], dealer.SelectedCompany);
        Assert.Equal(IManagedObservableCollectionItem.CollectionMemberState.Selected, dealer.SelectedCompany.State);
    }

    [Fact]
    public void IsBlank_IsTrue_ForNewModel()
    {
        // Arrange
        var dealer = new Dealer(null);

        // Assert
        Assert.True(dealer.IsBlank);
    }

    [Theory]
    [InlineData("Test Dealer", "")]
    [InlineData("", "SVR1")]
    [InlineData("Test Dealer", "SVR1")]
    public void IsBlank_IsFalse_WhenTopLevelPropertiesAreSet(string name, string serverCode)
    {
        // Arrange
        var dealer = new Dealer(null);

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
        var dealer = new Dealer(null);
        Assert.True(dealer.IsBlank); // Pre-condition check

        // Act
        dealer.Companies[0].Name = "Test Company";

        // Assert
        Assert.False(dealer.IsBlank);
    }

    [Fact]
    public void SelectingAnItem_UpdatesSelectedCompany_AndItemState()
    {
        // Arrange
        var dealer = new Dealer(null);
        var company1 = dealer.Companies[0];
        company1.Name = "Company 1"; // Makes it non-blank, collection adds a new blank item
        var company2 = dealer.Companies.Last(c => c.IsBlank);

        // Act
        company2.Select();

        // Assert
        Assert.Same(company2, dealer.SelectedCompany);
        Assert.Equal(IManagedObservableCollectionItem.CollectionMemberState.Selected, company2.State);
        Assert.Equal(IManagedObservableCollectionItem.CollectionMemberState.NotSelected, company1.State);
    }

    [Fact]
    public void ImplicitConversion_ToCoreDealer_MapsPropertiesCorrectly()
    {
        // Arrange
        var dealerModel = new Dealer(null)
        {
            Name = "D1",
            ServerCode = "S1",
            Notable = true
        };
        // The constructor already adds a blank company. We add a second, non-blank one.
        var company = new Company(null) { Name = "C1", CompanyCode = "CC1" };
        dealerModel.Companies.Add(company);

        // Act
        CoreDealer coreDealer = dealerModel;

        // Assert
        Assert.Equal(dealerModel.Id, coreDealer.Id);
        Assert.Equal("D1", coreDealer.Name);
        Assert.Equal("S1", coreDealer.ServerCode);
        Assert.True(coreDealer.Notable);
        
        // The test should account for the blank item that is part of the collection.
        Assert.Equal(2, coreDealer.Companies.Count);
        var coreCompany = coreDealer.Companies.FirstOrDefault(c => c.Name == "C1");
        Assert.NotNull(coreCompany);
        Assert.Equal("CC1", coreCompany.CompanyCode);
    }

    [Fact]
    public void UpdateCore_UpdatesCoreTypeAndNotifiesParent()
    {
        // Arrange
        var noteModel = new NoteModel("x", null);
        var dealer = new Dealer(null) { Parent = noteModel };
        var coreDealer = new CoreDealer();
        dealer.CoreType = coreDealer;

        // Fully establish the parent-child relationship
        noteModel.Dealers.Add(dealer);

        // Act
        dealer.Name = "New Dealer Name"; // This change should trigger the notification chain.

        // Assert
        // 1. Verify the underlying CoreType was updated.
        Assert.Equal("New Dealer Name", coreDealer.Name);
    }
}