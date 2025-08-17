using AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure;
using System.Collections.Generic;

namespace AMFormsCST.Desktop.Models.FormgenUtilities.Grouping;

/// <summary>
/// Represents the top-level collection of all CodeLines in the form.
/// </summary>
public class CodeLineCollection
{
    public IEnumerable<CodeLine> CodeLines { get; }

    public CodeLineCollection(IEnumerable<CodeLine> codeLines)
    {
        CodeLines = codeLines;
    }
}