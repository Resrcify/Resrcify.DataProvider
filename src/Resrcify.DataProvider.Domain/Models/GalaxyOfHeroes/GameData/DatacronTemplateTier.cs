using System.Collections.Generic;
using Resrcify.DataProvider.Domain.Models.GalaxyOfHeroes.Common;

namespace Resrcify.DataProvider.Domain.Models.GalaxyOfHeroes.GameData;

public class DatacronTemplateTier
{
    public int Id { get; set; }
    public UnitTier RequiredUnitTier { get; set; }
    public RelicTier RequiredRelicTier { get; set; }
    public List<string> AffixTemplateSetIds { get; set; } = [];
    public List<string> InitialAffixTemplateSetIds { get; set; } = [];
}
