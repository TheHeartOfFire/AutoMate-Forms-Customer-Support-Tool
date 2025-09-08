using AMFormsCST.Core.Interfaces.Notebook;
using AMFormsCST.Core.Interfaces;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json.Serialization;

namespace AMFormsCST.Core.Types.Notebook;
[JsonDerivedType(typeof(Company), typeDiscriminator: "company")]
public class Company : ICompany
{
    private readonly ILogService? _logger;

    public string Name { get; set; } = string.Empty;
    public string CompanyCode { get; set; } = string.Empty;
    public bool Notable { get; set; }
    public Guid Id => _id;

    public Company() : this(null) { }
    public Company(ILogService? logger)
    {
        _logger = logger;
        _logger?.LogInfo($"Company initialized. Id: {_id}");
    }
    public Company(Guid id, ILogService? logger = null) : this(logger)
    {
        _id = id;
        _logger?.LogInfo($"Company initialized with custom Id: {_id}");
    }

    #region Interface Implementation
    private readonly Guid _id = Guid.NewGuid();
    public string Dump()
    {
        var sb = new StringBuilder();
        sb.AppendLine($"Id: {Id}");
        sb.AppendLine($"Company: {Name}");
        sb.AppendLine($"Company Code: {CompanyCode}");
        _logger?.LogDebug($"Company Dump called for Id: {Id}");
        return sb.ToString();
    }

    public bool Equals(ICompany? other)
    {
        if (other is null && this is null) return true;
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
        if (this is null)
        {
            var ex = new ArgumentNullException(nameof(ICompany), "Cannot clone a null item.");
            _logger?.LogError("Attempted to clone a null Company.", ex);
            throw ex;
        }
        var clone = new Company(_logger)
        {
            Name = Name,
            CompanyCode = CompanyCode,
            Notable = Notable
        };
        _logger?.LogInfo($"Company cloned. Original Id: {_id}, Clone Id: {clone.Id}");
        return clone;
    }
    #endregion
}
