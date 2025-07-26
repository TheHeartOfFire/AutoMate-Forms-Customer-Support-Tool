using AMFormsCST.Core.Interfaces;
using AMFormsCST.Core.Interfaces.BestPractices;
using AMFormsCST.Core.Interfaces.Utils;
using AMFormsCST.Core.Types.BestPractices.TextTemplates.Models;
using AMFormsCST.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFormsCST.Core;
public class SupportTool : ISupportTool
{
    public IBestPracticeEnforcer Enforcer { get; set; }
    public ICodeBlocks CodeBlocks { get; set; }
    public IFormgenUtils FormgenUtils { get; set; }
    public INotebook Notebook { get; set; }
    public IReadOnlyCollection<ITextTemplateVariable> Variables { get; private set; }
    private readonly Properties _properties = new();

    #region To Become Settings
    private string _extSeparator = "x ";
    #endregion

    public SupportTool(IFormNameBestPractice formNameBestPractice)
    {
        _properties = new Properties();
        CodeBlocks = new CodeBlocks();
        Notebook = new Notebook();
        FormgenUtils = new FormgenUtils(_properties.FormgenUtils);
        Enforcer = new BestPracticeEnforcer(formNameBestPractice);
        Variables = RegisterVariables();
    }
    #region Template Variable Registration
    private List<ITextTemplateVariable> RegisterVariables()
    {
        var variables = new List<ITextTemplateVariable>
        {
            new TextTemplateVariable(
             properName: "Notes:ServerID",
             name: "serverid",
             prefix: "notes:",
             description: "Server ID#",
             aliases: [ "server", "serv"],
             getValue: () => Notebook.CurrentNote.ServerId ?? string.Empty
            ),
            new TextTemplateVariable(
             properName: "Notes:Companies",
             name: "companies",
             prefix: "notes:",
             description:"Company#(s)",
             aliases: ["company", "comp", "co"],
             getValue: () => Notebook.CurrentNote.Companies ?? string.Empty
            ),
            new TextTemplateVariable(
             properName: "Notes:Dealership",
             name: "dealership",
             prefix: "notes:",
             description:"Dealership Name",
             aliases: ["dealer", "dlr"],
             getValue: () => Notebook.CurrentNote.Dealership ?? string.Empty
            ),
            new TextTemplateVariable(
             properName: "Notes:ContactName",
             name: "contactname",
             prefix: "notes:",
             description:"Contact Name",
             aliases: ["name"],
             getValue: () => Notebook.CurrentNote.ContactName ?? string.Empty
            ),
            new TextTemplateVariable(
             properName: "Notes:EmailAddress",
             name: "emailaddress",
             prefix: "notes:",
             description:"E-Mail Address",
             aliases: ["email"],
             getValue: () => Notebook.CurrentNote.Email ?? string.Empty
            ),
            new TextTemplateVariable(
             properName: "Notes:Phone",
             name: "phone",
             prefix: "notes:",
             description:"Phone#",
             aliases: [],
             getValue: () => Notebook.CurrentNote.Phone ?? string.Empty
            ),
            new TextTemplateVariable(
             properName: "Notes:Notes",
             name: "notes",
             prefix: "notes:",
             description:"Notes",
             aliases: [],
             getValue: () => Notebook.CurrentNote.NotesText ?? string.Empty
            ),
            new TextTemplateVariable(
             properName: "Notes:CaseNumber",
             name: "casenumber",
             prefix: "notes:",
             description:"Case#",
             aliases: [ "caseno", "case" ],
             getValue: () => Notebook.CurrentNote.CaseText ?? string.Empty
            ),
            new TextTemplateVariable(
             properName: "Notes:Forms",
             name: "forms",
             prefix: "notes:",
             description:"Forms",
             aliases: [ "form" ],
             getValue: () => Notebook.CurrentNote.FormsText ?? string.Empty
            ),
            new TextTemplateVariable(
             properName: "Notes:FirstName",
             name: "firstname",
             prefix: "notes:",
             description:"First Name",
             aliases: [],
             getValue: () =>
             {

                 if(!string.IsNullOrEmpty(Notebook.CurrentNote.ContactName) && Notebook.CurrentNote.ContactName.Contains(' '))
                     return Notebook.CurrentNote.ContactName.Split(' ').FirstOrDefault() ?? string.Empty;

                   return string.Empty;
             }
            ),
            new TextTemplateVariable(
             properName: "AMMail:FullAddress",
             name: "fulladdress",
             prefix: "ammail:",
             description:"AutoMate Forms Mailing Address",
             aliases: [ "all", "full", "mailingaddress", "mailto" ],
             getValue: () => string.Empty //TODO: Pull Mailing Address from persistent data
            ),
            new TextTemplateVariable(
             properName: "AMMail:Name",
             name: "name",
             prefix: "ammail:",
             description:"AutoMate Forms Mailing Address - Name",
             aliases: [],
             getValue: () => string.Empty //TODO: Pull Mailing Address from persistent data
            ),
            new TextTemplateVariable(
             properName: "AMMail:Street",
             name: "streetaddress",
             prefix: "ammail:",
             description:"AutoMate Forms Mailing Address - Street Address",
             aliases: [ "street", "line1" ],
             getValue: () => string.Empty //TODO: Pull Mailing Address from persistent data
            ),
            new TextTemplateVariable(
             properName: "AMMail:City",
             name: "city",
             prefix: "ammail:",
             description:"AutoMate Forms Mailing Address - City",
             aliases: [],
             getValue: () => string.Empty //TODO: Pull Mailing Address from persistent data
            ),
            new TextTemplateVariable(
             properName: "AMMail:State",
             name: "state",
             prefix: "ammail:",
             description:"AutoMate Forms Mailing Address - State",
             aliases: [],
             getValue: () => string.Empty //TODO: Pull Mailing Address from persistent data
            ),
            new TextTemplateVariable(
             properName: "AMMail:ZipCode",
             name: "zipcode",
             prefix: "ammail:",
             description:"AutoMate Forms Mailing Address - Zip Code",
             aliases: [ "postalcode", "zip" ],
             getValue: () => string.Empty //TODO: Pull Mailing Address from persistent data
            ),
            new TextTemplateVariable(
             properName: "AMMail:CityStateZip",
             name: "citystatezip",
             prefix: "ammail:",
             description:"AutoMate Forms Mailing Address - City, State Zip",
             aliases: [ "csz", "line2" ],
             getValue: () => string.Empty //TODO: Pull Mailing Address from persistent data
            ),
            new TextTemplateVariable(
             properName: "FormNameGenerator:FormName",
             name: "formname",
             prefix: "formnamegenerator:",
             description:"Form Name Generator - Form Name",
             aliases: [ "generate", "name", "output" ],
             getValue: () => Enforcer.GetFormName()
            ),
            new TextTemplateVariable(
             properName: "Notes:DealNumber",
             name: "dealnumber",
             prefix: "notes:",
             description:"Deal#",
             aliases: [ "dealno", "deal" ],
             getValue: () => Notebook.CurrentNote.DealText ?? string.Empty
            ),
        };
        return variables;
    }
    #endregion
}
