using AMFormsCST.Desktop.Interfaces;
using AMFormsCST.Desktop.Models.FormgenUtilities.Grouping;
using System.Collections.Generic;
using System.Linq;

namespace AMFormsCST.Desktop.Models.FormgenUtilities;

public class CodeLineGroupProperties : IFormgenFileProperties
{
    private readonly CodeLineGroup _group;

    public CodeLineGroupProperties(CodeLineGroup group)
    {
        _group = group;
    }

    public IEnumerable<DisplayProperty> GetDisplayProperties()
    {
        yield return new DisplayProperty("Group Type:", _group.Type.ToString());
        yield return new DisplayProperty("Line Count:", _group.CodeLines.Count(c => !c.Settings?.Variable?.Equals("F0") ?? true).ToString());
    }
}