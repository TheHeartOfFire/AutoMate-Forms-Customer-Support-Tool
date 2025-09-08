using AMFormsCST.Core.Interfaces;
using AMFormsCST.Core.Interfaces.BestPractices;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AMFormsCST.Desktop.ViewModels.Pages.Tools.Templates
{
    public partial class TemplateVariableViewModel : ObservableObject
    {
        private readonly ILogService? _logger;

        public TemplateVariableViewModel(ITextTemplateVariable variable, ILogService? logger = null)
        {
            _variable = variable;
            _logger = logger;
            _logger?.LogInfo($"TemplateVariableViewModel initialized: Variable='{variable.Name}'");
        }

        [ObservableProperty]
        private ITextTemplateVariable _variable;

        [ObservableProperty]
        private string _value = string.Empty;

        partial void OnValueChanged(string value)
        {
            _logger?.LogDebug($"TemplateVariableViewModel value changed: Variable='{Variable.Name}', Value='{value}'");
        }

        public string DefaultValue => Variable.ProperName;
    }
}
