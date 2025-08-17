using AMFormsCST.Desktop.Models;

namespace AMFormsCST.Test.ModelTests;
using Assert = Xunit.Assert;

public class ContactModelTests
{
    private const string TestExtSeparator = "x";

    [Fact]
    public void Constructor_SetsPhoneExtensionDelimiter()
    {
        // Arrange & Act
        var contact = new Contact(TestExtSeparator);

        // Assert
        Assert.Equal(TestExtSeparator, contact.PhoneExtensionDelimiter);
    }

    [Fact]
    public void IsBlank_IsTrue_ForNewModel()
    {
        // Arrange
        var contact = new Contact(TestExtSeparator);

        // Assert
        Assert.True(contact.IsBlank);
    }

    [Theory]
    [InlineData("John Doe", null, null, null)]
    [InlineData(null, "john.doe@email.com", null, null)]
    [InlineData(null, null, "555-1234", null)]
    [InlineData(null, null, null, "101")]
    public void IsBlank_IsFalse_WhenAnyPropertyIsSet(string? name, string? email, string? phone, string? extension)
    {
        // Arrange
        var contact = new Contact(TestExtSeparator);

        // Act
        contact.Name = name;
        contact.Email = email;
        contact.Phone = phone;
        contact.PhoneExtension = extension;

        // Assert
        Assert.False(contact.IsBlank);
    }

    [Fact]
    public void FullPhone_ReturnsCombinedString_WhenAllPartsExist()
    {
        // Arrange
        var contact = new Contact(TestExtSeparator)
        {
            Phone = "555-1234",
            PhoneExtension = "101"
        };

        // Assert
        Assert.Equal($"555-1234{TestExtSeparator}101", contact.FullPhone);
    }

    [Fact]
    public void FullPhone_ReturnsOnlyPhone_WhenExtensionIsMissing()
    {
        // Arrange
        var contact = new Contact(TestExtSeparator)
        {
            Phone = "555-1234",
            PhoneExtension = ""
        };

        // Assert
        Assert.Equal("555-1234", contact.FullPhone);
    }

    [Fact]
    public void FullPhone_ReturnsEmpty_WhenPhoneIsMissing()
    {
        // Arrange
        var contact = new Contact(TestExtSeparator)
        {
            Phone = "",
            PhoneExtension = "101"
        };

        // Assert
        Assert.Empty(contact.FullPhone);
    }

    [Fact]
    public void SelectAndDeselect_UpdateIsSelectedProperty()
    {
        // Arrange
        var contact = new Contact(TestExtSeparator);
        Assert.False(contact.IsSelected); // Initial state

        // Act: Select
        contact.Select();

        // Assert
        Assert.True(contact.IsSelected);

        // Act: Deselect
        contact.Deselect();

        // Assert
        Assert.False(contact.IsSelected);
    }
}