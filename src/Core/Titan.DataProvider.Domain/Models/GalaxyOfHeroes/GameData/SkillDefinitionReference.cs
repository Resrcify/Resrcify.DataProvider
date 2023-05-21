using Titan.DataProvider.Domain.Models.GalaxyOfHeroes.Common;

namespace Titan.DataProvider.Domain.Models.GalaxyOfHeroes.GameData
{
    public class SkillDefinitionReference
    {
        public string? SkillId { get; set; }
        public UnitTier RequiredTier { get; set; }
        public Rarity RequiredRarity { get; set; }
        public RelicTier RequiredRelicTier { get; set; }

    }
}
