using System.Collections.Generic;

namespace Titan.DataProvider.Domain.Models.GalaxyOfHeroes.GameData;

public class AbilitySynergy
{
    public List<string> SeparateCategoryIds { get; set; } = new();
    public List<string> GroupedCategoryIds { get; set; } = new();
}
