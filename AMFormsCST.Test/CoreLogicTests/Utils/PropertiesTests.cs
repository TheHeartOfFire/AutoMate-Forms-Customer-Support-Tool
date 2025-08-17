using AMFormsCST.Core.Utils;
using Xunit;

public class PropertiesTests
{
    [Fact]
    public void Constructor_InitializesNestedProperties()
    {
        // Act
        var props = new Properties();

        // Assert
        Assert.NotNull(props.FormgenUtils);
        Assert.NotNull(props.ErrorMessages);
        Assert.NotNull(props.ErrorMessages.Notes);
    }

    [Fact]
    public void FormgenUtilsProperties_DefaultsToZero()
    {
        // Arrange
        var props = new Properties();

        // Assert
        Assert.Equal((uint)0, props.FormgenUtils.BackupRetentionQty);
    }

    [Fact]
    public void ErrorMessages_Notes_DefaultMessages_AreSet()
    {
        // Arrange
        var props = new Properties();
        var notes = props.ErrorMessages.Notes;

        // Assert
        Assert.Equal("There is no {0} in {1} that matches the {0}(s) provided.", notes.NoNoteInListMessage);
        Assert.Equal("{0} is missing:\n{1}", notes.NoteMissingMessage);
        Assert.Equal("Both notes are missing:\n{0}\n{1}", notes.BothNotesMissingMessage);
        Assert.Equal("There are no {0}s in {1} to load.", notes.EmptyListMessage);
    }

    [Fact]
    public void Properties_CanBeSetAndGet()
    {
        // Arrange
        var props = new Properties();
        var newFormgenUtils = new FormgenUtilsProperties { BackupRetentionQty = 5 };
        var newErrorMessages = new ErrorMessages
        {
            Notes = new NotesErrorMessages
            {
                NoNoteInListMessage = "Custom1",
                NoteMissingMessage = "Custom2",
                BothNotesMissingMessage = "Custom3",
                EmptyListMessage = "Custom4"
            }
        };

        // Act
        props.FormgenUtils = newFormgenUtils;
        props.ErrorMessages = newErrorMessages;

        // Assert
        Assert.Same(newFormgenUtils, props.FormgenUtils);
        Assert.Same(newErrorMessages, props.ErrorMessages);
        Assert.Equal("Custom1", props.ErrorMessages.Notes.NoNoteInListMessage);
        Assert.Equal((uint)5, props.FormgenUtils.BackupRetentionQty);
    }
}