using AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq;
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
        _children = new ObservableCollection<TreeItemNodeViewModel>(
            formgenFile.Pages.Select(p => new TreeItemNodeViewModel(p, this))
        );
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
}