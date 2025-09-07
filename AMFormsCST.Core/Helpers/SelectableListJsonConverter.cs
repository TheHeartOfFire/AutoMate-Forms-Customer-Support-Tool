using AMFormsCST.Core.Interfaces.Notebook;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AMFormsCST.Core.Helpers;

public class SelectableListJsonConverter<T> : JsonConverter<SelectableList<T>> where T : class, INotebookItem<T>
{
    public override SelectableList<T>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException("Expected StartArray token");
        }

        var list = new SelectableList<T>();

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndArray)
            {
                // Select the first item by default after deserialization
                list.SelectedItem = list.FirstOrDefault();
                return list;
            }

            // The serializer will use the correct derived type converter because of [JsonDerivedType] attributes
            var element = JsonSerializer.Deserialize<T>(ref reader, options);
            if (element != null)
            {
                list.Add(element);
            }
        }

        throw new JsonException("Expected EndArray token");
    }

    public override void Write(Utf8JsonWriter writer, SelectableList<T> value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        foreach (var item in value)
        {
            JsonSerializer.Serialize(writer, item, options);
        }
        writer.WriteEndArray();
    }
}