using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Resrcify.DataProvider.Domain.Internal.BaseData.ValueObjects.RelicData;

namespace Resrcify.DataProvider.Application.Converters;

internal sealed class RelicDataConverter : JsonConverter<RelicData>
{
    public override RelicData Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var gms = doc.RootElement.GetProperty("gms").ToString();
        var stats = doc.RootElement.GetProperty("stats").ToString();
        var gmsDictionary = JsonSerializer.Deserialize<Dictionary<string, long>>(gms, options);
        var statsDictionary = JsonSerializer.Deserialize<Dictionary<long, long>>(stats, options);
        return RelicData.Create(gmsDictionary ?? [], statsDictionary ?? []).Value;
    }

    public override void Write(Utf8JsonWriter writer, RelicData value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteStartObjectProperty("gms", value.Gms, options);
        writer.WriteStartObjectProperty("stats", value.Stats, options);
        writer.WriteEndObject();
    }
}
