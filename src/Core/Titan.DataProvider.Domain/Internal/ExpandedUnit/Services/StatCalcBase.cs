using System;
using System.Collections.Generic;
using Titan.DataProvider.Domain.Extensions;
using Titan.DataProvider.Domain.Models.GalaxyOfHeroes.PlayerProfile;
using GameData = Titan.DataProvider.Domain.Internal.BaseData.BaseData;

namespace Titan.DataProvider.Domain.Internal.ExpandedUnit.ValueObjects
{
    public abstract class StatCalcBase
    {
        public readonly Dictionary<long, double> _base = new();
        public readonly Dictionary<string, double> _growthModifiers = new();
        public readonly Dictionary<long, double> _gear = new();
        public readonly Dictionary<long, double> _mods = new();
        public readonly Dictionary<long, double> _crew = new();
        public readonly GameData _gameData;
        public readonly Unit _unit;
        public StatCalcBase(Unit unit, GameData gameData)
        {
            _gameData = gameData;
            _unit = unit;
        }

        public void CalculateBaseStats()
        {
            var level = _unit.CurrentLevel;
            var definitionId = _unit.DefinitionId!.Split(":")[0];
            // calculate bonus Primary stats from Growth Modifiers:
            _base[2] = _base.GetOrDefault(2) + Floor(_growthModifiers["2"] * level, 8); // Strength
            _base[3] = _base.GetOrDefault(3) + Floor(_growthModifiers["3"] * level, 8); // Agility
            _base[4] = _base.GetOrDefault(4) + Floor(_growthModifiers["4"] * level, 8); // Tactics

            if (_base.ContainsKey(61))
            {
                // calculate effects of Mastery on Secondary stats:
                var masteryModifierId = _gameData.Units[definitionId].MasteryModifierId;

                var mms = GetMasteryObject(masteryModifierId, _gameData);
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
            var primaryStat = _gameData.Units[definitionId].PrimaryStat;
            // calculate effects of Primary stats on Secondary stats:
            // Health += STR * Base
            _base[1] = _base.GetOrDefault(1) + (_base[2] * 18);
            // Ph. Damage += MainStat * 1.4
            _base[6] = Floor(_base.GetOrDefault(6), 8) + Floor(_base[primaryStat] * 1.4, 8);
            // Sp. Damage += TAC * 2.4
            _base[7] = Floor(_base.GetOrDefault(7), 8) + Floor(_base[4] * 2.4, 8);
            // Armor += STR*0.14 + AGI*0.07
            _base[8] = Floor(_base.GetOrDefault(8), 8) + Floor(_base[2] * 0.14 + _base[3] * 0.07, 8);
            // Resistance += TAC * 0.1
            _base[9] = Floor(_base.GetOrDefault(9), 8) + Floor(_base[4] * 0.1, 8);
            // Ph. Crit += AGI * 0.4
            _base[14] = Floor(_base.GetOrDefault(14), 8) + Floor(_base[3] * 0.4, 8);
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

        public void FormatStats()
        {
            double scale = 1e-8;
            foreach (var stat in _base)
                _base[stat.Key] = stat.Value * scale;

            foreach (var stat in _gear)
                _gear[stat.Key] = stat.Value * scale;

            foreach (var stat in _growthModifiers)
                _growthModifiers[stat.Key] = stat.Value * scale;

            foreach (var statId in _mods)
                _mods[statId.Key] = statId.Value * scale;

            foreach (var statId in _crew)
                _crew[statId.Key] = statId.Value * scale;

            ConvertPercent(14, val => ConvertFlatCritToPercent(val, scale * 1e8)); // Ph. Crit Rating -> Chance
            ConvertPercent(15, val => ConvertFlatCritToPercent(val, scale * 1e8)); // Sp. Crit Rating -> Chance
            ConvertPercent(8, val => ConvertFlatDefToPercent(val, _unit.CurrentLevel, scale * 1e8, _crew.Count != 0)); // Armor
            ConvertPercent(9, val => ConvertFlatDefToPercent(val, _unit.CurrentLevel, scale * 1e8, _crew.Count != 0)); // Resistance
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

            //Reseting moved mod values
            _mods[21] = 0;
            _mods[22] = 0;
            _mods[35] = 0;
            _mods[36] = 0;
        }



        public static double Floor(double value, long digits = 0)
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
            if (_crew.ContainsKey(statId))
            {
                flat += _crew[statId];
                _crew[statId] = ConvertFunc(flat) - last;
            }
            if (_gear.ContainsKey(statId))
            {
                flat += _gear[statId];
                percent = ConvertFunc(flat);
                _gear[statId] = percent - last;
                last = percent;
            }

            if (_mods.ContainsKey(statId))
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
    }
}