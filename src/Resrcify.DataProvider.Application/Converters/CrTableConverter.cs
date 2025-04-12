using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Resrcify.DataProvider.Application.Extensions;
using Resrcify.DataProvider.Domain.Internal.BaseData.ValueObjects.CrTable;

namespace Resrcify.DataProvider.Application.Converters;

internal sealed class CrTableConverter : JsonConverter<CrTable>
{
    public override CrTable Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException();

        Dictionary<string, double>? agilityRoleAttackerMastery = null;
        Dictionary<string, double>? agilityRoleTankMastery = null;
        Dictionary<string, double>? agilityRoleSupportMastery = null;
        Dictionary<string, double>? agilityRoleHealerMastery = null;
        Dictionary<string, double>? strengthRoleAttackerMastery = null;
        Dictionary<string, double>? strengthRoleTankMastery = null;
        Dictionary<string, double>? strengthRoleSupportMastery = null;
        Dictionary<string, double>? strengthRoleHealerMastery = null;
        Dictionary<string, double>? intelligenceRoleAttackerMastery = null;
        Dictionary<string, double>? intelligenceRoleTankMastery = null;
        Dictionary<string, double>? intelligenceRoleSupportMastery = null;
        Dictionary<string, double>? intelligenceRoleHealerMastery = null;
        Dictionary<string, long>? unitLevelCr = null;
        Dictionary<string, long>? relicTierCr = null;
        Dictionary<string, double>? relicTierLevelFactor = null;
        Dictionary<string, long>? abilityLevelCr = null;
        Dictionary<string, Dictionary<string, long>>? modRarityLevelCr = null;
        Dictionary<string, long>? gearLevelCr = null;
        Dictionary<string, long>? gearPieceCr = null;
        Dictionary<string, long>? crewRarityCr = null;
        Dictionary<string, double>? shipRarityFactor = null;
        Dictionary<string, double>? crewlessAbilityFactor = null;

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
                break;

            if (reader.TokenType != JsonTokenType.PropertyName)
                throw new JsonException();

            string propName = reader.GetString()!;
            reader.Read();

            switch (propName)
            {
                case "agilityRoleAttackerMastery":
                    agilityRoleAttackerMastery = JsonSerializer.Deserialize<Dictionary<string, double>>(ref reader, options);
                    break;
                case "agilityRoleTankMastery":
                    agilityRoleTankMastery = JsonSerializer.Deserialize<Dictionary<string, double>>(ref reader, options);
                    break;
                case "agilityRoleSupportMastery":
                    agilityRoleSupportMastery = JsonSerializer.Deserialize<Dictionary<string, double>>(ref reader, options);
                    break;
                case "agilityRoleHealerMastery":
                    agilityRoleHealerMastery = JsonSerializer.Deserialize<Dictionary<string, double>>(ref reader, options);
                    break;
                case "strengthRoleAttackerMastery":
                    strengthRoleAttackerMastery = JsonSerializer.Deserialize<Dictionary<string, double>>(ref reader, options);
                    break;
                case "strengthRoleTankMastery":
                    strengthRoleTankMastery = JsonSerializer.Deserialize<Dictionary<string, double>>(ref reader, options);
                    break;
                case "strengthRoleSupportMastery":
                    strengthRoleSupportMastery = JsonSerializer.Deserialize<Dictionary<string, double>>(ref reader, options);
                    break;
                case "strengthRoleHealerMastery":
                    strengthRoleHealerMastery = JsonSerializer.Deserialize<Dictionary<string, double>>(ref reader, options);
                    break;
                case "intelligenceRoleAttackerMastery":
                    intelligenceRoleAttackerMastery = JsonSerializer.Deserialize<Dictionary<string, double>>(ref reader, options);
                    break;
                case "intelligenceRoleTankMastery":
                    intelligenceRoleTankMastery = JsonSerializer.Deserialize<Dictionary<string, double>>(ref reader, options);
                    break;
                case "intelligenceRoleSupportMastery":
                    intelligenceRoleSupportMastery = JsonSerializer.Deserialize<Dictionary<string, double>>(ref reader, options);
                    break;
                case "intelligenceRoleHealerMastery":
                    intelligenceRoleHealerMastery = JsonSerializer.Deserialize<Dictionary<string, double>>(ref reader, options);
                    break;
                case "unitLevelCr":
                    unitLevelCr = JsonSerializer.Deserialize<Dictionary<string, long>>(ref reader, options);
                    break;
                case "relicTierCr":
                    relicTierCr = JsonSerializer.Deserialize<Dictionary<string, long>>(ref reader, options);
                    break;
                case "relicTierLevelFactor":
                    relicTierLevelFactor = JsonSerializer.Deserialize<Dictionary<string, double>>(ref reader, options);
                    break;
                case "abilityLevelCr":
                    abilityLevelCr = JsonSerializer.Deserialize<Dictionary<string, long>>(ref reader, options);
                    break;
                case "modRarityLevelCr":
                    modRarityLevelCr = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, long>>>(ref reader, options);
                    break;
                case "gearLevelCr":
                    gearLevelCr = JsonSerializer.Deserialize<Dictionary<string, long>>(ref reader, options);
                    break;
                case "gearPieceCr":
                    gearPieceCr = JsonSerializer.Deserialize<Dictionary<string, long>>(ref reader, options);
                    break;
                case "crewRarityCr":
                    crewRarityCr = JsonSerializer.Deserialize<Dictionary<string, long>>(ref reader, options);
                    break;
                case "shipRarityFactor":
                    shipRarityFactor = JsonSerializer.Deserialize<Dictionary<string, double>>(ref reader, options);
                    break;
                case "crewlessAbilityFactor":
                    crewlessAbilityFactor = JsonSerializer.Deserialize<Dictionary<string, double>>(ref reader, options);
                    break;
                default:
                    reader.Skip();
                    break;
            }
        }

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
