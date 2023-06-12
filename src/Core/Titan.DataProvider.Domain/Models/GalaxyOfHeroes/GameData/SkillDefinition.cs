using System.Collections.Generic;

namespace Titan.DataProvider.Domain.Models.GalaxyOfHeroes.GameData;

public class SkillDefinition
{
    public string? Id { get; set; }
    public string? NameKey { get; set; }
    public string? IconKey { get; set; }
    public string? AbilityReference { get; set; }
    public SkillType SkillType { get; set; }
    public bool IsZeta { get; set; }
    public OmicronMode OmicronMode { get; set; }
    public List<SkillTierDefinition> Tier { get; set; } = new();
}
