using AMFormsCST.Core.Interfaces;
using AMFormsCST.Core.Interfaces.Utils;
using AMFormsCST.Core.Types.BestPractices.TextTemplates.Models;
using AMFormsCST.Desktop.Models.Templates;
using AMFormsCST.Desktop.Services;
using AMFormsCST.Desktop.ViewModels.Pages.Tools.Templates;
using AMFormsCST.Desktop.Views.Dialogs;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows;

namespace AMFormsCST.Desktop.ViewModels.Pages.Tools;
public partial class TemplatesViewModel : ViewModel
{
    private readonly ILogService? _logger;

    [ObservableProperty]
    private ObservableCollection<TemplateItemViewModel> _templates;

    [ObservableProperty] 
    private TemplateItemViewModel? _selectedTemplate;

    private ISupportTool _supportTool;
    private readonly IFileSystem _fileSystem;

    public TemplatesViewModel(ISupportTool supportTool, IFileSystem fs, ILogService? logger = null)
    {
        _supportTool = supportTool ?? throw new ArgumentNullException(nameof(supportTool)); 
        _fileSystem = fs ?? throw new ArgumentNullException(nameof(fs));
        _logger = logger;
        _templates = new(_supportTool.Enforcer.Templates.Select(t => new TemplateItemViewModel(t, _supportTool) { Template = t }));

        if (_templates.Count > 0)
        {
            SelectTemplate(_selectedTemplate = _templates.First());
        }
        _logger?.LogInfo($"TemplatesViewModel initialized with {_templates.Count} templates.");
    }
    #region Design-Time Constructor
    public TemplatesViewModel()
    {
        _supportTool = new DesignTimeSupportTool();
        _fileSystem = new DesignTimeFileSystem();
        _templates = new(_supportTool.Enforcer.Templates.Select(t => new TemplateItemViewModel(t, _supportTool)));

        if (_templates.Any())
        {
            SelectTemplate(_templates.First());
        }
    }
    #endregion

    [RelayCommand]
    private void AddTemplate()
    {
        try
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
            _logger?.LogInfo($"Template added: {template.Template.Name}");
        }
        catch (Exception ex)
        {
            _logger?.LogError("Error adding template.", ex);
        }
    }

    [RelayCommand]
    private void EditTemplate()
    {
        try
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
            _logger?.LogInfo($"Template edited: {SelectedTemplate.Template.Name}");
        }
        catch (Exception ex)
        {
            _logger?.LogError("Error editing template.", ex);
        }
    }

    [RelayCommand]
    private void RemoveTemplate()
    {
        try
        {
            if (SelectedTemplate is null) return;

            _supportTool.Enforcer.RemoveTemplate(SelectedTemplate.Template);
            Templates = new(_supportTool.Enforcer.Templates.Select(t => new TemplateItemViewModel(t, _supportTool) { Template = t }));

            SelectTemplate(Templates.FirstOrDefault());
            _logger?.LogInfo($"Template removed: {SelectedTemplate?.Template.Name}");
        }
        catch (Exception ex)
        {
            _logger?.LogError("Error removing template.", ex);
        }
    }

    [RelayCommand]
    internal void SelectTemplate(TemplateItemViewModel? item)
    {
        if (item == SelectedTemplate) return;

        SelectedTemplate?.Deselect();
        SelectedTemplate = item;
        SelectedTemplate?.Select();
        _logger?.LogInfo($"Template selected: {item?.Template.Name}");
    }

    [RelayCommand]
    private void CopyTemplate(TemplateItemViewModel item)
    {
        try
        {
            if (item == SelectedTemplate) return;

            Clipboard.SetText(SelectedTemplate?.Output);
            _logger?.LogInfo($"Template output copied: {SelectedTemplate?.Template.Name}");
        }
        catch (Exception ex)
        {
            _logger?.LogError("Error copying template output.", ex);
        }
    }

    [RelayCommand]
    private void ResetTemplate(TemplateItemViewModel item)
    {
        _logger?.LogInfo($"ResetTemplate called for: {item.Template.Name}");
    }

    [RelayCommand]
    private void ImportTemplate(TemplateItemViewModel item)
    {
        try
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

            var importedTemplates = System.Text.Json.JsonSerializer.Deserialize<DeprecatedTemplateList>(json);

            if (importedTemplates is null || importedTemplates.TemplateList.Count == 0)
            {
                MessageBox.Show("No templates found in the selected file.", "Import Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                _logger?.LogWarning("ImportTemplate: No templates found in the selected file.");
                return;
            }

            foreach (var deprecated in importedTemplates.TemplateList)
            {
                var converted = (TextTemplate)deprecated;
                _supportTool.Enforcer.AddTemplate(converted);
            }

            Templates = new(_supportTool.Enforcer.Templates.Select(t => new TemplateItemViewModel(t, _supportTool) { Template = t }));
            _logger?.LogInfo($"Templates imported from: {filePath}");
        }
        catch (Exception ex)
        {
            _logger?.LogError("Error importing templates.", ex);
        }
    }
}