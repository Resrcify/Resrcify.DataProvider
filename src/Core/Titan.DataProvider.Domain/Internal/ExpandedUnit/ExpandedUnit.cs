using System.Linq;
using System;
using System.Collections.Generic;
using Titan.DataProvider.Domain.Internal.ExpandedUnit.Enums;
using Titan.DataProvider.Domain.Internal.ExpandedUnit.ValueObjects;
using Titan.DataProvider.Domain.Models.GalaxyOfHeroes.PlayerProfile;
using Titan.DataProvider.Domain.Primitives;
using Titan.DataProvider.Domain.Shared;
using Skill = Titan.DataProvider.Domain.Internal.ExpandedUnit.ValueObjects.Skill;
using GameData = Titan.DataProvider.Domain.Internal.BaseData.BaseData;
using Stat = Titan.DataProvider.Domain.Internal.ExpandedUnit.ValueObjects.Stat;
using Titan.DataProvider.Domain.Internal.ExpandedUnit.Services;
using Titan.DataProvider.Domain.Errors;

namespace Titan.DataProvider.Domain.Internal.ExpandedUnit
{
    public sealed class ExpandedUnit : AggregateRoot
    {
        public ExpandedUnit(Guid id, string definitionId, string name, string image, CombatType combatType, List<Stat> stats) : base(id)
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
        public List<GalacticPower> _galacticPower = new();
        public List<Stat> _stats = new();
        public List<Skill> _skills = new();

        public static Result<ExpandedUnit> Create(Unit unit, GameData gameData, Dictionary<string, Unit> crew)
        {
            var definitionId = unit.DefinitionId!.Split(":")[0];
            var combatType = (CombatType)gameData.Units[definitionId].CombatType - 1;

            var formattedStats = new List<Stat>();
            var stats = GetStats(combatType, unit, gameData, crew);
            if (stats.IsFailure)
                return Result.Failure<ExpandedUnit>(stats.Errors);
            foreach (var statKey in stats.Value.Base.Keys)
            {
                var baseValue = stats.Value.Base[statKey];
                var modValue = 0.0;
                var gearValue = 0.0;
                if (stats.Value.Mods.TryGetValue(statKey, out var foundModValue))
                    modValue = foundModValue;
                if (stats.Value.Gear.TryGetValue(statKey, out var foundGearValue))
                    gearValue = foundGearValue;
                var stat = Stat.Create((int)statKey, baseValue, gearValue, modValue);
                formattedStats.Add(stat.Value);
            }
            return new ExpandedUnit(Guid.NewGuid(), definitionId, gameData.Units[definitionId].Name, gameData.Units[definitionId].Image, combatType, formattedStats);
        }

        public static Result<List<ExpandedUnit>> Create(PlayerProfileResponse playerProfile, GameData gameData)
        {
            var expandedUnits = new List<ExpandedUnit>();
            foreach (var unit in playerProfile.RosterUnit)
            {
                var crew = GetCrewFromRoster(unit, playerProfile, gameData);
                var expandedUnit = Create(unit, gameData, crew);
                expandedUnits.Add(expandedUnit.Value);
            }
            return expandedUnits;
        }

        private static Dictionary<string, Unit> GetCrewFromRoster(Unit unit, PlayerProfileResponse playerProfile, GameData gameData)
        {
            var definitionId = unit.DefinitionId!.Split(":")[0];
            var combatType = (CombatType)gameData.Units[definitionId].CombatType - 1;
            if (combatType != CombatType.SHIP) return new Dictionary<string, Unit>();
            var crewUnits = new Dictionary<string, Unit>();
            foreach (var member in gameData.Units[definitionId].Crew)
            {
                var crewUnit = playerProfile.RosterUnit.FirstOrDefault(x => x.DefinitionId == member);
                if (crewUnit is null)
                    continue;
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