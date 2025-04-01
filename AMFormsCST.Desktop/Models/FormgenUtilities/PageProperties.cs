using AMFormsCST.Desktop.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFormsCST.Desktop.Models.FormgenUtilities;
public class PageProperties : BasicStats
{
    public new IFormgenFileSettings Settings { get; set; } = new PageSettings();
}
