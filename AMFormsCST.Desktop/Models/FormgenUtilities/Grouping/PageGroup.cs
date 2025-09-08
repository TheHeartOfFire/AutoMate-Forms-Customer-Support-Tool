using AMFormsCST.Core.Interfaces;
using AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure;

namespace AMFormsCST.Desktop.Models.FormgenUtilities.Grouping;

/// <summary>
/// Represents a group of all pages in the form.
/// </summary>
public class PageGroup
{
    public IEnumerable<FormPage> Pages { get; }
    private readonly ILogService? _logger;

    public PageGroup(IEnumerable<FormPage> pages, ILogService? logger = null)
    {
        Pages = pages;
        _logger = logger;
        _logger?.LogInfo($"PageGroup initialized with {Pages?.Count() ?? 0} pages.");
    }
}