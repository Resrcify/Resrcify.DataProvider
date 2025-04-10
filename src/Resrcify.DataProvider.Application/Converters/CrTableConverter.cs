using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Resrcify.DataProvider.Domain.Internal.BaseData.ValueObjects.CrTable;

namespace Resrcify.DataProvider.Application.Converters;

internal sealed class CrTableConverter : JsonConverter<CrTable>
{
    public override CrTable Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var rootElement = doc.RootElement;

        var agilityRoleAttackerMastery = JsonSerializer.Deserialize<Dictionary<string, double>>(rootElement.GetProperty("agilityRoleAttackerMastery").GetRawText());
        var agilityRoleTankMastery = JsonSerializer.Deserialize<Dictionary<string, double>>(rootElement.GetProperty("agilityRoleTankMastery").GetRawText());
        var agilityRoleSupportMastery = JsonSerializer.Deserialize<Dictionary<string, double>>(rootElement.GetProperty("agilityRoleSupportMastery").GetRawText());
        var agilityRoleHealerMastery = JsonSerializer.Deserialize<Dictionary<string, double>>(rootElement.GetProperty("agilityRoleHealerMastery").GetRawText());
        var strengthRoleAttackerMastery = JsonSerializer.Deserialize<Dictionary<string, double>>(rootElement.GetProperty("strengthRoleAttackerMastery").GetRawText());
        var strengthRoleTankMastery = JsonSerializer.Deserialize<Dictionary<string, double>>(rootElement.GetProperty("strengthRoleTankMastery").GetRawText());
        var strengthRoleSupportMastery = JsonSerializer.Deserialize<Dictionary<string, double>>(rootElement.GetProperty("strengthRoleSupportMastery").GetRawText());
        var strengthRoleHealerMastery = JsonSerializer.Deserialize<Dictionary<string, double>>(rootElement.GetProperty("strengthRoleHealerMastery").GetRawText());
        var intelligenceRoleAttackerMastery = JsonSerializer.Deserialize<Dictionary<string, double>>(rootElement.GetProperty("intelligenceRoleAttackerMastery").GetRawText());
        var intelligenceRoleTankMastery = JsonSerializer.Deserialize<Dictionary<string, double>>(rootElement.GetProperty("intelligenceRoleTankMastery").GetRawText());
        var intelligenceRoleSupportMastery = JsonSerializer.Deserialize<Dictionary<string, double>>(rootElement.GetProperty("intelligenceRoleSupportMastery").GetRawText());
        var intelligenceRoleHealerMastery = JsonSerializer.Deserialize<Dictionary<string, double>>(rootElement.GetProperty("intelligenceRoleHealerMastery").GetRawText());
        var unitLevelCr = JsonSerializer.Deserialize<Dictionary<string, long>>(rootElement.GetProperty("unitLevelCr").GetRawText());
        var relicTierCr = JsonSerializer.Deserialize<Dictionary<string, long>>(rootElement.GetProperty("relicTierCr").GetRawText());
        var relicTierLevelFactor = JsonSerializer.Deserialize<Dictionary<string, double>>(rootElement.GetProperty("relicTierLevelFactor").GetRawText());
        var abilityLevelCr = JsonSerializer.Deserialize<Dictionary<string, long>>(rootElement.GetProperty("abilityLevelCr").GetRawText());
        var modRarityLevelCr = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, long>>>(rootElement.GetProperty("modRarityLevelCr").GetRawText());
        var gearLevelCr = JsonSerializer.Deserialize<Dictionary<string, long>>(rootElement.GetProperty("gearLevelCr").GetRawText());
        var gearPieceCr = JsonSerializer.Deserialize<Dictionary<string, long>>(rootElement.GetProperty("gearPieceCr").GetRawText());
        var crewRarityCr = JsonSerializer.Deserialize<Dictionary<string, long>>(rootElement.GetProperty("crewRarityCr").GetRawText());
        var shipRarityFactor = JsonSerializer.Deserialize<Dictionary<string, double>>(rootElement.GetProperty("shipRarityFactor").GetRawText());
        var crewlessAbilityFactor = JsonSerializer.Deserialize<Dictionary<string, double>>(rootElement.GetProperty("crewlessAbilityFactor").GetRawText());

        return CrTable.Create(
            agilityRoleAttackerMastery ?? [],
            agilityRoleTankMastery ?? [],
            agilityRoleSupportMastery ?? [],
            agilityRoleHealerMastery ?? [],
            strengthRoleAttackerMastery ?? [],
            strengthRoleTankMastery ?? [],
            strengthRoleSupportMastery ?? [],
            strengthRoleHealerMastery ?? [],
            intelligenceRoleAttackerMastery ?? [],
            intelligenceRoleTankMastery ?? [],
            intelligenceRoleSupportMastery ?? [],
            intelligenceRoleHealerMastery ?? [],
            unitLevelCr ?? [],
            relicTierCr ?? [],
            relicTierLevelFactor ?? [],
            abilityLevelCr ?? [],
            modRarityLevelCr ?? [],
            gearLevelCr ?? [],
            gearPieceCr ?? [],
            crewRarityCr ?? [],
            shipRarityFactor ?? [],
            crewlessAbilityFactor ?? []
        ).Value;
    }

    public override void Write(Utf8JsonWriter writer, CrTable value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        writer.WriteStartObjectProperty("agilityRoleAttackerMastery", value.AgilityRoleAttackerMastery, options);
        writer.WriteStartObjectProperty("agilityRoleTankMastery", value.AgilityRoleTankMastery, options);
        writer.WriteStartObjectProperty("agilityRoleSupportMastery", value.AgilityRoleSupportMastery, options);
        writer.WriteStartObjectProperty("agilityRoleHealerMastery", value.AgilityRoleHealerMastery, options);
        writer.WriteStartObjectProperty("strengthRoleAttackerMastery", value.StrengthRoleAttackerMastery, options);
        writer.WriteStartObjectProperty("strengthRoleTankMastery", value.StrengthRoleTankMastery, options);
        writer.WriteStartObjectProperty("strengthRoleSupportMastery", value.StrengthRoleSupportMastery, options);
        writer.WriteStartObjectProperty("strengthRoleHealerMastery", value.StrengthRoleHealerMastery, options);
        writer.WriteStartObjectProperty("intelligenceRoleAttackerMastery", value.IntelligenceRoleAttackerMastery, options);
        writer.WriteStartObjectProperty("intelligenceRoleTankMastery", value.IntelligenceRoleTankMastery, options);
        writer.WriteStartObjectProperty("intelligenceRoleSupportMastery", value.IntelligenceRoleSupportMastery, options);
        writer.WriteStartObjectProperty("intelligenceRoleHealerMastery", value.IntelligenceRoleHealerMastery, options);
        writer.WriteStartObjectProperty("unitLevelCr", value.UnitLevelCr, options);
        writer.WriteStartObjectProperty("relicTierCr", value.RelicTierCr, options);
        writer.WriteStartObjectProperty("relicTierLevelFactor", value.RelicTierLevelFactor, options);
        writer.WriteStartObjectProperty("abilityLevelCr", value.AbilityLevelCr, options);
        writer.WriteStartObjectProperty("modRarityLevelCr", value.ModRarityLevelCr, options);
        writer.WriteStartObjectProperty("gearLevelCr", value.GearLevelCr, options);
        writer.WriteStartObjectProperty("gearPieceCr", value.GearPieceCr, options);
        writer.WriteStartObjectProperty("crewRarityCr", value.CrewRarityCr, options);
        writer.WriteStartObjectProperty("shipRarityFactor", value.ShipRarityFactor, options);
        writer.WriteStartObjectProperty("crewlessAbilityFactor", value.CrewlessAbilityFactor, options);

        writer.WriteEndObject();
    }
}
