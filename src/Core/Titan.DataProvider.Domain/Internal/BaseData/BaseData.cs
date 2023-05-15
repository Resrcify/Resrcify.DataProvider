using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Titan.DataProvider.Domain.Internal.BaseData.Entities;
using Titan.DataProvider.Domain.Internal.BaseData.ValueObjects;
using Titan.DataProvider.Domain.Models.GalaxyOfHeroes.GameData;
using Titan.DataProvider.Domain.Primitives;
using Titan.DataProvider.Domain.Shared;

namespace Titan.DataProvider.Domain.Internal.BaseData
{
    public sealed class BaseData : AggregateRoot
    {
        private readonly Dictionary<string, GearData> _gear = new();
        public IReadOnlyDictionary<string, GearData> Gear => _gear;
        private readonly Dictionary<string, ModSetData> _modSets = new();
        public IReadOnlyDictionary<string, ModSetData> ModSets => _modSets;
        public CrTable CrTable { get; private set; }
        public GpTable GpTable { get; private set; }
        private readonly Dictionary<string, RelicData> _relics = new();
        public IReadOnlyDictionary<string, RelicData> Relics => _relics;
        private readonly Dictionary<string, UnitData> _units = new();
        public IReadOnlyDictionary<string, UnitData> Units => _units;

        private BaseData(
            Guid id,
            Dictionary<string, GearData> gear,
            Dictionary<string, ModSetData> modSets,
            CrTable crTable,
            GpTable gpTable,
            Dictionary<string, RelicData> relics,
            Dictionary<string, UnitData> units
        ) : base(id)
        {
            _gear = gear;
            _modSets = modSets;
            CrTable = crTable;
            GpTable = gpTable;
            _relics = relics;
            _units = units;
        }
        public static Result<BaseData> Create(GameDataResponse data, List<string> localization)
        {
            var gearData = CreateGearData(data);
            var modSetData = CreateModSetData(data);
            var crTable = CrTable.Create(data);
            var crTableData = crTable.Value;
            var gpTable = GpTable.Create(
                data,
                crTableData.CrewRarityCr,
                crTableData.GearLevelCr,
                crTableData.ShipRarityFactor,
                crTableData.UnitLevelCr,
                crTableData.AbilityLevelCr
            );
            var statsTable = FetchStatsTable(data);
            var relicData = CreateRelicData(data, statsTable);

            var local = GetLocalizationDictionary(localization);
            var skills = CreateSkillData(data, local);
            var growthModifiers = CreateGrowthModifiers(data, statsTable);
            var unitData = CreateUnitData(data, local, skills, growthModifiers, statsTable);

            // TODO: FIX ERROR RESPONSES LATER
            return new BaseData(
                Guid.NewGuid(),
                gearData.Value,
                modSetData.Value,
                crTable.Value,
                gpTable.Value,
                relicData,
                unitData
            );
        }
        private static Dictionary<string, RelicData> CreateRelicData(GameDataResponse data, Dictionary<string, Dictionary<string, long>> statsTable)
        {
            var relicData = new Dictionary<string, RelicData>();
            foreach (var relic in data.RelicTierDefinition.OrderBy(s => s.Id!.Length).ThenBy(s => s.Id))
            {
                var stats = new Dictionary<long, long>();
                foreach (var stat in relic.Stat!.Stat.OrderBy(s => (int)s.UnitStatId))
                {
                    stats[(int)stat.UnitStatId] = stat.UnscaledDecimalValue;
                }
                var statsTableForRelics = statsTable[relic.RelicStatTable!.ToString()];
                var relicDataItem = RelicData.Create(statsTableForRelics, stats);
                relicData.Add(relic.Id!, relicDataItem.Value);
            }
            return relicData;
        }

        private static Dictionary<string, Dictionary<string, long>> FetchStatsTable(GameDataResponse data)
        {
            var statsTable = new Dictionary<string, Dictionary<string, long>>();
            foreach (var table in data.StatProgression)
            {
                if (table.Id!.StartsWith("stattable_"))
                {
                    var statsLine = new Dictionary<string, long>();
                    foreach (var stat in table.Stat!.Stat.OrderBy(s => (int)s.UnitStatId))
                    {
                        var id = (int)stat.UnitStatId;
                        statsLine[id.ToString()] = stat.UnscaledDecimalValue;
                    }
                    statsTable[table.Id] = statsLine;
                }
            }
            return statsTable;
        }
        private static Result<Dictionary<string, GearData>> CreateGearData(GameDataResponse data)
        {
            var gearData = new Dictionary<string, GearData>();
            foreach (var item in data.Equipment.OrderBy(s => s.Id!.Length).ThenBy(s => s.Id))
            {
                var stat = new Dictionary<long, long>();
                if (item?.EquipmentStat?.Stat is not null && item?.EquipmentStat?.Stat.Count > 0)
                {
                    foreach (var subItem in item.EquipmentStat.Stat.OrderBy(s => s.UnitStatId))
                    {
                        long key = (int)subItem.UnitStatId;
                        stat.Add(key, subItem.UnscaledDecimalValue);
                    }
                }
                gearData.Add(item!.Id!, GearData.Create(item).Value);
            }
            return gearData;
        }

        private static Result<Dictionary<string, ModSetData>> CreateModSetData(GameDataResponse data)
        {
            var modSet = new Dictionary<string, ModSetData>();
            foreach (var item in data.StatModSet.OrderBy(s => s.Id!.Length).ThenBy(s => s.Id))
            {
                var mod = ModSetData.Create(item);
                modSet.Add(item.Id!, mod.Value);
            }
            return modSet;
        }

        private static Dictionary<string, string> GetLocalizationDictionary(List<string> localization)
        {
            var tmp = new Dictionary<string, string>();
            foreach (string line in localization)
            {
                var split = line.Split("|");
                if (split.Length > 1)
                    tmp[split[0]] = split[1];
            }
            return tmp;
        }

        private static Dictionary<string, Skill> CreateSkillData(GameDataResponse data, Dictionary<string, string> local)
        {
            var skills = new Dictionary<string, Skill>();
            foreach (var skill in data.Skill)
            {
                var ability = data.Ability.Find(a => a.Id == skill.AbilityReference);
                var powerOverrideTags = new Dictionary<string, string>();
                foreach (var tier in skill.Tier.Select((Value, i) => new { i, Value }))
                {
                    if (!string.IsNullOrEmpty(tier.Value.PowerOverrideTag))
                        powerOverrideTags[(tier.i + 2).ToString()] = tier.Value.PowerOverrideTag;
                }
                skills[skill.Id!] = Skill.Create(
                    skill.Id!,
                    local[ability!.NameKey!],
                    ability.NameKey!,
                    skill.Tier.Count + 1,
                    (long)skill.SkillType,
                    ability.Icon!,
                    powerOverrideTags,
                    powerOverrideTags.ContainsValue("zeta"),
                    skill.Tier.Any(t => t.RecipeId!.Contains("OMICRON"))).Value;
            }
            return skills;
        }
        private static Dictionary<string, Dictionary<string, Dictionary<string, long>>> CreateGrowthModifiers(GameDataResponse data, Dictionary<string, Dictionary<string, long>> statsTable)
        {
            var ul = new Dictionary<string, Dictionary<string, Dictionary<string, long>>>();
            foreach (var unit in data.Units.Where(u => u.Obtainable && u.ObtainableTime == 0).OrderBy(s => (int)s.Rarity))
            {
                var rarity = (int)unit.Rarity;
                var baseId = unit.BaseId;
                var statProgressionId = unit.StatProgressionId;

                if (!ul.ContainsKey(baseId!))
                    ul[baseId!] = new Dictionary<string, Dictionary<string, long>>();

                ul[baseId!][rarity.ToString()] = statsTable[statProgressionId!];
            }
            return ul;
        }

        private static Dictionary<string, UnitData> CreateUnitData(
            GameDataResponse data,
            Dictionary<string, string> local,
            Dictionary<string, Skill> skills,
            Dictionary<string, Dictionary<string, Dictionary<string, long>>> growthModifiers,
            Dictionary<string, Dictionary<string, long>> statsTable)
        {
            var unitData = new Dictionary<string, UnitData>();
            foreach (var unit in data.Units.Where(u => u.Obtainable && u.ObtainableTime == 0 && (int)u.Rarity == 7))
            {
                var combatType = (int)unit.CombatType;
                var primaryUnitStat = (int)unit.PrimaryUnitStat;
                var forceAlignment = (int)unit.ForceAlignment;
                var thumbnailName = unit.ThumbnailName;
                var nameKey = unit.NameKey;
                var baseId = unit.BaseId;
                var categoryIdList = unit.CategoryId;
                var skillReferenceList = unit.SkillReference;
                var baseStat = unit.BaseStat;
                var unitClass = (int)unit.UnitClass;
                var relicDefinition = unit.RelicDefinition;

                var skillRef = skillReferenceList.Select(skill => skills[skill!.SkillId!]).ToList();

                if (combatType == 1) // character
                {
                    var unitTierList = unit.UnitTier;
                    var modRecommendationList = new List<ModRecommendation>();
                    foreach (var rec in unit.ModRecommendation)
                    {
                        modRecommendationList.Add(ModRecommendation.Create(rec.RecommendationSetId!, (long)rec.UnitTier).Value);
                    }

                    var tierData = new Dictionary<string, GearLevel>();

                    foreach (var gearTier in unitTierList.OrderBy(s => (int)s.Tier))
                    {
                        var stats = new Dictionary<long, long>();
                        var tier = (int)gearTier.Tier;
                        foreach (var stat in gearTier!.BaseStat!.Stat.OrderBy(s => (int)s.UnitStatId))
                            stats[(int)stat.UnitStatId] = stat.UnscaledDecimalValue;
                        tierData[tier.ToString()] = GearLevel.Create(gearTier.EquipmentSet, stats).Value;
                    }

                    var relicData = new Dictionary<string, string>();
                    foreach (var relic in relicDefinition!.RelicTierDefinitionId.OrderBy(s => s[^1..] + 2))
                    {
                        var id = int.Parse(relic[^1..]) + 2;
                        relicData[id.ToString()] = relic;
                    }
                    unitData[baseId] = UnitData
                        .Create(
                            baseId,
                            nameKey,
                            local[nameKey],
                            combatType,
                            forceAlignment,
                            categoryIdList,
                            unitClass,
                            thumbnailName,
                            primaryUnitStat,
                            tierData,
                            growthModifiers[baseId],
                            skillRef,
                            relicData,
                            FetchMasteryMultiplierName(primaryUnitStat.ToString(), categoryIdList),
                            modRecommendationList
                        )
                        .Value;
                }
                else //ships
                {
                    var crewContributionTableId = unit.CrewContributionTableId;
                    var crewList = unit?.Crew ?? Enumerable.Empty<CrewMember>();
                    var stats = new Dictionary<long, long>();
                    foreach (var stat in baseStat?.Stat ?? Enumerable.Empty<Stat>())
                        stats[(long)stat.UnitStatId] = stat.UnscaledDecimalValue;


                    foreach (var cm in crewList)
                        foreach (var s in cm.SkillReference)
                            skillRef.Add(skills[s.SkillId]);


                    var crew = crewList?.Select(cm => cm.UnitId).ToList() ?? Enumerable.Empty<string>();

                    unitData[baseId] = UnitData
                        .Create(
                            baseId,
                            nameKey,
                            local[nameKey],
                            combatType,
                            forceAlignment,
                            categoryIdList,
                            unitClass,
                            thumbnailName,
                            primaryUnitStat,
                            growthModifiers[baseId],
                            skillRef,
                            FetchMasteryMultiplierName(primaryUnitStat.ToString(), categoryIdList),
                            stats,
                            statsTable[crewContributionTableId],
                            crew.ToList()
                        )
                        .Value;
                }
            }
            return unitData;
        }

        private static string FetchMasteryMultiplierName(string primaryStatId, List<string> tags)
        {
            var primaryStats = new Dictionary<string, string>
            {
                { "2", "strength" },
                { "3", "agility" },
                { "4", "intelligence" }
            };
            var role = tags.FirstOrDefault(tag => // select 'role' tag that isn't role_leader
            {
                Regex rgx = new(@"^role_(?!leader)[^_]+");
                return rgx.IsMatch(tag);
            });
            return $"{primaryStats[primaryStatId]}_{role}_mastery";
        }
    }
}