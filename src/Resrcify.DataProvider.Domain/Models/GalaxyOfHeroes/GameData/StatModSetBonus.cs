using System.Collections.Generic;
using Resrcify.DataProvider.Domain.Models.GalaxyOfHeroes.Common;

namespace Resrcify.DataProvider.Domain.Models.GalaxyOfHeroes.GameData;

public class StatModSetBonus
{
    public Stat? Stat { get; set; }
    public List<string> AbilityIds { get; set; } = [];

}
