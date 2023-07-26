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
    public List<Skill> Skill { get; set; } = new();
    public List<EquipmentSlot> Equipment { get; set; } = new();
    public List<StatMod> EquippedStatMod { get; set; } = new();
    public List<string> PurchasedAbilityId { get; set; } = new();
}
