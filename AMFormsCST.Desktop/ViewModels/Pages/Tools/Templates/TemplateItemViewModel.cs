using AMFormsCST.Core.Interfaces.BestPractices;
using AMFormsCST.Core.Types.BestPractices.TextTemplates.Models;
using AMFormsCST.Desktop.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFormsCST.Desktop.ViewModels.Pages.Tools.Templates;

public partial class TemplateItemViewModel : ObservableObject, ISelectable
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Output))]
    private TextTemplate _template;

    [ObservableProperty]
    private bool _isSelected;

    private ObservableCollection<TemplateVariableViewModel> _variables;
    public ObservableCollection<TemplateVariableViewModel> Variables
    {
        get => _variables;
        set
        {
            if (SetProperty(ref _variables, value))
            {
                // Unsubscribe from previous items
                if (_variables != null)
                {
                    foreach (var variable in _variables)
                    {
                        variable.PropertyChanged -= OnVariablePropertyChanged;
                    }
                }

                _variables = value;

                // Subscribe to new items
                if (_variables != null)
                {
                    foreach (var variable in _variables)
                    {
                        variable.PropertyChanged += OnVariablePropertyChanged;
                    }
                    _variables.CollectionChanged += OnVariablesCollectionChanged; // Handle adds/removes to the collection
                }

                // Notify Output has changed when the collection itself changes
                OnPropertyChanged(nameof(Output));
            }
        }
    }
    public TemplateItemViewModel(TextTemplate template)
    {
        _template = template;
        _variables = new ObservableCollection<TemplateVariableViewModel>(
            template.GetVariables(SupportTool.SupportToolInstance)
                    .Select(variable => new TemplateVariableViewModel(variable) { Variable = variable }));

        // Initial subscription for existing variables
        foreach (var variable in Variables)
        {
            variable.PropertyChanged += OnVariablePropertyChanged;
        }
        Variables.CollectionChanged += OnVariablesCollectionChanged;
    }

    public string Output
    {
        get
        {
            var preprocessedTemplate = PreProcessTemplate(Template.Text);
            var variables = preprocessedTemplate.variables.Select(v => v.variable).ToList();
            var values = Variables.Select(v => v.Value).ToList();

            return TextTemplate.Process(preprocessedTemplate.processedText, variables, values) ?? string.Empty;
        }
    }

    private readonly Guid _id = Guid.NewGuid();

    public Guid Id => _id;

    public void Select()
    {
        IsSelected = true;
    }

    public void Deselect()
    {
        IsSelected = false;
    }
    private void OnVariablePropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(TemplateVariableViewModel.Value))
        {
            OnPropertyChanged(nameof(Output)); 
        }
    }

    private void OnVariablesCollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        if (e.OldItems != null)
        {
            foreach (TemplateVariableViewModel variable in e.OldItems)
            {
                variable.PropertyChanged -= OnVariablePropertyChanged;
            }
        }

        if (e.NewItems != null)
        {
            foreach (TemplateVariableViewModel variable in e.NewItems)
            {
                variable.PropertyChanged += OnVariablePropertyChanged;
            }
        }

        OnPropertyChanged(nameof(Output)); 
    }

    private static (string processedText, List<(int position, ITextTemplateVariable variable, string alias)> variables)  PreProcessTemplate(string templateText)
    {
        if (string.IsNullOrEmpty(templateText))
        {
            return (string.Empty, []);
        }

        int counter = 0;
        var processedText = templateText;
        var variables = new List<(int position, ITextTemplateVariable variable, string alias)>();

        while (TextTemplate.ContainsVariable(processedText, SupportTool.SupportToolInstance))
        {
            var variable = TextTemplate.GetFirstVariable(processedText, SupportTool.SupportToolInstance);
            
            if(variable.variable == null) break;
            
            variables.Add((variable.position, variable.variable, variable.alias));

            processedText = processedText.Replace(variable.alias, $"{{{counter}}}");
            counter++;

        }

        return (processedText, variables);
    }
}

