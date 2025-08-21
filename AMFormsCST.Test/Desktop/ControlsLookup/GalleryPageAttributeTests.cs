using AMFormsCST.Desktop.ControlsLookup;
using Wpf.Ui.Controls;
using Xunit;

namespace AMFormsCST.Test.Desktop.ControlsLookup;
public class GalleryPageAttributeTests
{
    [Fact]
    public void Constructor_SetsPropertiesCorrectly()
    {
        // Arrange
        var description = "Test Description";
        var icon = SymbolRegular.MailTemplate24;

        // Act
        var attr = new GalleryPageAttribute(description, icon);

        // Assert
        Assert.Equal(description, attr.Description);
        Assert.Equal(icon, attr.Icon);
    }

    [Fact]
    public void AttributeUsage_IsClassOnly_AndSealed()
    {
        // Arrange
        var type = typeof(GalleryPageAttribute);

        // Act
        var usage = type.GetCustomAttributes(typeof(AttributeUsageAttribute), false);
        var isSealed = type.IsSealed;

        // Assert
        Assert.Single(usage);
        var attrUsage = (AttributeUsageAttribute)usage[0];
        Assert.Equal(AttributeTargets.Class, attrUsage.ValidOn);
        Assert.True(isSealed);
    }
}