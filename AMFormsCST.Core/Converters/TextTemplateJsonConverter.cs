using AMFormsCST.Core.Types.BestPractices.TextTemplates.Models;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Documents;
using System.Windows.Markup;

namespace AMFormsCST.Core.Converters;

public class TextTemplateJsonConverter : JsonConverter<TextTemplate>
{
    public override TextTemplate? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Expected StartObject token");
        }

        Guid id = default;
        string name = string.Empty;
        string description = string.Empty;
        FlowDocument text = new();
        TextTemplate.TemplateType type = default;

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
            {
                return new TextTemplate(id, name, description, text, type);
            }

            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException("Expected PropertyName token");
            }

            var propertyName = reader.GetString();
            reader.Read(); // Move to the property value

            switch (propertyName?.ToLowerInvariant())
            {
                case "id":
                    id = reader.GetGuid();
                    break;
                case "name":
                    name = reader.GetString() ?? string.Empty;
                    break;
                case "description":
                    description = reader.GetString() ?? string.Empty;
                    break;
                case "type":
                    // Handle both string and integer enum values
                    if (reader.TokenType == JsonTokenType.String)
                        Enum.TryParse(reader.GetString(), true, out type);
                    else if (reader.TokenType == JsonTokenType.Number)
                        type = (TextTemplate.TemplateType)reader.GetInt32();
                    break;
                case "text":
                    var textValue = reader.GetString() ?? string.Empty;
                    try
                    {
                        // Try to parse as XAML (new format)
                        text = (FlowDocument)XamlReader.Parse(textValue);
                    }
                    catch (XamlParseException)
                    {
                        // If it fails, treat it as plain text (old format)
                        text = new FlowDocument(new Paragraph(new Run(textValue)));
                    }
                    break;
                default:
                    reader.Skip();
                    break;
            }
        }

        throw new JsonException("Unexpected end of JSON.");
    }

    public override void Write(Utf8JsonWriter writer, TextTemplate value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("id", value.Id);
        writer.WriteString("name", value.Name);
        writer.WriteString("description", value.Description);
        writer.WriteNumber("type", (int)value.Type);

        // Serialize FlowDocument to a XAML string
        var xamlText = XamlWriter.Save(value.Text);
        writer.WriteString("text", xamlText);

        writer.WriteEndObject();
    }
}