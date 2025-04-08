using System.Collections.Generic;

namespace Resrcify.DataProvider.Domain.Models.GalaxyOfHeroes.GameData;

public class EffectReference
{
    public string? Id { get; set; }
    public int MaxBonusMove { get; set; }
    public List<int> ContextIndexs { get; set; } = [];
}
