using System.Collections.Generic;
using Titan.DataProvider.Domain.Models.GalaxyOfHeroes.Common;

namespace Titan.DataProvider.Domain.Models.GalaxyOfHeroes.GameData;

public class UnitDef
{
    public string? Id { get; set; }
    public string? UnitPrefab { get; set; }
    public string? ThumbnailName { get; set; }
    public string? NameKey { get; set; }
    public Rarity Rarity { get; set; }
    public Rarity MaxRarity { get; set; }
    public ForceAlignment ForceAlignment { get; set; }
    public string? XpTableId { get; set; }
    public int ActionCountMin { get; set; }
    public int ActionCountMax { get; set; }
    public CombatType CombatType { get; set; }
    public string? DescKey { get; set; }
    public ThreatLevel ThreatLevel { get; set; }
    public bool Obtainable { get; set; }
    public string? BaseId { get; set; }
    public string? PromotionRecipeReference { get; set; }
    public string? StatProgressionId { get; set; }
    public int TrainingXpWorthBaseValueOverride { get; set; }
    public int MaxLevelOverride { get; set; }
    public int TrainingCostMultiplierOverride { get; set; }
    public string? CreationRecipeReference { get; set; }
    public int BasePower { get; set; }
    public StatDef? BaseStat { get; set; }
    public string? PrimaryStat { get; set; }
    public Ability? BasicAttack { get; set; }
    public Ability? LeaderAbility { get; set; }
    public AbilityReference? BasicAttackRef { get; set; }
    public AbilityReference? LeaderAbilityRef { get; set; }
    public UnitStat PrimaryUnitStat { get; set; }
    public long ObtainableTime { get; set; }
    public int CommandCost { get; set; }
    public string? CrewContributionTableId { get; set; }
    public UnitClass UnitClass { get; set; }
    public string? BattlePortraitPrefab { get; set; }
    public string? BattlePortraitNameKey { get; set; }
    public RelicDefinition? RelicDefinition { get; set; }
    public string? CapitalUnlockKey { get; set; }
    public bool Legend { get; set; }
    public int SquadPositionPriority { get; set; }
    public bool Big { get; set; }
    public List<string> CategoryIds { get; set; } = new();
    public List<SkillDefinitionReference> SkillReferences { get; set; } = new();
    public List<UnitTierDef> UnitTiers { get; set; } = new();
    public List<Ability> LimitBreaks { get; set; } = new();
    public List<Ability> UniqueAbilities { get; set; } = new();
    public List<AbilityReference> LimitBreakRefs { get; set; } = new();
    public List<AbilityReference> UniqueAbilityRefs { get; set; } = new();
    public List<CrewMember> Crews { get; set; } = new();
    public List<UnitModRecommendation> ModRecommendations { get; set; } = new();
    public List<string> EffectIconPriorityOverrides { get; set; } = new();
    public List<SummonStatTable> SummonStatTables { get; set; } = new();
    public List<RecommendedSquad> ExampleSquads { get; set; } = new();
}
