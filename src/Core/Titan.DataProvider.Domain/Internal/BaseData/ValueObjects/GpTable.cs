using System;
using System.Collections.Generic;
using System.Linq;
using Titan.DataProvider.Domain.Models.GalaxyOfHeroes.GameData;
using Titan.DataProvider.Domain.Primitives;
using Titan.DataProvider.Domain.Shared;

namespace Titan.DataProvider.Domain.Internal.BaseData.ValueObjects
{
    public sealed class GpTable : ValueObject
    {
        private readonly Dictionary<string, double> _crewSizeFactor = new();
        public IReadOnlyDictionary<string, double> CrewSizeFactor => _crewSizeFactor;
        private readonly Dictionary<string, double> _relicTierLevelFactor = new();
        public IReadOnlyDictionary<string, double> RelicTierLevelFactor => _relicTierLevelFactor;
        private readonly Dictionary<string, long> _gearLevelGp = new();
        public IReadOnlyDictionary<string, long> GearLevelGp => _gearLevelGp;
        private readonly Dictionary<string, long> _relicTierGp = new();
        public IReadOnlyDictionary<string, long> RelicTierGp => _relicTierGp;
        private readonly Dictionary<string, long> _unitRarityGp = new();
        public IReadOnlyDictionary<string, long> UnitRarityGp => _unitRarityGp;
        private readonly Dictionary<string, double> _shipRarityFactor = new();
        public IReadOnlyDictionary<string, double> ShipRarityFactor => _shipRarityFactor;
        private readonly Dictionary<string, long> _abilitySpecialGp = new();
        public IReadOnlyDictionary<string, long> AbilitySpecialGp => _abilitySpecialGp;
        private readonly Dictionary<string, Dictionary<string, Dictionary<string, long>>> _modRarityLevelTierGp = new();
        public IReadOnlyDictionary<string, Dictionary<string, Dictionary<string, long>>> ModRarityLevelTierGp => _modRarityLevelTierGp;
        private readonly Dictionary<string, Dictionary<string, long>> _gearPieceGp = new();
        public IReadOnlyDictionary<string, Dictionary<string, long>> GearPieceGp => _gearPieceGp;
        private readonly Dictionary<string, double> _crewlessAbilityFactor = new();
        public IReadOnlyDictionary<string, double> CrewlessAbilityFactor => _crewlessAbilityFactor;
        private readonly Dictionary<string, long> _shipLevelGp = new();
        public IReadOnlyDictionary<string, long> ShipLevelGp => _shipLevelGp;
        private readonly Dictionary<string, long> _abilityLevelGp = new();
        public IReadOnlyDictionary<string, long> AbilityLevelGp => _abilityLevelGp;
        private readonly Dictionary<string, long> _shipAbilityLevelGp = new();
        public IReadOnlyDictionary<string, long> ShipAbilityLevelGp => _shipAbilityLevelGp;
        private readonly Dictionary<string, long> _unitLevelGp = new();
        public IReadOnlyDictionary<string, long> UnitLevelGp => _unitLevelGp;


        private GpTable(
            Dictionary<string, double> crewSizeFactor,
            Dictionary<string, double> relicTierLevelFactor,
            Dictionary<string, long> gearLevelGp,
            Dictionary<string, long> relicTierGp,
            Dictionary<string, long> unitRarityGp,
            Dictionary<string, double> shipRarityFactor,
            Dictionary<string, long> abilitySpecialGp,
            Dictionary<string, Dictionary<string, Dictionary<string, long>>> modRarityLevelTierGp,
            Dictionary<string, Dictionary<string, long>> gearPieceGp,
            Dictionary<string, double> crewlessAbilityFactor,
            Dictionary<string, long> shipLevelGp,
            Dictionary<string, long> abilityLevelGp,
            Dictionary<string, long> shipAbilityLevelGp,
            Dictionary<string, long> unitLevelGp
        )
        {
            _crewSizeFactor = crewSizeFactor;
            _relicTierLevelFactor = relicTierLevelFactor;
            _gearLevelGp = gearLevelGp;
            _relicTierGp = relicTierGp;
            _unitRarityGp = unitRarityGp;
            _shipRarityFactor = shipRarityFactor;
            _abilitySpecialGp = abilitySpecialGp;
            _modRarityLevelTierGp = modRarityLevelTierGp;
            _gearPieceGp = gearPieceGp;
            _crewlessAbilityFactor = crewlessAbilityFactor;
            _shipLevelGp = shipLevelGp;
            _abilityLevelGp = abilityLevelGp;
            _shipAbilityLevelGp = shipAbilityLevelGp;
            _unitLevelGp = unitLevelGp;
        }

        public static Result<GpTable> Create(
            GameDataResponse data,
            IReadOnlyDictionary<string, long> unitRarityGp,
            IReadOnlyDictionary<string, long> gearLevelGp,
            IReadOnlyDictionary<string, double> shipRarityFactor,
            IReadOnlyDictionary<string, long> unitLevelGp,
            IReadOnlyDictionary<string, long> abilityLevelGp
        )
        {
            var unitRarityGpFromCr = unitRarityGp.ToDictionary(x => x.Key, x => x.Value);
            var gearLevelGpFromCr = gearLevelGp.ToDictionary(x => x.Key, x => x.Value);
            var shipRarityFactorFromCr = shipRarityFactor.ToDictionary(x => x.Key, x => x.Value);
            var unitLevelGpFromCr = unitLevelGp.ToDictionary(x => x.Key, x => x.Value);
            var abilityLevelGpFromCr = abilityLevelGp.ToDictionary(x => x.Key, x => x.Value);
            var galacticPowerModifierPerShipCrewSizeTable = data.Table.First(x => x.Id == "galactic_power_modifier_per_ship_crew_size_table");
            var galacticPowerPerTierSlotTable = data.Table.First(x => x.Id == "galactic_power_per_tier_slot_table");
            var galacticPowerPerTaggedAbilityLevelTable = data.Table.First(x => x.Id == "galactic_power_per_tagged_ability_level_table"); ;
            var crewRatingPerModRarityLevelTier = data.Table.First(x => x.Id == "crew_rating_per_mod_rarity_level_tier"); ;
            var galacticPowerModifierPerRelicTier = data.Table.First(x => x.Id == "galactic_power_modifier_per_relic_tier"); ;
            var galacticPowerPerRelicTier = data.Table.First(x => x.Id == "galactic_power_per_relic_tier"); ;
            var galacticPowerModifierPerAbilityCrewlessShips = data.Table.First(x => x.Id == "galactic_power_modifier_per_ability_crewless_ships"); ;
            var galacticPowerPerShipLevelTable = data.XpTable.First(x => x.Id == "galactic_power_per_ship_level_table");
            var galacticPowerPerShipAbilityLevelTable = data.XpTable.First(x => x.Id == "galactic_power_per_ship_ability_level_table");
            var crewSizeFactor = CreateDictionary<double>(galacticPowerModifierPerShipCrewSizeTable.Row);
            var abilitySpecialGp = CreateDictionary<long>(galacticPowerPerTaggedAbilityLevelTable.Row);
            var relicTierLevelFactor = CreateDictionaryFromRelics<double>(galacticPowerModifierPerRelicTier.Row);
            var relicTierGp = CreateDictionaryFromRelics<long>(galacticPowerPerRelicTier.Row);
            var crewlessAbilityFactor = CreateDictionary<double>(galacticPowerModifierPerAbilityCrewlessShips.Row);

            var shipLevelGp = GetXpTable(galacticPowerPerShipLevelTable);
            var shipAbilityLevelGp = GetXpTable(galacticPowerPerShipAbilityLevelTable);
            var modRarityLevelTierGp = GetModRating(crewRatingPerModRarityLevelTier);
            var gearPieceGp = GetGearPieceGp(galacticPowerPerTierSlotTable);
            var temp = new Dictionary<string, Dictionary<string, long>>();
            return new GpTable(
                crewSizeFactor,
                relicTierLevelFactor,
                gearLevelGpFromCr,
                relicTierGp,
                unitRarityGpFromCr,
                shipRarityFactorFromCr,
                abilitySpecialGp,
                modRarityLevelTierGp,
                gearPieceGp,
                crewlessAbilityFactor,
                shipLevelGp,
                abilityLevelGpFromCr,
                shipAbilityLevelGp,
                unitLevelGpFromCr);
        }

        private static Dictionary<string, Dictionary<string, long>> GetGearPieceGp(Table galacticPowerPerTierSlotTable)
        {
            var g = new Dictionary<string, Dictionary<string, long>>();
            foreach (var row in galacticPowerPerTierSlotTable.Row.OrderBy(s => int.Parse(s.Key!.Split(':', 2)[1])).ThenBy(s => int.Parse(s.Key!.Split(':', 2)[0])))
            {
                var split = row.Key!.Split(':', 2);
                var tier = split[0];
                var slot = int.Parse(split[1]);

                if (!g.ContainsKey(tier))
                    g[tier] = new Dictionary<string, long>();

                var slotNum = --slot;
                g[tier][slotNum.ToString()] = long.Parse(row.Value!, System.Globalization.CultureInfo.InvariantCulture); // decrement slot by 1 as .help uses 0-based indexing for slot (game table is 1-based)
            }
            return g;
        }

        private static Dictionary<string, Dictionary<string, Dictionary<string, long>>> GetModRating(Table crewRatingPerModRarityLevelTier)
        {
            var g = new Dictionary<string, Dictionary<string, Dictionary<string, long>>>();
            foreach (var row in crewRatingPerModRarityLevelTier.Row.OrderBy(l => int.Parse(l.Key!.Split(':', 4)[1])).ThenBy(p => int.Parse(p.Key!.Split(':', 4)[0])))
            {
                if (row.Key!.Last().ToString() == "0") // only 'select' set 0, as set doesn't affect CR or GP
                {
                    var split = row.Key!.Split(':', 4);
                    var pips = split[0];
                    var level = split[1];
                    var tier = split[2];
                    var set = split[3];

                    if (!g.ContainsKey(pips)) // ensure rarity table exists
                        g[pips] = new Dictionary<string, Dictionary<string, long>>();
                    if (!g[pips].ContainsKey(level)) // ensure level table exists
                        g[pips][level] = new Dictionary<string, long>();
                    g[pips][level][tier] = long.Parse(row.Value!, System.Globalization.CultureInfo.InvariantCulture);
                }
            }
            return g;
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return CrewSizeFactor;
            yield return RelicTierLevelFactor;
            yield return GearLevelGp;
            yield return RelicTierGp;
            yield return UnitRarityGp;
            yield return ShipRarityFactor;
            yield return AbilitySpecialGp;
            yield return ModRarityLevelTierGp;
            yield return GearPieceGp;
            yield return CrewlessAbilityFactor;
            yield return ShipLevelGp;
            yield return AbilityLevelGp;
            yield return ShipAbilityLevelGp;
            yield return UnitLevelGp;
        }
        private static Dictionary<string, long> GetXpTable(XpTable table)
        {
            var tempTable = new Dictionary<string, long>();
            foreach (var row in table.Row)
            {
                int key = ++row.Index;
                tempTable[key.ToString()] = row.Xp;
            }

            return tempTable.ToDictionary(k => k.Key, v => Convert.ToInt64(v.Value, System.Globalization.CultureInfo.InvariantCulture));
        }

        // private static Dictionary<string, long> GetShipLevelGp(XpTable table)
        // {
        //     return GetXpTempTable(table);
        // }
        // private static Dictionary<string, long> GetShipAbilityLevelGp(GameDataResponse data)
        // {
        //     foreach (var table in data.XpTable)
        //     {
        //         if (table.Id!.StartsWith("crew_rating") || table.Id.StartsWith("galactic_power"))
        //         {
        //             Dictionary<string, long> tempTable = GetXpTempTable(table);
        //             switch (table.Id)
        //             {
        //                 case "galactic_power_per_ship_ability_level_table":
        //                     return tempTable.ToDictionary(k => k.Key, v => Convert.ToInt64(v.Value, System.Globalization.CultureInfo.InvariantCulture));
        //             }
        //         }
        //     }
        //     return null;
        // }

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
