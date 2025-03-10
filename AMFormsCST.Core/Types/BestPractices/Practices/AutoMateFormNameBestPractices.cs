using AMFormsCST.Core.Interfaces.BestPractices;
using AMFormsCST.Core.Types.BestPractices.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AMFormsCST.Core.Types.BestPractices.Models.AutoMateFormModel;

namespace AMFormsCST.Core.Types.BestPractices.Practices;
public class AutoMateFormNameBestPractices(AutoMateFormModel model) : IFormNameBestPractice
{
    public AutoMateFormModel Model { get; } = model;

    public string Generate()
    {
        var sb = new StringBuilder();

        var outerOpen = " [";
        var outerClose = ']';
        var innerOpen = '(';
        var innerClose = ')';

        if (Model.Format is FormFormat.LegacyImpact)
        {
            outerOpen = " (";
            outerClose = ')';
            innerOpen = ']';
            innerClose = ']';
        }
        // Pre-Encapsulation
        sb.Append(Model.State);

        if (!Model.State.Equals(string.Empty))
            sb.Append(' ');

        if(Model.IsLAW)
            sb.Append("LAW ");

        var coBank = Model.Company + Model.Bank;

        sb.Append(coBank);

        if (!coBank.Equals(string.Empty))
            sb.Append(' ');

        sb.Append(Model.Name);

        if (!Model.Name.Equals(string.Empty))
            sb.Append(' ');

        // Encapsulation
        var hasEncapsulation = !(Model.Code + Model.RevisionDate).Equals(string.Empty);
        var hasBoth = !Model.Code.Equals(string.Empty) && !Model.RevisionDate.Equals(string.Empty);

        if (hasEncapsulation)
            sb.Append(outerOpen);

        if (Model.IsLAW && !Model.Code.Equals(string.Empty))
            sb.Append("LAW ");

        sb.Append(Model.Code);

        if (!Model.Code.Equals(string.Empty))
            sb.Append(' ');

        if (hasBoth)
            sb.Append(innerOpen);

        sb.Append(Model.RevisionDate);

        if (hasBoth)
            sb.Append(innerClose);

        if (hasEncapsulation)
            sb.Append(outerClose);

        // Post-Encapsulation
        var manDeal = Model.Manufacturer + Model.Dealership;

        if (manDeal.Equals(string.Empty))
            sb.Append($" ({manDeal})");

        if (Model.VehicleType is NewUsed.Used)
            sb.Append(" (SOLD)");

        else if (Model.VehicleType is NewUsed.New)
            sb.Append(" (TRADE)");

        if (Model.IsCustom)
            sb.Append(" - Custom");

        return sb.ToString().Replace('/', '-');
    }
}
