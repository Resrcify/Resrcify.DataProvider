using System.Collections.Generic;

namespace Titan.DataProvider.Domain.Models.GalaxyOfHeroes.GameData
{
    public class EffectReference
    {
        public string? Id { get; set; }
        public int MaxBonusMove { get; set; }
        public List<int> ContextIndex { get; set; } = new();
    }
}
