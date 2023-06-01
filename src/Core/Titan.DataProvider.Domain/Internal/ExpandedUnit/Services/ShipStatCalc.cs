using System.Collections.Generic;
using Titan.DataProvider.Domain.Models.GalaxyOfHeroes.PlayerProfile;
using GameData = Titan.DataProvider.Domain.Internal.BaseData.BaseData;
using Titan.DataProvider.Domain.Internal.ExpandedUnit.ValueObjects;
using System.Linq;
using Titan.DataProvider.Domain.Shared;

namespace Titan.DataProvider.Domain.Internal.ExpandedUnit.Services
{
    public class ShipStatCalc : StatCalcBase, IStatCalc
    {
        public IReadOnlyDictionary<long, double> Base => _base;
        public IReadOnlyDictionary<long, double> Gear => _gear;
        public IReadOnlyDictionary<long, double> Mods => _mods;
        public IReadOnlyDictionary<long, double> Crew => _crew;
        private readonly Dictionary<string, Unit> _crewUnits;
        private ShipStatCalc(
            Unit unit,
            GameData gameData,
            Dictionary<string, Unit> crewUnits) : base(unit, gameData)
        {
            _crewUnits = crewUnits;
            CalculateRawStats();
            CalculateBaseStats();
            FormatStats();
        }
        public static Result<IStatCalc> Create(Unit unit, GameData gameData, Dictionary<string, Unit> crew)
        {
            return new ShipStatCalc(unit, gameData, crew);
        }
        private void CalculateRawStats()
        {
            var tierEnumValue = (int)_unit.CurrentTier;
            var rarityEnumValue = (int)_unit.CurrentRarity;
            var relicEnumValue = _unit.Relic?.CurrentTier ?? 0;
            var definitionId = _unit.DefinitionId!.Split(":")[0];

            var stats = _gameData.Units[definitionId].Stats;
            foreach (var stat in stats)
                _base.Add(stat.Key, stat.Value);

            foreach (var stat in _gameData.Units[definitionId].GrowthModifiers[rarityEnumValue.ToString()])
                _growthModifiers.Add(stat.Key, stat.Value);

            var crewRating = _crewUnits.Count == 0 ? GetCrewlessCrewRating(rarityEnumValue) : 1; //GetCrewRating();
            var statMultiplier = _gameData.CrTable.ShipRarityFactor[rarityEnumValue.ToString()] * crewRating;
            foreach (var (statId, statValue) in _gameData.Units[definitionId].CrewStats)
            {
                var longStatId = long.Parse(statId);
                _crew[longStatId] = Floor(statValue * statMultiplier, (longStatId < 16 || longStatId == 28) ? 8 : 0);
            }
        }

        //   // temporarily uses hard-coded multipliers, as the true in-game formula remains a mystery.
        //   // but these values have experimentally been found accurate for the first 3 crewless ships:
        //   //     (Vulture Droid, Hyena Bomber, and BTL-B Y-wing)
        private double GetCrewlessCrewRating(int rarity)
        {
            return Floor(
                _gameData.CrTable.CrewRarityCr[rarity.ToString()] +
                3.5 * _gameData.CrTable.UnitLevelCr[_unit.CurrentLevel.ToString()] +
                GetCrewlessSkillsCrewRating()
            );
        }

        private double GetCrewlessSkillsCrewRating()
        {
            return _unit.Skill
                .Sum(x =>
                    x.Id![..8] == "hardware" ? 696 : 2.46 * _gameData.CrTable.AbilityLevelCr[x.Tier.ToString()]
                );
        }

        // private static double GetCrewRating(List<Unit> crew)
        // {

        // }

        // function getShipRawStats(ship, crew) {
        //   // ensure crew is the correct crew
        //   if (crew.length != unitData[ship.defId].crew.length)
        //     throw new Error(`Incorrect number of crew members for ship ${ship.defId}.`);
        //   crew.forEach((char) => {
        //     if (!unitData[ship.defId].crew.includes(char.defId))
        //       throw new Error(`Unit ${char.defId} is not in ${ship.defId}'s crew.`);
        //   });
        //   // if still here, crew is good -- go ahead and determine stats
        //   const crewRating =
        //     crew.length == 0 ? getCrewlessCrewRating(ship) : getCrewRating(crew);
        //   const stats = {
        //     base: Object.assign({}, unitData[ship.defId].stats),
        //     crew: {},
        //     growthModifiers: Object.assign(
        //       {},
        //       unitData[ship.defId].growthModifiers[ship.rarity]
        //     ),
        //   };
        //   let statMultiplier = crTables.shipRarityFactor[ship.rarity] * crewRating;
        //   Object.entries(unitData[ship.defId].crewStats).forEach(
        //     ([statID, statValue]) => {
        //       // stats 1-15 and 28 all have final integer values
        //       // other stats require decimals -- shrink to 8 digits of precision through 'unscaled' values this calculator uses
        //       stats.crew[statID] = floor(
        //         statValue * statMultiplier,
        //         statID < 16 || statID == 28 ? 8 : 0
        //       );
        //     }
        //   );
        //   return stats;
        // }

        // function getCrewRating(crew) {
        //   let totalCR = crew.reduce((crewRating, char) => {
        //     crewRating +=
        //       crTables.unitLevelCR[char.level] + crTables.crewRarityCR[char.rarity]; // add CR from level/rarity
        //     crewRating += crTables.gearLevelCR[char.gear]; // add CR from complete gear levels
        //     crewRating +=
        //       crTables.gearPieceCR[char.gear] * char.equipments?.length || 0; // add CR from currently equipped gear
        //     crewRating = char?.skill?.reduce(
        //       (cr, skillRef) => cr + getSkillCrewRating(skillRef),
        //       crewRating
        //     ); // add CR from ability levels
        //     // add CR from mods
        //     if (char.mods)
        //       crewRating = char.mods.reduce(
        //         (cr, mod) => cr + crTables.modRarityLevelCR[mod.pips][mod.level],
        //         crewRating
        //       );
        //     else if (char.equippedStatMod)
        //       crewRating = char.equippedStatMod.reduce(
        //         (cr, mod) =>
        //           cr + crTables.modRarityLevelCR[+mod.definitionId[1]][mod.level],
        //         crewRating
        //       );

        //     // add CR from relics
        //     if (char.relic && char.relic.currentTier > 2) {
        //       crewRating += crTables.relicTierCR[char.relic.currentTier];
        //       crewRating +=
        //         char.level * crTables.relicTierLevelFactor[char.relic.currentTier];
        //     }

        //     return crewRating;
        //   }, 0);
        //   return totalCR;
        // }
        // function getSkillCrewRating(skill) {
        //   // Crew Rating for GP purposes depends on skill type (i.e. contract/hardware/etc.), but for stats it apparently doesn't.
        //   return crTables.abilityLevelCR[skill.tier];
        // }

        // function getCrewlessCrewRating(ship) {
        //   // temporarily uses hard-coded multipliers, as the true in-game formula remains a mystery.
        //   // but these values have experimentally been found accurate for the first 3 crewless ships:
        //   //     (Vulture Droid, Hyena Bomber, and BTL-B Y-wing)
        //   return (cr = floor(
        //     crTables.crewRarityCR[ship.rarity] +
        //       3.5 * crTables.unitLevelCR[ship.level] +
        //       getCrewlessSkillsCrewRating(ship.skills)
        //   ));
        // }
        // function getCrewlessSkillsCrewRating(skill) {
        //   return skill?.reduce((cr, skillRef) => {
        //     cr +=
        //       (skillRef?.id?.substring(0, 8) == "hardware" ? 0.696 : 2.46) *
        //       crTables.abilityLevelCR[skillRef.tier];
        //     return cr;
        //   }, 0);
        // }
    }
}