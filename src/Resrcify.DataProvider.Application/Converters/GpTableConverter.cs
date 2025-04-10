using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Resrcify.DataProvider.Domain.Internal.BaseData.ValueObjects.GpTable;

namespace Resrcify.DataProvider.Application.Converters;

internal sealed class GpTableConverter : JsonConverter<GpTable>
{
    public override GpTable Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;

        var crewSizeFactor = JsonSerializer.Deserialize<Dictionary<string, double>>(root.GetProperty("crewSizeFactor").GetRawText());
        var relicTierLevelFactor = JsonSerializer.Deserialize<Dictionary<string, double>>(root.GetProperty("relicTierLevelFactor").GetRawText());
        var gearLevelGp = JsonSerializer.Deserialize<Dictionary<string, long>>(root.GetProperty("gearLevelGp").GetRawText());
        var relicTierGp = JsonSerializer.Deserialize<Dictionary<string, long>>(root.GetProperty("relicTierGp").GetRawText());
        var unitRarityGp = JsonSerializer.Deserialize<Dictionary<string, long>>(root.GetProperty("unitRarityGp").GetRawText());
        var shipRarityFactor = JsonSerializer.Deserialize<Dictionary<string, double>>(root.GetProperty("shipRarityFactor").GetRawText());
        var abilitySpecialGp = JsonSerializer.Deserialize<Dictionary<string, long>>(root.GetProperty("abilitySpecialGp").GetRawText());
        var modRarityLevelTierGp = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, Dictionary<string, long>>>>(root.GetProperty("modRarityLevelTierGp").GetRawText());
        var gearPieceGp = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, long>>>(root.GetProperty("gearPieceGp").GetRawText());
        var crewlessAbilityFactor = JsonSerializer.Deserialize<Dictionary<string, double>>(root.GetProperty("crewlessAbilityFactor").GetRawText());
        var shipLevelGp = JsonSerializer.Deserialize<Dictionary<string, long>>(root.GetProperty("shipLevelGp").GetRawText());
        var abilityLevelGp = JsonSerializer.Deserialize<Dictionary<string, long>>(root.GetProperty("abilityLevelGp").GetRawText());
        var shipAbilityLevelGp = JsonSerializer.Deserialize<Dictionary<string, long>>(root.GetProperty("shipAbilityLevelGp").GetRawText());
        var unitLevelGp = JsonSerializer.Deserialize<Dictionary<string, long>>(root.GetProperty("unitLevelGp").GetRawText());

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
}
