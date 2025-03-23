using AMFormsCST.Core.Types.Notebook;

namespace AMFormsCST.Core.Interfaces.Utils;
public interface INotebook
{
    Note CurrentNote { get; }
    IList<Note> Notes { get; }

    void AddNote(bool select = false);
    void AddNote(Note note, bool select = false);
    void Clear();
    void RemoveNote(Note note);
    void SelectNote(Note note);
    void SwapNotes(Note note1, Note note2);
}