using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Resrcify.DataProvider.Application.Extensions;
using Resrcify.DataProvider.Domain.Internal.BaseData.ValueObjects.UnitData;

namespace Resrcify.DataProvider.Application.Converters;

internal sealed class GearLevelConverter : JsonConverter<GearLevel>
{
    public override GearLevel Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var gear = new List<string>();
        var stats = new Dictionary<long, long>();

        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException();

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
                break;

            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                string propertyName = reader.GetString() ?? string.Empty;

                if (propertyName == "gear")
                {
                    reader.Read();
                    if (reader.TokenType != JsonTokenType.StartArray)
                        throw new JsonException();

                    while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                    {
                        if (reader.TokenType == JsonTokenType.String)
                        {
                            gear.Add(reader.GetString() ?? string.Empty);
                        }
                        else
                        {
                            throw new JsonException("Expected string in gear array");
                        }
                    }
                }
                else if (propertyName == "stats")
                {
                    reader.Read();
                    if (reader.TokenType != JsonTokenType.StartObject)
                        throw new JsonException();

                    while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
                    {
                        if (reader.TokenType == JsonTokenType.PropertyName)
                        {
                            string statKey = reader.GetString() ?? string.Empty;
                            reader.Read();

                            if (reader.TokenType == JsonTokenType.Number)
                            {
                                long statValue = reader.GetInt64();
                                stats[Convert.ToInt64(statKey)] = statValue;
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
