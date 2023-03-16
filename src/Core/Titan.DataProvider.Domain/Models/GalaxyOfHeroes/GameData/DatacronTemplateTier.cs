using System.Collections.Generic;

namespace Titan.DataProvider.Domain.Models.GalaxyOfHeroes.GameData
{
    public class DatacronTemplateTier
    {
        public int Id { get; set; }
        public UnitTier RequiredUnitTier { get; set; }
        public RelicTier RequiredRelicTier { get; set; }
        public List<string> AffixTemplateSetId { get; set; } = new();
        public List<string> InitialAffixTemplateSetIds { get; set; } = new();
    }
}
