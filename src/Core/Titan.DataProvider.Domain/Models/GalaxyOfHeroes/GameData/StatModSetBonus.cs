using System.Collections.Generic;
using Titan.DataProvider.Domain.Models.GalaxyOfHeroes.Common;

namespace Titan.DataProvider.Domain.Models.GalaxyOfHeroes.GameData;

public class StatModSetBonus
{
    public Stat? Stat { get; set; }
    public List<string> AbilityIds { get; set; } = new();

}
