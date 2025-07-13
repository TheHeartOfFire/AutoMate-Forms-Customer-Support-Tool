using AMFormsCST.Desktop.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFormsCST.Desktop.Models.FormgenUtilities;
public class FieldStats : BasicStats, IFormgenFileProperties
{
    public int Fields { get; set; }
    public int Pages { get; set; }
}
