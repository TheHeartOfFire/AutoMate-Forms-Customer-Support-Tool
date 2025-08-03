using AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq;
using static AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure.CodeLineSettings;
using static AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure.DotFormgen;

namespace AMFormsCST.Desktop.ViewModels.Pages.Tools.FormgenUtils;

public partial class TreeItemNodeViewModel : ObservableObject
{
    private readonly FormgenUtilitiesViewModel _parentViewModel;

    [ObservableProperty]
    private string _name;

    [ObservableProperty]
    private object _data;

    [ObservableProperty]
    private ObservableCollection<TreeItemNodeViewModel> _children;

    [ObservableProperty]
    private bool _isExpanded;

    private bool _isSelected;
    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            if (SetProperty(ref _isSelected, value) && value)
            {
                _parentViewModel.SelectedNode = this;
            }
        }
    }

    // Constructor for the root .formgen file
    public TreeItemNodeViewModel(DotFormgen formgenFile, FormgenUtilitiesViewModel parentViewModel)
    {
        _name = "Root";
        _data = formgenFile;
        _parentViewModel = parentViewModel;
        _children = new ObservableCollection<TreeItemNodeViewModel>();
        IsExpanded = true; // Ensure the root node is expanded to show its children

        // Create a single top-level "Code Lines" node, expanded by default
        var codeLinesParent = new TreeItemNodeViewModel("Code Lines", this, true);
        
        // Group and add CodeLine nodes under the "Code Lines" parent
        var initCodeLines = formgenFile.CodeLines.Where(cl => cl.Settings?.Type == CodeType.INIT).ToList();
        if (initCodeLines.Any())
        {
            var initParent = new TreeItemNodeViewModel("Init", codeLinesParent);
            foreach (var codeLine in initCodeLines)
            {
                initParent.Children.Add(new TreeItemNodeViewModel(codeLine, initParent));
            }
            codeLinesParent.Children.Add(initParent);
        }

        var promptCodeLines = formgenFile.CodeLines.Where(cl => cl.Settings?.Type == CodeType.PROMPT).ToList();
        if (promptCodeLines.Any())
        {
            // The "Prompts" node should be expanded by default
            var promptParent = new TreeItemNodeViewModel("Prompts", codeLinesParent, true);
            foreach (var codeLine in promptCodeLines)
            {
                promptParent.Children.Add(new TreeItemNodeViewModel(codeLine, promptParent));
            }
            codeLinesParent.Children.Add(promptParent);
        }

        var postCodeLines = formgenFile.CodeLines.Where(cl => cl.Settings?.Type == CodeType.POST).ToList();
        if (postCodeLines.Any())
        {
            var postParent = new TreeItemNodeViewModel("Post Prompts", codeLinesParent);
            foreach (var codeLine in postCodeLines)
            {
                postParent.Children.Add(new TreeItemNodeViewModel(codeLine, postParent));
            }
            codeLinesParent.Children.Add(postParent);
        }

        // Only add the "Code Lines" parent if it has children
        if (codeLinesParent.Children.Any())
        {
            _children.Add(codeLinesParent);
        }

        // Add Page nodes after the "Code Lines" node
        foreach (var page in formgenFile.Pages)
        {
            _children.Add(new TreeItemNodeViewModel(page, this));
        }
    }

    // Constructor for a FormPage
    public TreeItemNodeViewModel(FormPage page, TreeItemNodeViewModel parent)
    {
        _name = $"Page {page.Settings?.PageNumber}";
        _data = page;
        _parentViewModel = parent._parentViewModel;
        _children = new ObservableCollection<TreeItemNodeViewModel>(
            page.Fields.Select(f => new TreeItemNodeViewModel(f, this))
        );
    }

    // Constructor for a FormField
    public TreeItemNodeViewModel(FormField field, TreeItemNodeViewModel parent)
    {
        _name = field.Expression ?? "Unnamed Field";
        _data = field;
        _parentViewModel = parent._parentViewModel;
        _children = new ObservableCollection<TreeItemNodeViewModel>(); // Fields have no children
    }

    // Constructor for a CodeLine
    public TreeItemNodeViewModel(CodeLine codeLine, TreeItemNodeViewModel parent)
    {
        var name = codeLine.Settings?.Variable ?? "Unnamed CodeLine";

        // For certain prompt types, use the type name as the display name.
        if (codeLine.Settings?.Type == CodeType.PROMPT && codeLine.PromptData?.Settings != null)
        {
            var promptType = codeLine.PromptData.Settings.Type;

            if (promptType == PromptDataSettings.PromptType.Label || 
                promptType == PromptDataSettings.PromptType.Separator)
            {
                name = promptType.ToString();
            }
        }

        _name = name;
        _data = codeLine;
        _parentViewModel = parent._parentViewModel;
        _children = new ObservableCollection<TreeItemNodeViewModel>(); // CodeLines have no children
    }

    // Constructor for a "folder" or "group" node
    public TreeItemNodeViewModel(string name, TreeItemNodeViewModel parent, bool isExpanded = false)
    {
        _name = name;
        _data = name; // Use the name as data for simple group nodes
        _parentViewModel = parent._parentViewModel;
        _children = new ObservableCollection<TreeItemNodeViewModel>();
        _isExpanded = isExpanded;
    }
}