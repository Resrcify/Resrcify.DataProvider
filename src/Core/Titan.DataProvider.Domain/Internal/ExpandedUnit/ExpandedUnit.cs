using System.Linq;
using System.Collections.Generic;
using Titan.DataProvider.Domain.Models.GalaxyOfHeroes.PlayerProfile;
using Titan.DataProvider.Domain.Models.GalaxyOfHeroes.Common;
using Titan.DataProvider.Domain.Shared;
using Skill = Titan.DataProvider.Domain.Internal.ExpandedUnit.ValueObjects.Skill;
using GameData = Titan.DataProvider.Domain.Internal.BaseData.BaseData;
using Stat = Titan.DataProvider.Domain.Internal.ExpandedUnit.ValueObjects.Stat;
using Titan.DataProvider.Domain.Internal.ExpandedUnit.Services;
using Titan.DataProvider.Domain.Errors;
using System;
using Titan.DataProvider.Domain.Models.GalaxyOfHeroes.GameData;

namespace Titan.DataProvider.Domain.Internal.ExpandedUnit;

public sealed class ExpandedUnit
{
    public ExpandedUnit(string definitionId, string name, string image, CombatType combatType, List<Stat> stats, double gp, List<Skill> skills)
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
    public IReadOnlyList<Stat> Stats => _stats;
    public IReadOnlyList<Skill> Skills => _skills;
    private readonly List<Stat> _stats = new();
    private readonly List<Skill> _skills = new();
    public static Result<ExpandedUnit> Create(Unit unit, GameData gameData, List<Unit> crew, bool withStats, bool withoutGp, bool withoutMods, bool withoutSkills)
    {
        var definitionId = unit.DefinitionId!.Split(":")[0];
        var combatType = (CombatType)gameData.Units[definitionId].CombatType;

        var stats = GetStats(combatType, unit, gameData, crew, withStats, withoutGp, withoutMods);
        if (stats.IsFailure)
            return Result.Failure<ExpandedUnit>(stats.Errors);
        var formattedStats = GetFormattedStats(stats.Value);

        var skills = new List<Skill>();
        if (!withoutSkills) skills = Skill.Create(unit, gameData.Units[definitionId]).Value;

        return new ExpandedUnit(definitionId, gameData.Units[definitionId].Name, gameData.Units[definitionId].Image, combatType, formattedStats.ToList(), stats.Value.Gp, skills);
    }

    private static IEnumerable<Stat> GetFormattedStats(IStatCalc stats)
    {
        foreach (var statKey in Enum.GetValues<UnitStat>())
        {
            var baseValue = stats.Base.TryGetValue((int)statKey, out var foundBaseValue) ? foundBaseValue : 0.0;
            var modValue = stats.Mods.TryGetValue((int)statKey, out var foundModValue) ? foundModValue : 0.0;
            var gearValue = stats.Gear.TryGetValue((int)statKey, out var foundGearValue) ? foundGearValue : 0.0;
            var crewValue = stats.Crew.TryGetValue((int)statKey, out var foundCrewValue) ? foundCrewValue : 0.0;
            var stat = Stat.Create((int)statKey, baseValue, gearValue, modValue, crewValue);
            if (stat.IsSuccess)
                yield return stat.Value;
        }
    }

    public static IEnumerable<KeyValuePair<string, ExpandedUnit>> Create(PlayerProfileResponse playerProfile, bool withStats, bool withoutGp, bool withoutMods, bool withoutSkills, GameData gameData)
    {
        var rosterUnits = playerProfile.RosterUnit.ToDictionary(x => x.DefinitionId!.Split(":")[0]);
        var crewDict = GetCrewHashmap(gameData, rosterUnits);

        foreach (var unit in playerProfile.RosterUnit)
        {
            var definitionId = unit.DefinitionId!.Split(":")[0];
            crewDict.TryGetValue(definitionId, out var crew);
            var expandedUnit = Create(unit, gameData, crew ?? new List<Unit>(), withStats, withoutGp, withoutMods, withoutSkills);
            yield return new(definitionId, expandedUnit.Value);
        }
    }

    private static Dictionary<string, List<Unit>> GetCrewHashmap(GameData gameData, Dictionary<string, Unit> rosterUnits)
    {
        Dictionary<string, List<Unit>> crewDict = new();
        foreach (var val in gameData.Units.Where(x => x.Value.CombatType == (long)CombatType.SHIP))
        {
            crewDict[val.Key] = new List<Unit>();
            foreach (var crew in val.Value.Crew)
                crewDict[val.Key].Add(rosterUnits[crew]);
        }
        return crewDict;
    }
    private static Result<IStatCalc> GetStats(CombatType type, Unit unit, GameData gameData, List<Unit> crew, bool withStats, bool withoutGp, bool withoutMods)
        => type switch
        {
            CombatType.CHARACTER => CharacterStatCalc.Create(unit, gameData, withStats, withoutGp, withoutMods),
            CombatType.SHIP => ShipStatCalc.Create(unit, gameData, crew, withStats, withoutGp),
            _ => Result.Failure<IStatCalc>(DomainErrors.ExpandedUnit.CombatTypeNotFound)
        };
}
