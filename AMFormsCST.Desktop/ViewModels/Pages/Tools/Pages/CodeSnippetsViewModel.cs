using AMFormsCST.Core;
using AMFormsCST.Core.Interfaces;
using AMFormsCST.Core.Interfaces.CodeBlocks;
using AMFormsCST.Core.Types.BestPractices.Practices;
using AMFormsCST.Desktop.ViewModels.Pages.Tools.CodeSnippets;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AMFormsCST.Desktop.ViewModels.Pages.Tools;
public partial class CodeSnippetsViewModel : ViewModel
{
    [ObservableProperty]
    public ObservableCollection<CodeSnippetItemViewModel> _codeSnippets = [];

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SelectCodeSnippetCommand))] 
    private CodeSnippetItemViewModel? _selectedCodeSnippet;

    private ISupportTool _supportTool;
    public CodeSnippetsViewModel(ISupportTool supportTool)
    {
        _supportTool = supportTool ?? throw new ArgumentNullException(nameof(supportTool));

        CodeSnippets = new ObservableCollection<CodeSnippetItemViewModel>(
            _supportTool.CodeBlocks.GetBlocks()
                .OrderBy(x => x.Name)
                .Select(x => new CodeSnippetItemViewModel(x))
        );

        if (CodeSnippets.Any())
        {
            SelectedCodeSnippet = CodeSnippets.First();
            SelectedCodeSnippet.IsSelected = true;
        }
    }

    #region Design Time Constructor
    // I'm not happy about it, so i don't want to look at it
    /// <summary>
    /// Parameterless constructor for design-time data.
    /// Do not use in production code.
    /// </summary>
    public CodeSnippetsViewModel()
    {
        _supportTool = new SupportTool(new AutoMateFormNameBestPractices(new Core.Types.BestPractices.Models.AutoMateFormModel()));

        CodeSnippets = new ObservableCollection<CodeSnippetItemViewModel>(
            _supportTool.CodeBlocks.GetBlocks()
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

    }

    [RelayCommand]
    public void CopyOutput()
    {
        if (SelectedCodeSnippet is null || string.IsNullOrEmpty(SelectedCodeSnippet.Output))
        {
            return;
        }
        try
        {
            Clipboard.SetText(SelectedCodeSnippet.Output);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error copying output: {ex.Message}");
        }
    }
}
