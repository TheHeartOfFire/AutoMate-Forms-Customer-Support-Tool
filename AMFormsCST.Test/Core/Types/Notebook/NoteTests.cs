using AMFormsCST.Core.Interfaces;
using AMFormsCST.Core.Interfaces.Notebook;
using AMFormsCST.Core.Types.Notebook;
using Moq;
using System;
using System.Linq;
using Xunit;

namespace AMFormsCST.Test.Core.Types.Notebook;
public class NoteTests
{
    [Fact]
    public void DefaultConstructor_InitializesWithNewGuid()
    {
        // Arrange & Act
        var note = new Note();

        // Assert
        Assert.NotEqual(Guid.Empty, note.Id);
    }

    [Fact]
    public void Constructor_WithGuid_SetsId()
    {
        // Arrange
        var guid = Guid.NewGuid();

        // Act
        var note = new Note(guid);

        // Assert
        Assert.Equal(guid, note.Id);
    }

    [Fact]
    public void Clone_CreatesDeepCopyWithNewGuids()
    {
        // Arrange
        var original = new Note()
        {
            CaseText = "CT123",
            NotesText = "Some notes"
        };
        original.Dealers.Add(new Dealer { Name = "D1" });
        original.Contacts.Add(new Contact { Name = "C1" });
        original.Forms.Add(new Form { Name = "F1" });

        // Act
        var clone = original.Clone();

        // Assert
        Assert.NotSame(original, clone);
        Assert.NotEqual(original.Id, clone.Id);
        Assert.Equal(original.CaseText, clone.CaseText);
        Assert.Equal(original.NotesText, clone.NotesText);

        Assert.Equal(original.Dealers.Count, clone.Dealers.Count);
        Assert.NotSame(original.Dealers[0], clone.Dealers[0]);
        Assert.NotEqual(original.Dealers[0].Id, clone.Dealers[0].Id);
        Assert.Equal(original.Dealers[0].Name, clone.Dealers[0].Name);

        Assert.Equal(original.Contacts.Count, clone.Contacts.Count);
        Assert.NotSame(original.Contacts[0], clone.Contacts[0]);
        Assert.NotEqual(original.Contacts[0].Id, clone.Contacts[0].Id);
        Assert.Equal(original.Contacts[0].Name, clone.Contacts[0].Name);

        Assert.Equal(original.Forms.Count, clone.Forms.Count);
        Assert.NotSame(original.Forms[0], clone.Forms[0]);
        Assert.NotEqual(original.Forms[0].Id, clone.Forms[0].Id);
        Assert.Equal(original.Forms[0].Name, clone.Forms[0].Name);
    }

    [Fact]
    public void Equals_ReturnsTrue_ForSameId()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var note1 = new Note(guid);
        var note2 = new Note(guid);

        // Act & Assert
        Assert.True(note1.Equals((INote)note2));
        Assert.True(note1.Equals((object)note2));
    }

    [Fact]
    public void Equals_ReturnsFalse_ForDifferentId()
    {
        // Arrange
        var note1 = new Note(Guid.NewGuid());
        var note2 = new Note(Guid.NewGuid());

        // Act & Assert
        Assert.False(note1.Equals((INote)note2));
        Assert.False(note1.Equals((object)note2));
    }

    [Fact]
    public void Equals_WithNull_ReturnsFalse()
    {
        // Arrange
        var note = new Note();

        // Act & Assert
        Assert.False(note.Equals((INote?)null));
        Assert.False(note.Equals((object?)null));
    }

    [Fact]
    public void GetHashCode_ReturnsIdHashCode()
    {
        // Arrange
        var note = new Note();
        var expected = note.Id.GetHashCode();

        // Act
        var hash = note.GetHashCode();

        // Assert
        Assert.Equal(expected, hash);
    }

    [Fact]
    public void EqualsComparer_ImplementsEqualityCorrectly()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var note1 = new Note(guid);
        var note2 = new Note(guid);
        var note3 = new Note(Guid.NewGuid());

        // Act & Assert
        Assert.True(note1.Equals(note1, note2));
        Assert.False(note1.Equals(note1, note3));
        Assert.False(note1.Equals(note1, null));
        Assert.False(note1.Equals(null, note3));
        Assert.True(note1.Equals(null, null));
    }

    [Fact]
    public void GetHashCodeComparer_ReturnsIdHashCode()
    {
        // Arrange
        var note = new Note();
        INote inote = note;

        // Act
        var hash = note.GetHashCode(inote);

        // Assert
        Assert.Equal(note.Id.GetHashCode(), hash);
    }

    [Fact]
    public void Dump_ReturnsAllPropertiesInString()
    {
        // Arrange
        var note = new Note()
        {
            CaseText = "CT123",
            NotesText = "Some notes"
        };
        note.Dealers.Add(new Dealer { Name = "Test Dealer" });

        // Act
        var dump = note.Dump();

        // Assert
        Assert.Contains("CaseText: CT123", dump);
        Assert.Contains("NotesText: Some notes", dump);
        Assert.Contains("Dealer: Test Dealer", dump);
        Assert.Contains("Id: " + note.Id, dump);
    }
}