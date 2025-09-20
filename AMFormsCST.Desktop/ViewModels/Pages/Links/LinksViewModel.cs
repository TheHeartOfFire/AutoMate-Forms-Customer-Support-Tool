using AMFormsCST.Core.Interfaces;
using AMFormsCST.Desktop.Models;
using AMFormsCST.Desktop.Services;
using AMFormsCST.Desktop.ViewModels.Dialogs;
using AMFormsCST.Desktop.Views.Dialogs;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Wpf.Ui.Abstractions.Controls;
using Wpf.Ui.Controls;

namespace AMFormsCST.Desktop.ViewModels.Pages.Links
{
    public partial class LinksViewModel : ObservableObject, INavigationAware
    {
        private readonly ILogService? _logger;
        private readonly ILinksService _linksService;
        private readonly IDialogService? _dialogService;

        [ObservableProperty]
        private ObservableCollection<LinkModel> _links = [];

        /// <summary>
        /// Initializes a new instance of the <see cref="LinksViewModel"/> class for design-time purposes.
        /// </summary>
        public LinksViewModel() : this(new DesignTimeLinksService(), new DesignTimeDialogService())
        {
        }

        public LinksViewModel(ILinksService linksService, IDialogService dialogService, ILogService? logger = null)
        {
            _linksService = linksService;
            _dialogService = dialogService;
            _logger = logger;

            // Initialize with default links if the collection is empty
            if (Links.Count == 0)
            {
                Links = new ObservableCollection<LinkModel>
                {
                    new() { DisplayName = "ADP", Key = "ADP" },
                    new() { DisplayName = "SalesForce", Key = "SalesForce" },
                    new() { DisplayName = "MyApps", Key = "MyApps" },
                    new() { DisplayName = "Client Info Report", Key = "ClientInfoReport" },
                    new() { DisplayName = "Forms Tracker", Key = "FormsTracker" },
                    new() { DisplayName = "Workday", Key = "Workday" },
                    new() { DisplayName = "CST", Key = "CST" },
                    new() { DisplayName = "Formgen", Key = "Formgen" }
                };
            }
        }

        public void OnNavigatedTo()
        {
            _logger?.LogInfo("LinksViewModel navigated to.");
        }

        public void OnNavigatedFrom()
        {
            _logger?.LogInfo("LinksViewModel navigated from.");
        }

        [RelayCommand]
        private void OnLinkClick(LinkModel? parameter)
        {
            if (parameter is null || string.IsNullOrEmpty(parameter.Key))
            {
                return;
            }
            _linksService.OpenLink(parameter.Key);
        }

        [RelayCommand]
        private void OnAddLinkClick()
        {
            if (_dialogService is null) return;

            var addLinkVm = new AddLinkDialogViewModel();
            var addLinkPage = new AddLinkDialogPage(addLinkVm);

            var result = _dialogService.ShowDialog("Add New Link", addLinkPage);

            if (result == IDialogService.DialogResult.Primary)
            {
                if (!string.IsNullOrWhiteSpace(addLinkVm.DisplayName) && !string.IsNullOrWhiteSpace(addLinkVm.Path))
                {
                    var newLink = new LinkModel { DisplayName = addLinkVm.DisplayName, Key = addLinkVm.Path };
                    Links.Add(newLink);
                    _logger?.LogInfo($"New link added: {newLink.DisplayName}");
                }
            }
        }
        public Task OnNavigatedToAsync()
        {
            _logger?.LogInfo("LinksViewModel navigated to asynchronously.");
            return Task.CompletedTask;
        }

        public Task OnNavigatedFromAsync()
        {
            _logger?.LogInfo("LinksViewModel navigated from asynchronously.");
            return Task.CompletedTask;
        }
    }
}