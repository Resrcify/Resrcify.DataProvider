using Titan.DataProvider.Domain.Models.GalaxyOfHeroes.Common;

namespace Titan.DataProvider.Domain.Models.GalaxyOfHeroes.GameData;

public class AbilityReference
{
    public string? AbilityId { get; set; }
    public UnitTier RequiredTier { get; set; }
    public Rarity RequiredRarity { get; set; }
    public int RequiredSkillTier { get; set; }
    public RelicTier RequiredRelicTier { get; set; }
    public bool OverrideAlwaysDisplayInBattleUi { get; set; }
    public string? OverrideIcon { get; set; }
    public string? OverrideNameKey { get; set; }
    public string? OverrideDescKey { get; set; }
    public string? PowerAdditiveTag { get; set; }
    public string? UnlockRecipeId { get; set; }
    public string? InheritMappingId { get; set; }
}
