using System.Collections.Generic;

namespace Titan.DataProvider.Domain.Models.GalaxyOfHeroes.GameData
{
    public class StatModSetBonus
    {
        public Stat? Stat { get; set; }
        public List<string> AbilityId { get; set; } = new();

    }
}
