using AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure;
using AMFormsCST.Desktop.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFormsCST.Desktop.Models.FormgenUtilities;
public class PageProperties : BasicStats
{

    public PageProperties(FormPage page)
    {
        Settings = new PageSettings(page.Settings);
    }

    public new IFormgenFileSettings Settings { get; set; } = new PageSettings();
}
