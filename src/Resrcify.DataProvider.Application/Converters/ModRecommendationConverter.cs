using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Resrcify.DataProvider.Domain.Internal.BaseData.ValueObjects.UnitData;

namespace Resrcify.DataProvider.Application.Converters;

internal sealed class ModRecommendationConverter : JsonConverter<ModRecommendation>
{
    public override ModRecommendation Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? recommendationSetId = null;
        long unitTier = 0;

        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException();

        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
        {
            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                string propertyName = reader.GetString() ?? string.Empty;
                reader.Read();

                switch (propertyName)
                {
                    case "recommendationSetId":
                        recommendationSetId = reader.TokenType == JsonTokenType.String
                            ? reader.GetString() ?? string.Empty
                            : string.Empty;
                        break;
                    case "unitTier":
                        unitTier = reader.TokenType == JsonTokenType.Number
                            ? reader.GetInt64()
                            : 0;
                        break;
                    default:
                        reader.Skip();
                        break;
                }
            }
        }

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
