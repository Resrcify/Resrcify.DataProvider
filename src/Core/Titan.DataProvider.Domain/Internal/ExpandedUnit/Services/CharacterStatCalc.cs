using Titan.DataProvider.Domain.Extensions;
using Titan.DataProvider.Domain.Models.GalaxyOfHeroes.PlayerProfile;
using GameData = Titan.DataProvider.Domain.Internal.BaseData.BaseData;
using Titan.DataProvider.Domain.Internal.ExpandedUnit.ValueObjects;
using Titan.DataProvider.Domain.Shared;
using System.Collections.Generic;
using System;

namespace Titan.DataProvider.Domain.Internal.ExpandedUnit.Services
{
    public class CharacterStatCalc : StatCalcBase, IStatCalc
    {
        public IReadOnlyDictionary<long, double> Base => _base;
        public IReadOnlyDictionary<long, double> Gear => _gear;
        public IReadOnlyDictionary<long, double> Mods => _mods;
        public IReadOnlyDictionary<long, double> Crew => _crew;
        private CharacterStatCalc(
            Unit unit,
            GameData gameData) : base(unit, gameData)
        {
            CalculateRawStats();
            CalculateBaseStats();
            CalculateModStats();
            FormatStats();
        }
        public static Result<IStatCalc> Create(Unit unit, GameData gameData)
        {
            return new CharacterStatCalc(unit, gameData);
        }

        private void CalculateRawStats()
        {
            var tierEnumValue = (int)_unit.CurrentTier;
            var rarityEnumValue = (int)_unit.CurrentRarity;
            var relicEnumValue = _unit.Relic?.CurrentTier ?? 0;

            var definitionId = _unit.DefinitionId!.Split(":")[0];

            var stats = _gameData.Units[definitionId].GearLevels[tierEnumValue.ToString()].Stats;
            foreach (var stat in stats)
                _base.Add(stat.Key, stat.Value);

            foreach (var stat in _gameData.Units[definitionId].GrowthModifiers[rarityEnumValue.ToString()])
                _growthModifiers.Add(stat.Key, stat.Value);

            CalculateEquipmentStats();
            CalculateRelicStats(definitionId, (int)relicEnumValue);
        }

        private void CalculateRelicStats(string definitionId, int relicEnumValue)
        {

            if (relicEnumValue <= 2) return;
            var relic = _gameData.Relics[_gameData.Units[definitionId].Relics[relicEnumValue.ToString()]];

            foreach (var stat in relic.Stats)
                _base[stat.Key] = _base.GetOrDefault(stat.Key) + stat.Value;
            foreach (var stat in relic.Gms)
                _growthModifiers[stat.Key] = _growthModifiers.GetOrDefault(stat.Key) + stat.Value;
        }

        private void CalculateEquipmentStats()
        {
            if (_unit.Equipment is null) return;
            foreach (var gearPiece in _unit.Equipment)
            {
                var gearId = gearPiece.EquipmentId;
                if (gearId is null || !_gameData.Gear.ContainsKey(gearId)) continue;

                var gearStats = _gameData.Gear[gearId].Stats;
                foreach (var stat in gearStats)
                {
                    // Primary Stat, applies before mods
                    if (stat.Key == 2 || stat.Key == 3 || stat.Key == 4)
                    {
                        _base[stat.Key] = _base.GetOrDefault(stat.Key) + stat.Value;
                        continue;
                    }
                    // Secondary Stat, applies after mods
                    if (!_gear.TryAdd(stat.Key, stat.Value))
                        _gear[stat.Key] = _gear.GetOrDefault(stat.Key) + stat.Value;
                }
            }
        }

        private void CalculateModStats()
        {
            var modSets = CreateModSets();
            var rawModStats = GetRawModStats();

            foreach (var modSet in modSets)
            {
                var setDef = _gameData.ModSets[((int)modSet.Key).ToString()];
                var multiplier = Math.Floor((double)(modSet.Value.Count / setDef.Count)) + Math.Floor((double)(modSet.Value.MaxLevelCount / setDef.Count));
                var addedValue = setDef.Value * multiplier;

                if (rawModStats.TryGetValue((int)setDef.Id, out _))
                {
                    rawModStats[(int)setDef.Id] = rawModStats[(int)setDef.Id] + addedValue;
                    continue;
                }
                rawModStats.Add((int)setDef.Id, addedValue);
            }

            foreach (var stat in rawModStats)
                CalculateModStat(stat.Key, stat.Value);
        }

        private void CalculateModStat(long statId, double value)
        {
            switch (Math.Floor((double)statId))
            {
                case 41: // Offense
                    _mods[6] = _mods.GetOrDefault(6) + value; // Ph. Damage
                    _mods[7] = _mods.GetOrDefault(7) + value; // Sp. Damage
                    break;
                case 42: // Defense
                    _mods[8] = _mods.GetOrDefault(8) + value; // Armor
                    _mods[9] = _mods.GetOrDefault(9) + value; // Resistance
                    break;
                case 48: // Offense %
                    _mods[6] = Floor(_mods.GetOrDefault(6) + _base.GetOrDefault(6) * 1e-8 * value, 8); // Ph. Damage
                    _mods[7] = Floor(_mods.GetOrDefault(7) + _base.GetOrDefault(7) * 1e-8 * value, 8); // Sp. Damage
                    break;
                case 49: // Defense %
                    _mods[8] = Floor(_mods.GetOrDefault(8) + _base.GetOrDefault(8) * 1e-8 * value, 8); // Armor
                    _mods[9] = Floor(_mods.GetOrDefault(9) + _base.GetOrDefault(9) * 1e-8 * value, 8); // Resistance
                    break;
                case 53: // Crit Chance
                    _mods[21] = _mods.GetOrDefault(21) + value; // Ph. Crit Chance
                    _mods[22] = _mods.GetOrDefault(22) + value; // Sp. Crit Chance
                    break;
                case 54: // Crit Avoid
                    _mods[35] = _mods.GetOrDefault(35) + value; // Ph. Crit Avoid
                    _mods[36] = _mods.GetOrDefault(36) + value; // Ph. Crit Avoid
                    break;
                case 55: // Heatlth %
                    _mods[1] = Floor(_mods.GetOrDefault(1) + _base.GetOrDefault(1) * 1e-8 * value, 8); // Health
                    break;
                case 56: // Protection %
                    _mods[28] = Floor(_mods.GetOrDefault(28) + _base.GetOrDefault(28) * 1e-8 * value, 8); // Protection may not exist in base
                    break;
                case 57: // Speed %
                    _mods[5] = Floor(_mods.GetOrDefault(5) + _base.GetOrDefault(5) * 1e-8 * value, 8); // Speed
                    break;
                default:
                    // other stats add like flat values
                    _mods[statId] = _mods.GetOrDefault(statId) + value;
                    break;
            }
        }

        private Dictionary<long, double> GetRawModStats()
        {
            var rawModStats = new Dictionary<long, double>();
            foreach (var mod in _unit.EquippedStatMod)
            {
                if (mod?.PrimaryStat?.Stat is null) continue;
                if (!rawModStats.TryAdd((int)mod.PrimaryStat.Stat.UnitStatId, mod.PrimaryStat.Stat.UnscaledDecimalValue))
                    rawModStats[(int)mod.PrimaryStat.Stat.UnitStatId] = rawModStats[(int)mod.PrimaryStat.Stat.UnitStatId] + mod.PrimaryStat.Stat.UnscaledDecimalValue;
                foreach (var secondaryStat in mod.SecondaryStat)
                {
                    if (secondaryStat?.Stat?.UnitStatId is null) continue;
                    if (!rawModStats.TryAdd((int)secondaryStat.Stat.UnitStatId, secondaryStat.Stat.UnscaledDecimalValue))
                        rawModStats[(int)secondaryStat.Stat.UnitStatId] = rawModStats[(int)secondaryStat.Stat.UnitStatId] + secondaryStat.Stat.UnscaledDecimalValue;
                }
            }
            return rawModStats;
        }

        private Dictionary<ModType, ModSet> CreateModSets()
        {
            var modSetBonuses = new Dictionary<ModType, ModSet>();
            foreach (var mod in _unit.EquippedStatMod)
            {
                var modType = int.Parse(mod.DefinitionId![..1]);
                if (!modSetBonuses.TryAdd((ModType)modType, ModSet.Create((ModType)modType, mod.Level).Value))
                    modSetBonuses[(ModType)modType].AddMod(mod.Level);
            }
            return modSetBonuses;
        }
    }
}