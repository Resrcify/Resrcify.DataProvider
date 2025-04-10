using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Resrcify.DataProvider.Domain.Internal.BaseData.ValueObjects.UnitData;

namespace Resrcify.DataProvider.Application.Converters;

internal sealed class UnitDataConverter : JsonConverter<UnitData>
{
    public override UnitData Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;
        var id = root.GetProperty("id").GetString();
        var nameKey = root.GetProperty("nameKey").GetString();
        var name = root.GetProperty("name").GetString();
        var combatType = root.GetProperty("combatType").GetInt64();
        var forceAlignment = root.GetProperty("forceAlignment").GetInt64();
        var categoryIdList = JsonSerializer.Deserialize<List<string>>(root.GetProperty("categoryIdList").GetRawText(), options);
        var unitClass = root.GetProperty("unitClass").GetInt64();
        var isGalacticLegend = root.GetProperty("isGalacticLegend").GetBoolean();
        var image = root.GetProperty("image").GetString();
        var primaryStat = root.GetProperty("primaryStat").GetInt64();
        var gearLevels = JsonSerializer.Deserialize<Dictionary<string, GearLevel>>(root.GetProperty("gearLevels").GetRawText(), options);
        var growthModifiers = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, long>>>(root.GetProperty("growthModifiers").GetRawText(), options);
        var skills = JsonSerializer.Deserialize<List<Skill>>(root.GetProperty("skills").GetRawText(), options);
        var relics = JsonSerializer.Deserialize<Dictionary<string, string>>(root.GetProperty("relics").GetRawText(), options);
        var masteryModifierId = root.GetProperty("masteryModifierId").GetString();
        var modRecommendations = JsonSerializer.Deserialize<List<ModRecommendation>>(root.GetProperty("modRecommendations").GetRawText(), options);
        var stats = JsonSerializer.Deserialize<Dictionary<long, long>>(root.GetProperty("stats").GetRawText(), options);
        var crewStats = JsonSerializer.Deserialize<Dictionary<string, long>>(root.GetProperty("crewStats").GetRawText(), options);
        var crew = JsonSerializer.Deserialize<List<string>>(root.GetProperty("crew").GetRawText(), options);

        return UnitData.Create(
            id ?? string.Empty,
            nameKey ?? string.Empty,
            name ?? string.Empty,
            combatType,
            forceAlignment,
            categoryIdList ?? [],
            unitClass,
            isGalacticLegend,
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
