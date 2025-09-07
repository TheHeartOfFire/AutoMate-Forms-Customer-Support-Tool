using AMFormsCST.Desktop.Models;
using Moq;
using Xunit;
using CoreContact = AMFormsCST.Core.Types.Notebook.Contact;

namespace AMFormsCST.Test.Desktop.Models.Notebook;

public class ContactModelTests
{
    private const string TestExtSeparator = "x";

    [Fact]
    public void Constructor_SetsPhoneExtensionDelimiter()
    {
        // Arrange & Act
        var contact = new Contact(TestExtSeparator, null);

        // Assert
        Assert.Equal(TestExtSeparator, contact.PhoneExtensionDelimiter);
    }

    [Fact]
    public void IsBlank_IsTrue_ForNewModel()
    {
        // Arrange
        var contact = new Contact(TestExtSeparator, null);

        // Assert
        Assert.True(contact.IsBlank);
    }

    [Theory]
    [InlineData("John Doe", "", "", "")]
    [InlineData("", "john.doe@email.com", "", "")]
    [InlineData("", "", "555-1234", "")]
    [InlineData("", "", "", "101")]
    public void IsBlank_IsFalse_WhenAnyPropertyIsSet(string name, string email, string phone, string extension)
    {
        // Arrange
        var contact = new Contact(TestExtSeparator, null);

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
        var contact = new Contact(TestExtSeparator, null)
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
        var contact = new Contact(TestExtSeparator, null)
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
        var contact = new Contact(TestExtSeparator, null)
        {
            Phone = "",
            PhoneExtension = "101"
        };

        // Assert
        Assert.Empty(contact.FullPhone);
    }

    [Fact]
    public void ParsePhone_SplitsPhoneAndExtensionCorrectly()
    {
        // Arrange
        var contact = new Contact(" ", null); // Use space as delimiter for this test

        // Act
        contact.ParsePhone("555-1234 101");

        // Assert
        Assert.Equal("555-1234", contact.Phone);
        Assert.Equal("101", contact.PhoneExtension);
    }

    [Fact]
    public void ImplicitConversion_ToCoreContact_MapsPropertiesCorrectly()
    {
        // Arrange
        var contactModel = new Contact(TestExtSeparator, null)
        {
            Name = "Jane Doe",
            Email = "jane.doe@email.com",
            Phone = "555-5678",
            PhoneExtension = "102",
            PhoneExtensionDelimiter = TestExtSeparator
        };

        // Act
        CoreContact coreContact = contactModel;

        // Assert
        Assert.Equal(contactModel.Id, coreContact.Id);
        Assert.Equal("Jane Doe", coreContact.Name);
        Assert.Equal("jane.doe@email.com", coreContact.Email);
        Assert.Equal("555-5678", coreContact.Phone);
        Assert.Equal("102", coreContact.PhoneExtension);
        Assert.Equal(TestExtSeparator, coreContact.PhoneExtensionDelimiter);
    }

    [Fact]
    public void UpdateCore_UpdatesCoreTypeAndNotifiesParent()
    {
        // Arrange
        var noteModel = new NoteModel(TestExtSeparator, null);
        var contact = new Contact(TestExtSeparator, null) { Parent = noteModel };
        var coreContact = new CoreContact();
        contact.CoreType = coreContact;

        // Fully establish the parent-child relationship
        noteModel.Contacts.Add(contact);

        bool wasNotified = false;
        noteModel.PropertyChanged += (sender, args) =>
        {
            // The notification bubbles up as a generic "all properties changed" signal.
            if (string.IsNullOrEmpty(args.PropertyName))
            {
                wasNotified = true;
            }
        };

        // Act
        contact.Name = "New Contact Name"; // This change should trigger the notification chain.

        // Assert
        // 1. Verify the underlying CoreType was updated.
        Assert.Equal("New Contact Name", coreContact.Name);

        // 2. Verify the notification bubbled up to the NoteModel.
        Assert.True(wasNotified, "The NoteModel's PropertyChanged event was not raised as expected.");
    }
}