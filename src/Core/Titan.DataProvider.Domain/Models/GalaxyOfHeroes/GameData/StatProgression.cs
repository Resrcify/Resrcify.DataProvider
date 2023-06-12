using Titan.DataProvider.Domain.Models.GalaxyOfHeroes.Common;

namespace Titan.DataProvider.Domain.Models.GalaxyOfHeroes.GameData;

public class StatProgression
{
    public string? Id { get; set; }
    public StatDef? Stat { get; set; }
    public OperationType Operation { get; set; }

}
