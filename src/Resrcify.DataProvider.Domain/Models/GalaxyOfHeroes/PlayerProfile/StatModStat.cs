using System.Collections.Generic;
using Resrcify.DataProvider.Domain.Models.GalaxyOfHeroes.Common;

namespace Resrcify.DataProvider.Domain.Models.GalaxyOfHeroes.PlayerProfile;

public class StatModStat
{
    public Stat? Stat { get; set; }
    public int StatRolls { get; set; }
    public long StatRollerBoundsMin { get; set; }
    public long StatRollerBoundsMax { get; set; }
    public List<string> Rolls { get; set; } = [];
    public List<long> UnscaledRollValues { get; set; } = [];
}
