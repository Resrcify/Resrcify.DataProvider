using System.Collections.Generic;

namespace Titan.DataProvider.Domain.Models.GalaxyOfHeroes.GameData;

public class AbilitySynergy
{
    public List<string> SeparateCategoryId { get; set; } = new();
    public List<string> GroupedCategoryId { get; set; } = new();
}
