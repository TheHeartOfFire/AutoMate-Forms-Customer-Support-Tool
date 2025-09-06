using AMFormsCST.Core.Helpers;
using AMFormsCST.Core.Interfaces.Notebook;
using AMFormsCST.Core.Interfaces.Utils;
using AMFormsCST.Core.Types.Notebook;
using AMFormsCST.Core.Interfaces;

namespace AMFormsCST.Core.Utils;
public class Notebook : INotebook
{
    private readonly ILogService? _logger;

    public SelectableList<INote> Notes { get; set; }

    public Notebook(ILogService? logger = null)
    {
        _logger = logger;
        if (IO.Logger is null && logger is not null) IO.ConfigureLogger(logger);
        Notes = [.. IO.LoadNotes()];
        _logger?.LogInfo($"Notebook initialized with {Notes.Count} notes.");

        if (Notes.Count == 0)
        {
            Notes.Add(new Note(logger));
            _logger?.LogInfo("No notes found. Added a blank note.");
        }
        // Always select the first note
        Notes.SelectedItem = Notes.FirstOrDefault();
        _logger?.LogDebug($"Selected note: {Notes.SelectedItem?.Id}");

        if (Notes.SelectedItem is null) return;
        Notes.SelectedItem.Dealers.SelectedItem = Notes.SelectedItem.Dealers.FirstOrDefault();
        Notes.SelectedItem.Contacts.SelectedItem = Notes.SelectedItem.Contacts.FirstOrDefault();
        Notes.SelectedItem.Forms.SelectedItem = Notes.SelectedItem.Forms.FirstOrDefault();

        if (Notes.SelectedItem.Dealers.SelectedItem is null) return;
        Notes.SelectedItem.Dealers.SelectedItem.Companies.SelectedItem = Notes.SelectedItem.Dealers.SelectedItem.Companies.FirstOrDefault();
        if (Notes.SelectedItem.Forms.SelectedItem is null) return;
        Notes.SelectedItem.Forms.SelectedItem.TestDeals.SelectedItem = Notes.SelectedItem.Forms.SelectedItem.TestDeals.FirstOrDefault();
    }

    public void AddNote(bool select = false) => AddNote(new Note(_logger), select);

    public void AddNote(INote note, bool select = false)
    {
        Notes.Add(note);
        _logger?.LogInfo($"Note added: {note.Id}");

        if (select)
        {
            Notes.SelectedItem = note;
            _logger?.LogDebug($"Note selected: {note.Id}");
        }
    }

    public void RemoveNote(INote note)
    {
        ErrorHandler.Notes.NoteNotFoundErrorCheck(note, Notes);
        Notes.Remove(note);
        _logger?.LogInfo($"Note removed: {note.Id}");

        if (Notes.Count == 0)
        {
            Notes.SelectedItem = new Note();
            Notes.Add(Notes.SelectedItem);
            _logger?.LogInfo("All notes removed. Added a blank note.");
            return;
        }

        if (Notes.SelectedItem == note)
        {
            Notes.SelectedItem = Notes[0];
            _logger?.LogDebug($"Note selected after removal: {Notes.SelectedItem?.Id}");
        }
    }

    public void Clear()
    {
        Notes.Clear();
        Notes.SelectedItem = new Note(_logger);
        Notes.Add(Notes.SelectedItem);
        _logger?.LogInfo("Notebook cleared. Added a blank note.");
    }

    public void SelectNote(INote note)
    {
        ErrorHandler.Notes.NoteNotFoundErrorCheck(note, Notes);
        Notes.SelectedItem = note;
        _logger?.LogDebug($"Note selected: {note.Id}");
    }

    public void SwapNotes(INote note1, INote note2)
    {
        ErrorHandler.Notes.NotesNotFoundErrorCheck(note1, note2, Notes);

        var index1 = Notes.IndexOf(note1);
        var index2 = Notes.IndexOf(note2);
        Notes[index1] = note2;
        Notes[index2] = note1;
        _logger?.LogInfo($"Notes swapped: {note1.Id} <-> {note2.Id}");
    }

    internal void Load(SelectableList<INote> notes)
    {
        ErrorHandler.Notes.NoNotesErrorCheck(notes);

        Notes = notes;
        Notes.SelectedItem = Notes[0];
        _logger?.LogInfo($"Notebook loaded with {Notes.Count} notes. Selected: {Notes.SelectedItem?.Id}");
    }
}
