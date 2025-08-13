using AMFormsCST.Core.Interfaces.Notebook;
using AMFormsCST.Core.Utils;
using Moq;
using System;
using System.Collections.Generic;
using Assert = Xunit.Assert;

namespace AMFormsCST.Test.CoreLogicTests;

public class ErrorHandlerTests
{
    [Fact]
    public void NoteNotFoundErrorCheck_WhenNoteIsMissing_ThrowsNullReferenceException()
    {
        // Arrange
        var noteToFind = new Mock<INote>();
        noteToFind.Setup(n => n.Dump()).Returns("Note Dump"); // Mock the Dump method for the error message
        var listToSearch = new List<INote>(); // An empty list guarantees the note is not found

        // Act & Assert
        var ex = Assert.Throws<NullReferenceException>(() => ErrorHandler.Notes.NoteNotFoundErrorCheck(noteToFind.Object, listToSearch));
        
        // Verify the outer and inner exception messages
        Assert.Contains("that matches the INote(s) provided", ex.Message);
        Assert.NotNull(ex.InnerException);
        Assert.Contains("is missing", ex.InnerException.Message);
        Assert.Contains("Note Dump", ex.InnerException.Message);
    }

    [Fact]
    public void NoteNotFoundErrorCheck_WhenNoteExists_DoesNotThrow()
    {
        // Arrange
        var noteToFind = new Mock<INote>();
        var listToSearch = new List<INote> { noteToFind.Object };

        // Act
        var exception = Record.Exception(() => ErrorHandler.Notes.NoteNotFoundErrorCheck(noteToFind.Object, listToSearch));

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void NoNotesErrorCheck_WhenListIsEmpty_ThrowsNullReferenceException()
    {
        // Arrange
        var emptyList = new List<INote>();

        // Act & Assert
        var ex = Assert.Throws<NullReferenceException>(() => ErrorHandler.Notes.NoNotesErrorCheck(emptyList));
        Assert.Contains("There are no INotes in notes to load", ex.Message);
    }

    [Fact]
    public void NoNotesErrorCheck_WhenListIsNotEmpty_DoesNotThrow()
    {
        // Arrange
        var listWithNote = new List<INote> { new Mock<INote>().Object };

        // Act
        var exception = Record.Exception(() => ErrorHandler.Notes.NoNotesErrorCheck(listWithNote));

        // Assert
        Assert.Null(exception);
    }
}