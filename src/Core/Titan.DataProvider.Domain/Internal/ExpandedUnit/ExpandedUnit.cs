using System.Linq;
using System.Collections.Generic;
using Titan.DataProvider.Domain.Internal.ExpandedUnit.Enums;
using Titan.DataProvider.Domain.Models.GalaxyOfHeroes.PlayerProfile;
using Titan.DataProvider.Domain.Models.GalaxyOfHeroes.Common;
using Titan.DataProvider.Domain.Shared;
using Skill = Titan.DataProvider.Domain.Internal.ExpandedUnit.ValueObjects.Skill;
using GameData = Titan.DataProvider.Domain.Internal.BaseData.BaseData;
using Stat = Titan.DataProvider.Domain.Internal.ExpandedUnit.ValueObjects.Stat;
using Titan.DataProvider.Domain.Internal.ExpandedUnit.Services;
using Titan.DataProvider.Domain.Errors;
using System;

namespace Titan.DataProvider.Domain.Internal.ExpandedUnit;

public sealed class ExpandedUnit
{
    public ExpandedUnit(string definitionId, string name, string image, CombatType combatType, Dictionary<int, Stat> stats, double gp, Dictionary<string, Skill> skills)
    {
        DefinitionId = definitionId;
        Name = name;
        Image = image;
        CombatType = combatType;
        _stats = stats;
        Gp = gp;
        _skills = skills;
    }
    public string DefinitionId { get; private set; }
    public string Name { get; private set; }
    public string Image { get; private set; }
    public CombatType CombatType { get; private set; }
    public double Gp { get; private set; }
    public IReadOnlyDictionary<int, Stat> Stats => _stats;
    public IReadOnlyDictionary<string, Skill> Skills => _skills;
    private readonly Dictionary<int, Stat> _stats = new();
    private readonly Dictionary<string, Skill> _skills = new();

    public static Result<ExpandedUnit> Create(Unit unit, GameData gameData, Dictionary<string, Unit> crew, bool withStats, bool withoutGp, bool withoutMods, bool withoutSkills, bool withoutDatacrons)
    {
        var definitionId = unit.DefinitionId!.Split(":")[0];
        var combatType = (CombatType)gameData.Units[definitionId].CombatType - 1;

        var stats = GetStats(combatType, unit, gameData, crew, withStats, withoutGp, withoutMods);
        if (stats.IsFailure)
            return Result.Failure<ExpandedUnit>(stats.Errors);
        var formattedStats = GetFormattedStats(stats.Value);

        var skills = new Dictionary<string, Skill>();
        if (!withoutSkills) skills = Skill.Create(unit, gameData.Units[definitionId]).Value;

        return new ExpandedUnit(definitionId, gameData.Units[definitionId].Name, gameData.Units[definitionId].Image, combatType, formattedStats, stats.Value.Gp, skills);
    }

    private static Dictionary<int, Stat> GetFormattedStats(IStatCalc stats)
    {
        var statsDict = new Dictionary<int, Stat>();
        foreach (var statKey in Enum.GetValues<UnitStat>())
        {
            var baseValue = stats.Base.TryGetValue((int)statKey, out var foundBaseValue) ? foundBaseValue : 0.0;
            var modValue = stats.Mods.TryGetValue((int)statKey, out var foundModValue) ? foundModValue : 0.0;
            var gearValue = stats.Gear.TryGetValue((int)statKey, out var foundGearValue) ? foundGearValue : 0.0;
            var crewValue = stats.Crew.TryGetValue((int)statKey, out var foundCrewValue) ? foundCrewValue : 0.0;
            var stat = Stat.Create((int)statKey, baseValue, gearValue, modValue, crewValue);
            if (stat.IsSuccess)
                statsDict.Add((int)stat.Value.UnitStat, stat.Value);
        }
        return statsDict;
    }

    public static Result<Dictionary<string, ExpandedUnit>> Create(PlayerProfileResponse playerProfile, bool withStats, bool withoutGp, bool withoutMods, bool withoutSkills, bool withoutDatacrons, GameData gameData)
    {
        var expandedUnits = new Dictionary<string, ExpandedUnit>();
        var rosterUnits = playerProfile.RosterUnit.ToDictionary(x => x.DefinitionId!.Split(":")[0]);

        var crewDict = GetCrewHashmap(gameData, rosterUnits);

        foreach (var unit in playerProfile.RosterUnit)
        {
            var definitionId = unit.DefinitionId!.Split(":")[0];
            crewDict.TryGetValue(definitionId, out var crew);
            crew ??= new Dictionary<string, Unit>();
            var expandedUnit = Create(unit, gameData, crew, withStats, withoutGp, withoutMods, withoutSkills, withoutDatacrons);
            expandedUnits.Add(definitionId, expandedUnit.Value);
        }
        return expandedUnits;
    }

    private static Dictionary<string, Dictionary<string, Unit>> GetCrewHashmap(GameData gameData, Dictionary<string, Unit> rosterUnits)
    {
        Dictionary<string, Dictionary<string, Unit>> crewDict = new();
        var crewStringDict = gameData.Units
            .Where(x => (CombatType)x.Value.CombatType - 1 == CombatType.SHIP)
            .ToDictionary(x => x.Value.Id, x => x.Value.Crew);

        foreach (var val in crewStringDict)
        {
            crewDict[val.Key] = new Dictionary<string, Unit>();
            foreach (var crew in val.Value)
                crewDict[val.Key].TryAdd(crew, rosterUnits[crew]);
        }

        return crewDict;
    }
    private static Result<IStatCalc> GetStats(CombatType type, Unit unit, GameData gameData, Dictionary<string, Unit> crew, bool withStats, bool withoutGp, bool withoutMods)
        => type switch
        {
            CombatType.SQUAD => CharacterStatCalc.Create(unit, gameData, withStats, withoutGp, withoutMods),
            CombatType.SHIP => ShipStatCalc.Create(unit, gameData, crew, withStats, withoutGp),
            _ => Result.Failure<IStatCalc>(DomainErrors.ExpandedUnit.CombatTypeNotFound)
        };
}
