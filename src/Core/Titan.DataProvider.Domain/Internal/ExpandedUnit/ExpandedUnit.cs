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
using Titan.DataProvider.Domain.Abstractions;
using Titan.DataProvider.Domain.Internal.ExpandedUnit.ValueObjects;

namespace Titan.DataProvider.Domain.Internal.ExpandedUnit;

public sealed class ExpandedUnit
{
    public ExpandedUnit(string definitionId, string name, string image, CombatType combatType, ForceAlignment alignment, bool isGalacticLegend, List<Stat> stats, double gp, List<Skill> skills, List<Mod> mods)
    {
        DefinitionId = definitionId;
        Name = name;
        Image = image;
        CombatType = combatType;
        Alignment = alignment;
        IsGalacticLegend = isGalacticLegend;
        _stats = stats;
        Gp = gp;
        _skills = skills;
        _mods = mods;
    }
    public string DefinitionId { get; private set; }
    public string Name { get; private set; }
    public string Image { get; private set; }
    public CombatType CombatType { get; private set; }
    public ForceAlignment Alignment { get; private set; }
    public bool IsGalacticLegend { get; private set; }
    public double Gp { get; private set; }
    public IReadOnlyList<Stat> Stats => _stats;
    public IReadOnlyList<Mod> Mods => _mods;
    public IReadOnlyList<Skill> Skills => _skills;
    private readonly List<Stat> _stats = new();
    private readonly List<Mod> _mods = new();
    private readonly List<Skill> _skills = new();
    public static Result<ExpandedUnit> Create(string definitionId, CombatType combatType, Unit unit, GameData gameData, List<Unit> crew, bool withStats, bool withoutGp, bool withoutModStats, bool withoutMods, bool withoutSkills)
    {
        var stats = GetStats(combatType, unit, gameData, crew, withStats, withoutGp, withoutModStats);
        if (stats.IsFailure)
            return Result.Failure<ExpandedUnit>(stats.Errors);
        var formattedStats = GetFormattedStats(stats.Value);

        var gameDataUnit = gameData.Units[definitionId];
        var alignment = gameDataUnit.ForceAlignment;
        var isGalacticLegend = gameDataUnit.IsGalacticLegend;
        var skills = new List<Skill>();
        if (!withoutSkills) skills = Skill.Create(unit, gameDataUnit).Value;

        var mods = new List<Mod>();
        if (!withoutMods) mods = Mod.Create(unit.EquippedStatMod).Value;

        return new ExpandedUnit(definitionId, gameDataUnit.Name, gameDataUnit.Image, combatType, (ForceAlignment)(int)alignment, isGalacticLegend, formattedStats.ToList(), stats.Value.Gp, skills, mods);
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

    public static IEnumerable<KeyValuePair<string, ExpandedUnit>> Create(PlayerProfileResponse playerProfile, bool withStats, bool withoutGp, bool withoutModStats, bool withoutMods, bool withoutSkills, GameData gameData)
    {
        var characterUnits = new Dictionary<string, Unit>();
        foreach (var unit in playerProfile.RosterUnit
            .OrderBy(unit => GetCombatType(gameData, unit)))
        {
            var definitionId = unit.DefinitionId!.Split(":")[0];
            if (IsCombatType(gameData, unit, CombatType.CHARACTER)) characterUnits.Add(definitionId, unit);

            var crew = Enumerable.Empty<Unit>();
            if (IsCombatType(gameData, unit, CombatType.SHIP)) crew = GetCrewUnits(gameData, characterUnits, definitionId);

            var expandedUnit = Create(definitionId, GetCombatType(gameData, unit), unit, gameData, crew.ToList(), withStats, withoutGp, withoutModStats, withoutMods, withoutSkills);
            yield return new(definitionId, expandedUnit.Value);
        }
    }
    public static IEnumerable<KeyValuePair<string, ExpandedUnit>> Create(string definitionId, PlayerProfileResponse playerProfile, bool withStats, bool withoutGp, bool withoutModStats, bool withoutMods, bool withoutSkills, GameData gameData)
    {

        var unit = playerProfile.RosterUnit.FirstOrDefault(x => x.DefinitionId!.Split(":")[0] == definitionId);
        if (unit is null) yield break;

        var crew = Enumerable.Empty<Unit>();
        if (IsCombatType(gameData, unit, CombatType.SHIP))
        {
            var characterUnits = new Dictionary<string, Unit>();
            foreach (var crewId in gameData.Units[definitionId].Crew)
            {
                var crewMember = playerProfile.RosterUnit.FirstOrDefault(x => x.DefinitionId!.Split(":")[0] == crewId);
                if (crewMember is null) continue;
                characterUnits.Add(crewId, crewMember);
            }
            crew = GetCrewUnits(gameData, characterUnits, definitionId);
        }

        var expandedUnit = Create(definitionId, GetCombatType(gameData, unit), unit, gameData, crew.ToList(), withStats, withoutGp, withoutModStats, withoutMods, withoutSkills);
        yield return new(definitionId, expandedUnit.Value);
    }

    private static IEnumerable<Unit> GetCrewUnits(GameData gameData, Dictionary<string, Unit> characterUnits, string definitionId)
    {
        foreach (var crewId in gameData.Units[definitionId].Crew)
        {
            characterUnits.TryGetValue(crewId, out var unit);
            yield return unit!;
        }

    }

    private static bool IsCombatType(GameData gameData, Unit unit, CombatType combatType)
        => GetCombatType(gameData, unit) == combatType;

    private static CombatType GetCombatType(GameData gameData, Unit unit)
    {
        var definitionId = unit.DefinitionId!.Split(":")[0];
        return (CombatType)gameData.Units[definitionId].CombatType;
    }

    private static Result<IStatCalc> GetStats(CombatType type, Unit unit, GameData gameData, List<Unit> crew, bool withStats, bool withoutGp, bool withoutMods)
        => type switch
        {
            CombatType.CHARACTER => CharacterStatCalc.Create(unit, gameData, withStats, withoutGp, withoutMods),
            CombatType.SHIP => ShipStatCalc.Create(unit, gameData, crew, withStats, withoutGp),
            _ => Result.Failure<IStatCalc>(DomainErrors.ExpandedUnit.CombatTypeNotFound)
        };
}
