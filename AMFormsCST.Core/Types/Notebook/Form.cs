using AMFormsCST.Core.Helpers;
using AMFormsCST.Core.Interfaces.Notebook;
using AMFormsCST.Core.Interfaces;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json.Serialization;

namespace AMFormsCST.Core.Types.Notebook;
[JsonDerivedType(typeof(Form), typeDiscriminator: "form")]
public class Form : IForm
{
    private readonly ILogService? _logger;

    public string Name { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public SelectableList<ITestDeal> TestDeals { get; set; }
    public bool Notable { get; set; } = true;
    public Guid Id => _id;

    public Form() : this(null) { }
    public Form(ILogService? logger)
    {
        _logger = logger;
        TestDeals = new SelectableList<ITestDeal>(_logger);
        _logger?.LogInfo($"Form initialized. Id: {_id}");
    }
    public Form(Guid id, ILogService? logger = null) : this(logger)
    {
        _id = id;
        _logger?.LogInfo($"Form initialized with custom Id: {_id}");
    }

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
        _logger?.LogDebug($"Form Dump called for Id: {Id}");
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
        if (this is null)
        {
            var ex = new ArgumentNullException(nameof(IForm), "Cannot clone a null item.");
            _logger?.LogError("Attempted to clone a null Form.", ex);
            throw ex;
        }
        var clone = new Form(_logger)
        {
            Name = Name,
            Notes = Notes,
            TestDeals = new SelectableList<ITestDeal>(TestDeals.Select(td => td.Clone()), _logger),
            Notable = Notable
        };
        _logger?.LogInfo($"Form cloned. Original Id: {_id}, Clone Id: {clone.Id}");
        return clone;
    }
    #endregion
}
