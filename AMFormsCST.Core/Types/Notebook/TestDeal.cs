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
[JsonDerivedType(typeof(TestDeal), typeDiscriminator: "testdeal")]
public class TestDeal : ITestDeal
{
    private readonly ILogService? _logger;

    public string DealNumber { get; set; } = string.Empty;
    public string Purpose { get; set; } = string.Empty;

    public Guid Id => _id;

    public TestDeal() : this(null) { }
    public TestDeal(ILogService? logger)
    {
        _logger = logger;
        _logger?.LogInfo($"TestDeal initialized. Id: {_id}");
    }
    public TestDeal(Guid id, ILogService? logger = null) : this(logger)
    {
        _id = id;
        _logger?.LogInfo($"TestDeal initialized with custom Id: {_id}");
    }

    #region Interface Implementation
    private readonly Guid _id = Guid.NewGuid();

    public ITestDeal Clone()
    {
        if (this is null)
        {
            var ex = new ArgumentNullException(nameof(ITestDeal), "Cannot clone a null item.");
            _logger?.LogError("Attempted to clone a null TestDeal.", ex);
            throw ex;
        }
        var clone = new TestDeal(_logger)
        {
            DealNumber = DealNumber,
            Purpose = Purpose
        };
        _logger?.LogInfo($"TestDeal cloned. Original Id: {_id}, Clone Id: {clone.Id}");
        return clone;
    }

    public string Dump()
    {
        var sb = new StringBuilder();
        sb.AppendLine($"Id: {Id}");
        sb.AppendLine($"Deal Number: {DealNumber}");
        sb.AppendLine($"Purpose: {Purpose}");
        _logger?.LogDebug($"TestDeal Dump called for Id: {Id}");
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
