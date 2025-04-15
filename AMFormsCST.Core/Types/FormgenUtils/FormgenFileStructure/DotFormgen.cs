using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure
{
    public class DotFormgen
    {
        public DotFormgenSettings Settings { get; set; }
        public List<FormPage> Pages { get; set; } = [];
        public string? Title { get; set; }
        public bool TradePrompt { get; set; }
        public Format FormType { get; set; }
        public bool SalesPersonPrompt { get; set; }
        public string? Username { get; set; }
        public string? BillingName { get; set; }
        public List<CodeLine> CodeLines { get; set; } = [];
        public FormCategory Category { get; set; }
        public List<string> States { get; set; } = [];

        public enum Format
        {
            Impact,
            ImpactLabelRoll,
            ImpactLabelSheet,
            LaserLabelSheet,
            LegacyImpact,
            LegacyLaser,
            Pdf
        }

        public enum FormCategory
        {
            Aftermarket,
            BuyersGuide,
            Commission,
            CreditLifeAH,
            Custom,
            DealRecap,
            EnvelopeDealJacket,
            ExtendedWarranties,
            Gap,
            Insurance,
            Label,
            Lease,
            Maintenance,
            MemberApplication,
            NoticeToCosigner,
            NoticeToCustomer,
            Other,
            PurchaseOrderInvoice,
            RebateIncentive,
            Retail,
            StateSpecificDMV,
            WeOweYouOweDueBill
        }
        public DotFormgen(XmlElement document)
        {
            Settings = new DotFormgenSettings(document.Attributes);
            foreach (XmlNode node in document.ChildNodes)
            {
                switch (node.Name)
                {
                    case "pages":
                        Pages.Add(new FormPage(node));
                        break;
                    case "title":
                        Title = node.InnerText;
                        break;
                    case "tradePrompt":
                        if (bool.TryParse(node.InnerText, out var parsedBool))
                            TradePrompt = parsedBool;
                        break;
                    case "formPrintType":
                        FormType = GetFormat(node.InnerText);
                        break;
                    case "salespersonPrompt":
                        if (bool.TryParse(node.InnerText, out parsedBool))
                            SalesPersonPrompt = parsedBool;
                        break;
                    case "username":
                        Username = node.InnerText;
                        break;
                    case "billingName":
                        BillingName = node.InnerText;
                        break;
                    case "codeLines":
                        CodeLines.Add(new CodeLine(node));
                        break;
                    case "formCategory":
                        Category = GetCategory(node.InnerText);
                        break;
                    case "validStates":
                        States.Add(node.InnerText);
                        break;
                }
            }
        }

        public CodeLine GetPrompt(int index)
        {
            var prompts = CodeLines.Where(x => x.Settings is { Type: CodeLineSettings.CodeType.PROMPT }).ToList();

            return prompts[index];
        }

        public FormField GetField(int index)
        {
            var fields = new List<FormField>();
            foreach (var page in Pages) fields.AddRange(page.Fields);

            return fields[index];
        }
        public int FieldCount()
        {
            var count = 0;
            foreach (var page in Pages)
                foreach (var field in page.Fields)
                    count++;

            return count;
        }
        public int InitCount()
        {
            return CodeLines.Count(x => x.Settings is { Type: CodeLineSettings.CodeType.INIT });
        }
        public int PromptCount()
        {
            return CodeLines.Count(x => x.Settings is { Type: CodeLineSettings.CodeType.PROMPT });
        }
        public int PostCount()
        {
            return CodeLines.Count(x => x.Settings is { Type: CodeLineSettings.CodeType.POST });
        }
        public CodeLine ClonePrompt(CodeLine prompt, string? newName, int newIndex)
        {
            var cl = new CodeLine(prompt, newName, newIndex);
            CodeLines.Add(cl);
            return cl;
        }
        public static Format GetFormat(string format) => format switch
        {
            "Pdf" => Format.Pdf,
            "LegacyImpact" => Format.LegacyImpact,
            _ => Format.Pdf,
        };

        public static string GetFormat(Format format) => format switch
        {
            Format.Pdf => "Pdf",
            Format.LegacyImpact => "LegacyImpact",
            _ => "Pdf",
        };

        public static FormCategory GetCategory(string category) => category switch
        {
            "Aftermarket" => FormCategory.Aftermarket,
            "BuyersGuide" => FormCategory.BuyersGuide,
            "Commission" => FormCategory.Commission,
            "CreditLifeAH" => FormCategory.CreditLifeAH,
            "Custom" => FormCategory.Custom,
            "DealRecap" => FormCategory.DealRecap,
            "EnvelopeDealJacket" => FormCategory.EnvelopeDealJacket,
            "ExtendedWarranties" => FormCategory.ExtendedWarranties,
            "Gap" => FormCategory.Gap,
            "Insurance" => FormCategory.Insurance,
            "Label" => FormCategory.Label,
            "Lease" => FormCategory.Lease,
            "Maintenance" => FormCategory.Maintenance,
            "MemberApplication" => FormCategory.MemberApplication,
            "NoticeToCoSigner" => FormCategory.NoticeToCosigner,
            "NoticeToCustomer" => FormCategory.NoticeToCustomer,
            "Other" => FormCategory.Other,
            "PurchaseOrderInvoice" => FormCategory.PurchaseOrderInvoice,
            "RebateIncentive" => FormCategory.RebateIncentive,
            "Retail" => FormCategory.Retail,
            "StateSpecificDMV" => FormCategory.StateSpecificDMV,
            "WeOweYouOweDueBill" => FormCategory.WeOweYouOweDueBill,
            _ => FormCategory.Other,

        };

        public static string GetCategory(FormCategory category) => category switch
        {
            FormCategory.Aftermarket => "Aftermarket",
            FormCategory.BuyersGuide => "BuyersGuide",
            FormCategory.Commission => "Commission",
            FormCategory.CreditLifeAH => "CreditLifeAH",
            FormCategory.Custom => "Custom",
            FormCategory.DealRecap => "DealRecap",
            FormCategory.EnvelopeDealJacket => "EnvelopeDealJacket",
            FormCategory.ExtendedWarranties => "ExtendedWarranties",
            FormCategory.Gap => "Gap",
            FormCategory.Insurance => "Insurance",
            FormCategory.Label => "Label",
            FormCategory.Lease => "Lease",
            FormCategory.Maintenance => "Maintenance",
            FormCategory.MemberApplication => "MemberApplication",
            FormCategory.NoticeToCosigner => "NoticeToCoSigner",
            FormCategory.NoticeToCustomer => "NoticeToCustomer",
            FormCategory.Other => "Other",
            FormCategory.PurchaseOrderInvoice => "PurchaseOrderInvoice",
            FormCategory.RebateIncentive => "RebateIncentive",
            FormCategory.Retail => "Retail",
            FormCategory.StateSpecificDMV => "StateSpecificDMV",
            FormCategory.WeOweYouOweDueBill => "WeOweYouOweDueBill",
            _ => "Other",

        };

        public string GenerateXML()
        {
            var output = new StringBuilder();
            var sw = new StringWriterWithEncoding(output, Encoding.UTF8);
            var xml = XmlWriter.Create(sw, new XmlWriterSettings() { Indent = true });

            xml.WriteStartDocument(true);

            xml.WriteStartElement("formDef");
            Settings.GenerateXML(xml);

            foreach (var page in Pages)
                page.GenerateXml(xml);

            xml.WriteStartElement("title");
            xml.WriteString(Title);
            xml.WriteEndElement();

            xml.WriteStartElement("tradePrompt");
            xml.WriteString(TradePrompt.ToString().ToLowerInvariant());
            xml.WriteEndElement();

            xml.WriteStartElement("formPrintType");
            xml.WriteString(GetFormat(FormType));
            xml.WriteEndElement();

            xml.WriteStartElement("salespersonPrompt");
            xml.WriteString(SalesPersonPrompt.ToString().ToLowerInvariant());
            xml.WriteEndElement();

            xml.WriteStartElement("username");
            xml.WriteString(Username);
            xml.WriteEndElement();

            if (BillingName != null)
            {
                xml.WriteStartElement("billingName");
                xml.WriteString(BillingName);
                xml.WriteEndElement();
            }

            foreach (var line in CodeLines.Where(line => line.Settings is { Type: CodeLineSettings.CodeType.INIT }))
            {
                line.GenerateXml(xml);
            }

            foreach (var line in CodeLines.Where(line => line.Settings is { Type: CodeLineSettings.CodeType.PROMPT }))
            {
                line.GenerateXml(xml);
            }

            foreach (var line in CodeLines.Where(line => line.Settings is { Type: CodeLineSettings.CodeType.POST }))
            {
                line.GenerateXml(xml);
            }


            xml.WriteStartElement("formCategory");
            xml.WriteString(GetCategory(Category));
            xml.WriteEndElement();

            foreach (var state in States)
            {
                xml.WriteStartElement("validStates");
                xml.WriteString(state);
                xml.WriteEndElement();
            }

            xml.WriteEndDocument();
            xml.Close();
            return output.ToString();
        }
    }
    public class StringWriterWithEncoding(StringBuilder sb, Encoding encoding) : StringWriter(sb)
    {
        public override Encoding Encoding { get; } = encoding;
    }
}
