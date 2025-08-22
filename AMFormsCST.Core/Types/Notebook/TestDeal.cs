using AMFormsCST.Core.Interfaces.Notebook;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFormsCST.Core.Types.Notebook;
public class TestDeal : ITestDeal
{
    public string DealNumber { get; set; } = string.Empty;
    public string Purpose { get; set; } = string.Empty;

    public Guid Id => _id;
    public TestDeal() { }
    public TestDeal(Guid id) { _id = id; }

    #region Interface Implementation
    private readonly Guid _id = Guid.NewGuid();

    public ITestDeal Clone()
    {
        if (this is null) throw new ArgumentNullException(nameof(ITestDeal), "Cannot clone a null item.");
        return new TestDeal
        {
            DealNumber = DealNumber,
            Purpose = Purpose
        };
    }

    public string Dump()
    {
        var sb = new StringBuilder();
        sb.AppendLine($"Id: {Id}");
        sb.AppendLine($"Deal Number: {DealNumber}");
        sb.AppendLine($"Purpose: {Purpose}");
        return sb.ToString();
    }

    public bool Equals(ITestDeal? other)
    {
        if (other is null && this is null) return true;
        if (other is null || this is null) return false;
        return _id == other.Id;
    }

    public bool Equals(ITestDeal? x, ITestDeal? y)
    {
        if (x is null && y is null) return true;
        if (x is null || y is null) return false;
        return x.Equals(y);
    }

    public int GetHashCode([DisallowNull] ITestDeal obj) => obj.Id.GetHashCode();
    #endregion
}
