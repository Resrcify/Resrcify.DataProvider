using Titan.DataProvider.Domain.Models.GalaxyOfHeroes.Common;

namespace Titan.DataProvider.Domain.Models.GalaxyOfHeroes.GameData
{
    public class SkillTierDefinition
    {
        public string? RecipeId { get; set; }
        public int RequiredUnitLevel { get; set; }
        public Rarity RequiredUnitRarity { get; set; }
        public UnitTier RequiredUnitTier { get; set; }
        public string? PowerOverrideTag { get; set; }
        public RelicTier RequiredUnitRelicTier { get; set; }
        public bool IsZetaTier { get; set; }
        public bool IsOmicronTier { get; set; }
    }
}
