using AMFormsCST.Core.Interfaces;
using AMFormsCST.Core.Interfaces.BestPractices;
using AMFormsCST.Core.Types.BestPractices.TextTemplates.Models;
using AMFormsCST.Desktop.Models;
using AMFormsCST.Desktop.Interfaces;
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

    private readonly ISupportTool _supportTool;
    private readonly ILogService? _logger;

    public TemplateItemViewModel(TextTemplate template, ISupportTool supportTool, ILogService? logger = null)
    {
        _supportTool = supportTool ?? throw new ArgumentNullException(nameof(supportTool));
        _template = template;
        _logger = logger;
        _variables = new ObservableCollection<TemplateVariableViewModel>(
            template.GetVariables(_supportTool)
                    .Select(variable => new TemplateVariableViewModel(variable, _logger) { Variable = variable }));

        // Initial subscription for existing variables
        foreach (var variable in Variables)
        {
            variable.PropertyChanged += OnVariablePropertyChanged;
        }
        Variables.CollectionChanged += OnVariablesCollectionChanged;
        _logger?.LogInfo($"TemplateItemViewModel initialized: Template='{template.Name}'");
    }

    public string Output
    {
        get
        {
            var preprocessedTemplate = PreProcessTemplate(Template.Text);
            var variables = preprocessedTemplate.variables.Select(v => v.variable).ToList();
            var values = Variables.Select(v => v.Value).ToList();

            var result = TextTemplate.Process(preprocessedTemplate.processedText, variables, values) ?? string.Empty;
            _logger?.LogDebug($"TemplateItemViewModel Output generated for '{Template.Name}': {result}");
            return result;
        }
    }

    private readonly Guid _id = Guid.NewGuid();

    public Guid Id => _id;

    [ObservableProperty]
    private bool _isSelected;

    private void OnVariablePropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(TemplateVariableViewModel.Value))
        {
            OnPropertyChanged(nameof(Output));
            _logger?.LogDebug($"Variable value changed in TemplateItemViewModel '{Template.Name}'. Output updated.");
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
        _logger?.LogDebug($"Variables collection changed in TemplateItemViewModel '{Template.Name}'. Output updated.");
    }

    private (string processedText, List<(int position, ITextTemplateVariable variable, string alias)> variables) PreProcessTemplate(string templateText)
    {
        if (string.IsNullOrEmpty(templateText))
        {
            return (string.Empty, []);
        }

        int counter = 0;
        var processedText = templateText;
        var variables = new List<(int position, ITextTemplateVariable variable, string alias)>();

        while (TextTemplate.ContainsVariable(processedText, _supportTool))
        {
            var variable = TextTemplate.GetFirstVariable(processedText, _supportTool);

            if (variable.variable == null) break;

            variables.Add((variable.position, variable.variable, variable.alias));

            processedText = processedText.Replace(variable.alias, $"{{{counter}}}");
            counter++;
        }

        return (processedText, variables);
    }
    public void RefreshTemplateData()
    {
        // First, notify that the base Template object might have changed (e.g., Name, Description)
        OnPropertyChanged(nameof(Template)); // This will re-evaluate bindings like Template.Name, Template.Description

        // Then, re-process the template text to update variables
        var preprocessedTemplate = PreProcessTemplate(Template.Text);

        // Create new TemplateVariableViewModels for the updated template content
        var newVariables = new ObservableCollection<TemplateVariableViewModel>(
            preprocessedTemplate.variables
                .Select(variable => new TemplateVariableViewModel(variable.variable, _logger) { Variable = variable.variable }));

        // Update the existing Variables collection.
        Variables.Clear();
        foreach (var v in newVariables)
        {
            Variables.Add(v); // This will trigger OnVariablesCollectionChanged and subscribe the new variable
        }

        // After updating variables, explicitly notify Output to re-evaluate
        OnPropertyChanged(nameof(Output));
        _logger?.LogInfo($"TemplateItemViewModel '{Template.Name}' data refreshed.");
    }

    public void Select()
    {
        IsSelected = true;
        _logger?.LogDebug($"TemplateItemViewModel '{Template.Name}' selected.");
    }

    public void Deselect()
    {
        IsSelected = false;
        _logger?.LogDebug($"TemplateItemViewModel '{Template.Name}' deselected.");
    }
}

