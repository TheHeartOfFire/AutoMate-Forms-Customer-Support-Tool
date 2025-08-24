using AMFormsCST.Core.Interfaces.BestPractices;
using AMFormsCST.Core.Types.BestPractices.Models;
using AMFormsCST.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AMFormsCST.Core.Types.BestPractices.Models.AutoMateFormModel;

namespace AMFormsCST.Core.Types.BestPractices.Practices;
public class AutoMateFormNameBestPractices : IFormNameBestPractice
{
    private readonly ILogService? _logger;

    public IFormModel Model { get; set; }

    public AutoMateFormNameBestPractices(AutoMateFormModel model, ILogService? logger = null)
    {
        Model = model;
        _logger = logger;
        _logger?.LogInfo("AutoMateFormNameBestPractices initialized.");
    }

    public string Generate()
    {
        try
        {
            var model = Model as AutoMateFormModel ?? throw new ArgumentException("Model must be of type AutoMateFormModel", nameof(Model));
            var sb = new StringBuilder();

            var outerOpen = " [";
            var outerClose = ']';
            var innerOpen = " (";
            var innerClose = ')';

            if (model.Format is FormFormat.LegacyImpact)
            {
                outerOpen = " (";
                outerClose = ')';
                innerOpen = " [";
                innerClose = ']';
            }
            // Pre-Encapsulation
            sb.Append(model.State);

            if (!model.State.Equals(string.Empty))
                sb.Append(' ');

            if(model.IsLAW)
                sb.Append("LAW ");

            var coBank = model.Company + model.Bank;

            sb.Append(coBank);

            if (!coBank.Equals(string.Empty))
                sb.Append(' ');

            sb.Append(model.Name);

            // Encapsulation
            var hasEncapsulation = !(model.Code + model.RevisionDate).Equals(string.Empty);
            var hasBoth = !model.Code.Equals(string.Empty) && !model.RevisionDate.Equals(string.Empty);

            if (hasEncapsulation)
                sb.Append(outerOpen);

            if (model.IsLAW && !model.Code.Equals(string.Empty))
                sb.Append("LAW ");

            sb.Append(model.Code);

            if (hasBoth)
                sb.Append(innerOpen);

            sb.Append(model.RevisionDate);

            if (hasBoth)
                sb.Append(innerClose);

            if (hasEncapsulation)
                sb.Append(outerClose);

            // Post-Encapsulation
            var manDeal = model.Manufacturer + model.Dealership;

            if (!manDeal.Equals(string.Empty))
                sb.Append($" ({manDeal})");

            if (model.VehicleType is SoldTrade.Sold)
                sb.Append(" (SOLD)");

            else if (model.VehicleType is SoldTrade.Trade)
                sb.Append(" (TRADE)");

            if (model.IsCustom)
                sb.Append(" -Custom");

            if (model.IsVehicleMerchandising)
                sb.Append(" -VM");

            var result = sb.ToString().Replace('/', '-');
            _logger?.LogInfo($"Form name generated: {result}");
            return result;
        }
        catch (Exception ex)
        {
            _logger?.LogError("Error generating form name.", ex);
            throw;
        }
    }
}
