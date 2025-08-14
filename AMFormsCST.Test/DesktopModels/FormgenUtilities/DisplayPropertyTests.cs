using AMFormsCST.Desktop.Models.FormgenUtilities;
using Xunit;

public class DisplayPropertyTests
{
    [Fact]
    public void Constructor_SetsNameAndValue()
    {
        // Arrange
        var name = "PropertyName";
        var value = "PropertyValue";

        // Act
        var prop = new DisplayProperty(name, value);

        // Assert
        Assert.Equal(name, prop.Name);
        Assert.Equal(value, prop.Value);
    }

    [Fact]
    public void Value_CanBeNull()
    {
        // Arrange
        var name = "PropertyName";
        string? value = null;

        // Act
        var prop = new DisplayProperty(name, value);

        // Assert
        Assert.Equal(name, prop.Name);
        Assert.Null(prop.Value);
    }
}