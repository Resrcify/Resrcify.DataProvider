using System.Collections.Generic;

namespace Resrcify.DataProvider.Domain.Models.GalaxyOfHeroes.PlayerProfile;

public class StatMod
{
    public string? Id { get; set; }
    public string? DefinitionId { get; set; }
    public int Level { get; set; }
    public StatModTier Tier { get; set; }
    public StatModStat? PrimaryStat { get; set; }
    public int Xp { get; set; }
    public List<StatModStat> SecondaryStats { get; set; } = [];
    public int RerolledCount { get; set; }
}
