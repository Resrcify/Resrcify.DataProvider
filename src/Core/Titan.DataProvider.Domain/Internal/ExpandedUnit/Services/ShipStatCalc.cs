using System.Collections.Generic;
using Titan.DataProvider.Domain.Models.GalaxyOfHeroes.PlayerProfile;
using GameData = Titan.DataProvider.Domain.Internal.BaseData.BaseData;
using System.Linq;
using Titan.DataProvider.Domain.Shared;
using Skill = Titan.DataProvider.Domain.Models.GalaxyOfHeroes.PlayerProfile.Skill;
using System.Runtime.InteropServices;
using Titan.DataProvider.Domain.Abstractions;

namespace Titan.DataProvider.Domain.Internal.ExpandedUnit.Services;

public class ShipStatCalc : StatCalcBase, IStatCalc
{
    public IReadOnlyDictionary<int, double> Base => _base;
    public IReadOnlyDictionary<int, double> Gear => _gear;
    public IReadOnlyDictionary<int, double> Mods => _mods;
    public IReadOnlyDictionary<int, double> Crew => _crew;
    public double Gp => BaseGp;
    private readonly List<Unit> _crewUnits;
    private ShipStatCalc(
        Unit unit,
        GameData gameData,
        List<Unit> crewUnits,
        bool withStats,
        bool withoutGp) : base(unit, gameData)
    {
        _crewUnits = crewUnits;
        if (withStats)
        {
            CalculateRawStats();
            CalculateBaseStats();
            FormatStats();
        }
        if (!withoutGp) BaseGp = CalculateShipGp();
    }
    public static Result<IStatCalc> Create(Unit unit, GameData gameData, List<Unit> crew, bool withStats, bool withoutGp)
    {
        return new ShipStatCalc(unit, gameData, crew, withStats, withoutGp);
    }
    private void CalculateRawStats()
    {
        var rarityEnumValue = (int)_unit.CurrentRarity;
        var definitionId = _unit.DefinitionId!.Split(":")[0];

        foreach (var stat in _gameData.Units[definitionId].Stats)
            _base.Add((int)stat.Key, stat.Value);

        foreach (var stat in _gameData.Units[definitionId].GrowthModifiers[rarityEnumValue.ToString()])
            _growthModifiers.Add(stat.Key, stat.Value);

        var crewRating = _crewUnits.Count == 0 ? GetCrewlessCrewRating(rarityEnumValue) : GetCrewRating();
        var statMultiplier = _gameData.CrTable.ShipRarityFactor[rarityEnumValue.ToString()] * crewRating;
        foreach (var (statId, statValue) in _gameData.Units[definitionId].CrewStats)
        {
            var longStatId = int.Parse(statId);
            // stats 1-15 and 28 all have final integer values
            // other stats require decimals -- shrink to 8 digits of precision through 'unscaled' values this calculator uses
            _crew[longStatId] = Floor(statValue * statMultiplier, (longStatId < 16 || longStatId == 28) ? 8 : 0);
        }
    }

    // Crew Rating for GP purposes depends on skill type (i.e. contract/hardware/etc.), but for stats it apparently doesn't.
    private double GetSkillCrewRating(Skill skill)
        => _gameData.CrTable.AbilityLevelCr[(skill.Tier + 2).ToString()];

    //   // temporarily uses hard-coded multipliers, as the true in-game formula remains a mystery.
    //   // but these values have experimentally been found accurate for the first 3 crewless ships:
    //   //     (Vulture Droid, Hyena Bomber, and BTL-B Y-wing)
    private double GetCrewlessCrewRating(int rarity)
        => Floor(
            _gameData.CrTable.CrewRarityCr[rarity.ToString()] +
            3.5 * _gameData.CrTable.UnitLevelCr[_unit.CurrentLevel.ToString()] +
            GetCrewlessSkillsCrewRating()
        );

    private double GetCrewlessSkillsCrewRating()
        => _unit.Skill.Sum(x => (x.Id![..8] == "hardware" ? 0.696 : 2.46) * _gameData.CrTable.AbilityLevelCr[(x.Tier + 2).ToString()]);

    private double GetCrewRating()
    {
        var crewRating = 0.0;
        foreach (var member in CollectionsMarshal.AsSpan(_crewUnits))
        {
            var definitionId = member.DefinitionId!.Split(":")[0];
            var tierEnumValue = (int)member.CurrentTier;
            var rarityEnumValue = (int)member.CurrentRarity;
            var level = member.CurrentLevel;
            var relicEnumValue = member.Relic?.CurrentTier is not null ? (int)member.Relic!.CurrentTier : 0;
            crewRating += _gameData.CrTable.UnitLevelCr[level.ToString()] + _gameData.CrTable.CrewRarityCr[rarityEnumValue.ToString()]; // add CR from level/rarity
            crewRating += _gameData.CrTable.GearLevelCr[tierEnumValue.ToString()]; // add CR from complete gear levels

            if (_gameData.CrTable.GearPieceCr.TryGetValue(tierEnumValue.ToString(), out var gearPeiceCrValue))
                crewRating += gearPeiceCrValue * member.Equipment?.Count ?? 0; // add CR from currently equipped gear
            crewRating += member.Skill.Sum(GetSkillCrewRating); // add CR from ability levels

            // add CR from mods
            if (member?.EquippedStatMod?.Count > 0)
                crewRating += member.EquippedStatMod.Sum(x => _gameData.CrTable.ModRarityLevelCr[x.DefinitionId![1].ToString()][x.Level.ToString()]);

            //     // add CR from relics
            if (member?.Relic is not null && (int)member.Relic.CurrentTier > 2)
            {
                crewRating += _gameData.CrTable.RelicTierCr[relicEnumValue.ToString()];
                crewRating += level * _gameData.CrTable.RelicTierLevelFactor[relicEnumValue.ToString()];
            }
        }
        return crewRating;
    }

    public double CalculateShipGp()
        => _crewUnits.Count == 0 ?
            CalculateCrewlessShipGp() :
            CalculateCrewShipGp();

    public double CalculateCrewlessShipGp()
    {
        var levelGp = _gameData.GpTable.UnitLevelGp[_unit.CurrentLevel.ToString()];
        var abilityGp = GetCrewlessAbilityGp();
        var reinforcementGp = GetCrewlessReinforcementGp();
        var gp = (levelGp * 3.5 + abilityGp * 5.74 + reinforcementGp * 1.61) * _gameData.GpTable.ShipRarityFactor[((int)_unit.CurrentRarity).ToString()];
        gp += levelGp + abilityGp + reinforcementGp;
        return Floor(gp * 1.5);
    }

    public double CalculateCrewShipGp()
    {
        var defId = _unit.DefinitionId?.Split(":")[0];
        if (defId is null) return 0;
        var gp = _crewUnits.Sum(CalculateCharacterGp);
        if (_gameData.GpTable.ShipRarityFactor.TryGetValue(((int)_unit.CurrentRarity).ToString(), out var value))
            gp *= value * _gameData.GpTable.CrewSizeFactor[_crewUnits.Count.ToString()];
        gp += _gameData.GpTable.UnitLevelGp[_unit.CurrentLevel.ToString()];
        foreach (var skill in CollectionsMarshal.AsSpan(_unit.Skill))
            gp += GetSkillGp(defId, skill);
        return Floor(gp * 1.5);
    }
}