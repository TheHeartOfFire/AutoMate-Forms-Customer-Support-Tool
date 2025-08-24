using AMFormsCST.Core.Interfaces;
using AMFormsCST.Core.Interfaces.Notebook;
using AMFormsCST.Core.Types.Notebook;
using AMFormsCST.Desktop.Interfaces;
using AMFormsCST.Desktop.Types;
using CommunityToolkit.Mvvm.ComponentModel;
using Serilog.Context;
using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace AMFormsCST.Desktop.Models
{
    public partial class Dealer : ObservableObject, ISelectable, IBlankMaybe
    {
        private readonly ILogService? _logger;

        [ObservableProperty]
        private string? _name = string.Empty;
        [ObservableProperty]
        private string? _serverCode = string.Empty;

        public ManagedObservableCollection<Company> Companies { get; set; }

        internal NoteModel? Parent { get; set; }
        internal IDealer? CoreType { get; set; }

        private Company? _selectedCompany = null;
        public Company SelectedCompany
        {
            get => _selectedCompany;
            set => SetProperty(ref _selectedCompany, value);
        }
        public Guid Id { get; } = Guid.NewGuid();

        public bool IsBlank
        {
            get
            {
                if (!string.IsNullOrEmpty(Name) || !string.IsNullOrEmpty(ServerCode))
                    return false;
                if (Companies.Any(c => !c.IsBlank))
                    return false;
                return true;
            }
        }

        [ObservableProperty]
        private bool _isSelected = false;
        internal IDealer CoreTypeInstance = new Core.Types.Notebook.Dealer();
        public void Select()
        {
            IsSelected = true;
            _logger?.LogInfo("Dealer selected.");
        }
        public void Deselect()
        {
            IsSelected = false;
            _logger?.LogInfo("Dealer deselected.");
        }

        public Dealer(ILogService? logger = null)
        {
            _logger = logger;
            Companies = new(() => new Company(_logger), _logger);
            Companies.CollectionChanged += Companies_CollectionChanged;
            foreach (var company in Companies) company.Parent = this;
            SelectCompany(Companies.FirstOrDefault(x => !x.IsBlank));
            _logger?.LogInfo("Dealer initialized.");
        }

        public Dealer(IDealer dealer, ILogService? logger = null)
        {
            _logger = logger;
            CoreType = dealer;
            Companies = new(() => new Company(_logger), _logger);
            foreach (var c in dealer.Companies)
            {
                var company = new Company(c, _logger) { Parent = this };
                Companies.Add(company);
            }
            Companies.CollectionChanged += Companies_CollectionChanged;
            SelectCompany(Companies.FirstOrDefault(x => !x.IsBlank));
            ServerCode = dealer.ServerCode;
            Name = dealer.Name;
            _logger?.LogInfo("Dealer loaded from core type.");
        }

        private void Companies_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
                foreach (Company c in e.NewItems) c.Parent = this;
            UpdateCore();
            _logger?.LogDebug("Companies collection changed.");
        }

        public void SelectCompany(ISelectable selectedCompany)
        {
            if (selectedCompany is null) return;
            foreach (var company in Companies) company.Deselect();
            SelectedCompany = Companies.FirstOrDefault(c => c.Id == selectedCompany.Id);
            SelectedCompany?.Select();
            _logger?.LogInfo($"Company selected: {SelectedCompany?.Id}");
        }

        partial void OnNameChanged(string? value)
        {
            OnPropertyChanged(nameof(IsBlank));
            UpdateCore();
            using (LogContext.PushProperty("DealerId", Id))
            using (LogContext.PushProperty("Name", value))
            using (LogContext.PushProperty("ServerCode", ServerCode))
            using (LogContext.PushProperty("Companies", Companies.Count))
            {
                _logger?.LogInfo($"Dealer name changed: {value}");
            }
        }
        partial void OnServerCodeChanged(string? value)
        {
            OnPropertyChanged(nameof(IsBlank));
            UpdateCore();
            using (LogContext.PushProperty("DealerId", Id))
            using (LogContext.PushProperty("Name", Name))
            using (LogContext.PushProperty("ServerCode", value))
            using (LogContext.PushProperty("Companies", Companies.Count))
            {
                _logger?.LogInfo($"Dealer server code changed: {value}");
            }
        }

        internal void UpdateCore()
        {
            if (CoreType == null && Parent?.CoreType != null)
                CoreType = Parent.CoreType.Dealers.FirstOrDefault(d => d.Id == Id);
            if (CoreType == null) return;
            CoreType.Name = Name ?? string.Empty;
            CoreType.ServerCode = ServerCode ?? string.Empty;
            CoreType.Companies.Clear();
            CoreType.Companies.AddRange(Companies.Select(c => (Core.Types.Notebook.Company)c));
            Parent?.UpdateCore();
            _logger?.LogDebug("Dealer core updated.");
        }

        public static implicit operator Core.Types.Notebook.Dealer(Dealer dealer)
        {
            if (dealer is null) return new Core.Types.Notebook.Dealer();
            return new Core.Types.Notebook.Dealer(dealer.Id)
            {
                Name = dealer.Name ?? string.Empty,
                ServerCode = dealer.ServerCode ?? string.Empty,
                Companies = [..dealer.Companies.Select(c => (Core.Types.Notebook.Company)c)]
            };
        }
    }
}
