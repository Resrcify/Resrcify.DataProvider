using System.Collections.Generic;
using Titan.DataProvider.Domain.Models.GalaxyOfHeroes.Common;

namespace Titan.DataProvider.Domain.Models.GalaxyOfHeroes.PlayerProfile;

public class Unit
{
    public string? Id { get; set; }
    public string? DefinitionId { get; set; }
    public Rarity CurrentRarity { get; set; }
    public int CurrentLevel { get; set; }
    public int CurrentXp { get; set; }
    public StatDef? UnitStat { get; set; }
    public UnitTier CurrentTier { get; set; }
    public Relic? Relic { get; set; }
    public List<Skill> Skills { get; set; } = new();
    public List<EquipmentSlot> Equipments { get; set; } = new();
    public List<StatMod> EquippedStatMods { get; set; } = new();
    public List<string> PurchasedAbilityIds { get; set; } = new();
}
