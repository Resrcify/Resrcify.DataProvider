using System.Collections.Generic;
namespace Resrcify.DataProvider.Domain.Models.GalaxyOfHeroes.GameData;

public class RelicDefinition
{
    public string? UpgradeTableId { get; set; }
    public string? AlignmentColorOverride { get; set; }
    public string? Texture { get; set; }
    public string? NameKey { get; set; }
    public List<string> RelicTierDefinitionIds { get; set; } = [];
}
