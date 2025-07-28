using AMFormsCST.Core.Types.CodeBlocks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFormsCST.Desktop.ViewModels.Pages.Tools.CodeSnippets;

public partial class CodeInputViewModel : ObservableObject
{
    private readonly CodeInput _codeInputModel; 
    
    [ObservableProperty]
    private string? _bindableValue; 

    public string Description => _codeInputModel.Description; 
    public int Index => _codeInputModel.Index; 
    private readonly CodeSnippetItemViewModel _parent; 

    public CodeInputViewModel(CodeInput codeInputModel, CodeSnippetItemViewModel parent)
    {
        _codeInputModel = codeInputModel;
        _bindableValue = _codeInputModel.Value?.ToString();
        _parent = parent; 
    }

    partial void OnBindableValueChanged(string? value)
    {
        _codeInputModel.SetValue(value ?? string.Empty); 
        _parent.InputChanged(this);
    }
    
}
