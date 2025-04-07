using System.Collections.Generic;

namespace Titan.DataProvider.Domain.Models.GalaxyOfHeroes.PlayerProfile;

public class DatacronBattleStat
{
    public string? Id { get; set; }
    public int SetId { get; set; }
    public string? TemplateId { get; set; }
    public List<string> Tags { get; set; } = [];
    public List<DatacronAffix> Affixs { get; set; } = [];
}
