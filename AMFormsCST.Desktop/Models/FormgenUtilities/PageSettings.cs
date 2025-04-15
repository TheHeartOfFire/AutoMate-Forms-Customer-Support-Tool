using AMFormsCST.Desktop.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFormsCST.Desktop.Models.FormgenUtilities;
public class PageSettings : IFormgenFileSettings
{
    public int PageNumber { get; set; }
    public int DefaultFontSize { get; set; }
    public int LeftPrinterMargin { get; set; }
    public int RightPrinterMargin { get; set; }
    public int TopPrinterMargin { get; set; }
    public int BottomPrinterMargin { get; set; }
}
