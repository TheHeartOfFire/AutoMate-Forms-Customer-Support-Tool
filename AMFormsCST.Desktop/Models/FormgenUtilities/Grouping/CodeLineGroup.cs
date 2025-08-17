using AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure;
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

    public CodeLineGroup(CodeType type, IEnumerable<CodeLine> codeLines)
    {
        Type = type;
        CodeLines = codeLines;
    }
}