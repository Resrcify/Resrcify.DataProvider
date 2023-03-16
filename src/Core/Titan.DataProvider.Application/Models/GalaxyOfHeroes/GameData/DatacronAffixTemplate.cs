using System.Collections.Generic;

namespace Titan.DataProvider.Application.Models.GalaxyOfHeroes.GameData
{
    public class DatacronAffixTemplate
    {
        public string? AbilityId { get; set; }
        public string? TargetRule { get; set; }
        public UnitStat StatType { get; set; }
        public long StatValueMin { get; set; }
        public long StatValueMax { get; set; }
        public int MinTier { get; set; }
        public int MaxTier { get; set; }
        public string? ScopeIcon { get; set; }
        public List<string>? Tag { get; set; }
    }
}
