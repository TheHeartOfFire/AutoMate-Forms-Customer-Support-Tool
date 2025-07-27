using AMFormsCST.Core.Interfaces.CodeBlocks;
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
    public CodeSnippetsViewModel()
    {
        CodeSnippets = new ObservableCollection<CodeSnippetItemViewModel>(
            SupportTool.SupportToolInstance.CodeBlocks.GetBlocks()
                .OrderBy(x => x.Name)
                .Select(x => new CodeSnippetItemViewModel(x))
        );

        if (CodeSnippets.Any())
        {
            SelectedCodeSnippet = CodeSnippets.First();
            SelectedCodeSnippet.IsSelected = true;
        }
    }

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
