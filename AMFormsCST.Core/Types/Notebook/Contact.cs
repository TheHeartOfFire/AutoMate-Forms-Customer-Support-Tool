using AMFormsCST.Core.Interfaces.Notebook;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFormsCST.Core.Types.Notebook;
public class Contact : IContact
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string PhoneExtension { get; set; } = string.Empty;
    public string PhoneExtensionDelimiter { get; set; } = " ";
    public Guid Id => _id;
    public Contact() { }
    public Contact(Guid id) { _id = id; }

    #region Interface Implementation
    private readonly Guid _id = Guid.NewGuid();

    public string Dump()
    {
        var sb = new StringBuilder();
        sb.AppendLine($"Id: {Id}");
        sb.AppendLine($"Name: {Name}");
        sb.AppendLine($"Email: {Email}");
        sb.AppendLine($"Phone: {Phone}");
        sb.AppendLine($"Phone Extension: {PhoneExtension}");
        sb.AppendLine($"Phone Extension Delimiter: {PhoneExtensionDelimiter}");
        return sb.ToString();
    }

    public bool Equals(IContact? other)
    {
        if (other is null && this is null) return true;
        if (other is null || this is null) return false;
        return _id == other.Id;
    }

    public bool Equals(IContact? x, IContact? y)
    {
        if (x is null && y is null) return true;
        if (x is null || y is null) return false;
        return x.Equals(y);
    }

    public int GetHashCode([DisallowNull] IContact obj) => obj.Id.GetHashCode();

    public IContact Clone()
    {
        if (this is null) throw new ArgumentNullException(nameof(IContact), "Cannot clone a null item.");
        return new Contact
        {
            Name = Name,
            Email = Email,
            Phone = Phone,
            PhoneExtension = PhoneExtension,
            PhoneExtensionDelimiter = PhoneExtensionDelimiter
        };
    }
    #endregion
}
