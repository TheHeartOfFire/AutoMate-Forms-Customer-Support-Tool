using AMFormsCST.Core.Interfaces;
using AMFormsCST.Core.Interfaces.Utils;
using AMFormsCST.Core.Types.BestPractices.TextTemplates.Models;
using AMFormsCST.Desktop.Models.Templates;
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
    private TemplateItemViewModel? _selectedTemplate;

    private ISupportTool _supportTool;
    private readonly IFileSystem _fileSystem; // Or inject via constructor
    public TemplatesViewModel(ISupportTool supportTool, IFileSystem fs)
    {
        _supportTool = supportTool ?? throw new ArgumentNullException(nameof(supportTool)); 
        _fileSystem = fs ?? throw new ArgumentNullException(nameof(fs));
        _templates = new(_supportTool.Enforcer.Templates.Select(t => new TemplateItemViewModel(t, _supportTool) { Template = t }));

        if (_templates.Count > 0)
        {
            SelectTemplate(_selectedTemplate = _templates.First());
        }
    }

    [RelayCommand]
    private void AddTemplate()
    {
        var dialog = new NewTemplateDialog();

        bool? result = dialog.ShowDialog();

        if (result is not true) return;

        var template = new TemplateItemViewModel(
            new TextTemplate(
                dialog.TemplateName, 
                dialog.TemplateDescription, 
                dialog.TemplateContent,
                dialog.Type), 
            _supportTool);

        _supportTool.Enforcer.AddTemplate(template.Template);


        Templates = new(_supportTool.Enforcer.Templates.Select(t => new TemplateItemViewModel(t, _supportTool) { Template = t }));

        SelectTemplate(template);
    }
    [RelayCommand]
    private void EditTemplate()
    {
        if (SelectedTemplate == null) return; 

        var dialog = new NewTemplateDialog(
            SelectedTemplate.Template.Name, 
            SelectedTemplate.Template.Description,
            SelectedTemplate.Template.Text, 
            SelectedTemplate.Template.Type); 

        bool? result = dialog.ShowDialog();

        if (result is not true) return; 

        SelectedTemplate.Template.Name = dialog.TemplateName;
        SelectedTemplate.Template.Description = dialog.TemplateDescription;
        SelectedTemplate.Template.Text = dialog.TemplateContent;
        SelectedTemplate.Template.Type = dialog.Type;

        SelectedTemplate.RefreshTemplateData();
        _supportTool.Enforcer.UpdateTemplate(SelectedTemplate.Template);
        SelectTemplate(SelectedTemplate);
    }
    [RelayCommand]
    private void RemoveTemplate()
    {
        if (SelectedTemplate is null) return;

        _supportTool.Enforcer.RemoveTemplate(SelectedTemplate.Template);
        Templates = new(_supportTool.Enforcer.Templates.Select(t => new TemplateItemViewModel(t, _supportTool) { Template = t }));

        // Always select the first item after collection change
        SelectTemplate(Templates.FirstOrDefault());
    }
    [RelayCommand]
    internal void SelectTemplate(TemplateItemViewModel? item)
    {
        if (item == SelectedTemplate) return;

        SelectedTemplate?.Deselect();
        SelectedTemplate = item;
        SelectedTemplate?.Select();
    }
    [RelayCommand]
    private void CopyTemplate(TemplateItemViewModel item)
    {
        if (item == SelectedTemplate) return;

        Clipboard.SetText(SelectedTemplate?.Output);
    }
    [RelayCommand]
    private void ResetTemplate(TemplateItemViewModel item)
    {
    }
    [RelayCommand]
    private void ImportTemplate(TemplateItemViewModel item)
    {
        var dialog = new Microsoft.Win32.OpenFileDialog
        {
            Title = "Import Templates",
            Filter = "JSON Files (*.json)|*.json",
            InitialDirectory = _fileSystem.CombinePath(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "FormgenAssistant"
            ),
            FileName = "Templates.json",
            CheckFileExists = true,
            CheckPathExists = true
        };

        bool? result = dialog.ShowDialog();
        if (result != true) return;

        var filePath = dialog.FileName;
        if (!_fileSystem.FileExists(filePath)) return;

        var json = _fileSystem.ReadAllText(filePath);

        // Deserialize to a list of DeprecatedTemplate
        var importedTemplates = System.Text.Json.JsonSerializer.Deserialize<DeprecatedTemplateList>(json);

        if (importedTemplates is null || importedTemplates.TemplateList.Count == 0)
        {
            MessageBox.Show("No templates found in the selected file.", "Import Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        // Example: Convert and add to your current templates
        foreach (var deprecated in importedTemplates.TemplateList)
        {
            var converted = (TextTemplate)deprecated;
            _supportTool.Enforcer.AddTemplate(converted);
        }

        Templates = new(_supportTool.Enforcer.Templates.Select(t => new TemplateItemViewModel(t, _supportTool) { Template = t }));
    }

}