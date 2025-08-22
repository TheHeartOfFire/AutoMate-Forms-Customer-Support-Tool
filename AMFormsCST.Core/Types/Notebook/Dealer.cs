using AMFormsCST.Core.Helpers;
using AMFormsCST.Core.Interfaces.Notebook;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFormsCST.Core.Types.Notebook;
public class Dealer : IDealer
{
    public string Name { get; set; } = string.Empty;
    public string ServerCode { get; set; } = string.Empty;
    public SelectableList<ICompany> Companies { get; set; } = [];
    public bool Notable { get; set; }
    public Guid Id => _id;
    public Dealer() { }
    public Dealer(Guid id) { _id = id; }

    #region Interface Implementation
    private readonly Guid _id = Guid.NewGuid();

    public string Dump()
    {
        var sb = new StringBuilder();
        sb.AppendLine($"Id: {Id}");
        sb.AppendLine($"Dealer: {Name}");
        sb.AppendLine($"Server Code: {ServerCode}");
        sb.AppendLine("Companies:");
        foreach (var company in Companies)
        {
            sb.AppendLine($"- {company.Dump()}");
        }
        return sb.ToString();
    }

    public bool Equals(IDealer? other)
    {
        if (other is null && this is null) return true;
        if (other is null || this is null) return false;
        return _id == other.Id;
    }

    public bool Equals(IDealer? x, IDealer? y)
    {
        if (x is null && y is null) return true;
        if (x is null || y is null) return false;
        return x.Equals(y);
    }

    public int GetHashCode([DisallowNull] IDealer obj) => obj.Id.GetHashCode();

    public IDealer Clone()
    {
        if (this is null) throw new ArgumentNullException(nameof(IDealer), "Cannot clone a null item.");
        return new Dealer
        {
            Name = Name,
            ServerCode = ServerCode,
            Companies = [..Companies.ConvertAll(c => (Company)c.Clone())],
            Notable = Notable

        };
    }
    #endregion
}
