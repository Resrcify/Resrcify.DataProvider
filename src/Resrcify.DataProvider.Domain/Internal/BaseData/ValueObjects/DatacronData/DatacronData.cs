using System.Linq;
using System;
using System.Collections.Generic;
using Resrcify.DataProvider.Domain.Models.GalaxyOfHeroes.GameData;
using Resrcify.SharedKernel.DomainDrivenDesign.Primitives;
using Resrcify.SharedKernel.ResultFramework.Primitives;
using Resrcify.DataProvider.Domain.Models.GalaxyOfHeroes.Common;

namespace Resrcify.DataProvider.Domain.Internal.BaseData.ValueObjects.DatacronData;

public sealed partial class DatacronData : ValueObject
{
    public string Id { get; private set; }
    public int SetId { get; private set; }
    public bool IsFocused { get; private set; }
    public string NameKey { get; private set; }
    public string IconKey { get; private set; }
    public string DetailPrefab { get; private set; }
    public long ExpirationTimeMs { get; private set; }
    public bool AllowReroll { get; private set; }
    public int InitialTiers { get; private set; }
    public int MaxRerolls { get; private set; }
    public string ReferenceTemplateId { get; private set; }
    public IReadOnlyList<DatacronSetMaterial> SetMaterial => _setMaterial;
    private readonly List<DatacronSetMaterial> _setMaterial = [];
    public IReadOnlyList<string> FixedTag => _fixedTag;
    private readonly List<string> _fixedTag = [];
    public IReadOnlyList<DatacronSetTier> SetTier => _setTier;
    private readonly List<DatacronSetTier> _setTier = [];
    public IReadOnlyList<DatacronTemplateTier> Tier => _tier;
    private readonly List<DatacronTemplateTier> _tier = [];
    public IReadOnlyList<DatacronAffixTemplateSet> AffixSet => _affixSet;
    private readonly List<DatacronAffixTemplateSet> _affixSet = [];
    public IReadOnlyDictionary<string, Ability> Abilities => _abilities;
    private readonly Dictionary<string, Ability> _abilities = [];
    public IReadOnlyDictionary<string, Stat> Stats => _stats;
    private readonly Dictionary<string, Stat> _stats = [];

    private DatacronData(
        string id,
        int setId,
        bool isFocused,
        string nameKey,
        string iconKey,
        string detailPrefab,
        long expirationTimeMs,
        bool allowReroll,
        int initialTiers,
        int maxRerolls,
        string referenceTemplateId,
        List<DatacronSetMaterial> setMaterial,
        List<string> fixedTag,
        List<DatacronSetTier> setTier,
        List<DatacronTemplateTier> tier,
        List<DatacronAffixTemplateSet> affixSet,
        Dictionary<string, Ability> abilities,
        Dictionary<string, Stat> stats)
    {
        Id = id;
        SetId = setId;
        IsFocused = isFocused;
        NameKey = nameKey;
        IconKey = iconKey;
        DetailPrefab = detailPrefab;
        ExpirationTimeMs = expirationTimeMs;
        AllowReroll = allowReroll;
        InitialTiers = initialTiers;
        MaxRerolls = maxRerolls;
        ReferenceTemplateId = referenceTemplateId;
        _setMaterial = setMaterial;
        _fixedTag = fixedTag;
        _setTier = setTier;
        _tier = tier;
        _affixSet = affixSet;
        _abilities = abilities;
        _stats = stats;
    }

    public static Result<DatacronData> Create(
        string id,
        int setId,
        bool isFocused,
        string nameKey,
        string iconKey,
        string detailPrefab,
        long expirationTimeMs,
        bool allowReroll,
        int initialTiers,
        int maxRerolls,
        string referenceTemplateId,
        List<DatacronSetMaterial> setMaterial,
        List<string> fixedTag,
        List<DatacronSetTier> setTier,
        List<DatacronTemplateTier> tier,
        List<DatacronAffixTemplateSet> affixSet,
        Dictionary<string, Ability> abilities,
        Dictionary<string, Stat> stats)
        => new DatacronData(
            id,
            setId,
            isFocused,
            nameKey,
            iconKey,
            detailPrefab,
            expirationTimeMs,
            allowReroll,
            initialTiers,
            maxRerolls,
            referenceTemplateId,
            setMaterial,
            fixedTag,
            setTier,
            tier,
            affixSet,
            abilities,
            stats);
    public static Result<Dictionary<string, DatacronData>> Create(
        GameDataResponse data,
        Dictionary<string, string> local, bool onlyActive = false)
    {
        var abilities = MapAbilites(data, local);
        var factions = MapFactions(data, local);
        var units = MapUnits(data, local, factions);
        var stats = MapStatEnums(local);
        var datacronDataDict = new Dictionary<string, DatacronData>();
        Dictionary<string, DatacronAffixTemplateSet> affixSetDict = data.DatacronAffixTemplateSets.ToDictionary(x => x.Id ?? string.Empty);
        Dictionary<string, EffectTarget> targetingRulesDict = data.BattleTargetingRules.ToDictionary(x => x.Id ?? string.Empty);
        var affixSetList = new List<DatacronAffixTemplateSet>();
        var datacronSetDict = data.DatacronSets.ToDictionary(x => x.Id);
        foreach (var cron in data.DatacronTemplates)
        {
            Dictionary<string, Ability> datacronAbilities = [];
            Dictionary<string, Stat> datacronStats = [];
            if (!datacronSetDict.TryGetValue(cron.SetId, out var cronSet))
                continue;

            var unixEpochNow = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            if (cronSet?.ExpirationTimeMs is null || onlyActive && cronSet.ExpirationTimeMs < unixEpochNow)
                continue;

            foreach (var (tierValue, i) in cron.Tiers.Select((value, i) => (value, i)))
            {
                foreach (var affixTemplatSetIdValue in tierValue.AffixTemplateSetIds)
                {
                    if (!affixSetDict.TryGetValue(affixTemplatSetIdValue, out var affixSet))
                        continue;
                    affixSetList.Add(affixSet);
                    foreach (var affixValue in affixSet.Affixs)
                    {
                        AddStats(stats, datacronStats, affixValue);

                        if (string.IsNullOrEmpty(affixValue.AbilityId) || string.IsNullOrEmpty(affixValue.TargetRule)
                            || !abilities.TryGetValue(affixValue.AbilityId, out var mappedAbility))
                            continue;

                        if (!targetingRulesDict.TryGetValue(affixValue.TargetRule, out var target))
                            continue;
                        Dictionary<string, Target> targets = GetTargets(factions, affixValue, mappedAbility, target);

                        if (datacronAbilities.TryGetValue(affixValue.AbilityId, out var foundAbility))
                            foreach (var oldTarget in foundAbility.Targets)
                                targets.TryAdd(oldTarget.Key, oldTarget.Value);

                        var ability = Ability.Create(affixValue.AbilityId, targets);
                        if (!datacronAbilities.TryAdd(affixValue.AbilityId, ability.Value))
                            datacronAbilities[affixValue.AbilityId] = ability.Value;
                    }
                }
            }

            var nameKey = local[cronSet?.DisplayName ?? ""];
            var datacron = Create(
                cron.Id ?? string.Empty,
                cronSet?.Id ?? 0,
                cron.Focused,
                nameKey,
                cron.Focused
                    ? cron.FocusedIcon ?? string.Empty
                    : cronSet?.Icon ?? string.Empty,
                cron.Focused
                    ? cron.FocusedPrefab ?? string.Empty
                    : cronSet?.DetailPrefab ?? string.Empty,
                cronSet?.ExpirationTimeMs ?? 0,
                cron.AllowReroll,
                cron.InitialTiers,
                cron.MaxRerolls,
                cron.ReferenceTemplateId ?? string.Empty,
                cronSet?.SetMaterials ?? [],
                cron.FixedTags,
                cron.Focused
                    ? cronSet?.FocusedTiers ?? []
                    : cronSet?.Tiers ?? [],
                cron.Tiers,
                affixSetList,
                datacronAbilities,
                datacronStats.OrderBy(x => x.Value.Id).ToDictionary(x => x.Key, x => x.Value));
            datacronDataDict.TryAdd(cron?.Id ?? string.Empty, datacron.Value);
        }
        return datacronDataDict;
    }

    private static void AddStats(
        Dictionary<int, StatEnum> stats,
        Dictionary<string, Stat> datacronStats,
        DatacronAffixTemplate affixValue)
    {
        if (affixValue.StatType > 0 && stats.TryGetValue((int)affixValue.StatType, out var stat))
        {
            var statValue = Stat.Create((int)affixValue.StatType, stat.EnumNameKey, affixValue.ScopeIcon ?? string.Empty);
            datacronStats.TryAdd($"{(int)affixValue.StatType}", statValue.Value);
        }
    }

    private static Dictionary<string, Target> GetTargets(
        Dictionary<string, Faction> factions,
        DatacronAffixTemplate affixValue,
        MappedAbility mappedAbility,
        EffectTarget? target)
    {
        if (target?.Category?.Categories is null)
            return [];

        var targets = new Dictionary<string, Target>();
        foreach (var categoryValue in target.Category.Categories)
        {
            if (categoryValue.Exclude || !factions.TryGetValue(categoryValue?.CategoryId ?? string.Empty, out var faction))
                continue;

            Unit? unit = null;
            if (faction.Units.Count == 1)
            {
                var foundUnit = faction.Units[0];
                unit = Unit.Create(foundUnit.BaseId, foundUnit.NameKey, foundUnit.CombatType).Value;
            }
            var targetNameKey = string.Format(mappedAbility.NameKey, faction.NameKey);
            var targetDescKey = string.Format(mappedAbility.DescKey, faction.NameKey);
            var newTarget = Target.Create(
                affixValue?.TargetRule ?? string.Empty,
                targetNameKey,
                targetDescKey,
                affixValue?.ScopeIcon ?? string.Empty,
                unit
            );
            targets.TryAdd(affixValue?.TargetRule ?? string.Empty, newTarget.Value);
        }

        return targets;
    }

    private static Dictionary<string, MappedAbility> MapAbilites(
        GameDataResponse data,
        Dictionary<string, string> local)
    {
        Dictionary<string, MappedAbility> abilities = [];
        foreach (var ability in data.Abilities.Where(x =>
            x.NameKey is not null &&
            x.NameKey.Contains("DATACRON")))
        {
            if (ability is null ||
                ability.Id is null ||
                ability.Icon is null ||
                ability.NameKey is null ||
                ability.DescKey is null ||
                !local.TryGetValue(ability.NameKey, out var abilityName) ||
                !local.TryGetValue(ability.DescKey, out var abilityDescription))
                continue;

            var newAbility = MappedAbility.Create(
                abilityName,
                abilityDescription,
                ability.Icon);

            abilities.Add(ability.Id, newAbility.Value);
        }
        return abilities;
    }
    private static Dictionary<string, Faction> MapFactions(
        GameDataResponse data,
        Dictionary<string, string> local)
    {
        Dictionary<string, Faction> factions = [];
        foreach (var faction in data.Categories)
        {
            if (faction?.Id is null ||
                faction?.DescKey is null ||
                faction.DescKey == "PLACEHOLDER" ||
                !local.TryGetValue(faction.DescKey, out var name))
                continue;

            var newFaction = Faction.Create(faction.Id, name, faction.Visible);
            factions.Add(faction.Id, newFaction.Value);
        }
        return factions;
    }
    private static Dictionary<string, Unit> MapUnits(
        GameDataResponse data,
        Dictionary<string, string> local,
        Dictionary<string, Faction> factions)
    {
        Dictionary<string, Unit> units = [];
        foreach (var unit in data.Units)
        {
            if ((int)unit.Rarity != 7)
                continue;
            if (unit.Obtainable != true)
                continue;
            if (unit.ObtainableTime != 0)
                continue;
            if (unit?.BaseId is null || unit?.NameKey is null || !local.TryGetValue(unit.NameKey, out var name))
                continue;
            var newUnit = Unit.Create(unit.BaseId, name, (int)unit.CombatType);
            units.Add(unit.BaseId, newUnit.Value);
            foreach (var (factionValue, f) in unit.CategoryIds.Select((value, f) => (value, f)))
            {
                if (!factions.TryGetValue(factionValue, out var faction))
                    continue;
                faction.AddUnit(newUnit.Value);
            }
        }
        return units;

    }
    private static Dictionary<int, StatEnum> MapStatEnums(Dictionary<string, string> local)
    {
        Dictionary<int, StatEnum> statEnums = [];
        Dictionary<string, StatEnum> tmpLang = [];
        foreach (var item in local.Where(item => item.Key.StartsWith("UnitStat_") || item.Key.StartsWith("UNIT_STAT_")))
        {
            var enumValue = string.Join("", item.Key.Split("_"))?.Split("TU")[0]?.ToUpper()?.Replace("STATVIEW", "")?.Replace("STATSVIEW", "");
            if (enumValue is null)
                continue;
            var statEnum = new StatEnum(0, "", item.Key, local[item.Key]);
            tmpLang.TryAdd(enumValue, statEnum);
        }

        foreach (var enumValue in Enum.GetValues(typeof(UnitStat)))
        {
            StatEnum? statEnumResult = null;
            if (tmpLang.TryGetValue(enumValue.ToString()!.ToUpper(), out var lang))
                statEnumResult = new StatEnum((int)enumValue, enumValue.ToString()!, lang.EnumNameKey, lang.EnumName);

            var stat = enumValue.ToString()!.Replace("UNITSTATMAX", "UNITSTAT");
            if (tmpLang.TryGetValue(stat, out var newLang))
                statEnumResult = new StatEnum((int)enumValue, enumValue.ToString()!, newLang.EnumNameKey, newLang.EnumName);

            if (statEnumResult is not null)
                statEnums.TryAdd((int)enumValue, statEnumResult);
        }

        return statEnums;
    }
    private record StatEnum(
        int EnumValue,
        string EnumString,
        string EnumNameKey,
        string EnumName);

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return SetId;
        yield return AllowReroll;
        yield return DetailPrefab;
        yield return ExpirationTimeMs;
        yield return Abilities;
        yield return Stats;
    }
}