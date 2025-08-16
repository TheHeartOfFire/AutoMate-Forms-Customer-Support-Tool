using AMFormsCST.Desktop.Models;
using Moq;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Xunit;
using AMFormsCST.Desktop.ViewModels.Pages.Tools;

public class ToolsViewModelTests
{
    [Fact]
    public void Constructor_InitializesNavigationCards()
    {
        // Act
        var vm = new ToolsViewModel();

        // Assert
        Assert.NotNull(vm.NavigationCards);
        Assert.IsAssignableFrom<ICollection<NavigationCard>>(vm.NavigationCards);
        Assert.True(vm.NavigationCards.Count > 0);
        foreach (var card in vm.NavigationCards)
        {
            Assert.NotNull(card);
            Assert.IsType<NavigationCard>(card);
        }
    }

    [Fact]
    public void NavigationCards_ContainsExpectedProperties()
    {
        // Act
        var vm = new ToolsViewModel();

        // Assert
        foreach (var card in vm.NavigationCards)
        {
            // Name, Icon, Description, and PageType can be null, but should exist
            Assert.True(card is NavigationCard);
            // Optionally check for at least one non-null property
            Assert.True(card.Name != null || card.Description != null || card.PageType != null);
        }
    }

    [Fact]
    public void NavigationCards_CanBeReplaced()
    {
        // Arrange
        var vm = new ToolsViewModel();
        var newCards = new ObservableCollection<NavigationCard>
        {
            new NavigationCard { Name = "Test", Icon = Wpf.Ui.Controls.SymbolRegular.Home24, Description = "Desc", PageType = typeof(string) }
        };

        // Act
        vm.NavigationCards = newCards;

        // Assert
        Assert.Equal(newCards, vm.NavigationCards);
        Assert.Single(vm.NavigationCards);
        Assert.Equal("Test", vm.NavigationCards.First().Name);
    }
}