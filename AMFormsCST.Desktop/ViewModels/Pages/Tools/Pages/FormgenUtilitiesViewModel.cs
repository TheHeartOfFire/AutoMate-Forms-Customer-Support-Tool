using AMFormsCST.Core.Interfaces;
using AMFormsCST.Core.Interfaces.Utils;
using AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure;
using AMFormsCST.Desktop.Controls;
using AMFormsCST.Desktop.Interfaces;
using AMFormsCST.Desktop.Models.FormgenUtilities;
using AMFormsCST.Desktop.ViewModels.Pages.Tools.FormgenUtils;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System;
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
            // Deselect the old node if it exists and is different from the new one.
            if (_selectedNode != null && _selectedNode != value)
            {
                _selectedNode.IsSelected = false;
            }

            if (SetProperty(ref _selectedNode, value))
            {
                // Ensure the new node is marked as selected if it's not null.
                if (_selectedNode != null)
                {
                    _selectedNode.IsSelected = true;
                    UpdateSelectedNodeProperties();
                }
                else
                {
                    // Clear properties if nothing is selected
                    SelectedNodeProperties = null;
                }
            }
        }
    }

    [ObservableProperty]
    private UIElement? _selectedNodeProperties;

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
    public FormgenUtilitiesViewModel(ISupportTool supportTool)
    {
        _supportTool = supportTool ?? throw new ArgumentNullException(nameof(supportTool));
    }

    [RelayCommand]
    private void OpenFormgenFile()
    {
        var openFileDialog = new OpenFileDialog
        {
            Filter = "Formgen Files (*.formgen)|*.formgen|All files (*.*)|*.*",
            Title = "Open .formgen File"
        };

        if (openFileDialog.ShowDialog() == true)
        {
            FilePath = openFileDialog.FileName;
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
            var originalTitle = _supportTool.FormgenUtils.ParsedFormgenFile.Title ?? Path.GetFileNameWithoutExtension(FilePath!);
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
                var directory = Path.GetDirectoryName(FilePath!);
                FilePath = Path.Combine(directory!, FormTitle + ".formgen");
            }

            MessageBox.Show("File saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to save file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

            FormTitle = fileData.Title ?? Path.GetFileNameWithoutExtension(FilePath) ?? string.Empty;
            Uuid = fileData.Settings.UUID;

            // Check for associated image file
            var fileDir = Path.GetDirectoryName(FilePath);
            var fileNameNoExt = Path.GetFileNameWithoutExtension(FilePath);
            var pdfPath = Path.Combine(fileDir!, fileNameNoExt + ".pdf");
            var jpgPath = Path.Combine(fileDir!, fileNameNoExt + ".jpg");

            IsImageFound = fileData.FormType == Format.Pdf ? File.Exists(pdfPath) : File.Exists(jpgPath);
            ShouldRenameImage = IsImageFound ?? false;

            var rootNode = new TreeItemNodeViewModel(fileData, this);
            TreeViewNodes.Add(rootNode);

            // Select the root node by default
            if (TreeViewNodes.Count > 0)
            {
                SelectedNode = TreeViewNodes[0];
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to load file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
            FormPage page => new PageProperties(page),
            FormField field => new FieldProperties(field),
            CodeLine codeLine => new CodeLineProperties(codeLine),
            _ => null
        };

        SelectedNodeProperties = properties?.GetUIElements();
    }
}