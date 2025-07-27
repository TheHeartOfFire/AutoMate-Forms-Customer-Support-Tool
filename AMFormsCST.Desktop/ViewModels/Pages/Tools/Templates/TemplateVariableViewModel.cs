using AMFormsCST.Core.Interfaces.BestPractices;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFormsCST.Desktop.ViewModels.Pages.Tools.Templates
{
    public partial class TemplateVariableViewModel(ITextTemplateVariable variable) : ObservableObject
    {
        [ObservableProperty]
        private ITextTemplateVariable _variable = variable;

        [ObservableProperty]
        private string _value = string.Empty;

        public string DefaultValue => Variable.ProperName;
    }
}
