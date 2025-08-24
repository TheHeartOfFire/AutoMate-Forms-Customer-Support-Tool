using AMFormsCST.Core.Interfaces;
using AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure;
using AMFormsCST.Desktop.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AMFormsCST.Desktop.Models.FormgenUtilities;

public partial class FormSettings : ObservableObject, IFormgenFileSettings
{
    private readonly DotFormgenSettings _coreSettings;
    private readonly ILogService? _logger;

    public FormSettings(DotFormgenSettings settings, ILogService? logger = null)
    {
        _coreSettings = settings;
        _logger = logger;
        _logger?.LogInfo($"FormSettings initialized: Version={settings.Version}, UUID={settings.UUID}");
    }

    public string Version
    {
        get => _coreSettings.Version.ToString();
        // Version is typically read-only, so no setter is provided.
    }

    public string PublishedUUID
    {
        get => _coreSettings.UUID;
        set
        {
            SetProperty(_coreSettings.UUID, value, _coreSettings, (s, v) => s.UUID = v);
            _logger?.LogInfo($"PublishedUUID changed: {value}");
        }
    }

    public bool LegacyImport
    {
        get => _coreSettings.LegacyImport;
        set
        {
            SetProperty(_coreSettings.LegacyImport, value, _coreSettings, (s, v) => s.LegacyImport = v);
            _logger?.LogInfo($"LegacyImport changed: {value}");
        }
    }

    public int TotalPages
    {
        get => _coreSettings.TotalPages;
        set
        {
            SetProperty(_coreSettings.TotalPages, value, _coreSettings, (s, v) => s.TotalPages = v);
            _logger?.LogInfo($"TotalPages changed: {value}");
        }
    }

    public int DefaultPoints
    {
        get => _coreSettings.DefaultFontSize;
        set
        {
            SetProperty(_coreSettings.DefaultFontSize, value, _coreSettings, (s, v) => s.DefaultFontSize = v);
            _logger?.LogInfo($"DefaultPoints changed: {value}");
        }
    }

    public bool MissingSourceJpeg
    {
        get => _coreSettings.MissingSourceJpeg;
        set
        {
            SetProperty(_coreSettings.MissingSourceJpeg, value, _coreSettings, (s, v) => s.MissingSourceJpeg = v);
            _logger?.LogInfo($"MissingSourceJpeg changed: {value}");
        }
    }

    public bool Duplex
    {
        get => _coreSettings.Duplex;
        set
        {
            SetProperty(_coreSettings.Duplex, value, _coreSettings, (s, v) => s.Duplex = v);
            _logger?.LogInfo($"Duplex changed: {value}");
        }
    }

    public int MaxAccessoryLines
    {
        get => _coreSettings.MaxAccessoryLines;
        set
        {
            SetProperty(_coreSettings.MaxAccessoryLines, value, _coreSettings, (s, v) => s.MaxAccessoryLines = v);
            _logger?.LogInfo($"MaxAccessoryLines changed: {value}");
        }
    }

    public bool PrePrintedLaserForm
    {
        get => _coreSettings.PreprintedLaserForm;
        set
        {
            SetProperty(_coreSettings.PreprintedLaserForm, value, _coreSettings, (s, v) => s.PreprintedLaserForm = v);
            _logger?.LogInfo($"PrePrintedLaserForm changed: {value}");
        }
    }
}
