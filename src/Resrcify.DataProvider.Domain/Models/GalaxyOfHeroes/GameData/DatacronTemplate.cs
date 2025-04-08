using System.Collections.Generic;

namespace Resrcify.DataProvider.Domain.Models.GalaxyOfHeroes.GameData;

public class DatacronTemplate
{
    public string? Id { get; set; }
    public int SetId { get; set; }
    public int InitialTiers { get; set; }
    public string? ReferenceTemplateId { get; set; }
    public int MaxRerolls { get; set; }
    public bool AllowReroll { get; set; }
    public List<string> FixedTags { get; set; } = [];
    public List<DatacronTemplateTier> Tiers { get; set; } = [];

}
