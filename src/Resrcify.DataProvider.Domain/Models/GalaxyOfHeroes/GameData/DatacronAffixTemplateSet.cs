using System.Collections.Generic;

namespace Resrcify.DataProvider.Domain.Models.GalaxyOfHeroes.GameData;

public class DatacronAffixTemplateSet
{
    public string? Id { get; set; }
    public List<DatacronAffixTemplate> Affixs { get; set; } = [];

}
