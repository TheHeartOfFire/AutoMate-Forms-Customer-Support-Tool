using AMFormsCST.Core.Interfaces;
using AMFormsCST.Desktop.Interfaces;
using AMFormsCST.Desktop.Models.FormgenUtilities.Grouping;

namespace AMFormsCST.Desktop.Models.FormgenUtilities;

public class PageGroupProperties : IFormgenFileProperties
{
    private readonly PageGroup _group;
    private readonly ILogService? _logger;
    public IFormgenFileSettings? Settings { get; } = null;
    public int PageCount => _group.Pages.Count();

    public PageGroupProperties(PageGroup group, ILogService? logger = null)
    {
        _group = group;
        _logger = logger;
        _logger?.LogInfo($"PageGroupProperties initialized: PageCount={PageCount}");
    }
}