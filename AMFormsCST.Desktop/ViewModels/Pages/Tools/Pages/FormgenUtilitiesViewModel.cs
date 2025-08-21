using AMFormsCST.Core.Interfaces;
using AMFormsCST.Core.Interfaces.Utils;
using AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure;
using AMFormsCST.Desktop.Interfaces;
using AMFormsCST.Desktop.Models.FormgenUtilities;
using AMFormsCST.Desktop.Models.FormgenUtilities.Grouping;
using AMFormsCST.Desktop.Services;
using AMFormsCST.Desktop.ViewModels.Pages.Tools.FormgenUtils;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using static AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure.DotFormgen;

namespace AMFormsCST.Desktop.ViewModels.Pages.Tools;

public partial class FormgenUtilitiesViewModel : ViewModel
{

    [ObservableProperty]
    private ObservableCollection<TreeItemNodeViewModel> _treeViewNodes = [];

    private TreeItemNodeViewModel? _selectedNode;
    public virtual TreeItemNodeViewModel? SelectedNode
    {
        get => _selectedNode;
        set
        {
            // This setter is now primarily driven by the OnIsSelectedChanged callback
            // in the TreeItemNodeViewModel. We just need to update the property.
            if (SetProperty(ref _selectedNode, value))
            {
                UpdateSelectedNodeProperties();
            }
        }
    }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasChanged))]
    private ObservableCollection<DisplayProperty>? _selectedNodeProperties;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsFileLoaded))]
    private string? _filePath;

    [ObservableProperty]
    private string _formTitle = string.Empty;

    [ObservableProperty]
    private string _uuid = string.Empty;

    [ObservableProperty]
    private bool? _isImageFound;

    [ObservableProperty]
    private bool _shouldRenameImage;

    [ObservableProperty]
    private bool _isBusy;

    public bool HasChanged => _supportTool.FormgenUtils.HasChanged || _backupLoaded;

    public bool IsFileLoaded => !string.IsNullOrEmpty(FilePath);
    private bool _backupLoaded = false;

    private readonly ISupportTool _supportTool;
    private readonly IDialogService _dialogService;
    private readonly IFileSystem _fileSystem;

    public FormgenUtilitiesViewModel(ISupportTool supportTool, IDialogService dialogService, IFileSystem fileSystem)
    {
        _supportTool = supportTool ?? throw new ArgumentNullException(nameof(supportTool));
        _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        _supportTool.FormgenUtils.FormgenFileChanged += (s, e) => OnPropertyChanged(nameof(HasChanged));
    }


    [RelayCommand]
    private void OpenFormgenFile()
    {
        var filter = "Formgen Files (*.formgen)|*.formgen|All files (*.*)|*.*";
        var selectedFile = _dialogService.ShowOpenFileDialog(filter);

        if (!string.IsNullOrEmpty(selectedFile))
        {
            FilePath = selectedFile;
            _backupLoaded = false;
            LoadFileContent();
        }
    }

    [RelayCommand]
    private void SaveFormgenFile()
    {
        if (!IsFileLoaded || _supportTool.FormgenUtils.ParsedFormgenFile is null || !HasChanged) return;

        IsBusy = true;
        try
        {
            var originalTitle = _supportTool.FormgenUtils.ParsedFormgenFile.Title ?? _fileSystem.GetFileNameWithoutExtension(FilePath!);
            bool titleHasChanged = !string.Equals(originalTitle, FormTitle, StringComparison.Ordinal);

            _supportTool.FormgenUtils.ParsedFormgenFile.Title = FormTitle;
            _supportTool.FormgenUtils.ParsedFormgenFile.Settings.UUID = Uuid;

            // Save changes to the current file path.
            _supportTool.FormgenUtils.SaveFile(FilePath!);

            if (titleHasChanged)
            {
                _supportTool.FormgenUtils.RenameFile(FormTitle, ShouldRenameImage);
                var directory = _fileSystem.GetDirectoryName(FilePath!);
                FilePath = _fileSystem.CombinePath(directory!, FormTitle + ".formgen");
            }

            _dialogService.ShowMessageBox("File saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            _dialogService.ShowMessageBox($"Failed to save file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            IsBusy = false;
            if (IsFileLoaded)
            {
                _backupLoaded = false;
                LoadFileContent();
            }
        }
    }

    [RelayCommand]
    private void LoadBackup()
    {
        if (!IsFileLoaded || _supportTool.FormgenUtils.ParsedFormgenFile?.Settings.UUID is null) return;

        var backupDir = _fileSystem.CombinePath(AMFormsCST.Core.IO.BackupPath, _supportTool.FormgenUtils.ParsedFormgenFile.Settings.UUID);
        var filter = "Backup Files (*.bak)|*.bak|All files (*.*)|*.*";
        var selectedFile = _dialogService.ShowOpenFileDialog(filter, backupDir);

        if (!string.IsNullOrEmpty(selectedFile))
        {
            _supportTool.FormgenUtils.LoadBackup(selectedFile);
            _backupLoaded = true;
            LoadFileContent(selectedFile);
        }
    }

    [RelayCommand]
    private void ClearFile()
    {
        FilePath = null;
        FormTitle = string.Empty;
        Uuid = string.Empty;
        IsImageFound = null;
        ShouldRenameImage = false;
        TreeViewNodes.Clear();
        SelectedNode = null;
        SelectedNodeProperties = null;
        _supportTool.FormgenUtils.CloseFile();
    }

    [RelayCommand]
    private void RegenerateUuid()
    {
        if (!IsFileLoaded) return;
        Uuid = Guid.NewGuid().ToString();
    }

    private void LoadFileContent(string? filePath = null)
    {
        if (!IsFileLoaded) return;

        IsBusy = true;
        TreeViewNodes.Clear();
        SelectedNode = null;

        var path = filePath is not null ? filePath : FilePath;

        try
        {
            _supportTool.FormgenUtils.OpenFile(path!);
            var fileData = _supportTool.FormgenUtils.ParsedFormgenFile ?? throw new InvalidOperationException("Failed to parse the .formgen file.");
            FormTitle = fileData.Title ?? _fileSystem.GetFileNameWithoutExtension(FilePath) ?? string.Empty;
            Uuid = fileData.Settings.UUID;

            // Check for associated image file
            var fileDir = _fileSystem.GetDirectoryName(FilePath);
            var fileNameNoExt = _fileSystem.GetFileNameWithoutExtension(FilePath);
            var pdfPath = _fileSystem.CombinePath(fileDir!, fileNameNoExt + ".pdf");
            var jpgPath = _fileSystem.CombinePath(fileDir!, fileNameNoExt + ".jpg");

            IsImageFound = fileData.FormType == Format.Pdf ? _fileSystem.FileExists(pdfPath) : _fileSystem.FileExists(jpgPath);
            ShouldRenameImage = IsImageFound ?? false;

            var rootNode = new TreeItemNodeViewModel(fileData, this);
            TreeViewNodes.Add(rootNode);

            // Select the root node by default
            if (TreeViewNodes.Count > 0)
            {
                rootNode.IsSelected = true;
                rootNode.IsExpanded = true; // Expand the root node by default
            }
        }
        catch (Exception ex)
        {
            _dialogService.ShowMessageBox($"Failed to load file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            FilePath = null; // Reset filepath on failure
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void UpdateSelectedNodeProperties()
    {
        IFormgenFileProperties? properties = SelectedNode?.Data switch
        {
            DotFormgen formgenFile => new FormProperties(formgenFile),
            PageGroup pageGroup => new PageGroupProperties(pageGroup),
            CodeLineCollection codeLineCollection => new CodeLineCollectionProperties(codeLineCollection),
            CodeLineGroup codeLineGroup => new CodeLineGroupProperties(codeLineGroup),
            FormPage page => new PageProperties(page),
            FormField field => new FieldProperties(field),
            CodeLine codeLine => new CodeLineProperties(codeLine),
            _ => null
        };

        if (properties is null) return;

        // List of property names to be marked as read-only
        var readOnlyNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "ID", "UUID", "PublishedUUID", "TotalPages", "PageNumber", "Type", "Title" // Add more as needed
        };

        var displayProps = new List<DisplayProperty>();

        // Main properties
        foreach (var p in properties.GetType().GetProperties().Where(p => p.GetMethod != null))
        {
            if (p.Name.Equals("Settings") || p.Name.Equals("PromptData")) continue;

            bool isReadOnly = readOnlyNames.Contains(p.Name) || !p.CanWrite;
            displayProps.Add(new DisplayProperty(properties, p, isReadOnly));
        }

        // Settings properties
        if (properties.Settings != null)
        {
            foreach (var p in properties.Settings.GetType().GetProperties().Where(p => p.GetMethod != null))
            {
                bool isReadOnly = readOnlyNames.Contains(p.Name) || !p.CanWrite;
                displayProps.Add(new DisplayProperty(properties.Settings, p, isReadOnly));
            }
        }

        // Pattern matching with a local variable for CodeLineProperties
        if (properties is CodeLineProperties props && props.PromptData is not null)
        {
            foreach (var p in props.PromptData.GetType().GetProperties().Where(p => p.GetMethod != null))
            {
                if (p.Name.Equals("Settings")) continue;
                displayProps.Add(new DisplayProperty(props.PromptData, p));
            }

            foreach (var p in props.PromptData.Settings.GetType().GetProperties().Where(p => p.GetMethod != null))
            {
                displayProps.Add(new DisplayProperty(props.PromptData.Settings, p));
            }
        }

        SelectedNodeProperties = new ObservableCollection<DisplayProperty>(displayProps);
    }
}