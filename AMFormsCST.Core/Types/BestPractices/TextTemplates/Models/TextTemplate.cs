using AMFormsCST.Core.Interfaces.BestPractices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFormsCST.Core.Types.BestPractices.TextTemplates.Models;
public class TextTemplate(string text, IList<ITextTemplateVariable> variables) : IEquatable<TextTemplate>
{
    public string Text { get; } = text;
    private IList<ITextTemplateVariable> Variables { get; } = variables;
    private IReadOnlyCollection<string> _aliases => [.. Variables.SelectMany(x => x.Aliases)];

    private IReadOnlyCollection<string> _prefixes => GetPrefixes();

    public string Process()
    {
        var text = Text;
        foreach (var variable in Variables) 
        {
            var name = variable.Prefix + variable.Name;
            if (!text.Contains(variable.ProperName, StringComparison.InvariantCultureIgnoreCase))
            {
                name = CheckForAlias(Text, variable, name);
            }

            text = text.Replace(name, variable.GetValue());
        }

        return text;
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

    private List<string> GetPrefixes()
    {
        var prefixes = new List<string>();

        foreach (var template in Variables)
            if (!prefixes.Contains(template.Prefix))
                prefixes.Add(template.Prefix);

        return prefixes;
    }

    public bool Equals(TextTemplate? other)
    {
        return other is not null && other.Text.Equals(Text);
    }
}

