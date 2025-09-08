using AMFormsCST.Core.Interfaces;
using AMFormsCST.Core.Interfaces.Utils;
using AMFormsCST.Core.Utils;
using AMFormsCST.Desktop.ViewModels.Pages.Tools.CodeSnippets;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows;

namespace AMFormsCST.Desktop.ViewModels.Pages.Tools;
public partial class CodeSnippetsViewModel : ViewModel
{
    private readonly ILogService? _logger;

    [ObservableProperty]
    public ObservableCollection<CodeSnippetItemViewModel> _codeSnippets = [];

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SelectCodeSnippetCommand))] 
    private CodeSnippetItemViewModel? _selectedCodeSnippet;

    private ICodeBlocks _codeBlocks;

    public CodeSnippetsViewModel(ISupportTool supportTool, ILogService? logger = null)
    {
        _logger = logger;
        _codeBlocks = supportTool.CodeBlocks ?? throw new ArgumentNullException(nameof(_codeBlocks));

        CodeSnippets = new ObservableCollection<CodeSnippetItemViewModel>(
            _codeBlocks.GetBlocks()
                .OrderBy(x => x.Name)
                .Select(x => new CodeSnippetItemViewModel(x))
        );

        if (CodeSnippets.Any())
        {
            SelectedCodeSnippet = CodeSnippets.First();
            SelectedCodeSnippet.IsSelected = true;
            _logger?.LogInfo($"CodeSnippetsViewModel initialized with {CodeSnippets.Count} snippets. Selected: {SelectedCodeSnippet.Name}");
        }
        else
        {
            _logger?.LogInfo("CodeSnippetsViewModel initialized with no snippets.");
        }
    }

    #region Design Time Constructor
    /// <summary>
    /// Parameterless constructor for design-time data.
    /// Do not use in production code.
    /// </summary>
    public CodeSnippetsViewModel()
    {
        _codeBlocks = new CodeBlocks();

        CodeSnippets = new ObservableCollection<CodeSnippetItemViewModel>(
            _codeBlocks.GetBlocks()
                .OrderBy(x => x.Name)
                .Select(x => new CodeSnippetItemViewModel(x))
        );

        if (CodeSnippets.Any())
        {
            SelectedCodeSnippet = CodeSnippets.First();
            SelectedCodeSnippet.IsSelected = true;
        }
    }
    #endregion

    [RelayCommand]
    private void SelectCodeSnippet(CodeSnippetItemViewModel? item)
    {
        if (item == null || item == SelectedCodeSnippet)
        {
            return;
        }

        if (SelectedCodeSnippet != null)
        {
            SelectedCodeSnippet.IsSelected = false;
        }

        SelectedCodeSnippet = item;
        SelectedCodeSnippet.IsSelected = true;
        _logger?.LogInfo($"Code snippet selected: {item.Name}");
    }

    [RelayCommand]
    public void CopyOutput()
    {
        if (SelectedCodeSnippet is null || string.IsNullOrEmpty(SelectedCodeSnippet.Output))
        {
            _logger?.LogWarning("CopyOutput called but no snippet is selected or output is empty.");
            return;
        }
        try
        {
            Clipboard.SetText(SelectedCodeSnippet.Output);
            _logger?.LogInfo($"Copied output for snippet: {SelectedCodeSnippet.Name}");
        }
        catch (Exception ex)
        {
            _logger?.LogError("Error copying output to clipboard.", ex);
        }
    }
}
