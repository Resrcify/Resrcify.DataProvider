using Resrcify.DataProvider.Domain.Models.GalaxyOfHeroes.Common;

namespace Resrcify.DataProvider.Domain.Models.GalaxyOfHeroes.GameData;

public class RelicTierDefinition
{
    public string? Id { get; set; }
    public StatDef? Stat { get; set; }
    public string? RelicStatTable { get; set; }
    public RelicTier Tier { get; set; }
}
