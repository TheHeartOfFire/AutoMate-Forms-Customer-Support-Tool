using AMFormsCST.Core.Helpers;
using AMFormsCST.Core.Interfaces.Notebook;
using AMFormsCST.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AMFormsCST.Core.Types.Notebook;
[JsonDerivedType(typeof(Dealer), typeDiscriminator: "dealer")]
public class Dealer : IDealer
{
    private readonly ILogService? _logger;

    public string Name { get; set; } = string.Empty;
    public string ServerCode { get; set; } = string.Empty;
    public SelectableList<ICompany> Companies { get; set; }
    public bool Notable { get; set; }
    public Guid Id => _id;

    public Dealer() : this(null) { }
    public Dealer(ILogService? logger)
    {
        _logger = logger;
        Companies = new SelectableList<ICompany>(_logger);
        _logger?.LogInfo($"Dealer initialized. Id: {_id}");
    }
    public Dealer(Guid id, ILogService? logger = null) : this(logger)
    {
        _id = id;
        _logger?.LogInfo($"Dealer initialized with custom Id: {_id}");
    }

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
        _logger?.LogDebug($"Dealer Dump called for Id: {Id}");
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
        if (this is null)
        {
            var ex = new ArgumentNullException(nameof(IDealer), "Cannot clone a null item.");
            _logger?.LogError("Attempted to clone a null Dealer.", ex);
            throw ex;
        }
        var clone = new Dealer(_logger)
        {
            Name = Name,
            ServerCode = ServerCode,
            Companies = new SelectableList<ICompany>(Companies.Select(c => (Company)c.Clone()), _logger),
            Notable = Notable
        };
        _logger?.LogInfo($"Dealer cloned. Original Id: {_id}, Clone Id: {clone.Id}");
        return clone;
    }
    #endregion
}
