using AMFormsCST.Core.Interfaces;
using AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure;
using AMFormsCST.Desktop.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AMFormsCST.Desktop.Models.FormgenUtilities;

public partial class PageSettings : ObservableObject, IFormgenFileSettings
{
    private readonly FormPageSettings _coreSettings;
    private readonly ILogService? _logger;

    public PageSettings(FormPageSettings settings, ILogService? logger = null)
    {
        _coreSettings = settings;
        _logger = logger;
        _logger?.LogInfo($"PageSettings initialized: PageNumber={settings.PageNumber}, DefaultFontSize={settings.DefaultFontSize}");
    }

    public int PageNumber
    {
        get => _coreSettings.PageNumber;
        set
        {
            SetProperty(_coreSettings.PageNumber, value, _coreSettings, (s, v) => s.PageNumber = v);
            _logger?.LogInfo($"PageNumber changed: {value}");
        }
    }

    public int DefaultFontSize
    {
        get => _coreSettings.DefaultFontSize;
        set
        {
            SetProperty(_coreSettings.DefaultFontSize, value, _coreSettings, (s, v) => s.DefaultFontSize = v);
            _logger?.LogInfo($"DefaultFontSize changed: {value}");
        }
    }

    public int LeftPrinterMargin
    {
        get => _coreSettings.LeftPrinterMargin;
        set
        {
            SetProperty(_coreSettings.LeftPrinterMargin, value, _coreSettings, (s, v) => s.LeftPrinterMargin = v);
            _logger?.LogInfo($"LeftPrinterMargin changed: {value}");
        }
    }

    public int RightPrinterMargin
    {
        get => _coreSettings.RightPrinterMargin;
        set
        {
            SetProperty(_coreSettings.RightPrinterMargin, value, _coreSettings, (s, v) => s.RightPrinterMargin = v);
            _logger?.LogInfo($"RightPrinterMargin changed: {value}");
        }
    }

    public int TopPrinterMargin
    {
        get => _coreSettings.TopPrinterMargin;
        set
        {
            SetProperty(_coreSettings.TopPrinterMargin, value, _coreSettings, (s, v) => s.TopPrinterMargin = v);
            _logger?.LogInfo($"TopPrinterMargin changed: {value}");
        }
    }

    public int BottomPrinterMargin
    {
        get => _coreSettings.BottomPrinterMargin;
        set
        {
            SetProperty(_coreSettings.BottomPrinterMargin, value, _coreSettings, (s, v) => s.BottomPrinterMargin = v);
            _logger?.LogInfo($"BottomPrinterMargin changed: {value}");
        }
    }
}
