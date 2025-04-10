using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Resrcify.DataProvider.Domain.Internal.BaseData.ValueObjects.ModeSetData;

namespace Resrcify.DataProvider.Application.Converters;

internal sealed class ModSetDataConverter : JsonConverter<ModSetData>
{
    public override ModSetData Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;
        var id = root.GetProperty("id").GetInt64();
        var count = root.GetProperty("count").GetInt64();
        var value = root.GetProperty("value").GetInt64();
        var max = root.GetProperty("max").GetInt64();
        return ModSetData.Create(id, count, value, max).Value;
    }

    public override void Write(Utf8JsonWriter writer, ModSetData value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteNumber("id", value.Id);
        writer.WriteNumber("count", value.Count);
        writer.WriteNumber("value", value.Value);
        writer.WriteNumber("max", value.Max);
        writer.WriteEndObject();
    }
}
