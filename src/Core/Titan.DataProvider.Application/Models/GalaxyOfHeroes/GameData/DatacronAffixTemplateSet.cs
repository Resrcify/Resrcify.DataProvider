using System.Collections.Generic;

namespace Titan.DataProvider.Application.Models.GalaxyOfHeroes.GameData
{
    public class DatacronAffixTemplateSet
    {
        public string? Id { get; set; }
        public List<DatacronAffixTemplate> Affix { get; set; } = new();

    }
}
