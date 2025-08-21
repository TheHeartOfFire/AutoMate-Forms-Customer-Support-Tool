using AMFormsCST.Core.Interfaces.Notebook;
using AMFormsCST.Core.Types.Notebook;
using System;
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
        Assert.Equal(note.Id, note._id);
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
        Assert.Equal(guid, note._id);
    }

    [Fact]
    public void Clone_CreatesDeepCopyWithNewGuid()
    {
        // Arrange
        var original = new Note
        {
            ServerId = "S1",
            Companies = "C1",
            Dealership = "D1",
            ContactName = "CN",
            Email = "E",
            Phone = "P",
            PhoneExt = "PX",
            NotesText = "NT",
            CaseText = "CT",
            FormsText = "FT",
            DealText = "DT"
        };

        // Act
        var clone = Note.Clone(original);

        // Assert
        Assert.NotSame(original, clone);
        Assert.Equal(original.ServerId, clone.ServerId);
        Assert.Equal(original.Companies, clone.Companies);
        Assert.Equal(original.Dealership, clone.Dealership);
        Assert.Equal(original.ContactName, clone.ContactName);
        Assert.Equal(original.Email, clone.Email);
        Assert.Equal(original.Phone, clone.Phone);
        Assert.Equal(original.PhoneExt, clone.PhoneExt);
        Assert.Equal(original.NotesText, clone.NotesText);
        Assert.Equal(original.CaseText, clone.CaseText);
        Assert.Equal(original.FormsText, clone.FormsText);
        Assert.Equal(original.DealText, clone.DealText);
        // The clone gets a new Guid
        Assert.NotEqual(original.Id, clone.Id);
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
        Assert.True(note1.Equals((INote)note1));
        Assert.True(note1.Equals((object)note1));
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
        var note = new Note
        {
            ServerId = "S1",
            Companies = "C1",
            Dealership = "D1",
            ContactName = "CN",
            Email = "E",
            Phone = "P",
            PhoneExt = "PX",
            NotesText = "NT",
            CaseText = "CT",
            FormsText = "FT",
            DealText = "DT"
        };

        // Act
        var dump = note.Dump();

        // Assert
        Assert.Contains("ServerId: S1", dump);
        Assert.Contains("Companies: C1", dump);
        Assert.Contains("Dealership: D1", dump);
        Assert.Contains("ContactName: CN", dump);
        Assert.Contains("Email: E", dump);
        Assert.Contains("Phone: P", dump);
        Assert.Contains("PhoneExt: PX", dump);
        Assert.Contains("NotesText: NT", dump);
        Assert.Contains("CaseText: CT", dump);
        Assert.Contains("FormsText: FT", dump);
        Assert.Contains("DealText: DT", dump);
        Assert.Contains("Id: " + note.Id, dump);
    }
}