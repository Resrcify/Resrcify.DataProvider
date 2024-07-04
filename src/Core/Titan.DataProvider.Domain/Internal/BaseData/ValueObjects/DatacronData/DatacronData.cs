using System.Linq;
using System;
using System.Collections.Generic;
using Titan.DataProvider.Domain.Primitives;
using Titan.DataProvider.Domain.Shared;
using Titan.DataProvider.Domain.Models.GalaxyOfHeroes.GameData;
using Titan.DataProvider.Domain.Models.GalaxyOfHeroes.Common;

namespace Titan.DataProvider.Domain.Internal.BaseData.ValueObjects.DatacronData;

public sealed partial class DatacronData : ValueObject
{
    public string Id { get; private set; }
    public int SetId { get; private set; }
    public string NameKey { get; private set; }
    public string IconKey { get; private set; }
    public string DetailPrefab { get; private set; }
    public long ExpirationTimeMs { get; private set; }
    public bool AllowReroll { get; private set; }
    public int InitialTiers { get; private set; }
    public int MaxRerolls { get; private set; }
    public string ReferenceTemplateId { get; private set; }
    public IReadOnlyList<DatacronSetMaterial> SetMaterial => _setMaterial;
    private readonly List<DatacronSetMaterial> _setMaterial = new();
    public IReadOnlyList<string> FixedTag => _fixedTag;
    private readonly List<string> _fixedTag = new();
    public IReadOnlyList<DatacronSetTier> SetTier => _setTier;
    private readonly List<DatacronSetTier> _setTier = new();
    public IReadOnlyList<DatacronTemplateTier> Tier => _tier;
    private readonly List<DatacronTemplateTier> _tier = new();
    public IReadOnlyList<DatacronAffixTemplateSet> AffixSet => _affixSet;
    private readonly List<DatacronAffixTemplateSet> _affixSet = new();
    public IReadOnlyDictionary<string, Ability> Abilities => _abilities;
    private readonly Dictionary<string, Ability> _abilities = new();
    public IReadOnlyDictionary<string, Stat> Stats => _stats;
    private readonly Dictionary<string, Stat> _stats = new();

    private DatacronData(
        string id,
        int setId,
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
        return new DatacronData(id, setId, nameKey, iconKey, detailPrefab, expirationTimeMs, allowReroll, initialTiers, maxRerolls, referenceTemplateId, setMaterial, fixedTag, setTier, tier, affixSet, abilities, stats);
    }
    public static Result<Dictionary<string, DatacronData>> Create(GameDataResponse data, Dictionary<string, string> local, bool onlyActive = false)
    {
        var abilities = MapAbilites(data, local);
        var factions = MapFactions(data, local);
        var units = MapUnits(data, local, factions);
        var stats = MapStatEnums(local);
        var datacronDataDict = new Dictionary<string, DatacronData>();
        var affixSetList = new List<DatacronAffixTemplateSet>();
        foreach (var cron in data.DatacronTemplates)
        {
            Dictionary<string, Ability> datacronAbilities = new();
            Dictionary<string, Stat> datacronStats = new();
            var cronSet = data.DatacronSets.FirstOrDefault(x => x.Id == cron.SetId);

            var unixEpochNow = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            if (cronSet?.ExpirationTimeMs is null || (onlyActive && cronSet.ExpirationTimeMs < unixEpochNow))
                continue;

            foreach (var (tierValue, i) in cron.Tiers.Select((value, i) => (value, i)))
            {
                var type = GetTierType(i);
                foreach (var affixTemplatSetIdValue in tierValue.AffixTemplateSetIds)
                {
                    var affixSet = data.DatacronAffixTemplateSets.FirstOrDefault(x => x.Id == affixTemplatSetIdValue);
                    if (affixSet?.Affixs is null) continue;
                    affixSetList.Add(affixSet);
                    foreach (var affixValue in affixSet.Affixs)
                    {
                        AddStats(stats, datacronStats, affixValue);

                        if (type == TierType.STAT || string.IsNullOrEmpty(affixValue.AbilityId) || string.IsNullOrEmpty(affixValue.TargetRule)
                            || !abilities.TryGetValue(affixValue.AbilityId, out var mappedAbility)) continue;

                        var target = data.BattleTargetingRules.FirstOrDefault(x => x.Id == affixValue.TargetRule);
                        if (target?.Category?.Categories?.Count == 0) continue;
                        Dictionary<string, Target> targets = GetTargets(factions, type, affixValue, mappedAbility, target);

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
                cron.Id!,
                cronSet!.Id,
                nameKey,
                cronSet.Icon!,
                cronSet.DetailPrefab!,
                cronSet.ExpirationTimeMs,
                cron.AllowReroll,
                cron.InitialTiers,
                cron.MaxRerolls,
                cron.ReferenceTemplateId!,
                cronSet.SetMaterials,
                cron.FixedTags,
                cronSet.Tiers,
                cron.Tiers,
                affixSetList,
                datacronAbilities,
                datacronStats.OrderBy(x => x.Value.Id).ToDictionary(x => x.Key, x => x.Value));
            datacronDataDict.TryAdd(cron.Id!, datacron.Value);
        }
        return datacronDataDict;
    }

    private static TierType GetTierType(int i)
        => i switch
        {
            2 => TierType.ALIGNMENT,
            5 => TierType.FACTION,
            8 => TierType.UNIT,
            _ => TierType.STAT
        };

    private static void AddStats(Dictionary<string, StatEnum> stats, Dictionary<string, Stat> datacronStats, DatacronAffixTemplate affixValue)
    {
        if (affixValue.StatType > 0 && stats.TryGetValue(affixValue.StatType.ToString(), out var stat))
        {
            var statValue = Stat.Create((int)affixValue.StatType, stat.NameKey, affixValue.ScopeIcon!);
            datacronStats.TryAdd($"{(int)affixValue.StatType}", statValue.Value);
        }
    }

    private static Dictionary<string, Target> GetTargets(Dictionary<string, Faction> factions, TierType type, DatacronAffixTemplate affixValue, MappedAbility mappedAbility, EffectTarget? target)
    {
        var targets = new Dictionary<string, Target>();
        foreach (var categoryValue in target!.Category!.Categories)
        {
            if (categoryValue.Exclude || !factions.TryGetValue(categoryValue.CategoryId!, out var faction))
                continue;

            Unit unit = null!;
            if (type == TierType.UNIT)
            {
                var foundUnit = faction.Units[0]!;
                unit = Unit.Create(foundUnit.BaseId, foundUnit.NameKey, foundUnit.CombatType).Value;
            }
            var targetNameKey = string.Format(mappedAbility.NameKey, faction.NameKey);
            var targetDescKey = string.Format(mappedAbility.DescKey, faction.NameKey);
            var newTarget = Target.Create(
                affixValue.TargetRule!,
                targetNameKey,
                targetDescKey,
                affixValue.ScopeIcon!,
                unit
            );
            targets.TryAdd(affixValue.TargetRule!, newTarget.Value);
        }

        return targets;
    }

    private static Dictionary<string, MappedAbility> MapAbilites(GameDataResponse data, Dictionary<string, string> local)
    {
        Dictionary<string, MappedAbility> abilities = new();
        foreach (var ability in data.Abilities.Where(x => x.NameKey!.Contains("DATACRON")))
        {
            if (ability is null) continue;
            var newAbility = MappedAbility.Create(
                local[ability.NameKey!] ?? ability.NameKey!,
                local[ability.DescKey!] ?? ability.DescKey!,
                ability.Icon!);
            abilities.Add(ability.Id!, newAbility.Value);
        }
        return abilities;
    }
    private static Dictionary<string, Faction> MapFactions(GameDataResponse data, Dictionary<string, string> local)
    {
        Dictionary<string, Faction> factions = new();
        foreach (var faction in data.Categories)
        {
            if (faction?.DescKey is null || faction.DescKey == "PLACEHOLDER" || !local.TryGetValue(faction.DescKey, out var name))
                continue;

            var newFaction = Faction.Create(faction.Id!, name, faction.Visible);
            factions.Add(faction.Id!, newFaction.Value);
        }
        return factions;
    }
    private static Dictionary<string, Unit> MapUnits(GameDataResponse data, Dictionary<string, string> local, Dictionary<string, Faction> factions)
    {
        Dictionary<string, Unit> units = new();
        foreach (var unit in data.Units)
        {
            if ((int)unit.Rarity != 7) continue;
            if (unit.Obtainable != true) continue;
            if (unit.ObtainableTime != 0) continue;
            if (unit?.BaseId is null || unit?.NameKey is null || !local.TryGetValue(unit.NameKey, out var name)) continue;
            var newUnit = Unit.Create(unit.BaseId, name, (int)unit.CombatType);
            units.Add(unit.BaseId, newUnit.Value);
            foreach (var (factionValue, f) in unit.CategoryIds.Select((value, f) => (value, f)))
            {
                if (!factions.TryGetValue(factionValue, out var faction)) continue;
                faction.AddUnit(newUnit.Value);
            }
        }
        return units;

    }
    private static Dictionary<string, StatEnum> MapStatEnums(Dictionary<string, string> local)
    {
        Dictionary<string, StatEnum> statEnums = new();
        Dictionary<string, StatEnum> tmpLang = new();
        foreach (var item in local.Where(item => item.Key.StartsWith("UnitStat_") || item.Key.StartsWith("UNIT_STAT_")))
        {
            var enumValue = string.Join("", item.Key.Split("_"))?.Split("TU")[0]?.ToUpper()?.Replace("STATVIEW", "")?.Replace("STATSVIEW", "");
            if (enumValue is null) continue;
            var statEnum = StatEnum.Create(0, "", item.Key, local[item.Key]);
            tmpLang.TryAdd(enumValue, statEnum.Value);
        }
        foreach (var enumValue in Enum.GetValues(typeof(UnitStat)))
        {
            Result<StatEnum> statEnumResult = null!;
            if (tmpLang.TryGetValue(enumValue.ToString()!, out var lang))
                statEnumResult = StatEnum.Create((int)enumValue, enumValue.ToString()!, lang.LangId, lang.NameKey);

            var stat = enumValue.ToString()!.Replace("UNITSTATMAX", "UNITSTAT");
            if (tmpLang.TryGetValue(stat, out var newLang))
                statEnumResult = StatEnum.Create((int)enumValue, enumValue.ToString()!, newLang.LangId, newLang.NameKey);

            if (statEnumResult is not null)
                statEnums.TryAdd(enumValue.ToString()!, statEnumResult.Value);
        }
        return statEnums;
    }

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