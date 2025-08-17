using AMFormsCST.Desktop.Interfaces;
using AMFormsCST.Desktop.Models.FormgenUtilities.Grouping;
using System.Collections.Generic;
using System.Linq;

namespace AMFormsCST.Desktop.Models.FormgenUtilities;

public class PageGroupProperties : IFormgenFileProperties
{
    private readonly PageGroup _group;

    public PageGroupProperties(PageGroup group)
    {
        _group = group;
    }

    public IEnumerable<DisplayProperty> GetDisplayProperties()
    {
        yield return new DisplayProperty("Total Pages:", _group.Pages.Count().ToString());
    }
}