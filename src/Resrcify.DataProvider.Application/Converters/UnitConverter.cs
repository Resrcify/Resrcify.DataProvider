using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Resrcify.DataProvider.Domain.Internal.BaseData.ValueObjects.DatacronData;

namespace Resrcify.DataProvider.Application.Converters;

internal sealed class UnitConverter : JsonConverter<Unit>
{
    public override Unit Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException();

        string baseId = string.Empty;
        string nameKey = string.Empty;
        int combatType = 0;

        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
        {
            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                string propertyName = reader.GetString() ?? string.Empty;

                reader.Read();

                switch (propertyName)
                {
                    case "baseId":
                        baseId = reader.TokenType == JsonTokenType.String ? reader.GetString() ?? string.Empty : string.Empty;
                        break;
                    case "nameKey":
                        nameKey = reader.TokenType == JsonTokenType.String ? reader.GetString() ?? string.Empty : string.Empty;
                        break;
                    case "combatType":
                        combatType = reader.TokenType == JsonTokenType.Number ? reader.GetInt32() : 0;
                        break;
                    default:
                        reader.Skip();
                        break;
                }
            }
        }
        return Unit.Create(
            baseId ?? string.Empty,
            nameKey ?? string.Empty,
            combatType).Value;
    }

    public override void Write(Utf8JsonWriter writer, Unit value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("baseId", value.BaseId);
        writer.WriteString("nameKey", value.NameKey);
        writer.WriteNumber("combatType", value.CombatType);

        writer.WriteEndObject();
    }
}
