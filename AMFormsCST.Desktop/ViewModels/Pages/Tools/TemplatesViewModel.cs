using AMFormsCST.Core.Types.BestPractices.TextTemplates.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFormsCST.Desktop.ViewModels.Pages.Tools;
public partial class TemplatesViewModel : ViewModel
{
    [ObservableProperty]
    private ObservableCollection<TextTemplate> _templates = new(SupportTool.SupportToolInstance.Enforcer.Templates);

}
