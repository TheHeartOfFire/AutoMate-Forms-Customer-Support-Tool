using AMFormsCST.Desktop.Interfaces;
using AMFormsCST.Desktop.Models.FormgenUtilities.Grouping;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Animation;
using static AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure.CodeLineSettings;

namespace AMFormsCST.Desktop.Models.FormgenUtilities;

public class CodeLineGroupProperties : IFormgenFileProperties
{
    public IFormgenFileSettings? Settings { get; } = null;
    
    public string Type { get; }
    public int LineCount { get; }

    public CodeLineGroupProperties(CodeLineGroup group)
    {
        Type = group.Type.ToString();
        LineCount = group.CodeLines.Count(c => !c.Settings?.Variable?.Equals("F0") ?? true);
    }
}