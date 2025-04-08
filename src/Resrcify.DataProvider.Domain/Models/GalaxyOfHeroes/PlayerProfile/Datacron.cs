using System.Collections.Generic;

namespace Resrcify.DataProvider.Domain.Models.GalaxyOfHeroes.PlayerProfile;

public class Datacron
{
    public string? Id { get; set; }
    public int SetId { get; set; }
    public string? TemplateId { get; set; }
    public bool Locked { get; set; }
    public int RerollIndex { get; set; }
    public int RerollCount { get; set; }
    public List<string> Tags { get; set; } = [];
    public List<DatacronAffix> Affixs { get; set; } = [];
    public List<DatacronAffix> RerollOptions { get; set; } = [];
}
