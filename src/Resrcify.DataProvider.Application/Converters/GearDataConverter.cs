using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Resrcify.DataProvider.Domain.Internal.BaseData.ValueObjects.GearData;

namespace Resrcify.DataProvider.Application.Converters;

internal sealed class GearDataConverter : JsonConverter<GearData>
{
    public override GearData Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var jsonObject = JsonDocument.ParseValue(ref reader).RootElement;
        var stats = new Dictionary<long, long>();

        foreach (var statElement in jsonObject.GetProperty("stats").EnumerateObject())
        {
            var key = statElement.Name;
            var value = statElement.Value.GetInt64();

            stats[Convert.ToInt64(key)] = value;
        }

        return GearData.Create(stats).Value;
    }

    public override void Write(Utf8JsonWriter writer, GearData value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteStartObject("stats");

        foreach (var stat in value.Stats)
            writer.WriteNumber(stat.Key.ToString(), stat.Value);

        writer.WriteEndObject();
        writer.WriteEndObject();
    }
}
