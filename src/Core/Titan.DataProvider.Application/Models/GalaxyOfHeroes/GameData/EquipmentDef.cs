using System.Collections.Generic;

namespace Titan.DataProvider.Application.Models.GalaxyOfHeroes.GameData
{
    public class EquipmentDef
    {
        public string? Id { get; set; }
        public string? NameKey { get; set; }
        public string? IconKey { get; set; }
        public int RequiredLevel { get; set; }
        public StatDef? EquipmentStat { get; set; }
        public string? RecipeId { get; set; }
        public UnitTier Tier { get; set; }
        public CurrencyItem? SellValue { get; set; }
        public string? Mark { get; set; }
        public long ObtainableTime { get; set; }
        public EquipmentType Type { get; set; }
        public Rarity RequiredRarity { get; set; }
        public bool FindFlowDisabled { get; set; }
        public List<LookupMission>? LookupMission { get; set; }
        public List<LookupMission>? RaidLookup { get; set; }
        public List<LookupActionLink>? ActionLinkLookup { get; set; }

    }
}
