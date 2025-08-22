using AMFormsCST.Core.Helpers;
using AMFormsCST.Core.Interfaces.Notebook;
using AMFormsCST.Core.Interfaces.Utils;
using AMFormsCST.Core.Types.Notebook;

namespace AMFormsCST.Core.Utils;
public class Notebook : INotebook
{
    public SelectableList<INote> Notes { get; set; } = [.. IO.LoadNotes()];

    public Notebook()
    {
        if (Notes.Count == 0) Notes.Add(new Note());
        Notes.SelectedItem = Notes.FirstOrDefault();
    }

    public void AddNote(bool select = false) => AddNote(new Note(), select);
    public void AddNote(INote note, bool select = false)
    {
        Notes.Add(note);

        if (select)
            Notes.SelectedItem = note;

    }

    public void RemoveNote(INote note)
    {
        ErrorHandler.Notes.NoteNotFoundErrorCheck(note, Notes);
        Notes.Remove(note);

        if (Notes.Count == 0)
        {
            Notes.SelectedItem = new Note();
            Notes.Add(Notes.SelectedItem);
            return;
        }

        if (Notes.SelectedItem == note)
            Notes.SelectedItem = Notes[0];
    }

    

    public void Clear()
    {
        Notes.Clear();
        Notes.SelectedItem = new Note();
        Notes.Add(Notes.SelectedItem);
    }

    public void SelectNote(INote note)
    {
        ErrorHandler.Notes.NoteNotFoundErrorCheck(note, Notes);
        Notes.SelectedItem = note;
    }

    public void SwapNotes(INote note1, INote note2)
    {
        ErrorHandler.Notes.NotesNotFoundErrorCheck(note1, note2, Notes);

        var index1 = Notes.IndexOf(note1);
        var index2 = Notes.IndexOf(note2);
        Notes[index1] = note2;
        Notes[index2] = note1;
    }

    

    internal void Load(SelectableList<INote> notes)
    {
        ErrorHandler.Notes.NoNotesErrorCheck(notes);

        Notes = notes;
        Notes.SelectedItem = Notes[0];
    }

    
}
