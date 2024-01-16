using System.Collections.Generic;

namespace Titan.DataProvider.Domain.Models.GalaxyOfHeroes.GameData;

public class AbilityTier
{
    public string? DescKey { get; set; }
    public string? UpgradeDescKey { get; set; }
    public int CooldownMaxOverride { get; set; }
    public string? BlockingEffectId { get; set; }
    public string? BlockedLocKey { get; set; }
    public List<EffectReference> EffectReferences { get; set; } = new();
    public List<EffectTag> InteractsWithTags { get; set; } = new();
}
