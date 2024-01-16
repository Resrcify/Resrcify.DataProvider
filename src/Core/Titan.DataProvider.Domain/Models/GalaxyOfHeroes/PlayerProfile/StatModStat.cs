using System.Collections.Generic;
using Titan.DataProvider.Domain.Models.GalaxyOfHeroes.Common;

namespace Titan.DataProvider.Domain.Models.GalaxyOfHeroes.PlayerProfile;

public class StatModStat
{
    public Stat? Stat { get; set; }
    public int StatRolls { get; set; }
    public long StatRollerBoundsMin { get; set; }
    public long StatRollerBoundsMax { get; set; }
    public List<string> Rolls { get; set; } = new();
    public List<long> UnscaledRollValues { get; set; } = new();
}
