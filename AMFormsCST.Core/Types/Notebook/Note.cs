using AMFormsCST.Core.Interfaces.Notebook;
using System.Diagnostics.CodeAnalysis;

namespace AMFormsCST.Core.Types.Notebook;
public class Note : INote
{
    public string? ServerId { get; set; }
    public string? Companies { get; set; }
    public string? Dealership { get; set; }
    public string? ContactName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? PhoneExt { get; set; }
    public string? NotesText { get; set; }
    public string? CaseText { get; set; }
    public string? FormsText { get; set; }
    public string? DealText { get; set; }

    public Guid _id = Guid.NewGuid();
    public Guid Id => _id;

    public static Note Clone(Note note) => new()
    {
        ServerId = note.ServerId,
        Companies = note.Companies,
        Dealership = note.Dealership,
        ContactName = note.ContactName,
        Email = note.Email,
        Phone = note.Phone,
        PhoneExt = note.PhoneExt,
        NotesText = note.NotesText,
        CaseText = note.CaseText,
        FormsText = note.FormsText,
        DealText = note.DealText
    };


    public Note()
    {
        
    }
    public Note(Guid id)
    {
        _id = id;
    }

    #region Interface Implementation
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

    public string Dump() =>
        $"ServerId: {ServerId}\n" +
        $"Companies: {Companies}\n" +
        $"Dealership: {Dealership}\n" +
        $"ContactName: {ContactName}\n" +
        $"Email: {Email}\n" +
        $"Phone: {Phone}\n" +
        $"PhoneExt: {PhoneExt}\n" +
        $"NotesText: {NotesText}\n" +
        $"CaseText: {CaseText}\n" +
        $"FormsText: {FormsText}\n" +
        $"DealText: {DealText}\n" +
        $"Id: {_id}";
    #endregion

}
