using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Resrcify.DataProvider.Domain.Internal.BaseData.ValueObjects.DatacronData;

namespace Resrcify.DataProvider.Application.Converters;

internal sealed class StatConverter : JsonConverter<Stat>
{
    public override Stat Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        int id = 0;
        string nameKey = string.Empty;
        string iconKey = string.Empty;

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
                    case "id":
                        id = reader.TokenType == JsonTokenType.Number ? reader.GetInt32() : 0;
                        break;
                    case "nameKey":
                        nameKey = reader.TokenType == JsonTokenType.String ? reader.GetString() ?? string.Empty : string.Empty;
                        break;
                    case "iconKey":
                        iconKey = reader.TokenType == JsonTokenType.String ? reader.GetString() ?? string.Empty : string.Empty;
                        break;
                    default:
                        reader.Skip();
                        break;
                }
            }
        }
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
