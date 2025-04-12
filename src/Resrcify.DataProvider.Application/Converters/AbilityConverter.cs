using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Resrcify.DataProvider.Application.Extensions;
using Resrcify.DataProvider.Domain.Internal.BaseData.ValueObjects.DatacronData;

namespace Resrcify.DataProvider.Application.Converters;

internal sealed class AbilityConverter : JsonConverter<Ability>
{
    public override Ability Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException();

        string? id = null;
        Dictionary<string, Target>? targets = null;

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
                break;

            if (reader.TokenType != JsonTokenType.PropertyName)
                throw new JsonException();

            string propName = reader.GetString()!;
            reader.Read();

            switch (propName)
            {
                case "id":
                    if (reader.TokenType != JsonTokenType.String)
                        throw new JsonException();
                    id = reader.GetString();
                    break;

                case "targets":
                    targets = JsonSerializer.Deserialize<Dictionary<string, Target>>(ref reader, options);
                    break;

                default:
                    reader.Skip();
                    break;
            }
        }
        return Ability.Create(
            id ?? string.Empty,
            targets).Value;
    }

    public override void Write(Utf8JsonWriter writer, Ability value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("id", value.Id);
        writer.WriteStartObjectProperty("targets", value.Targets, options);
        writer.WriteEndObject();
    }
}
