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
        public List<GalacticPower> _galacticPower = new();
        public List<Stat> _stats = new();
        public List<Skill> skills = new();

        public static Result<ExpandedUnit> Create(Unit unit, GameData gameData)
        {
            var definitionId = unit.DefinitionId!.Split(":")[0];
            var combatType = (CombatType)gameData.Units[definitionId].CombatType - 1;

            var formattedStats = new List<Stat>();
            if (combatType == CombatType.SQUAD)
            {
                var stats = CalculateCharacterStats(unit, gameData);
                foreach (var statValue in stats._base)
                {
                    var stat = Stat.Create((int)statValue.Key, statValue.Value, 0);
                    formattedStats.Add(stat.Value);
                }
            }
            return new ExpandedUnit(Guid.NewGuid(), definitionId, gameData.Units[definitionId].Name, gameData.Units[definitionId].Image, combatType, formattedStats);
        }

        public static Result<List<ExpandedUnit>> Create(PlayerProfileResponse playerProfile, GameData gameData)
        {
            var expandedUnits = new List<ExpandedUnit>();
            foreach (var unit in playerProfile.RosterUnit)
            {
                var expandedUnit = Create(unit, gameData);
                expandedUnits.Add(expandedUnit.Value);
            }
            return expandedUnits;
        }
        private static StatCalcBase CalculateCharacterStats(Unit unit, GameData gameData)
        {
            var statCalcBase = StatCalcBase.Create();
            statCalcBase.Value.CalculateRawStats(unit, gameData);
            statCalcBase.Value.CalculateBaseStats(unit, gameData);
            statCalcBase.Value.FormatStats(unit.CurrentLevel);
            return statCalcBase.Value;
        }
    }
}