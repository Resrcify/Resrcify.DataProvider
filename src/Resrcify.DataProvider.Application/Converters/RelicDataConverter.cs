using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Resrcify.DataProvider.Application.Extensions;
using Resrcify.DataProvider.Domain.Internal.BaseData.ValueObjects.RelicData;

namespace Resrcify.DataProvider.Application.Converters;

internal sealed class RelicDataConverter : JsonConverter<RelicData>
{
    public override RelicData Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var gmsDictionary = new Dictionary<string, long>();
        var statsDictionary = new Dictionary<long, long>();

        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException("Expected StartObject token.");

        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
        {
            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                string propertyName = reader.GetString() ?? string.Empty;
                reader.Read();

                switch (propertyName)
                {
                    case "gms":
                        if (reader.TokenType == JsonTokenType.StartObject)
                        {
                            gmsDictionary = JsonSerializer.Deserialize<Dictionary<string, long>>(ref reader, options) ?? new Dictionary<string, long>();
                        }
                        break;
                    case "stats":
                        if (reader.TokenType == JsonTokenType.StartObject)
                        {
                            statsDictionary = JsonSerializer.Deserialize<Dictionary<long, long>>(ref reader, options) ?? new Dictionary<long, long>();
                        }
                        break;
                    default:
                        reader.Skip();
                        break;
                }
            }
        }

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
