using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Resrcify.DataProvider.Application.Extensions;
using Resrcify.DataProvider.Domain.Internal.BaseData.ValueObjects.UnitData;
using Resrcify.DataProvider.Domain.Models.GalaxyOfHeroes.GameData;

namespace Resrcify.DataProvider.Application.Converters;

internal sealed class SkillConverter : JsonConverter<Skill>
{
    public override Skill Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string id = string.Empty;
        string name = string.Empty;
        string nameKey = string.Empty;
        int maxTier = 0;
        long type = 0;
        string image = string.Empty;
        var powerOverrideTags = new Dictionary<string, string>();
        bool isZeta = false;
        int zetaTier = 0;
        bool isOmicron = false;
        int omicronTier = 0;
        OmicronMode omicronMode = OmicronMode.OmicronModeDEFAULT;
        string omicronModeName = string.Empty;

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
                    case "name":
                        name = reader.TokenType == JsonTokenType.String ? reader.GetString() ?? string.Empty : string.Empty;
                        break;
                    case "nameKey":
                        nameKey = reader.TokenType == JsonTokenType.String ? reader.GetString() ?? string.Empty : string.Empty;
                        break;
                    case "maxTier":
                        maxTier = reader.TokenType == JsonTokenType.Number ? reader.GetInt32() : 0;
                        break;
                    case "type":
                        type = reader.TokenType == JsonTokenType.Number ? reader.GetInt64() : 0;
                        break;
                    case "image":
                        image = reader.TokenType == JsonTokenType.String ? reader.GetString() ?? string.Empty : string.Empty;
                        break;
                    case "powerOverrideTags":
                        if (reader.TokenType == JsonTokenType.StartObject || reader.TokenType == JsonTokenType.StartArray)
                        {
                            powerOverrideTags = JsonSerializer.Deserialize<Dictionary<string, string>>(ref reader, options) ?? [];
                        }
                        break;
                    case "isZeta":
                        isZeta = reader.TokenType == JsonTokenType.True;
                        break;
                    case "zetaTier":
                        zetaTier = reader.TokenType == JsonTokenType.Number ? reader.GetInt32() : 0;
                        break;
                    case "isOmicron":
                        isOmicron = reader.TokenType == JsonTokenType.True;
                        break;
                    case "omicronTier":
                        omicronTier = reader.TokenType == JsonTokenType.Number ? reader.GetInt32() : 0;
                        break;
                    case "omicronMode":
                        if (reader.TokenType == JsonTokenType.String)
                        {
                            var omicronModeString = reader.GetString();
                            if (Enum.TryParse<OmicronMode>(omicronModeString, ignoreCase: true, out var parsedMode))
                            {
                                omicronMode = parsedMode;
                            }
                        }
                        else if (reader.TokenType == JsonTokenType.Number)
                        {
                            var enumNumber = reader.GetInt32();
                            if (Enum.IsDefined(typeof(OmicronMode), enumNumber))
                            {
                                omicronMode = (OmicronMode)enumNumber;
                            }
                        }
                        break;
                    case "omicronModeName":
                        omicronModeName = reader.TokenType == JsonTokenType.String ? reader.GetString() ?? string.Empty : string.Empty;
                        break;
                    default:
                        reader.Skip();
                        break;
                }
            }
        }

        return Skill.Create(
            id ?? string.Empty,
            name ?? string.Empty,
            nameKey ?? string.Empty,
            maxTier,
            type,
            image ?? string.Empty,
            powerOverrideTags ?? [],
            isZeta,
            zetaTier,
            isOmicron,
            omicronTier,
            omicronMode,
            omicronModeName ?? string.Empty).Value;
    }

    public override void Write(Utf8JsonWriter writer, Skill value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("id", value.Id);
        writer.WriteString("name", value.Name);
        writer.WriteString("nameKey", value.NameKey);
        writer.WriteNumber("maxTier", value.MaxTier);
        writer.WriteNumber("type", value.Type);
        writer.WriteString("image", value.Image);
        writer.WriteStartObjectProperty("powerOverrideTags", value.PowerOverrideTags, options);
        writer.WriteBoolean("isZeta", value.IsZeta);
        writer.WriteNumber("zetaTier", value.ZetaTier);
        writer.WriteBoolean("isOmicron", value.IsOmicron);
        writer.WriteNumber("omicronTier", value.OmicronTier);
        writer.WriteNumber("omicronMode", (int)value.OmicronMode);
        writer.WriteString("omicronModeName", value.OmicronModeName);
        writer.WriteEndObject();
    }

}
