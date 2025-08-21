using AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure;
using AMFormsCST.Desktop.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Reflection;
using static AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure.DotFormgen;

namespace AMFormsCST.Desktop.Models.FormgenUtilities;

public partial class FormProperties : ObservableObject, IFormgenFileProperties
{
    private readonly DotFormgen _coreFormgenFile;
    public IFormgenFileSettings Settings { get; set; }

    public string? Title
    {
        get => _coreFormgenFile.Title;
        set => SetProperty(_coreFormgenFile.Title, value, _coreFormgenFile, (f, v) => f.Title = v);
    }

    public bool TradePrompt
    {
        get => _coreFormgenFile.TradePrompt;
        set => SetProperty(_coreFormgenFile.TradePrompt, value, _coreFormgenFile, (f, v) => f.TradePrompt = v);
    }

    public Format FormType
    {
        get => _coreFormgenFile.FormType;
        set => SetProperty(_coreFormgenFile.FormType, value, _coreFormgenFile, (f, v) => f.FormType = v);
    }

    public bool SalesPersonPrompt
    {
        get => _coreFormgenFile.SalesPersonPrompt;
        set => SetProperty(_coreFormgenFile.SalesPersonPrompt, value, _coreFormgenFile, (f, v) => f.SalesPersonPrompt = v);
    }

    public string? Username
    {
        get => _coreFormgenFile.Username;
        set => SetProperty(_coreFormgenFile.Username, value, _coreFormgenFile, (f, v) => f.Username = v);
    }

    public string? BillingName
    {
        get => _coreFormgenFile.BillingName;
        set => SetProperty(_coreFormgenFile.BillingName, value, _coreFormgenFile, (f, v) => f.BillingName = v);
    }

    public FormCategory Category
    {
        get => _coreFormgenFile.Category;
        set => SetProperty(_coreFormgenFile.Category, value, _coreFormgenFile, (f, v) => f.Category = v);
    }

    public FormProperties(DotFormgen formgenFile)
    {
        _coreFormgenFile = formgenFile;
        Settings = new FormSettings(formgenFile.Settings);
    }

    public string GetFormType() =>
        FormType switch
        {
            Format.Pdf => "Pdf",
            Format.LegacyLaser => "Legacy Laser",
            Format.Impact => "Impact",
            Format.LegacyImpact => "Legacy Impact",
            Format.ImpactLabelRoll => "Impact Label Roll",
            Format.ImpactLabelSheet => "Impact Label Sheet",
            Format.LaserLabelSheet => "Laser Label Sheet",
            _ => "Unknown"

        };

    public string GetCategory() =>
        Category switch
        {
            FormCategory.Gap => "Gap",
            FormCategory.Maintenance => "Maintenance",
            FormCategory.RebateIncentive => "Rebate Incentive",
            FormCategory.Insurance => "Insurance",
            FormCategory.DealRecap => "Deal Recap",
            FormCategory.Aftermarket => "Aftermarket",
            FormCategory.BuyersGuide => "Buyers Guide",
            FormCategory.Commission => "Commission",
            FormCategory.CreditLifeAH => "Credit Life / AH",
            FormCategory.Custom => "Custom",
            FormCategory.EnvelopeDealJacket => "Envelope / Deal Jacket",
            FormCategory.ExtendedWarranties => "Extended Warranties",
            FormCategory.Label => "Label",
            FormCategory.Lease => "Lease",
            FormCategory.MemberApplication => "Member Application",
            FormCategory.NoticeToCosigner => "Notice To Cosigner",
            FormCategory.NoticeToCustomer => "Notice To Customer",
            FormCategory.Other => "Other",
            FormCategory.PurchaseOrderInvoice => "Purchase Order / Invoice",
            FormCategory.Retail => "Retail",
            FormCategory.StateSpecificDMV => "State Specific DMV",
            FormCategory.WeOweYouOweDueBill => "We Owe / You Owe / Due Bill",
            _ => "Unknown",
        };

    // This method replaces GetUIElements()
    public IEnumerable<DisplayProperty> GetDisplayProperties()
    {
        var formType = typeof(DotFormgen);

        // Editable properties from DotFormgen
        var titleProp = formType.GetProperty(nameof(DotFormgen.Title));
        if (titleProp != null)
            yield return new DisplayProperty(_coreFormgenFile, titleProp, true);

        var formTypeProp = formType.GetProperty(nameof(DotFormgen.FormType));
        if (formTypeProp != null)
            yield return new DisplayProperty(_coreFormgenFile, formTypeProp);

        var categoryProp = formType.GetProperty(nameof(DotFormgen.Category));
        if (categoryProp != null)
            yield return new DisplayProperty(_coreFormgenFile, categoryProp);

        var usernameProp = formType.GetProperty(nameof(DotFormgen.Username));
        if (usernameProp != null)
            yield return new DisplayProperty(_coreFormgenFile, usernameProp);

        var billingNameProp = formType.GetProperty(nameof(DotFormgen.BillingName));
        if (billingNameProp != null)
            yield return new DisplayProperty(_coreFormgenFile, billingNameProp);

        var tradePromptProp = formType.GetProperty(nameof(DotFormgen.TradePrompt));
        if (tradePromptProp != null)
            yield return new DisplayProperty(_coreFormgenFile, tradePromptProp);

        var salesPersonPromptProp = formType.GetProperty(nameof(DotFormgen.SalesPersonPrompt));
        if (salesPersonPromptProp != null)
            yield return new DisplayProperty(_coreFormgenFile, salesPersonPromptProp);

        // Add other properties from FormSettings, etc.
        if (Settings is FormSettings formSettings)
        {
            var settingsType = typeof(FormSettings);

            var uuidProp = settingsType.GetProperty(nameof(FormSettings.PublishedUUID));
            if (uuidProp != null)
                yield return new DisplayProperty(formSettings, uuidProp, true); // UUID is read-only

            var totalPagesProp = settingsType.GetProperty(nameof(FormSettings.TotalPages));
            if (totalPagesProp != null)
                yield return new DisplayProperty(formSettings, totalPagesProp);
        }
    }
}
