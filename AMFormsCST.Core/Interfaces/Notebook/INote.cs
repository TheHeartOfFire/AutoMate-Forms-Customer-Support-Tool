using AMFormsCST.Core.Types.Notebook;
using System.Diagnostics.CodeAnalysis;

namespace AMFormsCST.Core.Interfaces.Notebook;
public interface INote : IEquatable<Note>, IEqualityComparer<Note>
{
    string? CaseText { get; set; }
    string? Companies { get; set; }
    string? ContactName { get; set; }
    string? Dealership { get; set; }
    string? DealText { get; set; }
    string? Email { get; set; }
    string? FormsText { get; set; }
    string? NotesText { get; set; }
    string? Phone { get; set; }
    string? ServerId { get; set; }

    new bool Equals(Note? other);
    new bool Equals(Note? x, Note? y);
    bool Equals(object? obj);
    int GetHashCode();
    new int GetHashCode([DisallowNull] Note obj);
}