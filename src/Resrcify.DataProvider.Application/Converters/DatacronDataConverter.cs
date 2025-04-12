using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Resrcify.DataProvider.Application.Extensions;
using Resrcify.DataProvider.Domain.Internal.BaseData.ValueObjects.DatacronData;
using Resrcify.DataProvider.Domain.Models.GalaxyOfHeroes.GameData;

namespace Resrcify.DataProvider.Application.Converters;

internal sealed class DatacronDataConverter : JsonConverter<DatacronData>
{
    public override DatacronData Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string id = string.Empty, nameKey = string.Empty, iconKey = string.Empty, detailPrefab = string.Empty, referenceTemplateId = string.Empty;
        int setId = 0, initialTiers = 0, maxRerolls = 0;
        long expirationTimeMs = 0;
        bool allowReroll = false;
        bool isFocused = false;

        List<DatacronSetMaterial>? setMaterial = null;
        List<string>? fixedTag = null;
        List<DatacronSetTier>? setTier = null;
        List<DatacronTemplateTier>? tier = null;
        List<DatacronAffixTemplateSet>? affixSet = null;
        Dictionary<string, Domain.Internal.BaseData.ValueObjects.DatacronData.Ability>? abilities = null;
        Dictionary<string, Stat>? stats = null;

        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException();

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
                break;

            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                string propName = reader.GetString() ?? string.Empty;

                reader.Read();

                switch (propName)
                {
                    case "id":
                        id = reader.GetString() ?? string.Empty;
                        break;
                    case "setId":
                        setId = reader.GetInt32();
                        break;
                    case "isFocused":
                        isFocused = reader.GetBoolean();
                        break;
                    case "nameKey":
                        nameKey = reader.GetString() ?? string.Empty;
                        break;
                    case "iconKey":
                        iconKey = reader.GetString() ?? string.Empty;
                        break;
                    case "detailPrefab":
                        detailPrefab = reader.GetString() ?? string.Empty;
                        break;
                    case "expirationTimeMs":
                        expirationTimeMs = reader.GetInt64();
                        break;
                    case "allowReroll":
                        allowReroll = reader.GetBoolean();
                        break;
                    case "initialTiers":
                        initialTiers = reader.GetInt32();
                        break;
                    case "maxRerolls":
                        maxRerolls = reader.GetInt32();
                        break;
                    case "referenceTemplateId":
                        referenceTemplateId = reader.GetString() ?? string.Empty;
                        break;
                    case "setMaterial":
                        setMaterial = JsonSerializer.Deserialize<List<DatacronSetMaterial>>(ref reader, options);
                        break;
                    case "fixedTag":
                        fixedTag = JsonSerializer.Deserialize<List<string>>(ref reader, options);
                        break;
                    case "setTier":
                        setTier = JsonSerializer.Deserialize<List<DatacronSetTier>>(ref reader, options);
                        break;
                    case "tier":
                        tier = JsonSerializer.Deserialize<List<DatacronTemplateTier>>(ref reader, options);
                        break;
                    case "affixSet":
                        affixSet = JsonSerializer.Deserialize<List<DatacronAffixTemplateSet>>(ref reader, options);
                        break;
                    case "abilities":
                        abilities = JsonSerializer.Deserialize<Dictionary<string, Domain.Internal.BaseData.ValueObjects.DatacronData.Ability>>(ref reader, options);
                        break;
                    case "stats":
                        stats = JsonSerializer.Deserialize<Dictionary<string, Stat>>(ref reader, options);
                        break;
                    default:
                        reader.Skip();
                        break;
                }
            }
        }

        return DatacronData.Create(
            id ?? string.Empty,
            setId,
            isFocused,
            nameKey ?? string.Empty,
            iconKey ?? string.Empty,
            detailPrefab ?? string.Empty,
            expirationTimeMs,
            allowReroll,
            initialTiers,
            maxRerolls,
            referenceTemplateId ?? string.Empty,
            setMaterial ?? [],
            fixedTag ?? [],
            setTier ?? [],
            tier ?? [],
            affixSet ?? [],
            abilities ?? [],
            stats ?? []).Value;
    }

    public override void Write(Utf8JsonWriter writer, DatacronData value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        writer.WriteString("id", value.Id);
        writer.WriteNumber("setId", value.SetId);
        writer.WriteBoolean("isFocused", value.IsFocused);
        writer.WriteString("nameKey", value.NameKey);
        writer.WriteString("iconKey", value.IconKey);
        writer.WriteString("detailPrefab", value.DetailPrefab);
        writer.WriteNumber("expirationTimeMs", value.ExpirationTimeMs);
        writer.WriteBoolean("allowReroll", value.AllowReroll);
        writer.WriteNumber("initialTiers", value.InitialTiers);
        writer.WriteNumber("maxRerolls", value.MaxRerolls);
        writer.WriteString("referenceTemplateId", value.ReferenceTemplateId);

        writer.WriteStartObjectProperty("setMaterial", value.SetMaterial, options);
        writer.WriteStartObjectProperty("fixedTag", value.FixedTag, options);
        writer.WriteStartObjectProperty("setTier", value.SetTier, options);
        writer.WriteStartObjectProperty("tier", value.Tier, options);
        writer.WriteStartObjectProperty("affixSet", value.AffixSet, options);
        writer.WriteStartObjectProperty("abilities", value.Abilities, options);
        writer.WriteStartObjectProperty("stats", value.Stats, options);

        writer.WriteEndObject();
    }
}
