using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Resrcify.DataProvider.Application.Extensions;
using Resrcify.DataProvider.Domain.Internal.BaseData.ValueObjects.GpTable;

namespace Resrcify.DataProvider.Application.Converters;

internal sealed class GpTableConverter : JsonConverter<GpTable>
{
    public override GpTable Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var crewSizeFactor = new Dictionary<string, double>();
        var relicTierLevelFactor = new Dictionary<string, double>();
        var gearLevelGp = new Dictionary<string, long>();
        var relicTierGp = new Dictionary<string, long>();
        var unitRarityGp = new Dictionary<string, long>();
        var shipRarityFactor = new Dictionary<string, double>();
        var abilitySpecialGp = new Dictionary<string, long>();
        var modRarityLevelTierGp = new Dictionary<string, Dictionary<string, Dictionary<string, long>>>();
        var gearPieceGp = new Dictionary<string, Dictionary<string, long>>();
        var crewlessAbilityFactor = new Dictionary<string, double>();
        var shipLevelGp = new Dictionary<string, long>();
        var abilityLevelGp = new Dictionary<string, long>();
        var shipAbilityLevelGp = new Dictionary<string, long>();
        var unitLevelGp = new Dictionary<string, long>();

        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException();

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
                break;

            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                string propertyName = reader.GetString() ?? string.Empty;

                switch (propertyName)
                {
                    case "crewSizeFactor":
                        reader.Read();
                        ReadDictionary(ref reader, ref crewSizeFactor);
                        break;

                    case "relicTierLevelFactor":
                        reader.Read();
                        ReadDictionary(ref reader, ref relicTierLevelFactor);
                        break;

                    case "gearLevelGp":
                        reader.Read();
                        ReadDictionary(ref reader, ref gearLevelGp);
                        break;

                    case "relicTierGp":
                        reader.Read();
                        ReadDictionary(ref reader, ref relicTierGp);
                        break;

                    case "unitRarityGp":
                        reader.Read();
                        ReadDictionary(ref reader, ref unitRarityGp);
                        break;

                    case "shipRarityFactor":
                        reader.Read();
                        ReadDictionary(ref reader, ref shipRarityFactor);
                        break;

                    case "abilitySpecialGp":
                        reader.Read();
                        ReadDictionary(ref reader, ref abilitySpecialGp);
                        break;

                    case "modRarityLevelTierGp":
                        reader.Read();
                        ReadDictionaryOfDictionaries(ref reader, ref modRarityLevelTierGp);
                        break;

                    case "gearPieceGp":
                        reader.Read();
                        ReadDictionaryOfDictionaries(ref reader, ref gearPieceGp);
                        break;

                    case "crewlessAbilityFactor":
                        reader.Read();
                        ReadDictionary(ref reader, ref crewlessAbilityFactor);
                        break;

                    case "shipLevelGp":
                        reader.Read();
                        ReadDictionary(ref reader, ref shipLevelGp);
                        break;

                    case "abilityLevelGp":
                        reader.Read();
                        ReadDictionary(ref reader, ref abilityLevelGp);
                        break;

                    case "shipAbilityLevelGp":
                        reader.Read();
                        ReadDictionary(ref reader, ref shipAbilityLevelGp);
                        break;

                    case "unitLevelGp":
                        reader.Read();
                        ReadDictionary(ref reader, ref unitLevelGp);
                        break;

                    default:
                        reader.Skip();
                        break;
                }
            }
        }

        return GpTable.Create(
            crewSizeFactor ?? [],
            relicTierLevelFactor ?? [],
            gearLevelGp ?? [],
            relicTierGp ?? [],
            unitRarityGp ?? [],
            shipRarityFactor ?? [],
            abilitySpecialGp ?? [],
            modRarityLevelTierGp ?? [],
            gearPieceGp ?? [],
            crewlessAbilityFactor ?? [],
            shipLevelGp ?? [],
            abilityLevelGp ?? [],
            shipAbilityLevelGp ?? [],
            unitLevelGp ?? []
        ).Value;
    }

    public override void Write(Utf8JsonWriter writer, GpTable value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        writer.WriteStartObjectProperty("crewSizeFactor", value.CrewSizeFactor, options);
        writer.WriteStartObjectProperty("relicTierLevelFactor", value.RelicTierLevelFactor, options);
        writer.WriteStartObjectProperty("gearLevelGp", value.GearLevelGp, options);
        writer.WriteStartObjectProperty("relicTierGp", value.RelicTierGp, options);
        writer.WriteStartObjectProperty("unitRarityGp", value.UnitRarityGp, options);
        writer.WriteStartObjectProperty("shipRarityFactor", value.ShipRarityFactor, options);
        writer.WriteStartObjectProperty("abilitySpecialGp", value.AbilitySpecialGp, options);
        writer.WriteStartObjectProperty("modRarityLevelTierGp", value.ModRarityLevelTierGp, options);
        writer.WriteStartObjectProperty("gearPieceGp", value.GearPieceGp, options);
        writer.WriteStartObjectProperty("crewlessAbilityFactor", value.CrewlessAbilityFactor, options);
        writer.WriteStartObjectProperty("shipLevelGp", value.ShipLevelGp, options);
        writer.WriteStartObjectProperty("abilityLevelGp", value.AbilityLevelGp, options);
        writer.WriteStartObjectProperty("shipAbilityLevelGp", value.ShipAbilityLevelGp, options);
        writer.WriteStartObjectProperty("unitLevelGp", value.UnitLevelGp, options);

        writer.WriteEndObject();
    }
    private void ReadDictionaryOfDictionaries(ref Utf8JsonReader reader, ref Dictionary<string, Dictionary<string, Dictionary<string, long>>> dictionary)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException();

        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
        {
            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                string key = reader.GetString() ?? string.Empty;
                reader.Read();

                if (reader.TokenType == JsonTokenType.StartObject)
                {
                    var innerDict = new Dictionary<string, Dictionary<string, long>>();
                    ReadInnerDictionary(ref reader, ref innerDict);
                    dictionary[key] = innerDict;
                }
                else
                {
                    throw new JsonException("Expected object for nested dictionary");
                }
            }
        }
    }

    private void ReadInnerDictionary(ref Utf8JsonReader reader, ref Dictionary<string, Dictionary<string, long>> dictionary)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException();

        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
        {
            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                string key = reader.GetString() ?? string.Empty;
                reader.Read();

                if (reader.TokenType == JsonTokenType.StartObject)
                {
                    var innerMostDict = new Dictionary<string, long>();
                    ReadDictionary(ref reader, ref innerMostDict);
                    dictionary[key] = innerMostDict;
                }
                else
                {
                    throw new JsonException("Expected object for nested dictionary");
                }
            }
        }
    }
    private void ReadDictionary(ref Utf8JsonReader reader, ref Dictionary<string, double> dictionary)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException();

        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
        {
            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                string key = reader.GetString() ?? string.Empty;
                reader.Read();

                dictionary[key] = reader.TokenType == JsonTokenType.Number
                    ? reader.GetDouble()
                    : throw new JsonException("Expected number for dictionary value");
            }
        }
    }

    private void ReadDictionary(ref Utf8JsonReader reader, ref Dictionary<string, long> dictionary)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException();

        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
        {
            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                string key = reader.GetString() ?? string.Empty;
                reader.Read();

                dictionary[key] = reader.TokenType == JsonTokenType.Number
                    ? reader.GetInt64()
                    : throw new JsonException("Expected number for dictionary value");
            }
        }
    }

    private void ReadDictionaryOfDictionaries(ref Utf8JsonReader reader, ref Dictionary<string, Dictionary<string, long>> dictionary)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException();

        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
        {
            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                string key = reader.GetString() ?? string.Empty;
                reader.Read();

                if (reader.TokenType == JsonTokenType.StartObject)
                {
                    var innerDict = new Dictionary<string, long>();
                    ReadDictionary(ref reader, ref innerDict);
                    dictionary[key] = innerDict;
                }
                else
                {
                    throw new JsonException("Expected object for nested dictionary");
                }
            }
        }
    }
}
