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
using System.Windows;
using static AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure.DotFormgen;

namespace AMFormsCST.Desktop.ViewModels.Pages.Tools;

public partial class FormgenUtilitiesViewModel : ViewModel
{

    [ObservableProperty]
    private ObservableCollection<TreeItemNodeViewModel> _treeViewNodes = [];

    private TreeItemNodeViewModel? _selectedNode;
    public TreeItemNodeViewModel? SelectedNode
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
    private IEnumerable<DisplayProperty>? _selectedNodeProperties;

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

    public bool IsFileLoaded => !string.IsNullOrEmpty(FilePath);

    private readonly ISupportTool _supportTool;
    private readonly IDialogService _dialogService;
    private readonly IFileSystem _fileSystem;

    public FormgenUtilitiesViewModel(ISupportTool supportTool, IDialogService dialogService, IFileSystem fileSystem)
    {
        _supportTool = supportTool ?? throw new ArgumentNullException(nameof(supportTool));
        _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
    }

    [RelayCommand]
    private void OpenFormgenFile()
    {
        var filter = "Formgen Files (*.formgen)|*.formgen|All files (*.*)|*.*";
        var selectedFile = _dialogService.ShowOpenFileDialog(filter);

        if (!string.IsNullOrEmpty(selectedFile))
        {
            FilePath = selectedFile;
            LoadFileContent();
        }
    }

    [RelayCommand]
    private void SaveFormgenFile()
    {
        if (!IsFileLoaded || _supportTool.FormgenUtils.ParsedFormgenFile is null) return;

        IsBusy = true;
        try
        {
            var originalTitle = _supportTool.FormgenUtils.ParsedFormgenFile.Title ?? _fileSystem.GetFileNameWithoutExtension(FilePath!);
            bool titleHasChanged = !string.Equals(originalTitle, FormTitle, StringComparison.Ordinal);

            // Update the in-memory object before saving.
            // The properties from the dynamic UI are already updated on the core models
            // because the desktop wrappers now use pass-through properties.
            _supportTool.FormgenUtils.ParsedFormgenFile.Title = FormTitle;
            _supportTool.FormgenUtils.ParsedFormgenFile.Settings.UUID = Uuid;

            // Save changes to the current file path.
            _supportTool.FormgenUtils.SaveFile(FilePath!);

            // If the title has changed, rename the file and update the view model's file path.
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
            // Reload content to reflect the saved state. This is important if the file was renamed.
            if (IsFileLoaded)
            {
                LoadFileContent();
            }
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
        _supportTool.FormgenUtils.CloseFile();
    }

    [RelayCommand]
    private void RegenerateUuid()
    {
        if (!IsFileLoaded) return;
        Uuid = Guid.NewGuid().ToString();
    }

    private void LoadFileContent()
    {
        if (!IsFileLoaded) return;

        IsBusy = true;
        TreeViewNodes.Clear();
        SelectedNode = null;

        try
        {
            _supportTool.FormgenUtils.OpenFile(FilePath!);
            var fileData = _supportTool.FormgenUtils.ParsedFormgenFile;

            if (fileData == null)
            {
                throw new InvalidOperationException("Failed to parse the .formgen file.");
            }

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

        SelectedNodeProperties = properties?.GetDisplayProperties();
    }
}