using AMFormsCST.Core.Interfaces;
using AMFormsCST.Desktop.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFormsCST.Desktop.Models.Templates;
public class DeprecatedTemplateList
{
    public List<DeprecatedTemplate> TemplateList { get; set; } = new List<DeprecatedTemplate>();
    private readonly ILogService? _logger;

    public DeprecatedTemplateList(ILogService? logger = null)
    {
        _logger = logger;
        _logger?.LogInfo("DeprecatedTemplateList initialized.");
    }

    public void Add(DeprecatedTemplate template)
    {
        TemplateList.Add(template);
        _logger?.LogInfo($"DeprecatedTemplate added: {template.Name}");
    }

    public bool Remove(DeprecatedTemplate template)
    {
        var removed = TemplateList.Remove(template);
        if (removed)
            _logger?.LogInfo($"DeprecatedTemplate removed: {template.Name}");
        return removed;
    }

    public void Sort()
    {
        TemplateList.Sort((a, b) => string.Compare(a.Name, b.Name, StringComparison.OrdinalIgnoreCase));
        _logger?.LogInfo("DeprecatedTemplateList sorted.");
    }
}
