using AMFormsCST.Core.Interfaces.Notebook;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFormsCST.Core.Types.Notebook;
public class Company : ICompany
{
    public string Name { get; set; } = string.Empty;
    public string CompanyCode { get; set; } = string.Empty;
    public bool Notable { get; set; }
    public Guid Id => _id;
    public Company() { }
    public Company(Guid id) { _id = id; }

    #region Interface Implementation
    private readonly Guid _id = Guid.NewGuid();
    public string Dump()
    {
        var sb = new StringBuilder();
        sb.AppendLine($"Id: {Id}");
        sb.AppendLine($"Company: {Name}");
        sb.AppendLine($"Company Code: {CompanyCode}");
        return sb.ToString();
    }

    public bool Equals(ICompany? other)
    {
        if(other is null && this is null) return true;
        if (other is null || this is null) return false;
        return _id == other.Id;
    }

    public bool Equals(ICompany? x, ICompany? y)
    {
        if (x is null && y is null) return true;
        if (x is null || y is null) return false;
        return x.Equals(y);
    }

    public int GetHashCode([DisallowNull] ICompany obj) => obj.Id.GetHashCode();

    public ICompany Clone()
    {
        if (this is null) throw new ArgumentNullException(nameof(ICompany), "Cannot clone a null item.");
        return new Company
        {
            Name = Name,
            CompanyCode = CompanyCode
        };
    }
    #endregion
}
