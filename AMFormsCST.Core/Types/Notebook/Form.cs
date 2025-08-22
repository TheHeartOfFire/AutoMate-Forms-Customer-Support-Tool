using AMFormsCST.Core.Helpers;
using AMFormsCST.Core.Interfaces.Notebook;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFormsCST.Core.Types.Notebook;
public class Form : IForm
{
    public string Name { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public SelectableList<ITestDeal> TestDeals { get; set; } = [];
    public bool Notable { get; set; } = true;
    public Guid Id => _id;
    public Form() { }
    public Form(Guid id) { _id = id; }
    #region Interface Implementation
    private readonly Guid _id = Guid.NewGuid();

    public string Dump()
    {
        var sb = new StringBuilder();
        sb.AppendLine($"Id: {Id}");
        sb.AppendLine($"Form Name: {Name}");
        sb.AppendLine($"Notes: {Notes}");
        sb.AppendLine("Test Deals:");
        foreach (var testDeal in TestDeals)
        {
            sb.AppendLine($"- {testDeal.Dump()}");
        }
        return sb.ToString();
    }

    public bool Equals(IForm? other)
    {
        if (other is null && this is null) return true;
        if (other is null || this is null) return false;
        return _id == other.Id;
    }

    public bool Equals(IForm? x, IForm? y)
    {
        if (x is null && y is null) return true;
        if (x is null || y is null) return false;
        return x.Equals(y);
    }

    public int GetHashCode([DisallowNull] IForm obj) => obj.Id.GetHashCode();

    public IForm Clone()
    {
        if (this is null) throw new ArgumentNullException(nameof(IForm), "Cannot clone a null item.");
        return new Form
        {
            Name = Name,
            Notes = Notes,
            TestDeals = [..TestDeals.ConvertAll(td => td.Clone())],
            Notable = Notable
        };
    }
    #endregion
}
