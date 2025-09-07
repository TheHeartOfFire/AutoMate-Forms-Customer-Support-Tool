using AMFormsCST.Core.Interfaces;
using AMFormsCST.Core.Interfaces.Notebook;

namespace AMFormsCST.Core.Helpers;

public class SelectableList<T> : List<T> where T : class, INotebookItem<T>
{
    private readonly ILogService? _logger;

    public T? SelectedItem { get; set; }

    public SelectableList(ILogService? logger = null)
    {
        _logger = logger;
        _logger?.LogInfo($"SelectableList<{typeof(T).Name}> initialized.");
    }

    public SelectableList(IEnumerable<T> collection, ILogService? logger = null) : base(collection)
    {
        _logger = logger;
        _logger?.LogInfo($"SelectableList<{typeof(T).Name}> initialized with {collection.Count()} items.");
    }
}
