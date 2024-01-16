using System.Collections.Generic;

namespace Titan.DataProvider.Domain.Models.GalaxyOfHeroes.GameData;

public class DatacronSet
{
    public int Id { get; set; }
    public string? DisplayName { get; set; }
    public long ExpirationTimeMs { get; set; }
    public string? Icon { get; set; }
    public string? DetailPrefab { get; set; }
    public List<DatacronSetTier> Tiers { get; set; } = new();
    public List<DatacronSetMaterial> SetMaterials { get; set; } = new();
}
