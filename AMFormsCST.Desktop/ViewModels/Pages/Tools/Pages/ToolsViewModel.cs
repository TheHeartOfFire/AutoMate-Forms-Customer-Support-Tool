using AMFormsCST.Core.Interfaces;
using AMFormsCST.Desktop.ControlsLookup;
using AMFormsCST.Desktop.Models;
using AMFormsCST.Desktop.Views.Pages;
using AMFormsCST.Desktop.Views.Pages.Tools;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace AMFormsCST.Desktop.ViewModels.Pages.Tools
{
    public partial class ToolsViewModel : ViewModel
    {
        private readonly ILogService? _logger;

        [ObservableProperty]
        private ICollection<NavigationCard> _navigationCards = new ObservableCollection<NavigationCard>(
            ControlPages
                .FromNamespace(typeof(ToolsPage).Namespace!)
                .Select(x => new NavigationCard()
                {
                    Name = x.Name,
                    Icon = x.Icon,
                    Description = x.Description,
                    PageType = x.PageType,
                })
        );

        public ToolsViewModel(ILogService? logger = null)
        {
            _logger = logger;
            _logger?.LogInfo("ToolsViewModel initialized.");
            _logger?.LogDebug($"NavigationCards count: {_navigationCards.Count}");
        }
    }
}
