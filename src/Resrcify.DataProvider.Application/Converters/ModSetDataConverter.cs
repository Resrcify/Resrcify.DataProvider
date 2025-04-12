using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Resrcify.DataProvider.Domain.Internal.BaseData.ValueObjects.ModeSetData;

namespace Resrcify.DataProvider.Application.Converters;

internal sealed class ModSetDataConverter : JsonConverter<ModSetData>
{
    public override ModSetData Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        long id = 0;
        long count = 0;
        long value = 0;
        long max = 0;

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
                        id = reader.TokenType == JsonTokenType.Number ? reader.GetInt64() : 0;
                        break;
                    case "count":
                        count = reader.TokenType == JsonTokenType.Number ? reader.GetInt64() : 0;
                        break;
                    case "value":
                        value = reader.TokenType == JsonTokenType.Number ? reader.GetInt64() : 0;
                        break;
                    case "max":
                        max = reader.TokenType == JsonTokenType.Number ? reader.GetInt64() : 0;
                        break;
                    default:
                        reader.Skip();
                        break;
                }
            }
        }

        return ModSetData.Create(id, count, value, max).Value;
    }

    public override void Write(Utf8JsonWriter writer, ModSetData value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteNumber("id", value.Id);
        writer.WriteNumber("count", value.Count);
        writer.WriteNumber("value", value.Value);
        writer.WriteNumber("max", value.Max);
        writer.WriteEndObject();
    }
}
