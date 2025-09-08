using AMFormsCST.Core.Interfaces;
using AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure;
using AMFormsCST.Desktop.Interfaces;
using System.Drawing;
using static AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure.FormFieldSettings;

namespace AMFormsCST.Desktop.Models.FormgenUtilities;
public class FieldSettings : IFormgenFileSettings
{
    private readonly ILogService? _logger;

    public FieldSettings(FormFieldSettings? settings = null, ILogService? logger = null)
    {
        _logger = logger;
        if (settings != null)
        {
            DecimalPlaces = settings.DecimalPlaces;
            DisplayPartial = settings.DisplayPartial;
            EndIndex = settings.EndIndex;
            FontSize = settings.FontSize;
            FontAlignment = settings.FontAlignment;
            ID = settings.ID;
            ImpactPosition = settings.ImpactPosition;
            Kerning = settings.Kearning;
            LaserRect = settings.LaserRect;
            Length = settings.Length;
            ManualSize = settings.ManualSize;
            Bold = settings.Bold;
            ShrinkToFit = settings.ShrinkToFit;
            StartIndex = settings.StartIndex;
            Type = settings.Type;
            _logger?.LogInfo($"FieldSettings initialized: ID={ID}, Type={Type}, FontSize={FontSize}");
        }
    }

    public int DecimalPlaces { get; set; }
    public bool DisplayPartial { get; set; }
    public int EndIndex { get; set; }
    public int FontSize { get; set; }
    public Alignment FontAlignment { get; set; }
    public int ID { get; set; }
    public Point ImpactPosition { get; set; }
    public int Kerning { get; set; }
    public Rectangle LaserRect { get; set; }
    public int Length { get; set; }
    public bool ManualSize { get; set; }
    public bool Bold { get; set; }
    public bool ShrinkToFit { get; set; }
    public int StartIndex { get; set; }
    public FieldType Type { get; set; }

    public string GetFontAlignment() => FontAlignment switch
    {
        Alignment.LEFT => "LEFT",
        Alignment.CENTER => "CENTER",
        Alignment.RIGHT => "RIGHT",
        _ => "LEFT"
    };

    public string GetFieldType() => Type switch
    {
        FieldType.TEXT => "TEXT",
        FieldType.NUMERIC => "NUMERIC",
        FieldType.SIGNATURE => "SIGNATURE",
        FieldType.INITIALS => "INITIALS",
        _ => "TEXT"
    };
}
