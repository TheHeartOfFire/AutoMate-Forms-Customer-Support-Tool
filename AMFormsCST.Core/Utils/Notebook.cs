using AMFormsCST.Core.Interfaces.Utils;
using AMFormsCST.Core.Types.Notebook;

namespace AMFormsCST.Core.Utils;
public class Notebook : INotebook
{
    public IList<Note> Notes { get; private set; } = [new Note()];
    public Note CurrentNote { get; private set; }

    public Notebook()
    {
        CurrentNote = Notes[0];
    }

    public void AddNote(bool select = false) => AddNote(new Note(), select);
    public void AddNote(Note note, bool select = false)
    {
        Notes.Add(note);

        if (select)
            CurrentNote = note;

    }

    public void RemoveNote(Note note)
    {
        ErrorHandler.Notes.NoteNotFoundErrorCheck(note, Notes);
        Notes.Remove(note);

        if (Notes.Count == 0)
        {
            CurrentNote = new Note();
            Notes.Add(CurrentNote);
            return;
        }

        if (CurrentNote == note)
            CurrentNote = Notes[0];
    }

    

    public void Clear()
    {
        Notes.Clear();
        CurrentNote = new Note();
        Notes.Add(CurrentNote);
    }

    public void SelectNote(Note note)
    {
        ErrorHandler.Notes.NoteNotFoundErrorCheck(note, Notes);
        CurrentNote = note;
    }

    public void SwapNotes(Note note1, Note note2)
    {
        ErrorHandler.Notes.NotesNotFoundErrorCheck(note1, note2, Notes);

        var index1 = Notes.IndexOf(note1);
        var index2 = Notes.IndexOf(note2);
        Notes[index1] = note2;
        Notes[index2] = note1;
    }

    

    internal void Load(List<Note> notes)
    {
        ErrorHandler.Notes.NoNotesErrorCheck(notes);

        Notes = notes;
        CurrentNote = Notes[0];
    }

    
}
