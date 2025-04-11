using AMFormsCST.Core.Utils;
using AMFormsCST.Desktop.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using static AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure.DotFormgen;

namespace AMFormsCST.Desktop.Models.FormgenUtilities;
public class FormProperties : IFormgenFileProperties
{
    public IFormgenFileSettings Settings { get; set; } = new FormSettings();
    public string Title { get; set; } = string.Empty;
    public bool TradePrompt { get; set; }
    public Format FormType { get; set; }
    public bool SalesPersonPrompt { get; set; }
    public string Username { get; set; } = string.Empty;
    public string BillingName { get; set; } = string.Empty;
    public FormCategory Category { get; set; }

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

    public UIElement GetUIElements() => BasicStats.GetSettingsAndPropertiesUIElements(this);

}
