using System.Collections.Generic;

namespace Titan.DataProvider.Application.Models.GalaxyOfHeroes.GameData
{
    public class Category
    {
        public string? Id { get; set; }
        public string? DescKey { get; set; }
        public bool Visible { get; set; }
        public List<CombatType> UiFilter { get; set; } = new();
    }
}
