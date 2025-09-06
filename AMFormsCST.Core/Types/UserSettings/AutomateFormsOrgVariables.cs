using AMFormsCST.Core.Interfaces.BestPractices;
using AMFormsCST.Core.Interfaces.UserSettings;
using AMFormsCST.Core.Interfaces.Utils;
using AMFormsCST.Core.Types.BestPractices.TextTemplates.Models;
using AMFormsCST.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AMFormsCST.Core.Types.UserSettings;
public class AutomateFormsOrgVariables : IOrgVariables
{
    private readonly ILogService? _logger;

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
    [JsonConstructor]
    public AutomateFormsOrgVariables(IBestPracticeEnforcer? enforcer, INotebook? notebook)
    {
        Enforcer = enforcer;
        Notebook = notebook;
        Variables = RegisterVariables();
    }

    // Constructor for Dependency Injection
    public AutomateFormsOrgVariables(IBestPracticeEnforcer? enforcer, INotebook? notebook, ILogService? logger = null)
    {
        _logger = logger;
        Enforcer = enforcer;
        Notebook = notebook;
        Variables = RegisterVariables();
        _logger?.LogInfo("AutomateFormsOrgVariables initialized.");
    }

    public void InstantiateVariables(IBestPracticeEnforcer enforcer, INotebook notebook)
    {
        if (enforcer is null)
        {
            var ex = new ArgumentNullException(nameof(enforcer), "Enforcer cannot be null.");
            _logger?.LogError("Attempted to instantiate variables with null Enforcer.", ex);
            throw ex;
        }
        if (notebook is null)
        {
            var ex = new ArgumentNullException(nameof(notebook), "Notebook cannot be null.");
            _logger?.LogError("Attempted to instantiate variables with null Notebook.", ex);
            throw ex;
        }
        Enforcer = enforcer;
        Notebook = notebook;
        Variables = RegisterVariables();
        _logger?.LogInfo("Variables instantiated.");
    }

    #region Variable Registration
    private List<ITextTemplateVariable> RegisterVariables()
    {
        if (Enforcer is null || Notebook is null)
        {
            _logger?.LogWarning("RegisterVariables called with null Enforcer or Notebook. Returning empty variable list.");
            return [];
        }
        var selectedNote = Notebook.Notes.SelectedItem;
        var selectedDealer = selectedNote?.Dealers.SelectedItem;
        var selectedCompany = selectedDealer?.Companies.SelectedItem;
        var selectedContact = selectedNote?.Contacts.SelectedItem;
        var selectedForm = selectedNote?.Forms.SelectedItem;
        var selectedTestDeal = selectedForm?.TestDeals.SelectedItem;

        // Fix: Change square brackets [] to parentheses () for List initialization in RegisterVariables()

        var variables = new List<ITextTemplateVariable>
        { 
            new TextTemplateVariable(
             properName: "Notes:ServerID",
             name: "serverid",
             prefix: "notes:",
             description: "Server ID#",
             aliases: ["server", "serv"],
             getValue: () => selectedDealer?.ServerCode ?? string.Empty
            ),
            new TextTemplateVariable(
             properName: "Notes:Companies",
             name: "companies",
             prefix: "notes:",
             description: "Company#(s)",
             aliases: ["company", "comp", "co"],
             getValue: () => selectedCompany?.CompanyCode ?? string.Empty
            ),
            new TextTemplateVariable(
             properName: "Notes:Dealership",
             name: "dealership",
             prefix: "notes:",
             description: "Dealership Name",
             aliases: ["dealer", "dlr"],
             getValue: () => selectedCompany?.Name ?? string.Empty
            ),
            new TextTemplateVariable(
             properName: "Notes:ContactName",
             name: "contactname",
             prefix: "notes:",
             description: "Contact Name",
             aliases: ["name"],
             getValue: () => selectedContact?.Name ?? string.Empty
            ),
            new TextTemplateVariable(
             properName: "Notes:EmailAddress",
             name: "emailaddress",
             prefix: "notes:",
             description: "E-Mail Address",
             aliases: ["email"],
             getValue: () => selectedContact?.Email ?? string.Empty
            ),
            new TextTemplateVariable(
             properName: "Notes:Phone",
             name: "phone",
             prefix: "notes:",
             description: "Phone#",
             aliases: [],
             getValue: () => selectedContact?.Phone ?? string.Empty
            ),
            new TextTemplateVariable(
             properName: "Notes:Notes",
             name: "notes",
             prefix: "notes:",
             description: "Notes",
             aliases: [],
             getValue: () => selectedNote?.NotesText ?? string.Empty
            ),
            new TextTemplateVariable(
             properName: "Notes:CaseNumber",
             name: "casenumber",
             prefix: "notes:",
             description: "Case#",
             aliases: ["caseno", "case"],
             getValue: () => selectedNote?.CaseText ?? string.Empty
            ),
            new TextTemplateVariable(
             properName: "Notes:Forms",
             name: "forms",
             prefix: "notes:",
             description: "Forms",
             aliases: ["form"],
             getValue: () => ">" + string.Join("\n>", selectedNote?.Forms.Select(f => f.Name) ?? [])
            ),
            new TextTemplateVariable(
             properName: "Notes:FirstName",
             name: "firstname",
             prefix: "notes:",
             description: "First Name",
             aliases: [],
             getValue: () =>
             {

                 if (!string.IsNullOrEmpty(selectedContact?.Name) && selectedContact?.Name.Contains(' ') == true)
                     return selectedContact?.Name.Split(' ').FirstOrDefault() ?? string.Empty;

                 return string.Empty;
             }
            ),
            new TextTemplateVariable(
             properName: "AMMail:FullAddress",
             name: "fulladdress",
             prefix: "ammail:",
             description: "AutoMate Forms Mailing Address",
             aliases: ["all", "full", "mailingaddress", "mailto"],
             getValue: () => string.Empty //TODO: Pull Mailing Address from persistent data
            ),
            new TextTemplateVariable(
             properName: "AMMail:Name",
             name: "name",
             prefix: "ammail:",
             description: "AutoMate Forms Mailing Address - Name",
             aliases: [],
             getValue: () => string.Empty //TODO: Pull Mailing Address from persistent data
            ),
            new TextTemplateVariable(
             properName: "AMMail:Street",
             name: "streetaddress",
             prefix: "ammail:",
             description: "AutoMate Forms Mailing Address - Street Address",
             aliases: ["street", "line1"],
             getValue: () => string.Empty //TODO: Pull Mailing Address from persistent data
            ),
            new TextTemplateVariable(
             properName: "AMMail:City",
             name: "city",
             prefix: "ammail:",
             description: "AutoMate Forms Mailing Address - City",
             aliases: [],
             getValue: () => string.Empty //TODO: Pull Mailing Address from persistent data
            ),
            new TextTemplateVariable(
             properName: "AMMail:State",
             name: "state",
             prefix: "ammail:",
             description: "AutoMate Forms Mailing Address - State",
             aliases: [],
             getValue: () => string.Empty //TODO: Pull Mailing Address from persistent data
            ),
            new TextTemplateVariable(
             properName: "AMMail:ZipCode",
             name: "zipcode",
             prefix: "ammail:",
             description: "AutoMate Forms Mailing Address - Zip Code",
             aliases: ["postalcode", "zip"],
             getValue: () => string.Empty //TODO: Pull Mailing Address from persistent data
            ),
            new TextTemplateVariable(
             properName: "AMMail:CityStateZip",
             name: "citystatezip",
             prefix: "ammail:",
             description: "AutoMate Forms Mailing Address - City, State Zip",
             aliases: ["csz", "line2"],
             getValue: () => string.Empty //TODO: Pull Mailing Address from persistent data
            ),
            new TextTemplateVariable(
             properName: "FormNameGenerator:FormName",
             name: "formname",
             prefix: "formnamegenerator:",
             description: "Form Name Generator - Form Name",
             aliases: ["generate", "name", "output"],
             getValue: () => Enforcer.GetFormName()
            ),
            new TextTemplateVariable(
             properName: "Notes:DealNumber",
             name: "dealnumber",
             prefix: "notes:",
             description: "Deal#",
             aliases: ["dealno", "deal"],
             getValue: () => selectedTestDeal?.DealNumber ?? string.Empty
            ),
            new TextTemplateVariable(
             properName: "User:Input",
             name: "input",
             prefix: "user:",
             description: "User Input - No value",
             aliases: [],
             getValue: () => "[User Input]"
            )
        };

        _logger?.LogInfo($"Registered {variables.Count} text template variables.");
        return variables;
    }
    #endregion
}
