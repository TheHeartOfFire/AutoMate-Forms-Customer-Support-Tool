using AMFormsCST.Core.Interfaces;
using AMFormsCST.Desktop.Interfaces;
using AMFormsCST.Desktop.Models;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace AMFormsCST.Desktop.Types;

public class ManagedObservableCollection<T> : ObservableCollection<T>, INotifyPropertyChanged
    where T : class, IBlankMaybe, INotifyPropertyChanged
{
    private readonly Func<T> _blankFactory;
    private T? _selectedItem;
    private bool _isEnsuringBlank = false;
    private readonly ILogService? _logger;

    public T? SelectedItem
    {
        get => _selectedItem;
        set
        {
            if (!Equals(_selectedItem, value))
            {
                _selectedItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(SelectedItem)));
                _logger?.LogInfo($"SelectedItem changed: {value}");
            }
        }
    }

    public ManagedObservableCollection(Func<T> blankFactory, ILogService? logger = null)
    {
        _logger = logger;
        if (blankFactory == null)
        {
            _logger?.LogFatal("ManagedObservableCollection initialization failed: blankFactory is null.");
            throw new ArgumentNullException(nameof(blankFactory));
        }
        _blankFactory = blankFactory;
        CollectionChanged += OnCollectionChanged;
        _logger?.LogInfo($"ManagedObservableCollection<{typeof(T)}> initialized.");
    }

    protected override void InsertItem(int index, T item)
    {
        base.InsertItem(index, item);
        item.PropertyChanged += OnItemPropertyChanged;
        EnsureSingleBlank();
        UpdateSelectionAfterChange();
        _logger?.LogDebug($"Item inserted at {index}: {item}");
    }

    protected override void RemoveItem(int index)
    {
        var item = this[index];
        item.PropertyChanged -= OnItemPropertyChanged;
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
        SelectedItem = this.FirstOrDefault(x => !x.IsBlank);
        _logger?.LogInfo("Collection cleared.");
    }

    private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        EnsureSingleBlank();
        UpdateSelectionAfterChange();
        _logger?.LogDebug($"Collection changed: {e.Action}");
    }

    private void OnItemPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        _logger?.LogDebug($"PropertyChanged: {e.PropertyName} on {(sender is NoteModel nm ? nm.Id : sender)}");
        if (e.PropertyName == nameof(IBlankMaybe.IsBlank))
            EnsureSingleBlank();
    }

    private void EnsureSingleBlank()
    {
        if (_isEnsuringBlank) return;
        try
        {
            _isEnsuringBlank = true;
            var blankItems = this.Where(x => x.IsBlank).ToList();
            foreach (var extra in blankItems.Skip(1).ToList())
            {
                _logger?.LogDebug($"Removing extra blank: {(extra is NoteModel nm ? nm.Id : extra)}");
                base.Remove(extra);
            }
            if (!blankItems.Any())
            {
                if (_blankFactory == null)
                {
                    _logger?.LogFatal("Attempted to create blank item but _blankFactory is null.");
                    throw new InvalidOperationException("Blank factory is null.");
                }
                var newBlank = _blankFactory();
                _logger?.LogDebug("Adding blank item");
                base.Add(newBlank);
            }
            else
            {
                var blank = blankItems.First();
                var idx = IndexOf(blank);
                if (idx != Count - 1)
                {
                    _logger?.LogDebug($"Moving blank item from {idx} to {Count - 1}");
                    base.Move(idx, Count - 1);
                }
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
        // If selected item was removed or is blank, select first non-blank item
        if (removedItem != null && Equals(_selectedItem, removedItem))
            SelectedItem = this.FirstOrDefault(x => !x.IsBlank);
        else if (_selectedItem == null || !_selectedItem.IsBlank && !this.Contains(_selectedItem))
            SelectedItem = this.FirstOrDefault(x => !x.IsBlank);
        else if (_selectedItem != null && _selectedItem.IsBlank)
            SelectedItem = this.FirstOrDefault(x => !x.IsBlank);

        _logger?.LogDebug("Selection updated after collection change.");
    }
}

