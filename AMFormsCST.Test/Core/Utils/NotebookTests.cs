using AMFormsCST.Core.Types.Notebook;
using AMFormsCST.Core.Utils;
using System;
using System.IO;
using Xunit;
using Assert = Xunit.Assert;

namespace AMFormsCST.Test.Core.Utils;

public class NotebookTests : IDisposable
{
    private static readonly string _notesPath;

    // Static constructor to determine the path once for all tests in this class.
    static NotebookTests()
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var rootPath = Path.Combine(appData, "Solera Case Management Tool");
        _notesPath = Path.Combine(rootPath, "SavedNotes.json");
    }

    // Instance constructor runs before each test, ensuring a clean state.
    public NotebookTests()
    {
        if (File.Exists(_notesPath))
        {
            File.Delete(_notesPath);
        }
    }

    // Dispose runs after each test, cleaning up the created file.
    public void Dispose()
    {
        if (File.Exists(_notesPath))
        {
            File.Delete(_notesPath);
        }
        GC.SuppressFinalize(this);
    }

    [Fact]
    public void Constructor_InitializesWith1Note()
    {
        // Arrange & Act
        var notebook = new Notebook();

        // Assert
        Assert.NotNull(notebook.Notes);
        Assert.NotEmpty(notebook.Notes);
        Assert.NotNull(notebook.Notes.SelectedItem);
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
        Assert.Same(note, notebook.Notes.SelectedItem);
    }

    [Fact]
    public void AddNote_WithSelectFalse_AddsButDoesNotSelectNote()
    {
        // Arrange
        var notebook = new Notebook();
        var originalSelected = notebook.Notes.SelectedItem;
        var note = new Note();

        // Act
        notebook.AddNote(note, select: false);

        // Assert
        Assert.Equal(2, notebook.Notes.Count);
        Assert.Same(originalSelected, notebook.Notes.SelectedItem);
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
        var firstNote = notebook.Notes[0];
        var noteToRemove = new Note();
        notebook.AddNote(noteToRemove, select: true);
        Assert.Same(noteToRemove, notebook.Notes.SelectedItem);

        // Act
        notebook.RemoveNote(noteToRemove);

        // Assert
        Assert.Same(firstNote, notebook.Notes.SelectedItem);
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
        Assert.NotNull(notebook.Notes.SelectedItem);

        // Act
        notebook.Clear();

        // Assert
        Assert.Single(notebook.Notes);
        Assert.NotNull(notebook.Notes.SelectedItem);
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
        Assert.Same(note2, notebook.Notes.SelectedItem);
    }
}