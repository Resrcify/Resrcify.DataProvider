using System.Collections.Generic;
using Resrcify.DataProvider.Domain.Models.GalaxyOfHeroes.Common;

namespace Resrcify.DataProvider.Domain.Models.GalaxyOfHeroes.GameData;

public class UnitTierDef
{
    public UnitTier Tier { get; set; }
    public StatDef? BaseStat { get; set; }
    public List<string> EquipmentSets { get; set; } = [];
}
