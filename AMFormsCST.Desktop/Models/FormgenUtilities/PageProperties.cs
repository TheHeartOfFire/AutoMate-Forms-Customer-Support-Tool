using AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure;
using AMFormsCST.Desktop.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Reflection;
using AMFormsCST.Core.Interfaces;

namespace AMFormsCST.Desktop.Models.FormgenUtilities;
public class PageProperties : IFormgenFileProperties
{
    private readonly ILogService? _logger;

    public PageProperties(FormPage page, ILogService? logger = null)
    {
        _logger = logger;
        Settings = page.Settings != null ? new PageSettings(page.Settings, _logger) : null;
        _logger?.LogInfo("PageProperties initialized.");
    }

    public IFormgenFileSettings? Settings { get; set; }

    public IEnumerable<DisplayProperty> GetDisplayProperties()
    {
        if (Settings is PageSettings pageSettings)
        {
            var settingsType = typeof(PageSettings);

            var pageNumberProp = settingsType.GetProperty(nameof(PageSettings.PageNumber));
            if (pageNumberProp != null)
                yield return new DisplayProperty(pageSettings, pageNumberProp, false, _logger);

            var defaultFontSizeProp = settingsType.GetProperty(nameof(PageSettings.DefaultFontSize));
            if (defaultFontSizeProp != null)
                yield return new DisplayProperty(pageSettings, defaultFontSizeProp, false, _logger);

            var leftMarginProp = settingsType.GetProperty(nameof(PageSettings.LeftPrinterMargin));
            if (leftMarginProp != null)
                yield return new DisplayProperty(pageSettings, leftMarginProp, false, _logger);

            var rightMarginProp = settingsType.GetProperty(nameof(PageSettings.RightPrinterMargin));
            if (rightMarginProp != null)
                yield return new DisplayProperty(pageSettings, rightMarginProp, false, _logger);

            var topMarginProp = settingsType.GetProperty(nameof(PageSettings.TopPrinterMargin));
            if (topMarginProp != null)
                yield return new DisplayProperty(pageSettings, topMarginProp, false, _logger);

            var bottomMarginProp = settingsType.GetProperty(nameof(PageSettings.BottomPrinterMargin));
            if (bottomMarginProp != null)
                yield return new DisplayProperty(pageSettings, bottomMarginProp, false, _logger);
        }
        // If Settings is null, yields nothing (empty collection)
    }
}
