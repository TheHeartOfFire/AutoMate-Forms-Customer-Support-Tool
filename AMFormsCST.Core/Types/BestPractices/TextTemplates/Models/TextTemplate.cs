using AMFormsCST.Core.Interfaces;
using AMFormsCST.Core.Interfaces.BestPractices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFormsCST.Core.Types.BestPractices.TextTemplates.Models;
public class TextTemplate(string name, string description, string text) : IEquatable<TextTemplate>
{
    public string Name { get; set; } = name;
    public string Description { get; set; } = description;
    public string Text { get; } = text;

    public List<ITextTemplateVariable> GetVariables(ISupportTool supportTool)
    {
        var variables = new List<ITextTemplateVariable>();
        foreach (var variable in supportTool.Variables)
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
    

    private static string CheckForAlias(string text, ITextTemplateVariable variable, string name)
    {
        foreach (var alias in variable.Aliases)
        {
            var aliasName = variable.Prefix + alias;

            if (text.Contains(aliasName))
                name = aliasName;
        }

        return name;
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
        var variables = supportTool.Variables;
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
        var variables = supportTool.Variables;

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

