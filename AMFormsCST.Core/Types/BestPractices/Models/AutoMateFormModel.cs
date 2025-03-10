using AMFormsCST.Core.Interfaces.BestPractices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFormsCST.Core.Types.BestPractices.Models;
public class AutoMateFormModel
{
    public string Name { get; } = string.Empty;
    public string Code { get; } = string.Empty;
    public string RevisionDate { get; } = string.Empty;
    public string Company { get; } = string.Empty;
    public string Bank { get; } = string.Empty;
    public string Manufacturer { get; } = string.Empty;
    public string Dealership { get; } = string.Empty;
    public string State { get; } = string.Empty;
    public bool IsCustom { get; } = false;
    public bool IsLAW { get; } = false;
    public NewUsed VehicleType { get; } = NewUsed.None;
    public FormFormat Format { get; } = FormFormat.LegacyImpact;


    public enum NewUsed
    {
        None,
        New,
        Used
    }
    public enum FormFormat
    {
        LegacyImpact,
        Pdf
    }
}
