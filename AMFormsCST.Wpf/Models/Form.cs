using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFormsCST.Desktop.Models;
public class Form
{
    public string? Name { get; set; } = string.Empty;
    public string? Notes { get; set; } = string.Empty;
    public ObservableCollection<TestDeal> TestDeals { get; set; } = [ new() ];
    public bool Notable { get; set; } = true;
    public FormFormat Format { get; set; } = FormFormat.Pdf;

    public enum FormFormat
    {
        LegacyImpact,
        Pdf
    }

}
