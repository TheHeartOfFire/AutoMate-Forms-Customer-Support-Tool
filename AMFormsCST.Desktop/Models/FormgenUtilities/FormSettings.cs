using AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure;
using AMFormsCST.Desktop.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFormsCST.Desktop.Models.FormgenUtilities;
public class FormSettings : IFormgenFileSettings
{
    public FormSettings(DotFormgenSettings settings)
    {
        Version = settings.Version.ToString();
        PublishedUUID = settings.UUID ?? string.Empty;
        LegacyImport = settings.LegacyImport;
        TotalPages = settings.TotalPages;
        DefaultPoints = settings.DefaultFontSize;
        MissingSourceJpeg = settings.MissingSourceJpeg;
        Duplex = settings.Duplex;
        MaxAccessoryLines = settings.MaxAccessoryLines;
        PrePrintedLaserForm = settings.PreprintedLaserForm;
    }

    public string Version { get; set; } = string.Empty;
    public string PublishedUUID { get; set; } = string.Empty;
    public bool LegacyImport { get; set; }
    public int TotalPages { get; set; }
    public int DefaultPoints { get; set; }
    public bool MissingSourceJpeg { get; set; }
    public bool Duplex { get; set; }
    public int MaxAccessoryLines { get; set; }
    public bool PrePrintedLaserForm { get; set; }

}
