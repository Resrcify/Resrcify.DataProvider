using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Resrcify.DataProvider.Application.Extensions;
using Resrcify.DataProvider.Domain.Internal.BaseData.ValueObjects.UnitData;

namespace Resrcify.DataProvider.Application.Converters;

internal sealed class UnitDataConverter : JsonConverter<UnitData>
{
    public override UnitData Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string id = string.Empty;
        string nameKey = string.Empty;
        string name = string.Empty;
        long combatType = 0;
        long forceAlignment = 0;
        List<string> categoryIdList = [];
        long unitClass = 0;
        bool isGalacticLegend = false;
        bool isCapital = false;
        string image = string.Empty;
        long primaryStat = 0;
        Dictionary<string, GearLevel> gearLevels = [];
        Dictionary<string, Dictionary<string, long>> growthModifiers = [];
        List<Skill> skills = [];
        Dictionary<string, string> relics = [];
        string masteryModifierId = string.Empty;
        List<ModRecommendation> modRecommendations = [];
        Dictionary<long, long> stats = [];
        Dictionary<string, long> crewStats = [];
        List<string> crew = [];

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
                    case "name":
                        name = reader.TokenType == JsonTokenType.String ? reader.GetString() ?? string.Empty : string.Empty;
                        break;
                    case "combatType":
                        combatType = reader.TokenType == JsonTokenType.Number ? reader.GetInt64() : 0;
                        break;
                    case "forceAlignment":
                        forceAlignment = reader.TokenType == JsonTokenType.Number ? reader.GetInt64() : 0;
                        break;
                    case "categoryIdList":
                        if (reader.TokenType == JsonTokenType.StartArray)
                        {
                            categoryIdList = JsonSerializer.Deserialize<List<string>>(ref reader, options) ?? [];
                        }
                        break;
                    case "unitClass":
                        unitClass = reader.TokenType == JsonTokenType.Number ? reader.GetInt64() : 0;
                        break;
                    case "isGalacticLegend":
                        isGalacticLegend = reader.TokenType == JsonTokenType.True;
                        break;
                    case "isCapital":
                        isCapital = reader.TokenType == JsonTokenType.True;
                        break;
                    case "image":
                        image = reader.TokenType == JsonTokenType.String ? reader.GetString() ?? string.Empty : string.Empty;
                        break;
                    case "primaryStat":
                        primaryStat = reader.TokenType == JsonTokenType.Number ? reader.GetInt64() : 0;
                        break;
                    case "gearLevels":
                        if (reader.TokenType == JsonTokenType.StartObject)
                        {
                            gearLevels = JsonSerializer.Deserialize<Dictionary<string, GearLevel>>(ref reader, options) ?? [];
                        }
                        break;
                    case "growthModifiers":
                        if (reader.TokenType == JsonTokenType.StartObject)
                        {
                            growthModifiers = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, long>>>(ref reader, options) ?? [];
                        }
                        break;
                    case "skills":
                        if (reader.TokenType == JsonTokenType.StartArray)
                        {
                            skills = JsonSerializer.Deserialize<List<Skill>>(ref reader, options) ?? [];
                        }
                        break;
                    case "relics":
                        if (reader.TokenType == JsonTokenType.StartObject)
                        {
                            relics = JsonSerializer.Deserialize<Dictionary<string, string>>(ref reader, options) ?? [];
                        }
                        break;
                    case "masteryModifierId":
                        masteryModifierId = reader.TokenType == JsonTokenType.String ? reader.GetString() ?? string.Empty : string.Empty;
                        break;
                    case "modRecommendations":
                        if (reader.TokenType == JsonTokenType.StartArray)
                        {
                            modRecommendations = JsonSerializer.Deserialize<List<ModRecommendation>>(ref reader, options) ?? [];
                        }
                        break;
                    case "stats":
                        if (reader.TokenType == JsonTokenType.StartObject)
                        {
                            stats = JsonSerializer.Deserialize<Dictionary<long, long>>(ref reader, options) ?? [];
                        }
                        break;
                    case "crewStats":
                        if (reader.TokenType == JsonTokenType.StartObject)
                        {
                            crewStats = JsonSerializer.Deserialize<Dictionary<string, long>>(ref reader, options) ?? [];
                        }
                        break;
                    case "crew":
                        if (reader.TokenType == JsonTokenType.StartArray)
                        {
                            crew = JsonSerializer.Deserialize<List<string>>(ref reader, options) ?? [];
                        }
                        break;
                    default:
                        reader.Skip();
                        break;
                }
            }
        }

        return UnitData.Create(
            id ?? string.Empty,
            nameKey ?? string.Empty,
            name ?? string.Empty,
            combatType,
            forceAlignment,
            categoryIdList ?? [],
            unitClass,
            isGalacticLegend,
            isCapital,
            image ?? string.Empty,
            primaryStat,
            gearLevels ?? [],
            growthModifiers ?? [],
            skills ?? [],
            relics ?? [],
            masteryModifierId ?? string.Empty,
            modRecommendations ?? [],
            stats ?? [],
            crewStats ?? [],
            crew ?? []
        ).Value;
    }

    public override void Write(Utf8JsonWriter writer, UnitData value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("id", value.Id);
        writer.WriteString("nameKey", value.NameKey);
        writer.WriteString("name", value.Name);
        writer.WriteNumber("combatType", value.CombatType);
        writer.WriteNumber("forceAlignment", value.ForceAlignment);
        writer.WriteStartObjectProperty("categoryIdList", value.CategoryIdList, options);
        writer.WriteNumber("unitClass", value.UnitClass);
        writer.WriteBoolean("isGalacticLegend", value.IsGalacticLegend);
        writer.WriteString("image", value.Image);
        writer.WriteNumber("primaryStat", value.PrimaryStat);
        writer.WriteStartObjectProperty("gearLevels", value.GearLevels, options);
        writer.WriteStartObjectProperty("growthModifiers", value.GrowthModifiers, options);
        writer.WriteStartObjectProperty("skills", value.Skills, options);
        writer.WriteStartObjectProperty("relics", value.Relics, options);
        writer.WriteString("masteryModifierId", value.MasteryModifierId);
        writer.WriteStartObjectProperty("modRecommendations", value.ModRecommendations, options);
        writer.WriteStartObjectProperty("stats", value.Stats, options);
        writer.WriteStartObjectProperty("crewStats", value.CrewStats, options);
        writer.WriteStartObjectProperty("crew", value.Crew, options);
        writer.WriteEndObject();
    }
}
