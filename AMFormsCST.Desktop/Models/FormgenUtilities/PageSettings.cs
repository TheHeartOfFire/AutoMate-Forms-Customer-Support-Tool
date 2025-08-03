using AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure;
using AMFormsCST.Desktop.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AMFormsCST.Desktop.Models.FormgenUtilities;

public partial class PageSettings : ObservableObject, IFormgenFileSettings
{
    private readonly FormPageSettings _coreSettings;

    public PageSettings(FormPageSettings settings)
    {
        _coreSettings = settings;
    }

    public int PageNumber
    {
        get => _coreSettings.PageNumber;
        set => SetProperty(_coreSettings.PageNumber, value, _coreSettings, (s, v) => s.PageNumber = v);
    }

    public int DefaultFontSize
    {
        get => _coreSettings.DefaultFontSize;
        set => SetProperty(_coreSettings.DefaultFontSize, value, _coreSettings, (s, v) => s.DefaultFontSize = v);
    }

    public int LeftPrinterMargin
    {
        get => _coreSettings.LeftPrinterMargin;
        set => SetProperty(_coreSettings.LeftPrinterMargin, value, _coreSettings, (s, v) => s.LeftPrinterMargin = v);
    }

    public int RightPrinterMargin
    {
        get => _coreSettings.RightPrinterMargin;
        set => SetProperty(_coreSettings.RightPrinterMargin, value, _coreSettings, (s, v) => s.RightPrinterMargin = v);
    }

    public int TopPrinterMargin
    {
        get => _coreSettings.TopPrinterMargin;
        set => SetProperty(_coreSettings.TopPrinterMargin, value, _coreSettings, (s, v) => s.TopPrinterMargin = v);
    }

    public int BottomPrinterMargin
    {
        get => _coreSettings.BottomPrinterMargin;
        set => SetProperty(_coreSettings.BottomPrinterMargin, value, _coreSettings, (s, v) => s.BottomPrinterMargin = v);
    }
}
