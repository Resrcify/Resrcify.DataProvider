using System.Collections.Generic;
using Titan.DataProvider.Domain.Models.GalaxyOfHeroes.Common;

namespace Titan.DataProvider.Domain.Models.GalaxyOfHeroes.PlayerProfile;

public class DatacronAffix
{
    public string? TargetRule { get; set; }
    public string? AbilityId { get; set; }
    public UnitStat StatType { get; set; }
    public long StatValue { get; set; }
    public UnitTier RequiredUnitTier { get; set; }
    public RelicTier RequiredRelicTier { get; set; }
    public string? ScopeIcon { get; set; }
    public List<string> Tags { get; set; } = [];
}
