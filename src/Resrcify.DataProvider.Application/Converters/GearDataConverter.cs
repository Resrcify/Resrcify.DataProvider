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
        var stats = new Dictionary<long, long>();

        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException();

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
                break;

            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                string propName = reader.GetString() ?? string.Empty;

                if (propName == "stats")
                {
                    reader.Read();
                    if (reader.TokenType != JsonTokenType.StartObject)
                        throw new JsonException();

                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonTokenType.EndObject)
                            break;

                        if (reader.TokenType == JsonTokenType.PropertyName)
                        {
                            string statKey = reader.GetString() ?? string.Empty;
                            reader.Read();

                            if (reader.TokenType == JsonTokenType.Number)
                            {
                                long value = reader.GetInt64();
                                stats[Convert.ToInt64(statKey)] = value;
                            }
                            else
                            {
                                throw new JsonException("Expected number for stat value");
                            }
                        }
                    }
                }
                else
                {
                    reader.Skip();
                }
            }
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
