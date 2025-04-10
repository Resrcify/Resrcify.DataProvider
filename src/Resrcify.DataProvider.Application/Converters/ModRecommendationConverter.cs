using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Resrcify.DataProvider.Domain.Internal.BaseData.ValueObjects.UnitData;

namespace Resrcify.DataProvider.Application.Converters;

internal sealed class ModRecommendationConverter : JsonConverter<ModRecommendation>
{
    public override ModRecommendation Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;
        var recommendationSetId = root.GetProperty("recommendationSetId").GetString();
        var unitTier = root.GetProperty("unitTier").GetInt64();
        return ModRecommendation.Create(recommendationSetId ?? string.Empty, unitTier).Value;
    }

    public override void Write(Utf8JsonWriter writer, ModRecommendation value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("recommendationSetId", value.RecommendationSetId);
        writer.WriteNumber("unitTier", value.UnitTier);
        writer.WriteEndObject();
    }
}
