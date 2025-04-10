using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Resrcify.DataProvider.Domain.Internal.BaseData.ValueObjects.UnitData;

namespace Resrcify.DataProvider.Application.Converters;

internal sealed class GearLevelConverter : JsonConverter<GearLevel>
{
    public override GearLevel Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;
        var gear = JsonSerializer.Deserialize<List<string>>(root.GetProperty("gear").GetRawText(), options);
        var stats = JsonSerializer.Deserialize<Dictionary<long, long>>(root.GetProperty("stats").GetRawText(), options);

        return GearLevel.Create(
            gear ?? [],
            stats ?? []).Value;
    }

    public override void Write(Utf8JsonWriter writer, GearLevel value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteStartObjectProperty("gear", value.Gear, options);
        writer.WriteStartObjectProperty("stats", value.Stats, options);
        writer.WriteEndObject();
    }
}
