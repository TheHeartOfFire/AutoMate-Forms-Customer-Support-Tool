using AMFormsCST.Core.Interfaces.Notebook;
using AMFormsCST.Core.Interfaces.Utils;
using AMFormsCST.Core.Types.Notebook;

namespace AMFormsCST.Core.Utils;
public class Notebook : INotebook
{
    public IList<INote> Notes { get; set; } = IO.LoadNotes();
    public INote CurrentNote { get; set; }

    public Notebook()
    {
        
        CurrentNote = Notes.Count > 0 ? Notes[0] : new Note();
    }

    public void AddNote(bool select = false) => AddNote(new Note(), select);
    public void AddNote(INote note, bool select = false)
    {
        Notes.Add(note);

        if (select)
            CurrentNote = note;

    }

    public void RemoveNote(INote note)
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

    public void SelectNote(INote note)
    {
        ErrorHandler.Notes.NoteNotFoundErrorCheck(note, Notes);
        CurrentNote = note;
    }

    public void SwapNotes(INote note1, INote note2)
    {
        ErrorHandler.Notes.NotesNotFoundErrorCheck(note1, note2, Notes);

        var index1 = Notes.IndexOf(note1);
        var index2 = Notes.IndexOf(note2);
        Notes[index1] = note2;
        Notes[index2] = note1;
    }

    

    internal void Load(IList<INote> notes)
    {
        ErrorHandler.Notes.NoNotesErrorCheck(notes);

        Notes = notes;
        CurrentNote = Notes[0];
    }

    
}
