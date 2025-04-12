using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Resrcify.DataProvider.Application.Extensions;
using Resrcify.DataProvider.Domain.Internal.BaseData.ValueObjects.DatacronData;

namespace Resrcify.DataProvider.Application.Converters;

internal sealed class TargetConverter : JsonConverter<Target>
{
    public override Target Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string id = string.Empty;
        string nameKey = string.Empty;
        string descKey = string.Empty;
        string iconKey = string.Empty;
        Unit? unit = null;

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
                        id = reader.TokenType == JsonTokenType.String ? reader.GetString() ?? string.Empty : string.Empty;
                        break;
                    case "nameKey":
                        nameKey = reader.TokenType == JsonTokenType.String ? reader.GetString() ?? string.Empty : string.Empty;
                        break;
                    case "descKey":
                        descKey = reader.TokenType == JsonTokenType.String ? reader.GetString() ?? string.Empty : string.Empty;
                        break;
                    case "iconKey":
                        iconKey = reader.TokenType == JsonTokenType.String ? reader.GetString() ?? string.Empty : string.Empty;
                        break;
                    case "unit":
                        if (reader.TokenType == JsonTokenType.StartObject)
                        {
                            unit = JsonSerializer.Deserialize<Unit>(ref reader, options);
                        }
                        else
                        {
                            reader.Skip();
                        }
                        break;
                    default:
                        reader.Skip();
                        break;
                }
            }
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
