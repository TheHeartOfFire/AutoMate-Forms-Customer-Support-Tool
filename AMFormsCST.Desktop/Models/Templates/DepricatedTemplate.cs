using System.Text.Json.Serialization;
using AMFormsCST.Core.Types.BestPractices.TextTemplates.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AMFormsCST.Desktop.Models.Templates;
public class DeprecatedTemplate
{
    [JsonConstructor]
    public DeprecatedTemplate(string name, string text, ushort variables, List<string> variableDefaults, TemplateType type)
    {
        Name = name;
        Text = text;
        Variables = variables;
        VariableDefaults = variableDefaults;
        Type = type;
    }

    public string Name { get; set; }
    public string Text { get; set; }
    public ushort Variables { get; set; }
    public List<string> VariableDefaults { get; set; }
    public TemplateType Type { get; set; } = TemplateType.Other;

    public enum TemplateType
    {
        PublishComments,
        InternalComments,
        ClosureComments,
        Other
    }

    private static TextTemplate.TemplateType ConvertType(TemplateType type)
    {
        return type switch
        {
            TemplateType.PublishComments => TextTemplate.TemplateType.PublishComments,
            TemplateType.InternalComments => TextTemplate.TemplateType.InternalComments,
            TemplateType.ClosureComments => TextTemplate.TemplateType.ClosureComments,
            _ => TextTemplate.TemplateType.Other
        };
    }

    public static explicit operator TextTemplate(DeprecatedTemplate deprecated)
    {
        ArgumentNullException.ThrowIfNull(deprecated);

        string formattedText;
        try
        {
            // Defensive: pad with empty strings if not enough variables
            var variableCount = deprecated.Variables;
            var defaults = deprecated.VariableDefaults ?? new List<string>();
            var args = defaults.Concat(Enumerable.Repeat(string.Empty, Math.Max(0, variableCount - defaults.Count))).Take(variableCount).ToArray();

            formattedText = string.Format(deprecated.Text, args);
        }
        catch (FormatException)
        {
            // Fallback: use raw text if formatting fails
            formattedText = deprecated.Text;
        }

        var template = new TextTemplate(
            name: deprecated.Name,
            description: "[Converted]",
            text: formattedText,
            type: ConvertType(deprecated.Type)
        );

        return template;
    }
}
