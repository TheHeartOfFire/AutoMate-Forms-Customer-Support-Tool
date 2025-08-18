using AMFormsCST.Desktop.Interfaces;
using AMFormsCST.Desktop.Models.FormgenUtilities;
using AMFormsCST.Test.Helpers;
using System.Windows.Controls;
using Wpf.Ui.Controls;
using Assert = Xunit.Assert;

namespace AMFormsCST.Test.ModelTests.FormgenUtilities;

[Collection("STA Tests")]
public class BasicStatsTests
{
    // A mock class implementing IFormgenFileProperties for testing purposes.
    private class MockProperties : IFormgenFileProperties
    {
        public string Name { get; set; } = "Test Name";
        public int Count { get; set; } = 10;
        public bool IsEnabled { get; set; } = true;
        public IFormgenFileSettings? Settings { get; set; }

        public IEnumerable<DisplayProperty> GetDisplayProperties()
        {
            yield return new DisplayProperty("Name", Name);
            yield return new DisplayProperty("Count", Count.ToString());
        }
    }

    // A mock class for settings.
    private class MockSettings : IFormgenFileSettings
    {
        public string Setting1 { get; set; } = "Value1";
    }

    [WpfFact]
    public void GetSettingsAndPropertiesUIElements_CreatesMainPropertiesCard()
    {
        // Arrange
        var properties = new MockProperties();

        // Act
        var uiElement = BasicStats.GetSettingsAndPropertiesUIElements(properties);

        // Assert
        Assert.IsType<StackPanel>(uiElement);
        var stackPanel = (StackPanel)uiElement;
        Assert.NotEmpty(stackPanel.Children);

        var propertiesCard = stackPanel.Children[0] as CardControl;
        Assert.NotNull(propertiesCard);
        Assert.Equal("Properties", propertiesCard.Header);
    }

    [WpfFact]
    public void GetSettingsAndPropertiesUIElements_CreatesSeparateCardForSettings()
    {
        // Arrange
        var properties = new MockProperties
        {
            Settings = new MockSettings()
        };

        // Act
        var uiElement = BasicStats.GetSettingsAndPropertiesUIElements(properties);

        // Assert
        var stackPanel = (StackPanel)uiElement;
        Assert.Equal(2, stackPanel.Children.Count); // Should have Properties card and Settings card

        var settingsCard = stackPanel.Children[1] as CardControl; // Settings card is added after Properties
        Assert.NotNull(settingsCard);
        Assert.Equal("Settings", settingsCard.Header);
    }

    [WpfFact]
    public void GetSettingsAndPropertiesUIElements_DoesNotCreateSettingsCard_WhenSettingsIsNull()
    {
        // Arrange
        var properties = new MockProperties
        {
            Settings = null // Explicitly null
        };

        // Act
        var uiElement = BasicStats.GetSettingsAndPropertiesUIElements(properties);

        // Assert
        var stackPanel = (StackPanel)uiElement;
        Assert.Single(stackPanel.Children); // Only the Properties card should exist

        var propertiesCard = stackPanel.Children[0] as CardControl;
        Assert.NotNull(propertiesCard);
        Assert.Equal("Properties", propertiesCard.Header);
    }

    [WpfFact]
    public void GetSettingsAndPropertiesUIElements_CardContentIsGrid_WithCorrectStructure()
    {
        // Arrange
        var properties = new MockProperties();

        // Act
        var uiElement = BasicStats.GetSettingsAndPropertiesUIElements(properties);

        // Assert
        var stackPanel = (StackPanel)uiElement;
        var propertiesCard = (CardControl)stackPanel.Children[0];
        var grid = propertiesCard.Content as Grid;

        Assert.NotNull(grid);
        Assert.Equal(2, grid.ColumnDefinitions.Count); // Label and Editor columns
        
        // Expect 3 rows for 3 properties (Name, Count, IsEnabled). GetDisplayProperties is not reflected here.
        Assert.Equal(3, grid.RowDefinitions.Count); 
    }
}