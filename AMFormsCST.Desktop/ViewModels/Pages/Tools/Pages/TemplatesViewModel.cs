using AMFormsCST.Core.Interfaces;
using AMFormsCST.Core.Types.BestPractices.TextTemplates.Models;
using AMFormsCST.Desktop.ViewModels.Dialogs;
using AMFormsCST.Desktop.ViewModels.Pages.Tools.Templates;
using AMFormsCST.Desktop.Views.Dialogs;
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
    private ObservableCollection<TemplateItemViewModel> _templates;

    [ObservableProperty] 
    private TemplateItemViewModel _selectedTemplate;

    private ISupportTool _supportTool;
    public TemplatesViewModel(ISupportTool supportTool)
    {
        _supportTool = supportTool ?? throw new ArgumentNullException(nameof(supportTool)); 
        _templates = new(_supportTool.Enforcer.Templates.Select(t => new TemplateItemViewModel(t, _supportTool) { Template = t }));
        _selectedTemplate = new(new(string.Empty, string.Empty, string.Empty), _supportTool);
    }

    [RelayCommand]
    private void AddTemplate()
    {
        var dialog = new NewTemplateDialog();

        bool? result = dialog.ShowDialog();

        if (result is not true) return;

        var template = new TemplateItemViewModel(new TextTemplate(dialog.TemplateName, dialog.TemplateDescription, dialog.TemplateContent), _supportTool);

        _supportTool.Enforcer.AddTemplate(template.Template);


        Templates = new(_supportTool.Enforcer.Templates.Select(t => new TemplateItemViewModel(t, _supportTool) { Template = t }));

        SelectTemplate(template);
    }
    [RelayCommand]
    private void EditTemplate()
    {
        if (SelectedTemplate == null) return; 

        var dialog = new NewTemplateDialog(SelectedTemplate.Template.Name, SelectedTemplate.Template.Description, SelectedTemplate.Template.Text); 

        bool? result = dialog.ShowDialog();

        if (result is not true) return; 

        SelectedTemplate.Template.Name = dialog.TemplateName;
        SelectedTemplate.Template.Description = dialog.TemplateDescription;
        SelectedTemplate.Template.Text = dialog.TemplateContent;

        SelectedTemplate.RefreshTemplateData();
        _supportTool.Enforcer.UpdateTemplate(SelectedTemplate.Template);
        SelectTemplate(SelectedTemplate);
    }
    [RelayCommand]
    private void RemoveTemplate(TemplateItemViewModel? item)
    {
        if (item is null) return;

        _supportTool.Enforcer.RemoveTemplate(item.Template);
        Templates = new(_supportTool.Enforcer.Templates.Select(t => new TemplateItemViewModel(t, _supportTool) { Template = t }));


        SelectTemplate(Templates.Any() ? Templates.First() : new(new(string.Empty, string.Empty, string.Empty), _supportTool));
       
    }
    [RelayCommand]
    private void SelectTemplate(TemplateItemViewModel item)
    {
        if (item == SelectedTemplate) return;
        
        SelectedTemplate = item;
    }
    [RelayCommand]
    private void CopyTemplate(TemplateItemViewModel item)
    {
        if (item == SelectedTemplate) return;

        Clipboard.SetText(SelectedTemplate.Output);
    }
    [RelayCommand]
    private void ResetTemplate(TemplateItemViewModel item)
    {
    }

}