using AMFormsCST.Core.Interfaces;
using AMFormsCST.Core.Interfaces.BestPractices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AMFormsCST.Core.Types.BestPractices.TextTemplates.Models;
public class TextTemplate : IEquatable<TextTemplate>
{
    [JsonInclude]
    public Guid Id { get; private set; }// Default to empty GUID for new templates
    public string Name { get; set; }
    public string Description { get; set; }
    public string Text { get; set; }
    public TextTemplate(string name, string description, string text)
    {
        Id = Guid.NewGuid(); // Or pass in an existing ID for editing
        Name = name;
        Description = description;
        Text = text;
    }
    [JsonConstructor]
    public TextTemplate(Guid id, string name, string description, string text)
    {
        Id = id;
        Name = name;
        Description = description;
        Text = text;
    }

    public List<ITextTemplateVariable> GetVariables(ISupportTool supportTool)
    {
        var variables = new List<ITextTemplateVariable>();
        if (supportTool?.Settings?.UserSettings?.Organization?.Variables is null)
        {
            return variables;
        }
        foreach (var variable in supportTool.Settings.UserSettings.Organization.Variables)
        {
            if (Text.Contains(variable.ProperName, StringComparison.InvariantCultureIgnoreCase) ||
                Text.Contains(variable.Prefix + variable.Name, StringComparison.InvariantCultureIgnoreCase))
            {
                variables.Add(variable);
            }
        }
        return variables;
    }

    public static string Process(string processedText, List<ITextTemplateVariable> orderedListOfVariables, List<string> overrides) 
    {
        var variableValues = orderedListOfVariables.Select(x => x.GetValue()).ToList();

        for (int i = 0; i < overrides.Count; i++)
        {
            if (!overrides[i].Equals(string.Empty) || orderedListOfVariables[i].ProperName.Equals("User:Input"))
            {
                    variableValues[i] = overrides[i];
            }
        }



        return string.Format(processedText, [.. variableValues]);
    }

    public bool Equals(TextTemplate? other)
    {
        return other is not null && other.Text.Equals(Text);
    }
    public bool Equals(string? other)
    {
        return other is not null && other.Equals(Text);
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as TextTemplate);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Text, Name, Description);
    }

    public static bool ContainsVariable(string text, ISupportTool supportTool)
    {
        var variables = supportTool.Settings.UserSettings.Organization.Variables;
        foreach (var variable in variables)
        {
            if (text.Contains(variable.ProperName, StringComparison.InvariantCultureIgnoreCase) ||
                text.Contains(variable.Prefix + variable.Name, StringComparison.InvariantCultureIgnoreCase))
            {
                return true;
            }
            foreach (var alias in variable.Aliases)
            {
                if (text.Contains(variable.Prefix + alias, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public static (int position, ITextTemplateVariable? variable, string alias) GetFirstVariable(string text, ISupportTool supportTool)
    {
        var variables = supportTool.Settings.UserSettings.Organization.Variables;

        (int position, ITextTemplateVariable? variable, string alias) lowestIndex = (-1, null, string.Empty);

        if(!ContainsVariable(text, supportTool))
            return lowestIndex;

        foreach (var variable in variables)
        {
            
            int indexProperName = text.IndexOf(variable.ProperName, StringComparison.InvariantCultureIgnoreCase);

            if (indexProperName != -1 && (lowestIndex.variable == null || indexProperName < lowestIndex.position))
                lowestIndex = (indexProperName, variable, variable.ProperName);

                
            int indexPrefixName = text.IndexOf(variable.Prefix + variable.Name, StringComparison.InvariantCultureIgnoreCase);

            if (indexPrefixName != -1 && (lowestIndex.variable == null || indexPrefixName < lowestIndex.position))
                lowestIndex = (indexPrefixName, variable, variable.Prefix + variable.Name);


            foreach (var alias in variable.Aliases)
            {
                int indexAlias = text.IndexOf(variable.Prefix + alias, StringComparison.InvariantCultureIgnoreCase);

                if (indexAlias != -1 && (lowestIndex.variable == null || indexAlias < lowestIndex.position))
                        lowestIndex = (indexAlias, variable, variable.Prefix + alias);
            }
        }

        return lowestIndex;
    }
}

