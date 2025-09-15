using System.Linq;
using System.Collections.Generic;
using Resrcify.DataProvider.Domain.Models.GalaxyOfHeroes.PlayerProfile;
using Skill = Resrcify.DataProvider.Domain.Internal.ExpandedUnit.ValueObjects.Skill;
using GameData = Resrcify.DataProvider.Domain.Internal.BaseData.BaseData;
using Stat = Resrcify.DataProvider.Domain.Internal.ExpandedUnit.ValueObjects.Stat;
using Resrcify.DataProvider.Domain.Internal.ExpandedUnit.Services;
using Resrcify.DataProvider.Domain.Errors;
using System;
using Resrcify.DataProvider.Domain.Models.GalaxyOfHeroes.GameData;
using Resrcify.DataProvider.Domain.Abstractions;
using Resrcify.DataProvider.Domain.Internal.ExpandedUnit.ValueObjects;
using Resrcify.SharedKernel.ResultFramework.Primitives;
using Resrcify.DataProvider.Domain.Models.GalaxyOfHeroes.Common;

namespace Resrcify.DataProvider.Domain.Internal.ExpandedUnit;

public sealed class ExpandedUnit
{
    private ExpandedUnit(
        string id,
        string definitionId,
        string name,
        string image,
        CombatType combatType,
        ForceAlignment alignment,
        Rarity rarity,
        int level,
        UnitTier gearTier,
        RelicTier relicTier,
        bool isGalacticLegend,
        bool isCapital,
        List<Stat> stats,
        double gp,
        double crewGp,
        List<Skill> skills,
        List<Mod> mods)
    {
        Id = id;
        DefinitionId = definitionId;
        Name = name;
        Image = image;
        CombatType = combatType;
        Alignment = alignment;
        Rarity = rarity;
        Level = level;
        GearTier = gearTier;
        RelicTier = relicTier;
        IsGalacticLegend = isGalacticLegend;
        IsCapital = isCapital;
        _stats = stats;
        Gp = gp;
        CrewGp = crewGp;
        _skills = skills;
        _mods = mods;
    }
    public string Id { get; private set; }
    public string DefinitionId { get; private set; }
    public string Name { get; private set; }
    public string Image { get; private set; }
    public CombatType CombatType { get; private set; }
    public ForceAlignment Alignment { get; private set; }
    public Rarity Rarity { get; private set; }
    public int Level { get; private set; }
    public UnitTier GearTier { get; private set; }
    public RelicTier RelicTier { get; private set; }
    public bool IsGalacticLegend { get; private set; }
    public bool IsCapital { get; private set; }
    public double Gp { get; private set; }
    public double CrewGp { get; private set; }
    public IReadOnlyList<Stat> Stats => _stats;
    public IReadOnlyList<Mod> Mods => _mods;
    public IReadOnlyList<Skill> Skills => _skills;
    private readonly List<Stat> _stats = [];
    private readonly List<Mod> _mods = [];
    private readonly List<Skill> _skills = [];
    public static Result<ExpandedUnit> Create(
        string id,
        string definitionId,
        CombatType combatType,
        Unit unit,
        GameData gameData,
        List<Unit> crew,
        bool withStats,
        bool withoutGp,
        bool withoutModStats,
        bool withoutMods,
        bool withoutSkills)
    {
        var stats = GetStats(
            combatType,
            unit,
            gameData,
            crew,
            withStats,
            withoutGp,
            withoutModStats);
        if (stats.IsFailure)
            return Result.Failure<ExpandedUnit>(stats.Errors);
        var formattedStats = GetFormattedStats(stats.Value);

        var gameDataUnit = gameData.Units[definitionId];
        var alignment = gameDataUnit.ForceAlignment;
        var isGalacticLegend = gameDataUnit.IsGalacticLegend;
        var isCapital = gameDataUnit.UnitClass == (int)UnitClass.Unitclasscommander;
        var skills = new List<Skill>();
        if (!withoutSkills)
            skills = Skill.Create(unit, gameDataUnit).Value;

        var mods = new List<Mod>();
        if (!withoutMods)
            mods = Mod.Create(unit.EquippedStatMods).Value;

        var relic = unit.Relic?.CurrentTier ?? RelicTier.Reliclocked;
        var rarity = unit.CurrentRarity;
        var level = unit.CurrentLevel;
        var gear = unit.CurrentTier;

        return new ExpandedUnit(
            id,
            definitionId,
            gameDataUnit.Name,
            gameDataUnit.Image,
            combatType,
            (ForceAlignment)(int)alignment,
            rarity,
            level,
            gear,
            relic,
            isGalacticLegend,
            isCapital,
            formattedStats.ToList(),
            stats.Value.Gp,
            stats.Value.CrewGp,
            skills,
            mods);
    }

    private static IEnumerable<Stat> GetFormattedStats(IStatCalc stats)
    {
        foreach (var statKey in Enum.GetValues<UnitStat>())
        {
            var baseValue = stats.Base.TryGetValue((int)statKey, out var foundBaseValue) ? foundBaseValue : 0.0;
            var modValue = stats.Mods.TryGetValue((int)statKey, out var foundModValue) ? foundModValue : 0.0;
            var gearValue = stats.Gear.TryGetValue((int)statKey, out var foundGearValue) ? foundGearValue : 0.0;
            var crewValue = stats.Crew.TryGetValue((int)statKey, out var foundCrewValue) ? foundCrewValue : 0.0;
            var stat = Stat.Create(statKey, baseValue, gearValue, modValue, crewValue);
            if (stat.IsSuccess)
                yield return stat.Value;
        }
    }

    public static IEnumerable<KeyValuePair<string, ExpandedUnit>> Create(
        PlayerProfileResponse playerProfile,
        bool withStats,
        bool withoutGp,
        bool withoutModStats,
        bool withoutMods,
        bool withoutSkills,
        GameData gameData)
    {
        var characterUnits = new Dictionary<string, Unit>();
        foreach (var unit in playerProfile.RosterUnits
            .OrderBy(unit => GetCombatType(gameData, unit)))
        {
            if (unit.Id is null)
                continue;
            var definitionId = unit.DefinitionId!.Split(":")[0];
            if (IsCombatType(gameData, unit, CombatType.Character))
                characterUnits.Add(definitionId, unit);

            var crew = Enumerable.Empty<Unit>();
            if (IsCombatType(gameData, unit, CombatType.Ship))
                crew = GetCrewUnits(gameData, characterUnits, definitionId);

            var expandedUnit = Create(
                unit.Id,
                definitionId,
                GetCombatType(gameData, unit),
                unit,
                gameData,
                crew.ToList(),
                withStats,
                withoutGp,
                withoutModStats,
                withoutMods,
                withoutSkills);
            yield return new(
                definitionId,
                expandedUnit.Value);
        }
    }
    public static IEnumerable<KeyValuePair<string, ExpandedUnit>> Create(
        string definitionId,
        PlayerProfileResponse playerProfile,
        bool withStats,
        bool withoutGp,
        bool withoutModStats,
        bool withoutMods,
        bool withoutSkills,
        GameData gameData)
    {
        var unit = playerProfile.RosterUnits.FirstOrDefault(x => x.DefinitionId!.Split(":")[0] == definitionId);
        if (unit is null || unit.Id is null)
            yield break;

        var crew = Enumerable.Empty<Unit>();
        if (IsCombatType(gameData, unit, CombatType.Ship))
        {
            var characterUnits = new Dictionary<string, Unit>();
            foreach (var crewId in gameData.Units[definitionId].Crew)
            {
                var crewMember = playerProfile.RosterUnits.FirstOrDefault(x => x.DefinitionId!.Split(":")[0] == crewId);
                if (crewMember is null)
                    continue;
                characterUnits.Add(crewId, crewMember);
            }
            crew = GetCrewUnits(gameData, characterUnits, definitionId);
        }

        var expandedUnit = Create(
            unit.Id,
            definitionId,
            GetCombatType(gameData, unit),
            unit,
            gameData,
            crew.ToList(),
            withStats,
            withoutGp,
            withoutModStats,
            withoutMods,
            withoutSkills);
        yield return new(
            definitionId,
            expandedUnit.Value);
    }

    private static IEnumerable<Unit> GetCrewUnits(
        GameData gameData,
        Dictionary<string, Unit> characterUnits,
        string definitionId)
    {
        foreach (var crewId in gameData.Units[definitionId].Crew)
        {
            characterUnits.TryGetValue(crewId, out var unit);
            yield return unit!;
        }

    }

    private static bool IsCombatType(
        GameData gameData,
        Unit unit,
        CombatType combatType)
        => GetCombatType(gameData, unit) == combatType;

    private static CombatType GetCombatType(
        GameData gameData,
        Unit unit)
    {
        var definitionId = unit.DefinitionId!.Split(":")[0];
        return (CombatType)gameData.Units[definitionId].CombatType;
    }

    private static Result<IStatCalc> GetStats(
        CombatType type,
        Unit unit,
        GameData gameData,
        List<Unit> crew,
        bool withStats,
        bool withoutGp,
        bool withoutMods)
        => type switch
        {
            CombatType.Character => CharacterStatCalc.Create(
                unit,
                gameData,
                withStats,
                withoutGp,
                withoutMods),
            CombatType.Ship => ShipStatCalc.Create(
                unit,
                gameData,
                crew,
                withStats,
                withoutGp),
            _ => Result.Failure<IStatCalc>(DomainErrors.ExpandedUnit.CombatTypeNotFound)
        };
}
