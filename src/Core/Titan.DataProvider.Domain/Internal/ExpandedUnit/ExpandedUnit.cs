using System.Linq;
using System.Collections.Generic;
using Titan.DataProvider.Domain.Internal.ExpandedUnit.Enums;
using Titan.DataProvider.Domain.Internal.ExpandedUnit.ValueObjects;
using Titan.DataProvider.Domain.Models.GalaxyOfHeroes.PlayerProfile;
using Titan.DataProvider.Domain.Models.GalaxyOfHeroes.Common;
using Titan.DataProvider.Domain.Shared;
using Skill = Titan.DataProvider.Domain.Internal.ExpandedUnit.ValueObjects.Skill;
using GameData = Titan.DataProvider.Domain.Internal.BaseData.BaseData;
using Stat = Titan.DataProvider.Domain.Internal.ExpandedUnit.ValueObjects.Stat;
using Titan.DataProvider.Domain.Internal.ExpandedUnit.Services;
using Titan.DataProvider.Domain.Errors;
using System;

namespace Titan.DataProvider.Domain.Internal.ExpandedUnit
{
    public sealed class ExpandedUnit
    {
        public ExpandedUnit(string definitionId, string name, string image, CombatType combatType, List<Stat> stats)
        {
            DefinitionId = definitionId;
            Name = name;
            Image = image;
            CombatType = combatType;
            _stats = stats;
        }
        public string DefinitionId { get; private set; }
        public string Name { get; private set; }
        public string Image { get; private set; }
        public CombatType CombatType { get; private set; }
        public IReadOnlyList<GalacticPower> GalacticPower => _galacticPower;
        public IReadOnlyList<Stat> Stats => _stats;
        public IReadOnlyList<Skill> Skills => _skills;
        private readonly List<GalacticPower> _galacticPower = new();
        private readonly List<Stat> _stats = new();
        private readonly List<Skill> _skills = new();

        public static Result<ExpandedUnit> Create(Unit unit, GameData gameData, Dictionary<string, Unit> crew)
        {
            var definitionId = unit.DefinitionId!.Split(":")[0];
            var combatType = (CombatType)gameData.Units[definitionId].CombatType - 1;
            var stats = GetStats(combatType, unit, gameData, crew);
            if (stats.IsFailure)
                return Result.Failure<ExpandedUnit>(stats.Errors);
            var formattedStats = GetFormattedStats(stats.Value);
            return new ExpandedUnit(definitionId, gameData.Units[definitionId].Name, gameData.Units[definitionId].Image, combatType, formattedStats.ToList());
        }

        private static IEnumerable<Stat> GetFormattedStats(IStatCalc stats)
        {
            foreach (var statKey in Enum.GetValues(typeof(UnitStat)))
            {
                var baseValue = 0.0;
                var modValue = 0.0;
                var gearValue = 0.0;
                var crewValue = 0.0;
                if (stats.Base.TryGetValue((int)statKey, out var foundBaseValue))
                    baseValue = foundBaseValue;
                if (stats.Mods.TryGetValue((int)statKey, out var foundModValue))
                    modValue = foundModValue;
                if (stats.Gear.TryGetValue((int)statKey, out var foundGearValue))
                    gearValue = foundGearValue;
                if (stats.Crew.TryGetValue((int)statKey, out var foundCrewValue))
                    crewValue = foundCrewValue;
                var stat = Stat.Create((int)statKey, baseValue, gearValue, modValue, crewValue);
                if (stat.IsSuccess)
                    yield return stat.Value;
            }
        }

        public static Result<List<ExpandedUnit>> Create(PlayerProfileResponse playerProfile, GameData gameData)
        {
            var expandedUnits = new List<ExpandedUnit>();
            var rosterUnits = playerProfile.RosterUnit.ToDictionary(x => x.DefinitionId!.Split(":")[0]);
            foreach (var unit in playerProfile.RosterUnit)
            {
                var crew = GetCrewFromRoster(unit, rosterUnits, gameData);
                var expandedUnit = Create(unit, gameData, crew);
                expandedUnits.Add(expandedUnit.Value);
            }
            return expandedUnits;
        }

        private static Dictionary<string, Unit> GetCrewFromRoster(Unit unit, Dictionary<string, Unit> rosterUnits, GameData gameData)
        {
            var definitionId = unit.DefinitionId!.Split(":")[0];
            var combatType = (CombatType)gameData.Units[definitionId].CombatType - 1;
            if (combatType != CombatType.SHIP) return new Dictionary<string, Unit>();
            var crewUnits = new Dictionary<string, Unit>();
            foreach (var member in gameData.Units[definitionId].Crew)
            {
                if (!rosterUnits.TryGetValue(member, out var crewUnit)) continue;
                crewUnits.Add(member, crewUnit);
            }
            return crewUnits;
        }

        private static Result<IStatCalc> GetStats(CombatType type, Unit unit, GameData gameData, Dictionary<string, Unit> crew)
            => type switch
            {
                CombatType.SQUAD => CharacterStatCalc.Create(unit, gameData),
                CombatType.SHIP => ShipStatCalc.Create(unit, gameData, crew),
                _ => Result.Failure<IStatCalc>(DomainErrors.ExpandedUnit.CombatTypeNotFound)
            };
    }
}