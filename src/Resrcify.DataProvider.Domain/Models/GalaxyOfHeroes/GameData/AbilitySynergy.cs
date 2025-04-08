using System.Collections.Generic;

namespace Resrcify.DataProvider.Domain.Models.GalaxyOfHeroes.GameData;

public class AbilitySynergy
{
    public List<string> SeparateCategoryIds { get; set; } = [];
    public List<string> GroupedCategoryIds { get; set; } = [];
}
