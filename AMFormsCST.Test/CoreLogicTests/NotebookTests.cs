using AMFormsCST.Core.Types.Notebook;
using AMFormsCST.Core.Utils;
using Assert = Xunit.Assert;

namespace AMFormsCST.Test.CoreLogicTests;

public class NotebookTests
{
    [Fact]
    public void Constructor_InitializesWith1Note()
    {
        // Arrange & Act
        var notebook = new Notebook();

        // Assert
        Assert.NotNull(notebook.Notes);
        Assert.NotEmpty(notebook.Notes);
        Assert.NotNull(notebook.CurrentNote);
        Assert.Single(notebook.Notes);
    }

    [Fact]
    public void AddNote_WithSelectTrue_AddsAndSelectsNote()
    {
        // Arrange
        var notebook = new Notebook();
        var note = new Note();

        // Act
        notebook.AddNote(note, select: true);

        // Assert
        Assert.Equal(2, notebook.Notes.Count);
        Assert.Same(note, notebook.Notes[1]);
        Assert.Same(note, notebook.CurrentNote);
    }

    [Fact]
    public void AddNote_WithSelectFalse_AddsButDoesNotSelectNote()
    {
        // Arrange
        var notebook = new Notebook();
        var note = new Note();

        // Act
        notebook.AddNote(note, select: false);

        // Assert
        Assert.Equal(2, notebook.Notes.Count);
        Assert.False(notebook.CurrentNote.Id == note.Id);
    }

    [Fact]
    public void RemoveNote_RemovesTheCorrectNote()
    {
        // Arrange
        var notebook = new Notebook();
        var note1 = new Note();
        var note2 = new Note();
        notebook.AddNote(note1);
        notebook.AddNote(note2);

        // Act
        notebook.RemoveNote(note1);

        // Assert
        Assert.Equal(2, notebook.Notes.Count);
        Assert.DoesNotContain(note1, notebook.Notes);
        Assert.Contains(note2, notebook.Notes);
    }

    [Fact]
    public void RemoveNote_WhenSelectedNoteIsRemoved_CurrentNoteBecomesIndex0()
    {
        // Arrange
        var notebook = new Notebook();
        var note = new Note();
        notebook.AddNote(note, select: true);
        Assert.Same(note, notebook.CurrentNote);

        // Act
        notebook.RemoveNote(note);

        // Assert
        Assert.True(notebook.CurrentNote.Id == notebook.Notes[0].Id);
        Assert.False(notebook.CurrentNote.Id == note.Id);
    }

    [Fact]
    public void Clear_RemovesAllNotesAndResetsCurrentNote()
    {
        // Arrange
        var notebook = new Notebook();
        var note1 = new Note();
        var note2 = new Note();
        notebook.AddNote(note1, select: true);
        notebook.AddNote(note2);
        Assert.Equal(3, notebook.Notes.Count);
        Assert.NotNull(notebook.CurrentNote);

        // Act
        notebook.Clear();

        // Assert
        Assert.False(notebook.CurrentNote.Id == note1.Id || notebook.CurrentNote.Id == note2.Id);
    }

    [Fact]
    public void SelectNote_SetsCurrentNote()
    {
        // Arrange
        var notebook = new Notebook();
        var note1 = new Note();
        var note2 = new Note();
        notebook.AddNote(note1);
        notebook.AddNote(note2);

        // Act
        notebook.SelectNote(note2);

        // Assert
        Assert.Same(note2, notebook.CurrentNote);
    }
}