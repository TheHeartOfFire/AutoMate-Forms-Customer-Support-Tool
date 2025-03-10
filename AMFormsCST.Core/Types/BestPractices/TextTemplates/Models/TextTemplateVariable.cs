using AMFormsCST.Core.Interfaces.BestPractices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFormsCST.Core.Types.BestPractices.TextTemplates.Models;
public class TextTemplateVariable(string properName, string name, string prefix, string description, List<string> aliases, Func<string> getValue) : ITextTemplateVariable
{
    public string Name { get; } = name;
    public string Prefix { get; } = prefix;
    public string Description { get; } = description;
    public string ProperName { get; } = properName;
    public IReadOnlyCollection<string> Aliases { get { return _aliases; } }
    private readonly Func<string> _getValue = getValue;

    private readonly List<string> _aliases = aliases;

    public string GetValue()
    {
        return _getValue();
    }
}
