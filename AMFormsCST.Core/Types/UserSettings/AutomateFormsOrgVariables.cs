using AMFormsCST.Core.Interfaces.BestPractices;
using AMFormsCST.Core.Interfaces.UserSettings;
using AMFormsCST.Core.Interfaces.Utils;
using AMFormsCST.Core.Types.BestPractices.TextTemplates.Models;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AMFormsCST.Core.Types.UserSettings;
public class AutomateFormsOrgVariables : IOrgVariables
{
    [JsonIgnore]
    public IBestPracticeEnforcer? Enforcer { get; internal set; }
    [JsonIgnore]
    public INotebook? Notebook { get; internal set; }

    public Dictionary<string, string> LooseVariables { get; set; } =
        new()
        {
            { "AMMailingAddress", string.Empty },
            { "AMStreetAddress", string.Empty },
            { "AMCityStateZip", string.Empty },
            { "AMCity", string.Empty },
            { "AMState", string.Empty },
            { "AMZip", string.Empty },
        };

    [JsonIgnore]
    public List<ITextTemplateVariable> Variables { get; private set; }


    // Constructor for Dependency Injection
    public AutomateFormsOrgVariables(IBestPracticeEnforcer? enforcer, INotebook? notebook)
    {
        Enforcer = enforcer;
        Notebook = notebook;
        Variables = RegisterVariables();
    }
    public void InstantiateVariables(IBestPracticeEnforcer enforcer, INotebook notebook)
    {
        Enforcer = enforcer;
        Notebook = notebook;
        Variables = RegisterVariables();
    }

    #region Variable Registration
    private List<ITextTemplateVariable> RegisterVariables()
    {
        if (Enforcer is null || Notebook is null)
        {
            return [];
        }

        return
        [
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
            new TextTemplateVariable(
             properName: "User:Input",
             name: "input",
             prefix: "user:",
             description:"User Input - No value",
             aliases: [],
             getValue: () => "[User Input]"
            )
        ];
    }
    #endregion
}
