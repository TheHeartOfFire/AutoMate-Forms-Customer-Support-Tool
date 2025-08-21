using AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure;
using AMFormsCST.Desktop.Models.FormgenUtilities.Grouping;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq;
using static AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure.CodeLineSettings;

namespace AMFormsCST.Desktop.ViewModels.Pages.Tools.FormgenUtils;

public partial class TreeItemNodeViewModel : ObservableObject
{
    [ObservableProperty]
    private string? _header;

    [ObservableProperty]
    private bool _isSelected;

    [ObservableProperty]
    private bool _isExpanded;

    public object Data { get; }
    public ObservableCollection<TreeItemNodeViewModel> Children { get; } = [];
    private readonly FormgenUtilitiesViewModel _parentViewModel;

    public TreeItemNodeViewModel(object data, FormgenUtilitiesViewModel parentViewModel)
    {
        Data = data;
        _parentViewModel = parentViewModel;

        var clHeader = "Code";

        if(data is CodeLine line)
        {
            clHeader = line.Settings?.Variable;
            // Use string.Equals for a null-safe comparison.
            if (line.PromptData?.Settings?.Type is PromptDataSettings.PromptType.Label)
            {
                clHeader = $"<----- {line.PromptData?.Message} ----->" ;
            }
            if (line.PromptData?.Settings?.Type is PromptDataSettings.PromptType.Separator)
            {
                clHeader = "<----- Separator ----->";
            }
        }

        // Set the header based on the type of data
        Header = data switch
        {
            DotFormgen f => f.Title ?? "Formgen File",
            CodeLineCollection => "Code Lines",
            PageGroup => "Pages",
            CodeLineGroup g => g.Type.ToString(),
            FormPage p => $"Page {p.Settings?.PageNumber ?? 0}",
            FormField f => f.Expression ?? "Field",
            CodeLine c => clHeader,
            _ => "Unknown"
        };

        // Populate children based on the type of data
        switch (data)
        {
            case DotFormgen formgenFile:
                // Root node: Add CodeLines and Pages groups
                Children.Add(new TreeItemNodeViewModel(new CodeLineCollection(formgenFile.CodeLines), _parentViewModel) { IsExpanded = true });
                Children.Add(new TreeItemNodeViewModel(new PageGroup(formgenFile.Pages), _parentViewModel));
                break;

            case CodeLineCollection codeLineCollection:
                // CodeLines node: Add INIT, PROMPT, and POST groups
                var allCodeLines = codeLineCollection.CodeLines.ToList();
                Children.Add(new TreeItemNodeViewModel(new CodeLineGroup(CodeType.INIT, allCodeLines.Where(c => c.Settings?.Type == CodeType.INIT)), _parentViewModel));
                Children.Add(new TreeItemNodeViewModel(new CodeLineGroup(CodeType.PROMPT, allCodeLines.Where(c => c.Settings?.Type == CodeType.PROMPT)), _parentViewModel) { IsExpanded = true });
                Children.Add(new TreeItemNodeViewModel(new CodeLineGroup(CodeType.POST, allCodeLines.Where(c => c.Settings?.Type == CodeType.POST)), _parentViewModel));
                break;

            case PageGroup pageGroup:
                // Pages node: Add each FormPage as a child
                foreach (var page in pageGroup.Pages)
                {
                    Children.Add(new TreeItemNodeViewModel(page, _parentViewModel));
                }
                break;

            case CodeLineGroup codeLineGroup:
                // INIT/PROMPT/POST node: Add each individual CodeLine as a child
                foreach (var codeLine in codeLineGroup.CodeLines)
                {
                    Children.Add(new TreeItemNodeViewModel(codeLine, _parentViewModel));
                }
                break;

            case FormPage page:
                // Page node: Add each FormField as a child
                foreach (var field in page.Fields)
                {
                    Children.Add(new TreeItemNodeViewModel(field, _parentViewModel));
                }
                break;
        }
    }

    partial void OnIsSelectedChanged(bool value)
    {
        if (value)
        {
            _parentViewModel.SelectedNode = this;
        }
    }
}