using AMFormsCST.Desktop.Interfaces;
using AMFormsCST.Desktop.Models.FormgenUtilities.Grouping;
using System.Collections.Generic;
using System.Linq;
using static AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure.CodeLineSettings;

namespace AMFormsCST.Desktop.Models.FormgenUtilities;

public class CodeLineCollectionProperties : IFormgenFileProperties
{
    private readonly CodeLineCollection _collection;

    public CodeLineCollectionProperties(CodeLineCollection collection)
    {
        _collection = collection;
    }

    public IEnumerable<DisplayProperty> GetDisplayProperties()
    {
        var allLines = _collection.CodeLines.ToList();
        yield return new DisplayProperty("Total Code Lines:", allLines.Count.ToString());
        yield return new DisplayProperty("INIT Count:", allLines.Count(c => c.Settings?.Type == CodeType.INIT).ToString());
        yield return new DisplayProperty("PROMPT Count:", allLines.Count(c => c.Settings?.Type == CodeType.PROMPT && (!c.Settings?.Variable?.Equals("F0") ?? true)).ToString());
        yield return new DisplayProperty("POST Count:", allLines.Count(c => c.Settings?.Type == CodeType.POST).ToString());
    }
}