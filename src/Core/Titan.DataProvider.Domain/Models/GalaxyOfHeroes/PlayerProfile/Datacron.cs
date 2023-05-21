using System.Collections.Generic;

namespace Titan.DataProvider.Domain.Models.GalaxyOfHeroes.PlayerProfile
{
    public class Datacron
    {
        public string? Id { get; set; }
        public int SetId { get; set; }
        public string? TemplateId { get; set; }
        public bool Locked { get; set; }
        public int RerollIndex { get; set; }
        public int RerollCount { get; set; }
        public List<string> Tag { get; set; } = new();
        public List<DatacronAffix> Affix { get; set; } = new();
        public List<DatacronAffix> RerollOption { get; set; } = new();
    }
}
