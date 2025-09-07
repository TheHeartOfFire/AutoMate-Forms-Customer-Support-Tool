using AMFormsCST.Core.Interfaces;
using AMFormsCST.Desktop.Interfaces;
using AMFormsCST.Desktop.Models;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace AMFormsCST.Desktop.Types;

public class ManagedObservableCollection<T> : ObservableCollection<T>, INotifyPropertyChanged, IManagedObservableCollection 
    where T : class, IManagedObservableCollectionItem
{
    private readonly Func<T> _blankFactory;
    private readonly Action<T>? _postCreationAction;
    private T? _selectedItem;
    private bool _isEnsuringBlank = false;
    private bool _suppressBlankEnforcement = false;
    private readonly ILogService? _logger;
    public event EventHandler<GuidEventArgs>? SelectionChanged;

    public new event PropertyChangedEventHandler? PropertyChanged;

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        PropertyChanged?.Invoke(this, e);
    }

    public T? SelectedItem
    {
        get => _selectedItem;
    }

    public ManagedObservableCollection(Func<T> blankFactory, IEnumerable<T>? initialItems = null, ILogService? logger = null, Action<T>? postCreationAction = null)
    {
        _logger = logger;
        if (blankFactory == null)
        {
            _logger?.LogFatal("ManagedObservableCollection initialization failed: blankFactory is null.");
            throw new ArgumentNullException(nameof(blankFactory));
        }
        _blankFactory = blankFactory;
        _postCreationAction = postCreationAction;
        CollectionChanged += OnCollectionChanged;
        _logger?.LogInfo($"ManagedObservableCollection<{typeof(T)}> initialized.");

        if (initialItems != null)
        {
            _suppressBlankEnforcement = true;
            foreach (var item in initialItems)
            {
                base.Add(item);
            }
            _suppressBlankEnforcement = false;
            EnsureSingleBlank();
            this.FirstOrDefault()?.Select();
        }
    }

    protected override void InsertItem(int index, T item)
    {
        base.InsertItem(index, item);
        item.PropertyChanged += OnItemPropertyChanged;
        item.Selected += OnSelectionChanged;
        
            item.OnAddedToCollection(this);
        if (!_suppressBlankEnforcement)
            EnsureSingleBlank();
        UpdateSelectionAfterChange();
        _logger?.LogDebug($"Item inserted at {index}: {item}");
    }

    private void OnSelectionChanged(object? sender, GuidEventArgs e)
    {
        _selectedItem = this.FirstOrDefault(i => i.Id == e.Value);
        OnPropertyChanged(new PropertyChangedEventArgs(nameof(SelectedItem)));
        SelectionChanged?.Invoke(this, e);
        _logger?.LogInfo($"SelectedItem changed: {e.Value}");
    }

    protected override void RemoveItem(int index)
    {
        var item = this[index];
        item.PropertyChanged -= OnItemPropertyChanged;
        item.Selected -= OnSelectionChanged;

        item.OnRemovedFromCollection();

        base.RemoveItem(index);
        EnsureSingleBlank();
        UpdateSelectionAfterChange(item);
        _logger?.LogDebug($"Item removed at {index}: {item}");
    }

    protected override void ClearItems()
    {
        foreach (var item in this)
            item.PropertyChanged -= OnItemPropertyChanged;
        base.ClearItems();
        EnsureSingleBlank();
        this.FirstOrDefault()?.Select();
        _logger?.LogInfo("Collection cleared.");
    }

    private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (!_suppressBlankEnforcement)
            EnsureSingleBlank();
        UpdateSelectionAfterChange();
        _logger?.LogDebug($"Collection changed: {e.Action}");
    }

    private void OnItemPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        _logger?.LogDebug($"PropertyChanged: {e.PropertyName} on {(sender is NoteModel nm ? nm.Id : sender)}");
        if (e.PropertyName == nameof(IBlankMaybe.IsBlank))
        {
            EnsureSingleBlank();
        }
    }


    private void EnsureSingleBlank()
    {
        if (_isEnsuringBlank) return;
        try
        {
            _isEnsuringBlank = true;
            var blankItems = this.Where(x => x.IsBlank).ToList();

            // Remove any extra blank items
            foreach (var extra in blankItems.Skip(1))
            {
                _logger?.LogDebug($"Removing extra blank: {extra}");
                base.Remove(extra);
            }

            var singleBlank = this.FirstOrDefault(x => x.IsBlank);

            // If no blank item exists, create and add one to the end
            if (singleBlank == null)
            {
                if (_blankFactory == null)
                {
                    _logger?.LogFatal("Attempted to create blank item but _blankFactory is null.");
                    throw new InvalidOperationException("Blank factory is null.");
                }
                var newBlank = _blankFactory();
                _postCreationAction?.Invoke(newBlank);
                _logger?.LogDebug("Adding new blank item to the end.");
                base.Add(newBlank);
            }
            // If a blank item exists but it's not the last one, move it to the end
            else if (this.LastOrDefault() != singleBlank)
            {
                _logger?.LogDebug($"Moving blank item to the end: {singleBlank}");
                base.Move(base.IndexOf(singleBlank), base.Count - 1);
            }
        }
        catch (Exception ex)
        {
            _logger?.LogError("Error in EnsureSingleBlank.", ex);
        }
        finally
        {
            _isEnsuringBlank = false;
        }
    }

    private void UpdateSelectionAfterChange(T? removedItem = null)
    {
        // Only update selection if selected item was removed
        if (removedItem != null && Equals(_selectedItem, removedItem))
        {
            // Select first item if available
            this.FirstOrDefault()?.Select();
        }
        // Do not override selection to first non-blank, just keep current selection if possible
        _logger?.LogDebug("Selection updated after collection change.");
    }
}

