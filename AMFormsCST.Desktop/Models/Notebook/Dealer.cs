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

namespace AMFormsCST.Desktop.Models
{
    public partial class Dealer : ObservableObject, ISelectable, IBlankMaybe
    {
        [ObservableProperty]
        private string? _name = string.Empty;
        [ObservableProperty]
        private string? _serverCode = string.Empty;
        public ObservableCollection<Company> Companies { get; set; } = [];

        private Company? _selectedCompany = null;
        public Company SelectedCompany
        {
            get => _selectedCompany;
            set
            {
                SetProperty(ref _selectedCompany, value);
            }
        }
        public Guid Id { get; } = Guid.NewGuid();

        public bool IsBlank
        {
            get
            {
                if (!string.IsNullOrEmpty(Name) || !string.IsNullOrEmpty(ServerCode))
                {
                    return false;
                }

                if (Companies.Any(c => !c.IsBlank))
                {
                    return false;
                }
                return true;
            }
        }

        [ObservableProperty]
        private bool _isSelected = false;
        public void Select()
        {
            IsSelected = true;
        }
        public void Deselect()
        {
            IsSelected = false;
        }
        public Dealer()
        {

            Companies = new ObservableCollection<Company>();
            Companies.CollectionChanged += ChildCollection_CollectionChanged;
            Companies.Add(new Company()); 

            SelectCompany(Companies.FirstOrDefault());

            SubscribeToInitialChildren();
        }
        public void SelectCompany(ISelectable selectedCompany)
        {
            if (selectedCompany is null) return;

            foreach (var company in Companies) company.Deselect();

            SelectedCompany = Companies.FirstOrDefault(c => c.Id == selectedCompany.Id);
            SelectedCompany?.Select();
        }
        partial void OnNameChanged(string? value)
        {
            OnPropertyChanged(nameof(IsBlank)); 
        }

        partial void OnServerCodeChanged(string? value)
        {
            OnPropertyChanged(nameof(IsBlank)); 
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
            foreach (var company in Companies) { ((ObservableObject)company).PropertyChanged += ChildItem_IsBlankChanged; }
        }
    }
}
