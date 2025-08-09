using AMFormsCST.Core.Types.Notebook;
using System.Diagnostics.CodeAnalysis;

namespace AMFormsCST.Core.Interfaces.Notebook;
public interface INote : IEquatable<INote>, IEqualityComparer<INote>
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
    string? PhoneExt { get; set; }
    string? ServerId { get; set; }
    Guid Id { get; }

    new bool Equals(INote? other);
    new bool Equals(INote? x, INote? y);
    bool Equals(object? obj);
    int GetHashCode();
    new int GetHashCode([DisallowNull] INote obj);
    string Dump();
}