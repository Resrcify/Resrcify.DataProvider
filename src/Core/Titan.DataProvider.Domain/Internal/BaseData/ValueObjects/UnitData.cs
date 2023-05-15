using System;
using System.Collections.Generic;
using System.Linq;
using Titan.DataProvider.Domain.Primitives;
using Titan.DataProvider.Domain.Shared;

namespace Titan.DataProvider.Domain.Internal.BaseData.ValueObjects
{
    public sealed class UnitData : ValueObject
    {
        public string Id { get; private set; }
        public string NameKey { get; private set; }
        public string Name { get; private set; }
        public long CombatType { get; private set; }
        public long ForceAlignment { get; private set; }
        public IReadOnlyList<string> CategoryIdList => _categoryIdList;
        private readonly List<string> _categoryIdList = new();
        public long UnitClass { get; private set; }
        public string Image { get; private set; }
        public long PrimaryStat { get; private set; }
        public IReadOnlyDictionary<string, GearLevel> GearLevels => _gearLevels;
        private readonly Dictionary<string, GearLevel> _gearLevels = new();
        public IReadOnlyDictionary<string, Dictionary<string, long>> GrowthModifiers => _growthModifiers;
        private readonly Dictionary<string, Dictionary<string, long>> _growthModifiers = new();
        public IReadOnlyList<Skill> Skills => _skills;
        private readonly List<Skill> _skills = new();
        public IReadOnlyDictionary<string, string> Relics => _relics;
        private readonly Dictionary<string, string> _relics = new();
        public string MasteryModifierId { get; private set; }
        public IReadOnlyList<ModRecommendation> ModRecommendations => _modRecommendations;
        private readonly List<ModRecommendation> _modRecommendations = new();
        public IReadOnlyDictionary<long, long> Stats => _stats;
        private readonly Dictionary<long, long> _stats = new();
        public IReadOnlyDictionary<string, long> CrewStats => _crewStats;
        private readonly Dictionary<string, long> _crewStats = new();
        public IReadOnlyList<string> Crew => _crew;
        private readonly List<string> _crew = new();

        private UnitData(
            string id,
            string nameKey,
            string name,
            long combatType,
            long forceAlignment,
            List<string> categoryIdList,
            long unitClass,
            string image,
            long primaryStat,
            Dictionary<string, GearLevel> gearLevels,
            Dictionary<string, Dictionary<string, long>> growthModifiers,
            List<Skill> skills,
            Dictionary<string, string> relics,
            string masteryModifierId,
            List<ModRecommendation> modRecommendations,
            Dictionary<long, long> stats,
            Dictionary<string, long> crewStats,
            List<string> crew)
        {
            Id = id;
            NameKey = nameKey;
            Name = name;
            CombatType = combatType;
            ForceAlignment = forceAlignment;
            _categoryIdList = categoryIdList;
            UnitClass = unitClass;
            Image = image;
            PrimaryStat = primaryStat;
            _gearLevels = gearLevels;
            _growthModifiers = growthModifiers;
            _skills = skills;
            _relics = relics;
            MasteryModifierId = masteryModifierId;
            _modRecommendations = modRecommendations;
            _stats = stats;
            _crewStats = crewStats;
            _crew = crew;
        }
        public static Result<UnitData> Create(
            string id,
            string nameKey,
            string name,
            long combatType,
            long forceAlignment,
            List<string> categoryIdList,
            long unitClass,
            string image,
            long primaryStat,
            Dictionary<string, GearLevel> gearLevels,
            Dictionary<string, Dictionary<string, long>> growthModifiers,
            List<Skill> skills,
            Dictionary<string, string> relics,
            string masteryModifierId,
            List<ModRecommendation> modRecommendations)
        {
            Dictionary<long, long> stats = new();
            Dictionary<string, long> crewStats = new();
            List<string> crew = new();
            return new UnitData(
                id,
                nameKey,
                name,
                combatType,
                forceAlignment,
                categoryIdList,
                unitClass,
                image,
                primaryStat,
                gearLevels,
                growthModifiers,
                skills,
                relics,
                masteryModifierId,
                modRecommendations,
                stats,
                crewStats,
                crew
                );
        }

        public static Result<UnitData> Create(
            string id,
            string nameKey,
            string name,
            long combatType,
            long forceAlignment,
            List<string> categoryIdList,
            long unitClass,
            string image,
            long primaryStat,
            Dictionary<string, Dictionary<string, long>> growthModifiers,
            List<Skill> skills,
            string masteryModifierId,
            Dictionary<long, long> stats,
            Dictionary<string, long> crewStats,
            List<string> crew)
        {
            Dictionary<string, GearLevel> gearLevels = new();
            Dictionary<string, string> relics = new();
            List<ModRecommendation> modRecommendations = new();
            return new UnitData(
                id,
                nameKey,
                name,
                combatType,
                forceAlignment,
                categoryIdList,
                unitClass,
                image,
                primaryStat,
                gearLevels,
                growthModifiers,
                skills,
                relics,
                masteryModifierId,
                modRecommendations,
                stats,
                crewStats,
                crew
            );
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Id;
            yield return NameKey;
            yield return Name;
            yield return CombatType;
            yield return ForceAlignment;
            yield return CategoryIdList;
            yield return UnitClass;
            yield return Image;
            yield return PrimaryStat;
            yield return GearLevels!;
            yield return GrowthModifiers;
            yield return Skills;
            yield return Stats;
            yield return CrewStats;
            yield return Crew;
            yield return Relics;
            yield return MasteryModifierId;
            yield return ModRecommendations;
        }
    }
}