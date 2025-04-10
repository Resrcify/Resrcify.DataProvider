using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Resrcify.DataProvider.Domain.Internal.BaseData.ValueObjects.DatacronData;

namespace Resrcify.DataProvider.Application.Converters;

internal sealed class AbilityConverter : JsonConverter<Ability>
{
    public override Ability Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using JsonDocument doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;
        var id = root.GetProperty("id").GetString();
        var targets = JsonSerializer.Deserialize<Dictionary<string, Target>>(root.GetProperty("targets").GetRawText(), options);

        return Ability.Create(
            id ?? string.Empty,
            targets).Value;
    }

    public override void Write(Utf8JsonWriter writer, Ability value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("id", value.Id);
        writer.WriteStartObjectProperty("targets", value.Targets, options);
        writer.WriteEndObject();
    }
}
