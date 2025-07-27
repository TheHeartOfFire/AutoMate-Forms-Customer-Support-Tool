using AMFormsCST.Core.Types.BestPractices.TextTemplates.Models;
using AMFormsCST.Desktop.ViewModels.Pages.Tools.Templates;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static System.Net.Mime.MediaTypeNames;

namespace AMFormsCST.Desktop.ViewModels.Pages.Tools;
public partial class TemplatesViewModel : ViewModel
{
    [ObservableProperty]
    private ObservableCollection<TemplateItemViewModel> _templates = new(SupportTool.SupportToolInstance.Enforcer.Templates.Select(t => new TemplateItemViewModel(t) { Template = t }));

    [ObservableProperty] 
    private TemplateItemViewModel _selectedTemplate = new(new(string.Empty, string.Empty, string.Empty));

    [RelayCommand]
    private void AddTemplate()
    {
        SupportTool.SupportToolInstance.Enforcer.AddTemplate(new("Temporary Template! 4", "This template is temporary.", "This is a temporary template for the purposes of testing. Notes:Notes \n Notes:CaseNumber \n User:Input"));


        Templates = new(SupportTool.SupportToolInstance.Enforcer.Templates.Select(t => new TemplateItemViewModel(t) { Template = t }));
        if (Templates.Any())
        {
            SelectedTemplate = Templates.First();
            SelectedTemplate.IsSelected = true;
        }
        
    }
    [RelayCommand]
    private void RemoveTemplate(TemplateItemViewModel? item)
    {
        if (item is null)
        {
            return;
        }
        SupportTool.SupportToolInstance.Enforcer.RemoveTemplate(item.Template);
        Templates = new(SupportTool.SupportToolInstance.Enforcer.Templates.Select(t => new TemplateItemViewModel(t) { Template = t }));
    }
    [RelayCommand]
    private void SelectTemplate(TemplateItemViewModel item)
    {
        if (item == SelectedTemplate) return;
        
        SelectedTemplate.IsSelected = false;
        SelectedTemplate = item;
        SelectedTemplate.IsSelected = true;
    }
    [RelayCommand]
    private void CopyTemplate(TemplateItemViewModel item)
    {
        if (item == SelectedTemplate) return;

        Clipboard.SetText(SelectedTemplate.Output);
    }

}