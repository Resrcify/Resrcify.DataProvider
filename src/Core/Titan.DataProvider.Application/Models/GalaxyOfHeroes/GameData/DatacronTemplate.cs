using System.Collections.Generic;

namespace Titan.DataProvider.Application.Models.GalaxyOfHeroes.GameData
{
    public class DatacronTemplate
    {
        public string? Id { get; set; }
        public int SetId { get; set; }
        public int InitialTiers { get; set; }
        public string? ReferenceTemplateId { get; set; }
        public int MaxRerolls { get; set; }
        public bool AllowReroll { get; set; }
        public List<string> FixedTag { get; set; } = new();
        public List<DatacronTemplateTier> Tier { get; set; } = new();

    }
}
