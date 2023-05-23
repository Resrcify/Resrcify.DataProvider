using System;
using System.Collections.Generic;
using Titan.DataProvider.Domain.Extensions;
using Titan.DataProvider.Domain.Internal.ExpandedUnit.Services;
using Titan.DataProvider.Domain.Models.GalaxyOfHeroes.Common;
using Titan.DataProvider.Domain.Models.GalaxyOfHeroes.PlayerProfile;
using Titan.DataProvider.Domain.Primitives;
using Titan.DataProvider.Domain.Shared;
using GameData = Titan.DataProvider.Domain.Internal.BaseData.BaseData;

namespace Titan.DataProvider.Domain.Internal.ExpandedUnit.ValueObjects
{
    public sealed class StatCalcBase : ValueObject
    {
        public readonly Dictionary<long, double> _base = new();
        public readonly Dictionary<string, double> _growthModifiers = new();
        public readonly Dictionary<long, double> _gear = new();
        public readonly Dictionary<long, double> _mods = new();

        public static Result<StatCalcBase> Create()
        {
            return new StatCalcBase();
        }

        public void CalculateRawStats(Unit unit, GameData gameData)
        {
            var tierEnumValue = (int)unit.CurrentTier;
            var rarityEnumValue = (int)unit.CurrentRarity;
            var relicEnumValue = unit.Relic?.CurrentTier ?? 0;

            var definitionId = unit.DefinitionId!.Split(":")[0];

            var stats = gameData.Units[definitionId].GearLevels[tierEnumValue.ToString()].Stats;
            foreach (var stat in stats)
                _base.Add(stat.Key, stat.Value);

            foreach (var stat in gameData.Units[definitionId].GrowthModifiers[rarityEnumValue.ToString()])
                _growthModifiers.Add(stat.Key, stat.Value);

            CalculateEquipmentStats(unit, gameData);
            CalculateRelicStats(definitionId, (int)relicEnumValue, gameData);

        }

        public void CalculateBaseStats(Unit unit, GameData gameData)
        {
            var level = unit.CurrentLevel;
            var definitionId = unit.DefinitionId!.Split(":")[0];
            // calculate bonus Primary stats from Growth Modifiers:
            _base[2] = _base.GetOrDefault(2) + Floor(_growthModifiers["2"] * level, 8); // Strength
            _base[3] = _base.GetOrDefault(3) + Floor(_growthModifiers["3"] * level, 8); // Agility
            _base[4] = _base.GetOrDefault(4) + Floor(_growthModifiers["4"] * level, 8); // Tactics

            if (_base.ContainsKey(61))
            {
                // calculate effects of Mastery on Secondary stats:
                var masteryModifierId = gameData.Units[definitionId].MasteryModifierId;

                var mms = GetMasteryObject(masteryModifierId, gameData);
                foreach (var statId in mms)
                {
                    var longKey = long.Parse(statId.Key);
                    if (_base.ContainsKey(longKey))
                    {
                        _base[longKey] += _base[61] * mms[statId.Key];
                    }
                    else
                    {
                        _base.Add(longKey, _base[61] * mms[statId.Key]);
                    }
                }
            }
            var primaryStat = gameData.Units[definitionId].PrimaryStat;
            // calculate effects of Primary stats on Secondary stats:
            // Health += STR * Base
            _base[1] = _base.GetOrDefault(1) + (_base[2] * 18);
            // Ph. Damage += MainStat * 1.4
            _base[6] = Floor(_base.GetOrDefault(6) + (_base[primaryStat] * 1.4), 8);
            // Sp. Damage += TAC * 2.4
            _base[7] = Floor(_base.GetOrDefault(7) + (_base[4] * 2.4), 8);
            // Armor += STR*0.14 + AGI*0.07
            _base[8] = Floor(_base.GetOrDefault(8) + (_base[2] * 0.14) + (_base[3] * 0.07), 8);
            // Resistance += TAC * 0.1
            _base[9] = Floor(_base.GetOrDefault(9) + (_base[4] * 0.1), 8);
            // Ph. Crit += AGI * 0.4
            _base[14] = Floor(_base.GetOrDefault(14) + (_base[3] * 0.4), 8);
            // add hard-coded minimums or potentially missing stats
            // Dodge (24 -> 2%)
            _base[12] = _base.GetOrDefault(12) + 24 * 1e8;
            // Deflection (24 -> 2%)
            _base[13] = _base.GetOrDefault(13) + 24 * 1e8;
            // Sp. Crit
            _base[15] = _base.GetOrDefault(15);
            // +150% Crit Damage
            _base[16] = _base.GetOrDefault(16) + 150 * 1e6;
            // +15% Tenacity
            _base[18] = _base.GetOrDefault(18) + 15 * 1e6;
        }

        public void FormatStats(long level)
        {
            double scale = 1e-8;
            foreach (var stat in _base)
                _base[stat.Key] = stat.Value * scale;

            foreach (var stat in _gear)
                _gear[stat.Key] = stat.Value * scale;

            //    for (var statID in stats.crew) stats.crew[statID] *= scale;
            foreach (var stat in _growthModifiers)
                _growthModifiers[stat.Key] = stat.Value * scale;

            foreach (var statId in _mods)
                _mods[statId.Key] = Math.Round(statId.Value) * scale;

            ConvertPercent(14, val => ConvertFlatCritToPercent(val, scale * 1e8)); // Ph. Crit Rating -> Chance
            ConvertPercent(15, val => ConvertFlatCritToPercent(val, scale * 1e8)); // Sp. Crit Rating -> Chance
            ConvertPercent(8, val => ConvertFlatDefToPercent(val, level, scale * 1e8, false)); // Armor
            ConvertPercent(9, val => ConvertFlatDefToPercent(val, level, scale * 1e8, false)); // Resistance
            ConvertPercent(37, val => ConvertFlatAccToPercent(val, scale * 1e8)); // Physical Accuracy
            ConvertPercent(38, val => ConvertFlatAccToPercent(val, scale * 1e8)); // Special Accuracy
            ConvertPercent(12, val => ConvertFlatAccToPercent(val, scale * 1e8)); // Dodge
            ConvertPercent(13, val => ConvertFlatAccToPercent(val, scale * 1e8)); // Deflection
            ConvertPercent(39, val => ConvertFlatCritAvoidToPercent(val, scale * 1e8)); // Physical Crit Avoidance
            ConvertPercent(40, val => ConvertFlatCritAvoidToPercent(val, scale * 1e8)); // Special Crit Avoidance

            // stats with both Percent Stats get added to the ID for their flat stat (which was converted to % above)
            _mods[21 - 7] = _mods.GetOrDefault(21 - 7) + _mods.GetOrDefault(21); // Ph. Crit Chance // 21-14 = 7 = 22-15 ==> subtracting 7 from statID gets the correct flat stat
            _mods[22 - 7] = _mods.GetOrDefault(22 - 7) + _mods.GetOrDefault(22);// Sp. Crit Chance
            _mods[35 + 4] = _mods.GetOrDefault(35 + 4) + _mods.GetOrDefault(35);// Ph. Crit Avoid // 39-35 = 4 = 40-36 ==> adding 4 to statID gets the correct flat stat
            _mods[36 + 4] = _mods.GetOrDefault(36 + 4) + _mods.GetOrDefault(36);// Sp. Crit Avoid
        }

        private void CalculateRelicStats(string definitionId, int relicEnumValue, GameData gameData)
        {

            if (relicEnumValue <= 2) return;
            var relic = gameData.Relics[gameData.Units[definitionId].Relics[relicEnumValue.ToString()]];

            foreach (var stat in relic.Stats)
                _base[stat.Key] = _base.GetOrDefault(stat.Key) + stat.Value;
            foreach (var stat in relic.Gms)
                _growthModifiers[stat.Key] = _growthModifiers.GetOrDefault(stat.Key) + stat.Value;
        }

        private void CalculateEquipmentStats(Unit unit, GameData gameData)
        {
            if (unit.Equipment is null) return;
            foreach (var gearPiece in unit.Equipment)
            {
                var gearId = gearPiece.EquipmentId;
                if (gearId is null || !gameData.Gear.ContainsKey(gearId)) continue;

                var gearStats = gameData.Gear[gearId].Stats;
                foreach (var stat in gearStats)
                {
                    // Primary Stat, applies before mods
                    if (stat.Key == 2 || stat.Key == 3 || stat.Key == 4) _base[stat.Key] += stat.Value;
                    // Secondary Stat, applies after mods
                    else
                    {
                        if (_gear.ContainsKey(stat.Key))
                        {
                            _gear[stat.Key] += stat.Value;
                        }
                        else
                        {
                            _gear.Add(stat.Key, stat.Value);
                        }
                    }
                }
            }
        }

        private static IReadOnlyDictionary<string, double> GetMasteryObject(string type, GameData data)
        {
            return type switch
            {
                "agility_role_attacker_mastery" => data.CrTable.AgilityRoleAttackerMastery,
                "agility_role_healer_mastery" => data.CrTable.AgilityRoleHealerMastery,
                "agility_role_support_mastery" => data.CrTable.AgilityRoleSupportMastery,
                "agility_role_tank_mastery" => data.CrTable.AgilityRoleTankMastery,
                "intelligence_role_attacker_mastery" => data.CrTable.IntelligenceRoleAttackerMastery,
                "intelligence_role_healer_mastery" => data.CrTable.IntelligenceRoleHealerMastery,
                "intelligence_role_support_mastery" => data.CrTable.IntelligenceRoleSupportMastery,
                "intelligence_role_tank_mastery" => data.CrTable.IntelligenceRoleTankMastery,
                "strength_role_attacker_mastery" => data.CrTable.StrengthRoleAttackerMastery,
                "strength_role_healer_mastery" => data.CrTable.StrengthRoleHealerMastery,
                "strength_role_support_mastery" => data.CrTable.StrengthRoleSupportMastery,
                "strength_role_tank_mastery" => data.CrTable.StrengthRoleTankMastery,
                _ => new Dictionary<string, double>(),
            };
        }

        private static double Floor(double value, long digits = 0)
        {
            var multiplier = Math.Pow(10, digits);
            return Math.Floor(value / multiplier) * multiplier;
        }

        private void ConvertPercent(long statId, Func<double, double> ConvertFunc)
        {
            var flat = _base.GetOrDefault(statId);
            var percent = ConvertFunc(flat);
            _base[statId] = percent;
            var last = percent;
            if (_gear is not null && _gear.ContainsKey(statId))
            {
                flat += _gear[statId];
                percent = ConvertFunc(flat);
                _gear[statId] = percent - last;
                last = percent;
            }

            if (_mods is not null && _mods.ContainsKey(statId))
            {
                flat += _mods[statId];
                _mods[statId] = ConvertFunc(flat) - last;
            }
        }

        private static double ConvertFlatDefToPercent(double value, long level = 85, double scale = 1, bool isShip = false)
        {
            var val = value / scale;
            var level_effect = isShip ? 300 + level * 5 : level * 7.5;
            return val / (level_effect + val) * scale;
        }

        private static double ConvertFlatCritToPercent(double value, double scale = 1)
        {
            var val = value / scale;
            return (val / 2400 + 0.1) * scale;
        }

        private static double ConvertFlatAccToPercent(double value, double scale = 1)
        {
            var val = value / scale;
            return val / 1200 * scale;
        }

        private static double ConvertFlatCritAvoidToPercent(double value, double scale = 1)
        {
            var val = value / scale;
            return val / 2400 * scale;
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return _base;
            yield return _growthModifiers;
            yield return _gear;
            yield return _mods;
        }

        public void CalculateModStats(Unit unit, GameData gameData)
        {
            var modSets = CreateModSets(unit);
            var rawModStats = GetRawModStats(unit);

            foreach (var modSet in modSets)
            {
                var setDef = gameData.ModSets[((int)modSet.Key).ToString()];
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

        private static Dictionary<long, double> GetRawModStats(Unit unit)
        {
            var rawModStats = new Dictionary<long, double>();
            foreach (var mod in unit.EquippedStatMod)
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

        private static Dictionary<ModType, ModSet> CreateModSets(Unit unit)
        {
            var modSetBonuses = new Dictionary<ModType, ModSet>();
            foreach (var mod in unit.EquippedStatMod)
            {
                var modType = int.Parse(mod.DefinitionId![..1]);
                if (!modSetBonuses.TryAdd((ModType)modType, ModSet.Create((ModType)modType, mod.Level).Value))
                    modSetBonuses[(ModType)modType].AddMod(mod.Level);
            }
            return modSetBonuses;
        }

        //         function calculateModStats(baseStats, char)
        //         {
        //             // return empty object if no mods
        //             if (!char.mods && !char.equippedStatMod) return { };

        //             // calculate raw totals on mods
        //             const setBonuses = { };
        //             const rawModStats = { };

        //             char.equippedStatMod.forEach((mod) =>
        //             {
        //                 let setBonus;
        //                 if ((setBonus = setBonuses[+mod.definitionId[0]]))
        //                 {
        //                     // set bonus already found, increment
        //                     ++setBonus.count;
        //                     if (mod.level == 15) ++setBonus.maxLevel;
        //                 }
        //                 else
        //                 {
        //                     // new set bonus, create object
        //                     setBonuses[+mod.definitionId[0]] = {
        //                     count: 1,
        //           maxLevel: mod.level == 15 ? 1 : 0,
        //         };
        //                 }

        //                 // add Primary/Secondary stats to data
        //                 let stat = mod.primaryStat.stat,
        //                   i = 0;
        //                 do
        //                 {
        //                     rawModStats[stat.unitStatId] =
        //                       +stat.unscaledDecimalValue + (rawModStats[stat.unitStatId] || 0);
        //                     stat =
        //                       mod?.secondaryStat?.length > 0 &&
        //                       mod?.secondaryStat[i] &&
        //                       mod?.secondaryStat[i]?.stat;
        //                 } while (i++ < mod?.secondaryStat?.length);
        //             });
        // //   } else {
        // //     // return empty object if no mods
        // //     return {};
        // //   }

        //   // add stats given by set bonuses
        //   for (var setID in setBonuses)
        //             {
        //                 const setDef = modSetData[setID];
        //                 const { count: count, maxLevel: maxCount } = setBonuses[setID];
        //                 const multiplier = ~~(count / setDef.count) + ~~(maxCount / setDef.count);
        //                 rawModStats[setDef.id] =
        //                   (rawModStats[setDef.id] || 0) + setDef.value * multiplier;
        //             }

        //             // calcuate actual stat bonuses from mods
        //             const modStats = { };
        //   for (var statID in rawModStats)
        //             {
        //                 const value = rawModStats[statID];
        //                 switch (~~statID)
        //                 {
        //                     case 41: // Offense
        //                         modStats[6] = (modStats[6] || 0) + value; // Ph. Damage
        //                         modStats[7] = (modStats[7] || 0) + value; // Sp. Damage
        //                         break;
        //                     case 42: // Defense
        //                         modStats[8] = (modStats[8] || 0) + value; // Armor
        //                         modStats[9] = (modStats[9] || 0) + value; // Resistance
        //                         break;
        //                     case 48: // Offense %
        //                         modStats[6] = floor(
        //                           (modStats[6] || 0) + baseStats[6] * 1e-8 * value,
        //                           8
        //                         ); // Ph. Damage
        //                         modStats[7] = floor(
        //                           (modStats[7] || 0) + baseStats[7] * 1e-8 * value,
        //                           8
        //                         ); // Sp. Damage
        //                         break;
        //                     case 49: // Defense %
        //                         modStats[8] = floor(
        //                           (modStats[8] || 0) + baseStats[8] * 1e-8 * value,
        //                           8
        //                         ); // Armor
        //                         modStats[9] = floor(
        //                           (modStats[9] || 0) + baseStats[9] * 1e-8 * value,
        //                           8
        //                         ); // Resistance
        //                         break;
        //                     case 53: // Crit Chance
        //                         modStats[21] = (modStats[21] || 0) + value; // Ph. Crit Chance
        //                         modStats[22] = (modStats[22] || 0) + value; // Sp. Crit Chance
        //                         break;
        //                     case 54: // Crit Avoid
        //                         modStats[35] = (modStats[35] || 0) + value; // Ph. Crit Avoid
        //                         modStats[36] = (modStats[36] || 0) + value; // Ph. Crit Avoid
        //                         break;
        //                     case 55: // Heatlth %
        //                         modStats[1] = floor(
        //                           (modStats[1] || 0) + baseStats[1] * 1e-8 * value,
        //                           8
        //                         ); // Health
        //                         break;
        //                     case 56: // Protection %
        //                         modStats[28] = floor(
        //                           (modStats[28] || 0) + (baseStats[28] || 0) * 1e-8 * value,
        //                           8
        //                         ); // Protection may not exist in base
        //                         break;
        //                     case 57: // Speed %
        //                         modStats[5] = floor(
        //                           (modStats[5] || 0) + baseStats[5] * 1e-8 * value,
        //                           8
        //                         ); // Speed
        //                         break;
        //                     default:
        //                         // other stats add like flat values
        //                         modStats[statID] = (modStats[statID] || 0) + value;
        //                 }
        //             }

        //             return modStats;
        //         }
    }
}