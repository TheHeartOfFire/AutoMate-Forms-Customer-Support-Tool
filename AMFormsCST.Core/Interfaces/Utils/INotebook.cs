using AMFormsCST.Core.Helpers;
using AMFormsCST.Core.Interfaces.Notebook;
using AMFormsCST.Core.Types.Notebook;

namespace AMFormsCST.Core.Interfaces.Utils;
public interface INotebook
{
    SelectableList<INote> Notes { get; set; }
    void AddNote(bool select = false);
    void AddNote(INote note, bool select = false);
    void Clear();
    void RemoveNote(INote note);
    void SelectNote(INote note);
    void SwapNotes(INote note1, INote note2);
}