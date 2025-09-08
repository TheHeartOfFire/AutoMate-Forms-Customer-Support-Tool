using System.Text.Json.Serialization;
using AMFormsCST.Core.Types.BestPractices.TextTemplates.Models;
using AMFormsCST.Core.Interfaces;

namespace AMFormsCST.Desktop.Models.Templates;
public class DeprecatedTemplate
{
    private readonly ILogService? _logger;

    [JsonConstructor]
    public DeprecatedTemplate(string name, string text, ushort variables, List<string> variableDefaults, TemplateType type, ILogService? logger = null)
    {
        Name = name;
        Text = text;
        Variables = variables;
        VariableDefaults = variableDefaults;
        Type = type;
        _logger = logger;
        _logger?.LogInfo($"DeprecatedTemplate initialized: Name={name}, Type={type}, Variables={variables}");
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
            var variableCount = deprecated.Variables;
            var defaults = deprecated.VariableDefaults ?? new List<string>();
            var args = defaults.Concat(Enumerable.Repeat(string.Empty, Math.Max(0, variableCount - defaults.Count))).Take(variableCount).ToArray();

            formattedText = string.Format(deprecated.Text, args);
        }
        catch (FormatException ex)
        {
            formattedText = deprecated.Text;
            deprecated._logger?.LogWarning($"FormatException in DeprecatedTemplate conversion: {ex.Message}");
        }

        var template = new TextTemplate(
            name: deprecated.Name,
            description: "[Converted]",
            text: formattedText,
            type: ConvertType(deprecated.Type)
        );

        deprecated._logger?.LogInfo($"DeprecatedTemplate converted to TextTemplate: Name={deprecated.Name}, Type={deprecated.Type}");
        return template;
    }
}
