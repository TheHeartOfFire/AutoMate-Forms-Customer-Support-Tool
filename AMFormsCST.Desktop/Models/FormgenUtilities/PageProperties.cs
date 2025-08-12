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

    public IEnumerable<DisplayProperty> GetDisplayProperties()
    {
        if (Settings is PageSettings pageSettings)
        {
            yield return new DisplayProperty("Page Number:", pageSettings.PageNumber.ToString());
            yield return new DisplayProperty("Default Font Size:", pageSettings.DefaultFontSize.ToString());
            yield return new DisplayProperty("Left Margin:", pageSettings.LeftPrinterMargin.ToString());
            yield return new DisplayProperty("Right Margin:", pageSettings.RightPrinterMargin.ToString());
            yield return new DisplayProperty("Top Margin:", pageSettings.TopPrinterMargin.ToString());
            yield return new DisplayProperty("Bottom Margin:", pageSettings.BottomPrinterMargin.ToString());
        }
    }
}
