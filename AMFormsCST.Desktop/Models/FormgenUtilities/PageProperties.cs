using AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure;
using AMFormsCST.Desktop.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Reflection;

namespace AMFormsCST.Desktop.Models.FormgenUtilities;
public class PageProperties : IFormgenFileProperties
{
    public PageProperties(FormPage page)
    {
        Settings = page.Settings != null ? new PageSettings(page.Settings) : null;
    }

    public IFormgenFileSettings? Settings { get; set; }

    public IEnumerable<DisplayProperty> GetDisplayProperties()
    {
        if (Settings is PageSettings pageSettings)
        {
            var settingsType = typeof(PageSettings);

            var pageNumberProp = settingsType.GetProperty(nameof(PageSettings.PageNumber));
            if (pageNumberProp != null)
                yield return new DisplayProperty(pageSettings, pageNumberProp);

            var defaultFontSizeProp = settingsType.GetProperty(nameof(PageSettings.DefaultFontSize));
            if (defaultFontSizeProp != null)
                yield return new DisplayProperty(pageSettings, defaultFontSizeProp);

            var leftMarginProp = settingsType.GetProperty(nameof(PageSettings.LeftPrinterMargin));
            if (leftMarginProp != null)
                yield return new DisplayProperty(pageSettings, leftMarginProp);

            var rightMarginProp = settingsType.GetProperty(nameof(PageSettings.RightPrinterMargin));
            if (rightMarginProp != null)
                yield return new DisplayProperty(pageSettings, rightMarginProp);

            var topMarginProp = settingsType.GetProperty(nameof(PageSettings.TopPrinterMargin));
            if (topMarginProp != null)
                yield return new DisplayProperty(pageSettings, topMarginProp);

            var bottomMarginProp = settingsType.GetProperty(nameof(PageSettings.BottomPrinterMargin));
            if (bottomMarginProp != null)
                yield return new DisplayProperty(pageSettings, bottomMarginProp);
        }
        // If Settings is null, yields nothing (empty collection)
    }
}
