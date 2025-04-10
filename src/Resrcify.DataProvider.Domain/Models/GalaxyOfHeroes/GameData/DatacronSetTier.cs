using System.Collections.Generic;

namespace Resrcify.DataProvider.Domain.Models.GalaxyOfHeroes.GameData;

public class DatacronSetTier
{
    public int Id { get; set; }
    public string? UpgradeCostRecipeId { get; set; }
    public string? DustGrantRecipeId { get; set; }
    public DatacronScopeIdentifier ScopeIdentifier { get; set; }
    public List<string> RerollCostRecipeIds { get; set; } = [];

}
