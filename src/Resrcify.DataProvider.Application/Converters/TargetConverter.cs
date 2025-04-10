using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Resrcify.DataProvider.Domain.Internal.BaseData.ValueObjects.DatacronData;

namespace Resrcify.DataProvider.Application.Converters;

internal sealed class TargetConverter : JsonConverter<Target>
{
    public override Target Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using JsonDocument doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;
        var id = root.GetProperty("id").GetString();
        var nameKey = root.GetProperty("nameKey").GetString();
        var descKey = root.GetProperty("descKey").GetString();
        var iconKey = root.GetProperty("iconKey").GetString();
        Unit? unit = null;
        if (root.TryGetProperty("unit", out var unitElement) && unitElement.ValueKind != JsonValueKind.Null)
        {
            unit = JsonSerializer.Deserialize<Unit>(unitElement.GetRawText(), options);
        }
        return Target.Create(
            id ?? string.Empty,
            nameKey ?? string.Empty,
            descKey ?? string.Empty,
            iconKey ?? string.Empty,
            unit).Value;
    }

    public override void Write(Utf8JsonWriter writer, Target value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("id", value.Id);
        writer.WriteString("nameKey", value.NameKey);
        writer.WriteString("descKey", value.DescKey);
        writer.WriteString("iconKey", value.IconKey);
        if (value.Unit != null)
        {
            writer.WriteStartObjectProperty("unit", value.Unit, options);
        }
        else
        {
            writer.WriteNull("unit");
        }
        writer.WriteEndObject();
    }
}
