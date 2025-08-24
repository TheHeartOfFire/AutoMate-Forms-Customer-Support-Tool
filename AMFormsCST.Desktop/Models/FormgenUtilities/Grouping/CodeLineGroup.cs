using AMFormsCST.Core.Interfaces;
using AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure;
using AMFormsCST.Desktop.Interfaces;
using System.Collections.Generic;
using static AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure.CodeLineSettings;

namespace AMFormsCST.Desktop.Models.FormgenUtilities.Grouping;

/// <summary>
/// Represents a group of CodeLines of a specific type (e.g., INIT, PROMPT, POST).
/// </summary>
public class CodeLineGroup
{
    public CodeType Type { get; }
    public IEnumerable<CodeLine> CodeLines { get; }
    private readonly ILogService? _logger;

    public CodeLineGroup(CodeType type, IEnumerable<CodeLine> codeLines, ILogService? logger = null)
    {
        Type = type;
        CodeLines = codeLines;
        _logger = logger;
        _logger?.LogInfo($"CodeLineGroup initialized for type '{Type}' with {CodeLines?.Count() ?? 0} code lines.");
    }
}