using AMFormsCST.Desktop.Interfaces;
using AMFormsCST.Desktop.Models.FormgenUtilities.Grouping;
using System.Collections.Generic;
using System.Linq;

namespace AMFormsCST.Desktop.Models.FormgenUtilities;

public class PageGroupProperties : IFormgenFileProperties
{
    private readonly PageGroup _group;
    public IFormgenFileSettings? Settings { get; } = null;
    public int PageCount => _group.Pages.Count();

    public PageGroupProperties(PageGroup group)
    {
        _group = group;
    }

}