using AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure;
using AMFormsCST.Desktop.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AMFormsCST.Desktop.Models.FormgenUtilities;

public partial class FormSettings : ObservableObject, IFormgenFileSettings
{
    private readonly DotFormgenSettings _coreSettings;

    public FormSettings(DotFormgenSettings settings)
    {
        _coreSettings = settings;
    }

    public string Version
    {
        get => _coreSettings.Version.ToString();
        // Version is typically read-only, so no setter is provided.
    }

    public string PublishedUUID
    {
        get => _coreSettings.UUID;
        set => SetProperty(_coreSettings.UUID, value, _coreSettings, (s, v) => s.UUID = v);
    }

    public bool LegacyImport
    {
        get => _coreSettings.LegacyImport;
        set => SetProperty(_coreSettings.LegacyImport, value, _coreSettings, (s, v) => s.LegacyImport = v);
    }

    public int TotalPages
    {
        get => _coreSettings.TotalPages;
        set => SetProperty(_coreSettings.TotalPages, value, _coreSettings, (s, v) => s.TotalPages = v);
    }

    public int DefaultPoints
    {
        get => _coreSettings.DefaultFontSize;
        set => SetProperty(_coreSettings.DefaultFontSize, value, _coreSettings, (s, v) => s.DefaultFontSize = v);
    }

    public bool MissingSourceJpeg
    {
        get => _coreSettings.MissingSourceJpeg;
        set => SetProperty(_coreSettings.MissingSourceJpeg, value, _coreSettings, (s, v) => s.MissingSourceJpeg = v);
    }

    public bool Duplex
    {
        get => _coreSettings.Duplex;
        set => SetProperty(_coreSettings.Duplex, value, _coreSettings, (s, v) => s.Duplex = v);
    }

    public int MaxAccessoryLines
    {
        get => _coreSettings.MaxAccessoryLines;
        set => SetProperty(_coreSettings.MaxAccessoryLines, value, _coreSettings, (s, v) => s.MaxAccessoryLines = v);
    }

    public bool PrePrintedLaserForm
    {
        get => _coreSettings.PreprintedLaserForm;
        set => SetProperty(_coreSettings.PreprintedLaserForm, value, _coreSettings, (s, v) => s.PreprintedLaserForm = v);
    }
}
