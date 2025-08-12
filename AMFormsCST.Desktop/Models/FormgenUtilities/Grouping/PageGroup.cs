using AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure;
using System.Collections.Generic;

namespace AMFormsCST.Desktop.Models.FormgenUtilities.Grouping;

/// <summary>
/// Represents a group of all pages in the form.
/// </summary>
public class PageGroup
{
    public IEnumerable<FormPage> Pages { get; }

    public PageGroup(IEnumerable<FormPage> pages)
    {
        Pages = pages;
    }
}