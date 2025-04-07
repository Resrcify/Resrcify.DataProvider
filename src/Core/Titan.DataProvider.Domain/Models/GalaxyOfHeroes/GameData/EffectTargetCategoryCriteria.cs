using System.Collections.Generic;

namespace Titan.DataProvider.Domain.Models.GalaxyOfHeroes.GameData;

public class EffectTargetCategoryCriteria
{
    public bool Exclude { get; set; }
    public List<string> CategoryIds { get; set; } = [];
    public List<EffectTargetCategory> Categories { get; set; } = [];
}
