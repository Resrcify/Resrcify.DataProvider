using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Resrcify.DataProvider.Domain.Internal.BaseData.ValueObjects.DatacronData;

namespace Resrcify.DataProvider.Application.Converters;

internal sealed class StatConverter : JsonConverter<Stat>
{
    public override Stat Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using JsonDocument doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;
        var id = root.GetProperty("id").GetInt32();
        var nameKey = root.GetProperty("nameKey").GetString();
        var iconKey = root.GetProperty("iconKey").GetString();
        return Stat.Create(id, nameKey ?? string.Empty, iconKey ?? string.Empty).Value;
    }

    public override void Write(Utf8JsonWriter writer, Stat value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteNumber("id", value.Id);
        writer.WriteString("nameKey", value.NameKey);
        writer.WriteString("iconKey", value.IconKey);
        writer.WriteEndObject();
    }
}
