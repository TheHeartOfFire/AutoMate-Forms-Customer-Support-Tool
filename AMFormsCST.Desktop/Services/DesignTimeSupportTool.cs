using AMFormsCST.Core.Interfaces;
using AMFormsCST.Core.Interfaces.BestPractices;
using AMFormsCST.Core.Interfaces.CodeBlocks;
using AMFormsCST.Core.Interfaces.Notebook;
using AMFormsCST.Core.Interfaces.UserSettings;
using AMFormsCST.Core.Interfaces.Utils;
using AMFormsCST.Core.Types.BestPractices;
using AMFormsCST.Core.Types.BestPractices.Models;
using AMFormsCST.Core.Types.BestPractices.TextTemplates.Models;
using Moq;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using static AMFormsCST.Core.Types.BestPractices.TextTemplates.Models.TextTemplate;

namespace AMFormsCST.Desktop.Services;

/// <summary>
/// Provides a mock implementation of ISupportTool for use in design-time scenarios.
/// This class simulates the behavior of the real support tool and its dependencies.
/// </summary>
public class DesignTimeSupportTool : ISupportTool
{
    public ICodeBlocks CodeBlocks { get; set; }
    public IBestPracticeEnforcer Enforcer { get; set; }
    public IFormgenUtils FormgenUtils { get; set; }
    public INotebook Notebook { get; set; }
    public ISettings Settings { get; set; }

    public DesignTimeSupportTool()
    {
        // Mock the dependency chain required by the Form model
        var mockFormNamePractice = new Mock<IFormNameBestPractice>();
        var mockEnforcer = new Mock<IBestPracticeEnforcer>();
        mockFormNamePractice.SetupProperty(p => p.Model, new AutoMateFormModel
        {
            Format = AutoMateFormModel.FormFormat.Pdf,
            IsLAW = false,
            State = "CA",
            Bank = "Bank of America",
            Name = "Auto Loan Agreement",
            Code = "ALA123",
            RevisionDate = "2024-01-01",
            Manufacturer = "Toyota",
            Dealership = "Best Cars",
            VehicleType = AutoMateFormModel.SoldTrade.Sold,
            IsCustom = true,
            IsVehicleMerchandising = false
        });

        // When GetFormName is called, execute our design-time string generation logic
        mockEnforcer.Setup(e => e.GetFormName()).Returns(() =>
        {
            var model = mockFormNamePractice.Object.Model as AutoMateFormModel;
            if (model == null) return string.Empty;

            if (model.Format == AutoMateFormModel.FormFormat.Pdf)
            {
                return model.IsLAW ? GenerateLaserLawName(model) : GenerateLaserName(model);
            }
            return model.IsLAW ? GenerateImpactLawName(model) : GenerateImpactName(model);
        });

        // Setup mock for Templates
        var templates = new List<TextTemplate>
        {
            new("Case Intro", "Standard introduction for a new case.", "Hello {FirstName}, this is in regards to case #{CaseNumber}.", TemplateType.InternalComments),
            new("Form Issue", "Template for reporting a form issue.", "The form {FormName} is having an issue. Details: {IssueDetails}", TemplateType.Other),
            new("Closing", "Standard case closing.", "Thank you for your time. Case #{CaseNumber} will now be closed.", TemplateType.Email)
        };
        mockEnforcer.Setup(e => e.Templates).Returns(templates);

        mockEnforcer.Setup(e => e.FormNameBestPractice).Returns(mockFormNamePractice.Object);
        Enforcer = mockEnforcer.Object;

        // Initialize other properties with basic mocks to avoid null reference exceptions
        CodeBlocks = new Mock<ICodeBlocks>().Object;
        FormgenUtils = new Mock<IFormgenUtils>().Object;
        Notebook = new Mock<INotebook>().Object;
        Settings = new Mock<ISettings>().Object;
    }

    public void SaveAllSettings() { /* No-op for design time */ }

    #region File Name Generation Logic
    // This logic is adapted from your FormgenAssistant project to simulate the real behavior.
    private static string GenerateLaserLawName(AutoMateFormModel model)
    {
        var parts = new List<string>
        {
            "LAW",
            model.Bank,
            CultureInfo.InvariantCulture.TextInfo.ToTitleCase(model.Name)
        };

        var bracketParts = new List<string> { "LAW", model.Code, $"({model.RevisionDate})" };
        if (bracketParts.Any(s => !string.IsNullOrEmpty(s)))
            parts.Add($"[{string.Join(" ", bracketParts.Where(s => !string.IsNullOrEmpty(s)))}]");

        if (!string.IsNullOrEmpty(model.Manufacturer) || !string.IsNullOrEmpty(model.Dealership))
            parts.Add($"({string.Join(" ", new[] { model.Manufacturer, model.Dealership }.Where(s => !string.IsNullOrEmpty(s)))})");

        if (model.VehicleType == AutoMateFormModel.SoldTrade.Sold) parts.Add("(SOLD)");
        if (model.VehicleType == AutoMateFormModel.SoldTrade.Trade) parts.Add("(TRADE)");
        if (model.IsCustom) parts.Add("- Custom");

        return string.Join(" ", parts.Where(s => !string.IsNullOrEmpty(s))).Replace('/', '-');
    }

    private static string GenerateLaserName(AutoMateFormModel model)
    {
        var parts = new List<string> { model.State, model.Bank, CultureInfo.InvariantCulture.TextInfo.ToTitleCase(model.Name) };

        var bracketParts = new List<string>();
        if (!string.IsNullOrEmpty(model.Code)) bracketParts.Add(model.Code.ToUpperInvariant());
        if (!string.IsNullOrEmpty(model.RevisionDate)) bracketParts.Add($"({model.RevisionDate})");
        if (!string.IsNullOrEmpty(model.Manufacturer) || !string.IsNullOrEmpty(model.Dealership)) bracketParts.Add($"({string.Join(" ", new[] { model.Manufacturer, model.Dealership }.Where(s => !string.IsNullOrEmpty(s)))})");

        if (bracketParts.Count > 0)
            parts.Add($"[{string.Join(" ", bracketParts)}]");

        if (model.VehicleType == AutoMateFormModel.SoldTrade.Sold) parts.Add("(SOLD)");
        if (model.VehicleType == AutoMateFormModel.SoldTrade.Trade) parts.Add("(TRADE)");
        if (model.IsCustom) parts.Add("- Custom");
        if (model.IsVehicleMerchandising) parts.Add("- VM");

        return string.Join(" ", parts.Where(s => !string.IsNullOrEmpty(s))).Replace('/', '-');
    }

    private static string GenerateImpactLawName(AutoMateFormModel model)
    {
        var parts = new List<string>
        {
            "LAW",
            model.Code?.ToUpperInvariant(),
            model.RevisionDate,
            model.Bank,
            CultureInfo.InvariantCulture.TextInfo.ToTitleCase(model.Name)
        };

        if (!string.IsNullOrEmpty(model.Manufacturer) || !string.IsNullOrEmpty(model.Dealership))
            parts.Add($"({string.Join(" ", new[] { model.Manufacturer, model.Dealership }.Where(s => !string.IsNullOrEmpty(s)))})");

        if (model.VehicleType == AutoMateFormModel.SoldTrade.Sold) parts.Add("(SOLD)");
        if (model.VehicleType == AutoMateFormModel.SoldTrade.Trade) parts.Add("(TRADE)");
        if (model.IsCustom) parts.Add("- Custom");
        if (model.IsVehicleMerchandising) parts.Add("- VM");

        return string.Join(" ", parts.Where(s => !string.IsNullOrEmpty(s))).Replace('/', '-');
    }

    private static string GenerateImpactName(AutoMateFormModel model)
    {
        var parts = new List<string> { model.State, model.Bank, CultureInfo.InvariantCulture.TextInfo.ToTitleCase(model.Name) };

        var bracketParts = new List<string>();
        if (!string.IsNullOrEmpty(model.Code)) bracketParts.Add(model.Code.ToUpperInvariant());
        if (!string.IsNullOrEmpty(model.RevisionDate)) bracketParts.Add($"[{model.RevisionDate}]");
        if (!string.IsNullOrEmpty(model.Manufacturer) || !string.IsNullOrEmpty(model.Dealership)) bracketParts.Add($"({string.Join(" ", new[] { model.Manufacturer, model.Dealership }.Where(s => !string.IsNullOrEmpty(s)))})");

        if (bracketParts.Count > 0)
            parts.Add($"({string.Join(" ", bracketParts)})");

        if (model.VehicleType == AutoMateFormModel.SoldTrade.Sold) parts.Add("(SOLD)");
        if (model.VehicleType == AutoMateFormModel.SoldTrade.Trade) parts.Add("(TRADE)");
        if (model.IsCustom) parts.Add("- Custom");
        if (model.IsVehicleMerchandising) parts.Add("- VM");

        return string.Join(" ", parts.Where(s => !string.IsNullOrEmpty(s))).Replace('/', '-');
    }
    #endregion
}