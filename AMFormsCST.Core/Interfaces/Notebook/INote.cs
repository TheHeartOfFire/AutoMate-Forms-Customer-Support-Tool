using AMFormsCST.Core.Helpers;
using System.Diagnostics.CodeAnalysis;

namespace AMFormsCST.Core.Interfaces.Notebook;
public interface INote : INotable<INote>
{
    string CaseText { get; set; }
    string NotesText { get; set; }
    SelectableList<IDealer> Dealers { get; set; }
    SelectableList<IContact> Contacts { get; set; }
    SelectableList<IForm> Forms { get; set; }
}