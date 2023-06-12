using System.Collections.Generic;

namespace Titan.DataProvider.Domain.Models.GalaxyOfHeroes.GameData;

public class EffectTargetCategoryCriteria
{
    public bool Exclude { get; set; }
    public List<string> CategoryId { get; set; } = new();
    public List<EffectTargetCategory> Category { get; set; } = new();
}
