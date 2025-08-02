﻿using AMFormsCST.Core.Interfaces.CodeBlocks;
using AMFormsCST.Desktop.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFormsCST.Desktop.ViewModels.Pages.Tools.CodeSnippets;
public partial class CodeSnippetItemViewModel : ObservableObject, ISelectable
{
    public ICodeBase CodeBase { get; } 

    [ObservableProperty]
    private ObservableCollection<CodeInputViewModel> _inputs = [];

    [ObservableProperty]
    private string _output = string.Empty;
    public string? Name => CodeBase.Name;
    public string? Description => CodeBase.Description;

    [ObservableProperty]
    private bool _isSelected;
    private Guid _id = Guid.NewGuid();

    public Guid Id => _id;

    public CodeSnippetItemViewModel(ICodeBase codeBase)
    {
        CodeBase = codeBase;
        Inputs = new ObservableCollection<CodeInputViewModel>(
            codeBase.Inputs.Select(input => new CodeInputViewModel(input, this))
        );
        Output = CodeBase.GetCode();
    }

    public void Select()
    {
        IsSelected = true;
    }

    public void Deselect()
    {
        IsSelected = false;
    }

    public void InputChanged()
    {
        Output = CodeBase.GetCode();
    }

}
