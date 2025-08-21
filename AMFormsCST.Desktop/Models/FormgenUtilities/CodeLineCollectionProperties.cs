using AMFormsCST.Desktop.Interfaces;
using AMFormsCST.Desktop.Models.FormgenUtilities.Grouping;
using System.Collections.Generic;
using System.Linq;
using static AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure.CodeLineSettings;

namespace AMFormsCST.Desktop.Models.FormgenUtilities;

public class CodeLineCollectionProperties : IFormgenFileProperties
{
    private readonly CodeLineCollection _collection;
    public IFormgenFileSettings? Settings { get; } = null;

    public int TotalLines => _collection.CodeLines.Count();
    public int InitLines => _collection.CodeLines.Count(c => c.Settings?.Type == CodeType.INIT);
    public int PromptLines => _collection.CodeLines.Count(c => c.Settings?.Type == CodeType.PROMPT && (!c.Settings?.Variable?.Equals("F0") ?? true));
    public int PostLines => _collection.CodeLines.Count(c => c.Settings?.Type == CodeType.POST);


    public CodeLineCollectionProperties(CodeLineCollection collection)
    {
        _collection = collection;
    }
}