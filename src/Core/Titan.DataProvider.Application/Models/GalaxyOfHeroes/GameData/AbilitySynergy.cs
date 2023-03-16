using System.Collections.Generic;

namespace Titan.DataProvider.Application.Models.GalaxyOfHeroes.GameData
{
    public class AbilitySynergy
    {
        public List<string> SeparateCategoryId { get; set; } = new();
        public List<string> GroupedCategoryId { get; set; } = new();
    }
}
