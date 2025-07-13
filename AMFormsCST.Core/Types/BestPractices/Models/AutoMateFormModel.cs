using AMFormsCST.Core.Interfaces.BestPractices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFormsCST.Core.Types.BestPractices.Models;
public class AutoMateFormModel : IFormModel
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string RevisionDate { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public string Bank { get; set; } = string.Empty;
    public string Manufacturer { get; set; } = string.Empty;
    public string Dealership { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public bool IsCustom { get; set; } = false;
    public bool IsLAW { get; set; } = false;
    public SoldTrade VehicleType { get; set; } = SoldTrade.None;
    public FormFormat Format { get; set; } = FormFormat.LegacyImpact;


    public enum SoldTrade
    {
        None,
        Sold,
        Trade
    }
    public enum FormFormat
    {
        LegacyImpact,
        Pdf
    }
}
