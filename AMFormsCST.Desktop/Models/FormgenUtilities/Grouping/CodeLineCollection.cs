using AMFormsCST.Core.Interfaces;
using AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure;

namespace AMFormsCST.Desktop.Models.FormgenUtilities.Grouping;

/// <summary>
/// Represents the top-level collection of all CodeLines in the form.
/// </summary>
public class CodeLineCollection
{
    public IEnumerable<CodeLine> CodeLines { get; }
    private readonly ILogService? _logger;

    public CodeLineCollection(IEnumerable<CodeLine> codeLines, ILogService? logger = null)
    {
        CodeLines = codeLines;
        _logger = logger;
        _logger?.LogInfo($"CodeLineCollection initialized with {CodeLines?.Count() ?? 0} code lines.");
    }
}