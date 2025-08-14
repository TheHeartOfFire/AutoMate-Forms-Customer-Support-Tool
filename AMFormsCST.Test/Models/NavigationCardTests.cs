using AMFormsCST.Desktop.Models;
using Wpf.Ui.Controls;
using Xunit;
using System;

public class NavigationCardTests
{
    [Fact]
    public void Constructor_SetsAllProperties()
    {
        // Act
        var card = new NavigationCard
        {
            Name = "Home",
            Icon = SymbolRegular.Home24,
            Description = "Go to home page",
            PageType = typeof(string)
        };

        // Assert
        Assert.Equal("Home", card.Name);
        Assert.Equal(SymbolRegular.Home24, card.Icon);
        Assert.Equal("Go to home page", card.Description);
        Assert.Equal(typeof(string), card.PageType);
    }

    [Fact]
    public void Properties_AreInitOnly()
    {
        // Act
        var card = new NavigationCard
        {
            Name = "Settings",
            Icon = SymbolRegular.Settings24,
            Description = "Open settings",
            PageType = typeof(int)
        };

        // Assert
        Assert.Equal("Settings", card.Name);
        Assert.Equal(SymbolRegular.Settings24, card.Icon);
        Assert.Equal("Open settings", card.Description);
        Assert.Equal(typeof(int), card.PageType);
    }

    [Fact]
    public void Properties_CanBeNull()
    {
        // Act
        var card = new NavigationCard
        {
            Name = null,
            Icon = SymbolRegular.Checkmark24,
            Description = null,
            PageType = null
        };

        // Assert
        Assert.Null(card.Name);
        Assert.Equal(SymbolRegular.Checkmark24, card.Icon);
        Assert.Null(card.Description);
        Assert.Null(card.PageType);
    }
}