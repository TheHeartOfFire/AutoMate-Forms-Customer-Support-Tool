using AMFormsCST.Core.Helpers;
using AMFormsCST.Core.Interfaces.Notebook;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace AMFormsCST.Core.Types.Notebook;
public class Note : INote
{
    public string CaseText { get; set; } = string.Empty;
    public string NotesText { get; set; } = string.Empty;
    public SelectableList<IDealer> Dealers { get; set; } = [];
    public SelectableList<IContact> Contacts { get; set; } = [];
    public SelectableList<IForm> Forms { get; set; } = [];

    public Guid Id => _id;

    public Note() { }
    public Note(Guid id)
    {
        _id = id;
    }

    #region Interface Implementation
    private readonly Guid _id = Guid.NewGuid();
    public INote Clone() => new Note()
    {
        CaseText = CaseText,
        NotesText = NotesText,
        Dealers = [..Dealers.ConvertAll(d => d.Clone())],
        Contacts = [.. Contacts.ConvertAll(c => c.Clone())],
        Forms = [.. Forms.ConvertAll(f => f.Clone())],
    };
    public bool Equals(INote? other)
    {
        if (other == null) return false;
        return _id == other.Id;
    }
    public override bool Equals(object? obj)
    {
        if (obj is INote note)
            return Equals(note);
        return false;
    }
    public override int GetHashCode()
    {
        return _id.GetHashCode();
    }

    public bool Equals(INote? x, INote? y)
    {
        if (x is null || y is null) return false;
        return x.Id == y.Id;
    }

    public int GetHashCode([DisallowNull] INote obj) => obj.Id.GetHashCode();

    public string Dump() 
    {
        var sb = new StringBuilder();
        sb.AppendLine($"Note Dump:");
        sb.AppendLine($"Id: {_id}");
        sb.AppendLine($"CaseText: {CaseText}");
        sb.AppendLine($"NotesText: {NotesText}");
        sb.AppendLine($"Dealers: {string.Join(", ", Dealers.Select(d => d.Dump()))}");
        sb.AppendLine($"Contacts: {string.Join(", ", Contacts.Select(c => c.Dump()))}");
        sb.AppendLine($"Forms: {string.Join(", ", Forms.Select(f => f.Dump()))}");
        return sb.ToString();
    }
    #endregion

}
