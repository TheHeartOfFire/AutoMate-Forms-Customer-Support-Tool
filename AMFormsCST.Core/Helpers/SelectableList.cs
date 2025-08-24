using AMFormsCST.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFormsCST.Core.Helpers;
public class SelectableList<T> : List<T>
    where T : class
{
    private readonly ILogService? _logger;

    public T? SelectedItem { get; set; }

    public SelectableList(ILogService? logger = null) : base()
    {
        _logger = logger;
        _logger?.LogInfo("SelectableList initialized.");
    }

    public SelectableList(IEnumerable<T> collection, ILogService? logger = null) : base(collection)
    {
        _logger = logger;
        _logger?.LogInfo($"SelectableList initialized with {this.Count} items.");
    }

    public SelectableList(int capacity, ILogService? logger = null) : base(capacity)
    {
        _logger = logger;
        _logger?.LogInfo($"SelectableList initialized with capacity {capacity}.");
    }

    public void Select(T item)
    {
        if (Contains(item))
        {
            SelectedItem = item;
            _logger?.LogInfo($"Selected item: {item}");
        }
        else
        {
            var ex = new ArgumentException("Item not found in the list.", nameof(item));
            _logger?.LogError("Attempted to select an item not in the list.", ex);
            throw ex;
        }
    }
}
