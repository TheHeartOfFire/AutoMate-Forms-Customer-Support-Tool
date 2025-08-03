using AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure;
using AMFormsCST.Desktop.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AMFormsCST.Desktop.Models.FormgenUtilities;
public class PageProperties : IFormgenFileProperties
{
    public PageProperties(FormPage page)
    {
        Settings = new PageSettings(page.Settings);
    }

    public IFormgenFileSettings Settings { get; set; }

    public UIElement GetUIElements()
    {
        return BasicStats.GetSettingsAndPropertiesUIElements(this);
    }
}
