using AMFormsCST.Desktop.Interfaces;
using AMFormsCST.Desktop.ViewModels;
using AMFormsCST.Desktop.ViewModels.Pages;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFormsCST.Desktop.Models;
public partial class Form : ObservableObject, ISelectable, IBlankMaybe
{
    [ObservableProperty]
    private string? _name = string.Empty;
    [ObservableProperty]
    private string? _notes = string.Empty;
    public ObservableCollection<TestDeal> TestDeals { get; set; } = [ ];
    private TestDeal? _selectedTestDeal;
    public TestDeal? SelectedTestDeal
    {
        get => _selectedTestDeal;
        set
        {
            // 1. Unsubscribe from the old TestDeal's events
            if (_selectedTestDeal != null)
            {
                // Ensure TestDeal's PropertyChanged is unsubscribed from the ViewModel
                // This is crucial for avoiding memory leaks and multiple subscriptions
                _selectedTestDeal.PropertyChanged -= _viewModel.OnModelPropertyChanged;
            }

            SetProperty(ref _selectedTestDeal, value);

            // 2. Subscribe to the new TestDeal's events
            if (_selectedTestDeal != null)
            {
                // Ensure TestDeal's PropertyChanged is subscribed to the ViewModel
                _selectedTestDeal.PropertyChanged += _viewModel.OnModelPropertyChanged;
            }
        }
    }
    private readonly DashboardViewModel _viewModel;
    [ObservableProperty]
    private bool _notable = true;
    [ObservableProperty]
    private FormFormat _format = FormFormat.Pdf;

    public enum FormFormat
    {
        LegacyImpact,
        Pdf
    }
    public Guid Id { get; } = Guid.NewGuid();
    [ObservableProperty]
    private bool _isSelected  = false;
    public void Select()
    {
        IsSelected = true;
    }
    public void Deselect()
    {
        IsSelected = false;
    }
    public bool IsBlank
    {
        get
        {
            if (!string.IsNullOrEmpty(Name) || !string.IsNullOrEmpty(Notes))
            {
                return false;
            }

            if (TestDeals.Any(td => !td.IsBlank))
            {
                return false;
            }
            return true;
        }
    }
    partial void OnNameChanged(string? value)
    {
        OnPropertyChanged(nameof(IsBlank));
    }

    partial void OnNotesChanged(string? value)
    {
        OnPropertyChanged(nameof(IsBlank));
    }
    public Form(DashboardViewModel viewModel)
    {
        _viewModel = viewModel;

        TestDeals = new ObservableCollection<TestDeal>();
        TestDeals.CollectionChanged += ChildCollection_CollectionChanged;
        TestDeals.Add(new TestDeal()); 

        SelectTestDeal(TestDeals.FirstOrDefault());

        SubscribeToInitialChildren();
    }

    public void SelectTestDeal(ISelectable selectedTestDeal)
    {
        if (selectedTestDeal is null) return; 
        
        foreach (var deal in TestDeals) deal.Deselect();

        SelectedTestDeal = TestDeals.FirstOrDefault(c => c.Id == selectedTestDeal.Id);
        SelectedTestDeal?.Select();
    }
    private void ChildCollection_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        if (e.OldItems != null)
        {
            foreach (var item in e.OldItems) 
            {
                if (item is System.ComponentModel.INotifyPropertyChanged notifyItem) 
                {
                    notifyItem.PropertyChanged -= ChildItem_IsBlankChanged;
                }
            }
        }
        
        if (e.NewItems != null)
        {
            foreach (var item in e.NewItems) 
            {
                if (item is System.ComponentModel.INotifyPropertyChanged notifyItem) 
                {
                    notifyItem.PropertyChanged += ChildItem_IsBlankChanged;
                }
            }
        }
        OnPropertyChanged(nameof(IsBlank));
    }

    private void ChildItem_IsBlankChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(IBlankMaybe.IsBlank))
        {
            OnPropertyChanged(nameof(IsBlank));
        }
    }

    private void SubscribeToInitialChildren()
    {
        foreach (var deal in TestDeals) { ((ObservableObject)deal).PropertyChanged += ChildItem_IsBlankChanged; }
    }
}
