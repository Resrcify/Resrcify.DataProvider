using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Titan.DataProvider.Domain.Internal.BaseData.Enums;
using Titan.DataProvider.Domain.Models.GalaxyOfHeroes.GameData;
using Titan.DataProvider.Domain.Primitives;
using Titan.DataProvider.Domain.Shared;

namespace Titan.DataProvider.Domain.Internal.BaseData.Entities
{
    public sealed class CrTable : ValueObject
    {
        private readonly Dictionary<string, double> _agilityRoleSupportMastery = new();
        public IReadOnlyDictionary<string, double> AgilityRoleSupportMastery => _agilityRoleSupportMastery;

        private readonly Dictionary<string, double> _strengthRoleTankMastery = new();
        public IReadOnlyDictionary<string, double> StrengthRoleTankMastery => _strengthRoleTankMastery;

        private readonly Dictionary<string, long> _relicTierCr = new();
        public IReadOnlyDictionary<string, long> RelicTierCr => _relicTierCr;

        private readonly Dictionary<string, double> _intelligenceRoleTankMastery = new();
        public IReadOnlyDictionary<string, double> IntelligenceRoleTankMastery => _intelligenceRoleTankMastery;

        private readonly Dictionary<string, double> _agilityRoleAttackerMastery = new();
        public IReadOnlyDictionary<string, double> AgilityRoleAttackerMastery => _agilityRoleAttackerMastery;


        private readonly Dictionary<string, long> _gearLevelCr = new();
        public IReadOnlyDictionary<string, long> GearLevelCr => _gearLevelCr;

        private readonly Dictionary<string, double> _strengthRoleHealerMastery = new();
        public IReadOnlyDictionary<string, double> StrengthRoleHealerMastery => _strengthRoleHealerMastery;

        private readonly Dictionary<string, long> _crewRarityCr = new();
        public IReadOnlyDictionary<string, long> CrewRarityCr => _crewRarityCr;

        private readonly Dictionary<string, double> _relicTierLevelFactor = new();
        public IReadOnlyDictionary<string, double> RelicTierLevelFactor => _relicTierLevelFactor;

        private readonly Dictionary<string, double> _intelligenceRoleHealerMastery = new();
        public IReadOnlyDictionary<string, double> IntelligenceRoleHealerMastery => _intelligenceRoleHealerMastery;

        private readonly Dictionary<string, long> _gearPieceCr = new();
        public IReadOnlyDictionary<string, long> GearPieceCr => _gearPieceCr;

        private readonly Dictionary<string, double> _strengthRoleAttackerMastery = new();
        public IReadOnlyDictionary<string, double> StrengthRoleAttackerMastery => _strengthRoleAttackerMastery;

        private readonly Dictionary<string, double> _intelligenceRoleSupportMastery = new();
        public IReadOnlyDictionary<string, double> IntelligenceRoleSupportMastery => _intelligenceRoleSupportMastery;

        private readonly Dictionary<string, double> _crewlessAbilityFactor = new();
        public IReadOnlyDictionary<string, double> CrewlessAbilityFactor => _crewlessAbilityFactor;

        private readonly Dictionary<string, double> _shipRarityFactor = new();
        public IReadOnlyDictionary<string, double> ShipRarityFactor => _shipRarityFactor;

        private readonly Dictionary<string, double> _intelligenceRoleAttackerMastery = new();
        public IReadOnlyDictionary<string, double> IntelligenceRoleAttackerMastery => _intelligenceRoleAttackerMastery;

        private readonly Dictionary<string, Dictionary<string, long>> _modRarityLevelCr = new();
        public IReadOnlyDictionary<string, Dictionary<string, long>> ModRarityLevelCr => _modRarityLevelCr;

        private readonly Dictionary<string, double> _agilityRoleTankMastery = new();
        public IReadOnlyDictionary<string, double> AgilityRoleTankMastery => _agilityRoleTankMastery;

        private readonly Dictionary<string, double> _agilityRoleHealerMastery = new();
        public IReadOnlyDictionary<string, double> AgilityRoleHealerMastery => _agilityRoleHealerMastery;

        private readonly Dictionary<string, double> _strengthRoleSupportMastery = new();
        public IReadOnlyDictionary<string, double> StrengthRoleSupportMastery => _strengthRoleSupportMastery;

        private readonly Dictionary<string, long> _abilityLevelCr = new();
        public IReadOnlyDictionary<string, long> AbilityLevelCr => _abilityLevelCr;

        private readonly Dictionary<string, long> _unitLevelCr = new();
        public IReadOnlyDictionary<string, long> UnitLevelCr => _unitLevelCr;

        private CrTable(
            Dictionary<string, double> agilityRoleSupportMastery,
            Dictionary<string, double> strengthRoleTankMastery,
            Dictionary<string, long> relicTierCr,
            Dictionary<string, double> intelligenceRoleTankMastery,
            Dictionary<string, double> agilityRoleAttackerMastery,
            Dictionary<string, long> gearLevelCr,
            Dictionary<string, double> strengthRoleHealerMastery,
            Dictionary<string, long> crewRarityCr,
            Dictionary<string, double> relicTierLevelFactor,
            Dictionary<string, double> intelligenceRoleHealerMastery,
            Dictionary<string, long> gearPieceCr,
            Dictionary<string, double> strengthRoleAttackerMastery,
            Dictionary<string, double> intelligenceRoleSupportMastery,
            Dictionary<string, double> crewlessAbilityFactor,
            Dictionary<string, double> shipRarityFactor,
            Dictionary<string, double> intelligenceRoleAttackerMastery,
            Dictionary<string, Dictionary<string, long>> modRarityLevelCr,
            Dictionary<string, double> agilityRoleTankMastery,
            Dictionary<string, double> agilityRoleHealerMastery,
            Dictionary<string, double> strengthRoleSupportMastery,
            Dictionary<string, long> abilityLevelCr,
            Dictionary<string, long> unitLevelCr
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

            // foreach (var table in data.Table)
            // {
            //     switch (table.Id)
            //     {
            //         case "crew_rating_per_unit_rarity":
            //             _crewRarityCr = CreateDictionaryFromEnum<RarityEnum, long>(table.Row);
            //             break;
            //         case "crew_rating_per_gear_piece_at_tier":
            //             var gPieceCr = new Dictionary<string, long>();
            //             foreach (var row in table.Row.OrderBy(s => s.Key))
            //             {
            //                 var pattern = @"TIER_0?(\d+)";
            //                 Regex rgx = new(pattern, RegexOptions.IgnoreCase);
            //                 var gearMatches = rgx.Match(row.Key!);
            //                 if (gearMatches.Success)
            //                     gearPieceCr.Add(gearMatches.Groups[1].Value, long.Parse(row.Value!, System.Globalization.CultureInfo.InvariantCulture));
            //             }
            //             _gearPieceCr = gearPieceCr;
            //             break;
            //         case "galactic_power_per_complete_gear_tier_table":
            //             var gearLevelGp = new Dictionary<string, long>
            //             {
            //                 { "1", 0 } // initialize with value of 0 for unit's at gear 1 (which have none 'complete')
            //             };
            //             foreach (var row in table.Row.OrderBy(s => s.Key))
            //             {
            //                 var pattern = @"TIER_0?(\d+)";
            //                 Regex rgx = new(pattern, RegexOptions.IgnoreCase);
            //                 var gearMatches = rgx.Match(row.Key!);
            //                 if (gearMatches.Success)
            //                 {
            //                     var key = long.Parse(gearMatches.Groups[1].Value, System.Globalization.CultureInfo.InvariantCulture);
            //                     key = ++key;
            //                     gearLevelGp.Add(key.ToString(), long.Parse(row.Value!, System.Globalization.CultureInfo.InvariantCulture));
            //                 }
            //                 _gearLevelCr = gearLevelGp; // used for both GP and CR
            //             }
            //             break;
            //         case "crew_contribution_multiplier_per_rarity":
            //             _shipRarityFactor = CreateDictionaryFromEnum<RarityEnum, double>(table.Row);
            //             break;
            //         case "crew_rating_per_mod_rarity_level_tier":
            //             Dictionary<string, Dictionary<string, long>> c = _modRarityLevelCr = new Dictionary<string, Dictionary<string, long>>();
            //             foreach (var row in table.Row.OrderBy(l => int.Parse(l.Key!.Split(':', 4)[1])).ThenBy(p => int.Parse(p.Key!.Split(':', 4)[0])))
            //             {
            //                 if (row.Key!.Last().ToString() == "0") // only 'select' set 0, as set doesn't affect CR or GP
            //                 {
            //                     var split = row.Key!.Split(':', 4);
            //                     var pips = split[0];
            //                     var level = split[1];
            //                     var tier = split[2];
            //                     var set = split[3];

            //                     if (tier == "1") // tier doesn't affect CR, so only save for tier 1
            //                     {
            //                         if (!c.ContainsKey(pips)) // ensure rarity table exists
            //                             c[pips] = new Dictionary<string, long>();
            //                         //         c[pips] = c[pips] || { }; // ensure table exists for that rarity
            //                         c[pips][level] = long.Parse(row.Value!, System.Globalization.CultureInfo.InvariantCulture);

            //                     }
            //                 }
            //             };
            //             break;
            //         case "crew_rating_modifier_per_relic_tier":
            //             _relicTierLevelFactor = CreateDictionaryFromRelics<double>(table.Row);
            //             break;
            //         case "crew_rating_per_relic_tier":
            //             _relicTierCr = CreateDictionaryFromRelics<long>(table.Row);
            //             break;
            //         case "crew_rating_modifier_per_ability_crewless_ships":
            //             _crewlessAbilityFactor = CreateDictionary<double>(table.Row);
            //             break;
            //         case "agility_role_support_mastery":
            //             _agilityRoleSupportMastery = CreateDictionaryFromEnum<StatEnum, double>(table.Row);
            //             break;
            //         case "agility_role_tank_mastery":
            //             _agilityRoleTankMastery = CreateDictionaryFromEnum<StatEnum, double>(table.Row);
            //             break;
            //         case "agility_role_healer_mastery":
            //             _agilityRoleHealerMastery = CreateDictionaryFromEnum<StatEnum, double>(table.Row);
            //             break;
            //         case "agility_role_attacker_mastery":
            //             _agilityRoleAttackerMastery = CreateDictionaryFromEnum<StatEnum, double>(table.Row);
            //             break;
            //         case "intelligence_role_tank_mastery":
            //             _intelligenceRoleTankMastery = CreateDictionaryFromEnum<StatEnum, double>(table.Row);
            //             break;
            //         case "intelligence_role_healer_mastery":
            //             _intelligenceRoleHealerMastery = CreateDictionaryFromEnum<StatEnum, double>(table.Row);
            //             break;
            //         case "intelligence_role_support_mastery":
            //             _intelligenceRoleSupportMastery = CreateDictionaryFromEnum<StatEnum, double>(table.Row);
            //             break;
            //         case "intelligence_role_attacker_mastery":
            //             _intelligenceRoleAttackerMastery = CreateDictionaryFromEnum<StatEnum, double>(table.Row);
            //             break;
            //         case "strength_role_healer_mastery":
            //             _strengthRoleHealerMastery = CreateDictionaryFromEnum<StatEnum, double>(table.Row);
            //             break;
            //         case "strength_role_tank_mastery":
            //             _strengthRoleTankMastery = CreateDictionaryFromEnum<StatEnum, double>(table.Row);
            //             break;
            //         case "strength_role_attacker_mastery":
            //             _strengthRoleAttackerMastery = CreateDictionaryFromEnum<StatEnum, double>(table.Row);
            //             break;
            //         case "strength_role_support_mastery":
            //             _strengthRoleSupportMastery = CreateDictionaryFromEnum<StatEnum, double>(table.Row);
            //             break;
            //         default:
            //             break;
            //     }

            // }
            // foreach (var table in data.XpTable)
            // {
            //     if (table.Id!.StartsWith("crew_rating") || table.Id.StartsWith("galactic_power"))
            //     {
            //         var tempTable = new Dictionary<string, long>();
            //         foreach (var row in table.Row)
            //         {
            //             int key = ++row.Index;
            //             tempTable[key.ToString()] = row.Xp;
            //         }

            //         switch (table.Id)
            //         {
            //             // 'CR' tables appear to be for both CR and GP on characters
            //             // 'GP' tables specify ships, but are currently idendical to the 'CR' tables.
            //             case "crew_rating_per_unit_level":
            //                 _unitLevelCr = tempTable;
            //                 break;
            //             case "crew_rating_per_ability_level":
            //                 _abilityLevelCr = tempTable.ToDictionary(k => k.Key, v => Convert.ToInt64(v.Value, System.Globalization.CultureInfo.InvariantCulture));
            //                 break;
            //             default:
            //                 break;
            //         }
            //     }
            // }

        }

        // public static Result<CrTable> Create(GameDataResponse data)
        // {

        //     // return new CrTable(data);
        // }

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

        private static Dictionary<string, V> CreateDictionaryFromEnum<T, V>(List<TableRow> rows) where T : struct
        {
            var dictionary = new Dictionary<string, V>();
            foreach (var row in rows.OrderBy(s => Convert.ToInt32((T)Enum.Parse<T>(s.Key!))))
            {
                var e = (T)Enum.Parse<T>(row.Key!);
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
    }
}