using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Resrcify.SharedKernel.DomainDrivenDesign.Primitives;
using Resrcify.SharedKernel.ResultFramework.Primitives;
using Titan.DataProvider.Domain.Models.GalaxyOfHeroes.GameData;

namespace Titan.DataProvider.Domain.Internal.BaseData.ValueObjects.CrTable;

public sealed class CrTable : ValueObject
{
    private readonly Dictionary<string, double> _agilityRoleSupportMastery = [];
    public IReadOnlyDictionary<string, double> AgilityRoleSupportMastery => _agilityRoleSupportMastery;
    private readonly Dictionary<string, double> _strengthRoleTankMastery = [];
    public IReadOnlyDictionary<string, double> StrengthRoleTankMastery => _strengthRoleTankMastery;
    private readonly Dictionary<string, long> _relicTierCr = [];
    public IReadOnlyDictionary<string, long> RelicTierCr => _relicTierCr;
    private readonly Dictionary<string, double> _intelligenceRoleTankMastery = [];
    public IReadOnlyDictionary<string, double> IntelligenceRoleTankMastery => _intelligenceRoleTankMastery;
    private readonly Dictionary<string, double> _agilityRoleAttackerMastery = [];
    public IReadOnlyDictionary<string, double> AgilityRoleAttackerMastery => _agilityRoleAttackerMastery;
    private readonly Dictionary<string, long> _gearLevelCr = [];
    public IReadOnlyDictionary<string, long> GearLevelCr => _gearLevelCr;
    private readonly Dictionary<string, double> _strengthRoleHealerMastery = [];
    public IReadOnlyDictionary<string, double> StrengthRoleHealerMastery => _strengthRoleHealerMastery;
    private readonly Dictionary<string, long> _crewRarityCr = [];
    public IReadOnlyDictionary<string, long> CrewRarityCr => _crewRarityCr;
    private readonly Dictionary<string, double> _relicTierLevelFactor = [];
    public IReadOnlyDictionary<string, double> RelicTierLevelFactor => _relicTierLevelFactor;
    private readonly Dictionary<string, double> _intelligenceRoleHealerMastery = [];
    public IReadOnlyDictionary<string, double> IntelligenceRoleHealerMastery => _intelligenceRoleHealerMastery;
    private readonly Dictionary<string, long> _gearPieceCr = [];
    public IReadOnlyDictionary<string, long> GearPieceCr => _gearPieceCr;
    private readonly Dictionary<string, double> _strengthRoleAttackerMastery = [];
    public IReadOnlyDictionary<string, double> StrengthRoleAttackerMastery => _strengthRoleAttackerMastery;
    private readonly Dictionary<string, double> _intelligenceRoleSupportMastery = [];
    public IReadOnlyDictionary<string, double> IntelligenceRoleSupportMastery => _intelligenceRoleSupportMastery;
    private readonly Dictionary<string, double> _crewlessAbilityFactor = [];
    public IReadOnlyDictionary<string, double> CrewlessAbilityFactor => _crewlessAbilityFactor;
    private readonly Dictionary<string, double> _shipRarityFactor = [];
    public IReadOnlyDictionary<string, double> ShipRarityFactor => _shipRarityFactor;
    private readonly Dictionary<string, double> _intelligenceRoleAttackerMastery = [];
    public IReadOnlyDictionary<string, double> IntelligenceRoleAttackerMastery => _intelligenceRoleAttackerMastery;
    private readonly Dictionary<string, Dictionary<string, long>> _modRarityLevelCr = [];
    public IReadOnlyDictionary<string, Dictionary<string, long>> ModRarityLevelCr => _modRarityLevelCr;
    private readonly Dictionary<string, double> _agilityRoleTankMastery = [];
    public IReadOnlyDictionary<string, double> AgilityRoleTankMastery => _agilityRoleTankMastery;
    private readonly Dictionary<string, double> _agilityRoleHealerMastery = [];
    public IReadOnlyDictionary<string, double> AgilityRoleHealerMastery => _agilityRoleHealerMastery;
    private readonly Dictionary<string, double> _strengthRoleSupportMastery = [];
    public IReadOnlyDictionary<string, double> StrengthRoleSupportMastery => _strengthRoleSupportMastery;
    private readonly Dictionary<string, long> _abilityLevelCr = [];
    public IReadOnlyDictionary<string, long> AbilityLevelCr => _abilityLevelCr;
    private readonly Dictionary<string, long> _unitLevelCr = [];
    public IReadOnlyDictionary<string, long> UnitLevelCr => _unitLevelCr;

    private CrTable(
        Dictionary<string, double> agilityRoleAttackerMastery,
        Dictionary<string, double> agilityRoleTankMastery,
        Dictionary<string, double> agilityRoleSupportMastery,
        Dictionary<string, double> agilityRoleHealerMastery,
        Dictionary<string, double> strengthRoleAttackerMastery,
        Dictionary<string, double> strengthRoleTankMastery,
        Dictionary<string, double> strengthRoleSupportMastery,
        Dictionary<string, double> strengthRoleHealerMastery,
        Dictionary<string, double> intelligenceRoleAttackerMastery,
        Dictionary<string, double> intelligenceRoleTankMastery,
        Dictionary<string, double> intelligenceRoleSupportMastery,
        Dictionary<string, double> intelligenceRoleHealerMastery,
        Dictionary<string, long> unitLevelCr,
        Dictionary<string, long> relicTierCr,
        Dictionary<string, double> relicTierLevelFactor,
        Dictionary<string, long> abilityLevelCr,
        Dictionary<string, Dictionary<string, long>> modRarityLevelCr,
        Dictionary<string, long> gearLevelCr,
        Dictionary<string, long> gearPieceCr,
        Dictionary<string, long> crewRarityCr,
        Dictionary<string, double> shipRarityFactor,
        Dictionary<string, double> crewlessAbilityFactor
    )
    {
        _agilityRoleSupportMastery = agilityRoleSupportMastery;
        _strengthRoleTankMastery = strengthRoleTankMastery;
        _relicTierCr = relicTierCr;
        _intelligenceRoleTankMastery = intelligenceRoleTankMastery;
        _agilityRoleAttackerMastery = agilityRoleAttackerMastery;
        _gearLevelCr = gearLevelCr;
        _strengthRoleHealerMastery = strengthRoleHealerMastery;
        _crewRarityCr = crewRarityCr;
        _relicTierLevelFactor = relicTierLevelFactor;
        _intelligenceRoleHealerMastery = intelligenceRoleHealerMastery;
        _gearPieceCr = gearPieceCr;
        _strengthRoleAttackerMastery = strengthRoleAttackerMastery;
        _intelligenceRoleSupportMastery = intelligenceRoleSupportMastery;
        _crewlessAbilityFactor = crewlessAbilityFactor;
        _shipRarityFactor = shipRarityFactor;
        _intelligenceRoleAttackerMastery = intelligenceRoleAttackerMastery;
        _modRarityLevelCr = modRarityLevelCr;
        _agilityRoleTankMastery = agilityRoleTankMastery;
        _agilityRoleHealerMastery = agilityRoleHealerMastery;
        _strengthRoleSupportMastery = strengthRoleSupportMastery;
        _abilityLevelCr = abilityLevelCr;
        _unitLevelCr = unitLevelCr;
    }

    public static Result<CrTable> Create(GameDataResponse data)
    {
        var crewContributionMultiplierPerRarityTable = data.Tables.First(x => x.Id == "crew_contribution_multiplier_per_rarity");
        var crewRatingPerModRarityLevelTierTable = data.Tables.First(x => x.Id == "crew_rating_per_mod_rarity_level_tier");
        var crewRatingModifierPerRelicTierTable = data.Tables.First(x => x.Id == "crew_rating_modifier_per_relic_tier");
        var crewRatingPerRelicTierTable = data.Tables.First(x => x.Id == "crew_rating_per_relic_tier");
        var crewRatingModifierPerAbilityCrewlessShipsTable = data.Tables.First(x => x.Id == "crew_rating_modifier_per_ability_crewless_ships");
        var agilityRoleSupportMasteryTable = data.Tables.First(x => x.Id == "agility_role_support_mastery");
        var agilityRoleTankMasteryTable = data.Tables.First(x => x.Id == "agility_role_tank_mastery");
        var agilityRoleHealerMasteryTable = data.Tables.First(x => x.Id == "agility_role_healer_mastery");
        var agilityRoleAttackerMasteryTable = data.Tables.First(x => x.Id == "agility_role_attacker_mastery");
        var intelligenceRoleTankMasteryTable = data.Tables.First(x => x.Id == "intelligence_role_tank_mastery");
        var intelligenceRoleHealerMasteryTable = data.Tables.First(x => x.Id == "intelligence_role_healer_mastery");
        var intelligenceRoleSupportMasteryTable = data.Tables.First(x => x.Id == "intelligence_role_support_mastery");
        var intelligenceRoleAttackerMasteryTable = data.Tables.First(x => x.Id == "intelligence_role_attacker_mastery");
        var strengthRoleHealerMasteryTable = data.Tables.First(x => x.Id == "strength_role_healer_mastery");
        var strengthRoleTankMasteryTable = data.Tables.First(x => x.Id == "strength_role_tank_mastery");
        var strengthRoleAttackerMasteryTable = data.Tables.First(x => x.Id == "strength_role_attacker_mastery");
        var strengthRoleSupportMasteryTable = data.Tables.First(x => x.Id == "strength_role_support_mastery");
        var crewRatingPerUnitRarityTable = data.Tables.First(x => x.Id == "crew_rating_per_unit_rarity");
        var galacticPowerPerCompleteGearTierTable = data.Tables.First(x => x.Id == "galactic_power_per_complete_gear_tier_table");
        var crewRatingPerGearPieceAtTierTable = data.Tables.First(x => x.Id == "crew_rating_per_gear_piece_at_tier");
        var crewRatingPerUnitLevel = data.XpTables.First(x => x.Id == "crew_rating_per_unit_level");
        var crewRatingPerAbilityLevel = data.XpTables.First(x => x.Id == "crew_rating_per_ability_level");

        var gearPieceCr = GetCrewRating(crewRatingPerGearPieceAtTierTable);
        var gearLevelCr = GetGearRating(galacticPowerPerCompleteGearTierTable);
        var crewRarityCr = CreateDictionaryFromEnum<RarityEnum, long>(crewRatingPerUnitRarityTable.Rows);
        var agilityRoleAttackerMastery = CreateDictionaryFromEnum<StatEnum, double>(agilityRoleAttackerMasteryTable.Rows);
        var agilityRoleTankMastery = CreateDictionaryFromEnum<StatEnum, double>(agilityRoleTankMasteryTable.Rows);
        var agilityRoleSupportMastery = CreateDictionaryFromEnum<StatEnum, double>(agilityRoleSupportMasteryTable.Rows);
        var agilityRoleHealerMastery = CreateDictionaryFromEnum<StatEnum, double>(agilityRoleHealerMasteryTable.Rows);
        var strengthRoleAttackerMastery = CreateDictionaryFromEnum<StatEnum, double>(strengthRoleAttackerMasteryTable.Rows);
        var strengthRoleTankMastery = CreateDictionaryFromEnum<StatEnum, double>(strengthRoleTankMasteryTable.Rows);
        var strengthRoleSupportMastery = CreateDictionaryFromEnum<StatEnum, double>(strengthRoleSupportMasteryTable.Rows);
        var strengthRoleHealerMastery = CreateDictionaryFromEnum<StatEnum, double>(strengthRoleHealerMasteryTable.Rows);
        var intelligenceRoleAttackerMastery = CreateDictionaryFromEnum<StatEnum, double>(intelligenceRoleAttackerMasteryTable.Rows);
        var intelligenceRoleTankMastery = CreateDictionaryFromEnum<StatEnum, double>(intelligenceRoleTankMasteryTable.Rows);
        var intelligenceRoleSupportMastery = CreateDictionaryFromEnum<StatEnum, double>(intelligenceRoleSupportMasteryTable.Rows);
        var intelligenceRoleHealerMastery = CreateDictionaryFromEnum<StatEnum, double>(intelligenceRoleHealerMasteryTable.Rows);
        var relicTierLevelFactor = CreateDictionaryFromRelics<double>(crewRatingModifierPerRelicTierTable.Rows);
        var relicTierCr = CreateDictionaryFromRelics<long>(crewRatingPerRelicTierTable.Rows);
        var crewlessAbilityFactor = CreateDictionary<double>(crewRatingModifierPerAbilityCrewlessShipsTable.Rows);
        var shipRarityFactor = CreateDictionaryFromEnum<RarityEnum, double>(crewContributionMultiplierPerRarityTable.Rows);
        var modRarityLevelCr = GetModRating(crewRatingPerModRarityLevelTierTable);
        var abilityLevelCr = GetXpTable(crewRatingPerAbilityLevel);
        var unitLevelCr = GetXpTable(crewRatingPerUnitLevel);


        return new CrTable(
            agilityRoleAttackerMastery,
            agilityRoleTankMastery,
            agilityRoleSupportMastery,
            agilityRoleHealerMastery,
            strengthRoleAttackerMastery,
            strengthRoleTankMastery,
            strengthRoleSupportMastery,
            strengthRoleHealerMastery,
            intelligenceRoleAttackerMastery,
            intelligenceRoleTankMastery,
            intelligenceRoleSupportMastery,
            intelligenceRoleHealerMastery,
            unitLevelCr,
            relicTierCr,
            relicTierLevelFactor,
            abilityLevelCr,
            modRarityLevelCr,
            gearLevelCr,
            gearPieceCr,
            crewRarityCr,
            shipRarityFactor,
            crewlessAbilityFactor
        );
    }
    private static Dictionary<string, long> GetXpTable(XpTable table)
    {
        var tempTable = new Dictionary<string, long>();
        foreach (var row in table.Rows)
        {
            int key = row.Index + 1;
            tempTable[key.ToString()] = row.Xp;
        }

        return tempTable.ToDictionary(k => k.Key, v => Convert.ToInt64(v.Value, System.Globalization.CultureInfo.InvariantCulture));
    }

    private static Dictionary<string, Dictionary<string, long>> GetModRating(Table crewRatingPerModRarityLevelTierTable)
    {
        Dictionary<string, Dictionary<string, long>> c = [];
        foreach (var row in crewRatingPerModRarityLevelTierTable.Rows.OrderBy(l => int.Parse(l.Key!.Split(':', 4)[1])).ThenBy(p => int.Parse(p.Key!.Split(':', 4)[0])))
        {
            if (row.Key!.Last().ToString() == "0") // only 'select' set 0, as set doesn't affect CR or GP
            {
                var split = row.Key!.Split(':', 4);
                var pips = split[0];
                var level = split[1];
                var tier = split[2];
                var set = split[3];

                if (tier == "1") // tier doesn't affect CR, so only save for tier 1
                {
                    if (!c.ContainsKey(pips)) // ensure rarity table exists
                        c[pips] = [];
                    //         c[pips] = c[pips] || { }; // ensure table exists for that rarity
                    c[pips][level] = long.Parse(row.Value!, System.Globalization.CultureInfo.InvariantCulture);
                }
            }
        }
        return c;
    }

    private static Dictionary<string, long> GetGearRating(Table galacticPowerPerCompleteGearTierTable)
    {
        var gearLevelGp = new Dictionary<string, long>
        {
            { "1", 0 } // initialize with value of 0 for unit's at gear 1 (which have none 'complete')
        };
        foreach (var row in galacticPowerPerCompleteGearTierTable.Rows.OrderBy(s => s.Key))
        {
            var pattern = @"TIER_0?(\d+)";
            Regex rgx = new(pattern, RegexOptions.IgnoreCase);
            var gearMatches = rgx.Match(row.Key!);
            if (gearMatches.Success)
            {
                var key = long.Parse(gearMatches.Groups[1].Value, System.Globalization.CultureInfo.InvariantCulture);
                key = ++key;
                gearLevelGp.Add(key.ToString(), long.Parse(row.Value!, System.Globalization.CultureInfo.InvariantCulture));
            }
        }
        return gearLevelGp;
    }

    private static Dictionary<string, long> GetCrewRating(Table crewRatingPerGearPieceAtTier)
    {
        var gearPieceCr = new Dictionary<string, long>();
        foreach (var row in crewRatingPerGearPieceAtTier.Rows.OrderBy(s => s.Key))
        {
            var pattern = @"TIER_0?(\d+)";
            Regex rgx = new(pattern, RegexOptions.IgnoreCase);
            var gearMatches = rgx.Match(row.Key!);
            if (gearMatches.Success)
                gearPieceCr.Add(gearMatches.Groups[1].Value, long.Parse(row.Value!, System.Globalization.CultureInfo.InvariantCulture));
        }
        return gearPieceCr;
    }
    private static Dictionary<string, V> CreateDictionaryFromEnum<T, V>(List<TableRow> rows) where T : struct
    {
        var dictionary = new Dictionary<string, V>();
        foreach (var row in rows.OrderBy(s => Convert.ToInt32(Enum.Parse<T>(s.Key!))))
        {
            var e = Enum.Parse<T>(row.Key!);
            var key = Convert.ToInt32(e);
            dictionary[key.ToString()] = (V)Convert.ChangeType(row.Value!, typeof(V), System.Globalization.CultureInfo.InvariantCulture);
        }
        return dictionary;
    }

    private static Dictionary<string, V> CreateDictionaryFromRelics<V>(List<TableRow> rows)
    {
        var dictionary = new Dictionary<string, V>();
        foreach (var row in rows.OrderBy(s => s.Key))
        {
            var key = int.Parse(row.Key!);
            key += 2; // relic tier enum is relic level + 2
            dictionary[key.ToString()] = (V)Convert.ChangeType(row.Value!, typeof(V), System.Globalization.CultureInfo.InvariantCulture);
        }
        return dictionary;
    }

    private static Dictionary<string, T> CreateDictionary<T>(List<TableRow> rows)
    {
        var dictionary = new Dictionary<string, T>();
        foreach (var row in rows.OrderBy(s => s.Key))
            dictionary[row.Key!] = (T)Convert.ChangeType(row.Value!, typeof(T), System.Globalization.CultureInfo.InvariantCulture);
        return dictionary;
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return AgilityRoleSupportMastery;
        yield return StrengthRoleTankMastery;
        yield return RelicTierCr;
        yield return IntelligenceRoleTankMastery;
        yield return AgilityRoleAttackerMastery;
        yield return GearLevelCr;
        yield return StrengthRoleHealerMastery;
        yield return CrewRarityCr;
        yield return RelicTierLevelFactor;
        yield return IntelligenceRoleHealerMastery;
        yield return GearPieceCr;
        yield return StrengthRoleAttackerMastery;
        yield return IntelligenceRoleSupportMastery;
        yield return CrewlessAbilityFactor;
        yield return ShipRarityFactor;
        yield return IntelligenceRoleAttackerMastery;
        yield return ModRarityLevelCr;
        yield return AgilityRoleTankMastery;
        yield return AgilityRoleHealerMastery;
        yield return StrengthRoleSupportMastery;
        yield return AbilityLevelCr;
        yield return UnitLevelCr;
    }
}