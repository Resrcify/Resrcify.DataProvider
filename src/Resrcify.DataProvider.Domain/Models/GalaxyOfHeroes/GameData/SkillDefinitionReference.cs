using Resrcify.DataProvider.Domain.Models.GalaxyOfHeroes.Common;

namespace Resrcify.DataProvider.Domain.Models.GalaxyOfHeroes.GameData;

public class SkillDefinitionReference
{
    public string? SkillId { get; set; }
    public UnitTier RequiredTier { get; set; }
    public Rarity RequiredRarity { get; set; }
    public RelicTier RequiredRelicTier { get; set; }

}
