using AMFormsCST.Core.Types.BestPractices.TextTemplates.Models;

namespace AMFormsCST.Desktop.ViewModels.Pages.Tools.Templates;

public class TemplateTypeFilterItem
{
    public TextTemplate.TemplateType? Value { get; }
    public string DisplayName { get; }

    public TemplateTypeFilterItem(TextTemplate.TemplateType? value)
    {
        Value = value;
        DisplayName = value?.ToString() ?? "All Types";
    }

    public override string ToString() => DisplayName;
    }