using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AMFormsCST.Core.Helpers
{
    public class SelectableListJsonConverter<T> : JsonConverter<SelectableList<T>> where T : class
    {
        public override SelectableList<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var items = JsonSerializer.Deserialize<List<T>>(ref reader, options);
            return items != null ? new SelectableList<T>(items) : new SelectableList<T>();
        }

        public override void Write(Utf8JsonWriter writer, SelectableList<T> value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value.ToArray(), options);
        }
    }
}