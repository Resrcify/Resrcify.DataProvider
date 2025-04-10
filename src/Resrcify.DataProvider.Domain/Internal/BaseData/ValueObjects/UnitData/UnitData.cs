using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Resrcify.DataProvider.Domain.Models.GalaxyOfHeroes.GameData;
using GameDataStat = Resrcify.DataProvider.Domain.Models.GalaxyOfHeroes.Common.Stat;
using Resrcify.SharedKernel.DomainDrivenDesign.Primitives;
using Resrcify.SharedKernel.ResultFramework.Primitives;

namespace Resrcify.DataProvider.Domain.Internal.BaseData.ValueObjects.UnitData;

public sealed partial class UnitData : ValueObject
{
    public string Id { get; private set; }
    public string NameKey { get; private set; }
    public string Name { get; private set; }
    public long CombatType { get; private set; }
    public long ForceAlignment { get; private set; }
    public IReadOnlyList<string> CategoryIdList => _categoryIdList;
    private readonly List<string> _categoryIdList = [];
    public long UnitClass { get; private set; }
    public bool IsGalacticLegend { get; private set; }
    public string Image { get; private set; }
    public long PrimaryStat { get; private set; }
    public IReadOnlyDictionary<string, GearLevel> GearLevels => _gearLevels;
    private readonly Dictionary<string, GearLevel> _gearLevels = [];
    public IReadOnlyDictionary<string, Dictionary<string, long>> GrowthModifiers => _growthModifiers;
    private readonly Dictionary<string, Dictionary<string, long>> _growthModifiers = [];
    public IReadOnlyList<Skill> Skills => _skills;
    private readonly List<Skill> _skills = [];
    public IReadOnlyDictionary<string, string> Relics => _relics;
    private readonly Dictionary<string, string> _relics = [];
    public string MasteryModifierId { get; private set; }
    public IReadOnlyList<ModRecommendation> ModRecommendations => _modRecommendations;
    private readonly List<ModRecommendation> _modRecommendations = [];
    public IReadOnlyDictionary<long, long> Stats => _stats;
    private readonly Dictionary<long, long> _stats = [];
    public IReadOnlyDictionary<string, long> CrewStats => _crewStats;
    private readonly Dictionary<string, long> _crewStats = [];
    public IReadOnlyList<string> Crew => _crew;
    private readonly List<string> _crew = [];

    private UnitData(
        string id,
        string nameKey,
        string name,
        long combatType,
        long forceAlignment,
        List<string> categoryIdList,
        long unitClass,
        bool isGalacticLegend,
        string image,
        long primaryStat,
        Dictionary<string, GearLevel> gearLevels,
        Dictionary<string, Dictionary<string, long>> growthModifiers,
        List<Skill> skills,
        Dictionary<string, string> relics,
        string masteryModifierId,
        List<ModRecommendation> modRecommendations,
        Dictionary<long, long> stats,
        Dictionary<string, long> crewStats,
        List<string> crew)
    {
        Id = id;
        NameKey = nameKey;
        Name = name;
        CombatType = combatType;
        ForceAlignment = forceAlignment;
        _categoryIdList = categoryIdList;
        UnitClass = unitClass;
        IsGalacticLegend = isGalacticLegend;
        Image = image;
        PrimaryStat = primaryStat;
        _gearLevels = gearLevels;
        _growthModifiers = growthModifiers;
        _skills = skills;
        _relics = relics;
        MasteryModifierId = masteryModifierId;
        _modRecommendations = modRecommendations;
        _stats = stats;
        _crewStats = crewStats;
        _crew = crew;
    }
    public static Result<UnitData> Create(
        string id,
        string nameKey,
        string name,
        long combatType,
        long forceAlignment,
        List<string> categoryIdList,
        long unitClass,
        bool isGalacticLegend,
        string image,
        long primaryStat,
        Dictionary<string, GearLevel> gearLevels,
        Dictionary<string, Dictionary<string, long>> growthModifiers,
        List<Skill> skills,
        Dictionary<string, string> relics,
        string masteryModifierId,
        List<ModRecommendation> modRecommendations,
        Dictionary<long, long> stats,
        Dictionary<string, long> crewStats,
        List<string> crew)
    {
        return new UnitData(
            id,
            nameKey,
            name,
            combatType,
            forceAlignment,
            categoryIdList,
            unitClass,
            isGalacticLegend,
            image,
            primaryStat,
            gearLevels,
            growthModifiers,
            skills,
            relics,
            masteryModifierId,
            modRecommendations,
            stats,
            crewStats,
            crew
            );
    }
    public static Result<UnitData> Create(
        string id,
        string nameKey,
        string name,
        long combatType,
        long forceAlignment,
        List<string> categoryIdList,
        long unitClass,
        bool isGalacticLegend,
        string image,
        long primaryStat,
        Dictionary<string, GearLevel> gearLevels,
        Dictionary<string, Dictionary<string, long>> growthModifiers,
        List<Skill> skills,
        Dictionary<string, string> relics,
        string masteryModifierId,
        List<ModRecommendation> modRecommendations)
    {
        Dictionary<long, long> stats = [];
        Dictionary<string, long> crewStats = [];
        List<string> crew = [];
        return new UnitData(
            id,
            nameKey,
            name,
            combatType,
            forceAlignment,
            categoryIdList,
            unitClass,
            isGalacticLegend,
            image,
            primaryStat,
            gearLevels,
            growthModifiers,
            skills,
            relics,
            masteryModifierId,
            modRecommendations,
            stats,
            crewStats,
            crew
            );
    }

    public static Result<UnitData> Create(
        string id,
        string nameKey,
        string name,
        long combatType,
        long forceAlignment,
        List<string> categoryIdList,
        long unitClass,
        bool isGalacticLegend,
        string image,
        long primaryStat,
        Dictionary<string, Dictionary<string, long>> growthModifiers,
        List<Skill> skills,
        string masteryModifierId,
        Dictionary<long, long> stats,
        Dictionary<string, long> crewStats,
        List<string> crew)
    {
        Dictionary<string, GearLevel> gearLevels = [];
        Dictionary<string, string> relics = [];
        List<ModRecommendation> modRecommendations = [];
        return new UnitData(
            id,
            nameKey,
            name,
            combatType,
            forceAlignment,
            categoryIdList,
            unitClass,
            isGalacticLegend,
            image,
            primaryStat,
            gearLevels,
            growthModifiers,
            skills,
            relics,
            masteryModifierId,
            modRecommendations,
            stats,
            crewStats,
            crew
        );
    }

    public static Dictionary<string, UnitData> Create(
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
            var categoryIdList = unit.CategoryIds;
            var skillReferenceList = unit.SkillReferences;
            var baseStat = unit.BaseStat;
            var unitClass = (int)unit.UnitClass;
            var relicDefinition = unit.RelicDefinition;
            var isGalacticLegend = unit.LimitBreakRefs.Any(x => x.PowerAdditiveTag == "ultimate");

            var skillRef = skillReferenceList.Select(skill => skills[skill!.SkillId!]).ToList();

            if (combatType == 1) // character
            {
                var unitTierList = unit.UnitTiers;
                var modRecommendationList = new List<ModRecommendation>();
                foreach (var rec in unit.ModRecommendations)
                {
                    modRecommendationList.Add(ModRecommendation.Create(rec.RecommendationSetId!, (long)rec.UnitTier).Value);
                }

                var tierData = new Dictionary<string, GearLevel>();

                foreach (var gearTier in unitTierList.OrderBy(s => (int)s.Tier))
                {
                    var stats = new Dictionary<long, long>();
                    var tier = (int)gearTier.Tier;
                    foreach (var stat in gearTier.BaseStat!.Stats.OrderBy(s => (int)s.UnitStatId))
                        stats[(int)stat.UnitStatId] = stat.UnscaledDecimalValue;
                    tierData[tier.ToString()] = GearLevel.Create(gearTier.EquipmentSets, stats).Value;
                }

                var relicData = new Dictionary<string, string>();
                foreach (var relic in relicDefinition!.RelicTierDefinitionIds.OrderBy(s => s[^1..] + 2))
                {
                    var id = int.Parse(relic[^1..]) + 2;
                    relicData[id.ToString()] = relic;
                }
                unitData[baseId!] =
                    Create(
                        baseId!,
                        nameKey!,
                        local[nameKey!],
                        combatType,
                        forceAlignment,
                        categoryIdList,
                        unitClass,
                        isGalacticLegend,
                        thumbnailName!,
                        primaryUnitStat,
                        tierData,
                        growthModifiers[baseId!],
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
                var crewList = unit?.Crews ?? Enumerable.Empty<CrewMember>();
                var stats = new Dictionary<long, long>();
                foreach (var stat in baseStat?.Stats ?? Enumerable.Empty<GameDataStat>())
                    stats[(long)stat.UnitStatId] = stat.UnscaledDecimalValue;


                foreach (var cm in crewList)
                    foreach (var s in cm.SkillReferences)
                        skillRef.Add(skills[s.SkillId!]);


                var crew = crewList.Select(cm => cm.UnitId!).ToList() ?? Enumerable.Empty<string>();

                unitData[baseId!] =
                    Create(
                        baseId!,
                        nameKey!,
                        local[nameKey!],
                        combatType,
                        forceAlignment,
                        categoryIdList,
                        unitClass,
                        isGalacticLegend,
                        thumbnailName!,
                        primaryUnitStat,
                        growthModifiers[baseId!],
                        skillRef,
                        FetchMasteryMultiplierName(primaryUnitStat.ToString(), categoryIdList),
                        stats,
                        statsTable[crewContributionTableId!],
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
            Regex rgx = MasteryRegex();
            return rgx.IsMatch(tag);
        });
        return $"{primaryStats[primaryStatId]}_{role}_mastery";
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Id;
        yield return NameKey;
        yield return Name;
        yield return CombatType;
        yield return ForceAlignment;
        yield return CategoryIdList;
        yield return UnitClass;
        yield return IsGalacticLegend;
        yield return Image;
        yield return PrimaryStat;
        yield return GearLevels!;
        yield return GrowthModifiers;
        yield return Skills;
        yield return Stats;
        yield return CrewStats;
        yield return Crew;
        yield return Relics;
        yield return MasteryModifierId;
        yield return ModRecommendations;
    }

    [GeneratedRegex("^role_(?!leader)[^_]+")]
    private static partial Regex MasteryRegex();
}