using System.Collections.Generic;

namespace Titan.DataProvider.Domain.Models.GalaxyOfHeroes.PlayerProfile;

public class Squad
{
    public SquadType SquadType { get; set; }
    public DatacronBattleStat? Datacron { get; set; }
    public List<SquadCell> Cell { get; set; } = new();
}
