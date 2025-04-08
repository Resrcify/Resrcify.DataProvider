using System.Collections.Generic;

namespace Resrcify.DataProvider.Domain.Models.GalaxyOfHeroes.PlayerProfile;

public class Squad
{
    public SquadType SquadType { get; set; }
    public DatacronBattleStat? Datacron { get; set; }
    public List<SquadCell> Cells { get; set; } = [];
}
