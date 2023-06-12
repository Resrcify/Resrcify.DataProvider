using System.Collections.Generic;

namespace Titan.DataProvider.Domain.Models.GalaxyOfHeroes.GameData;

public class Ability
{
    public string? Id { get; set; }
    public string? NameKey { get; set; }
    public string? DescKey { get; set; }
    public string? PrefabName { get; set; }
    public string? StackingLineOverride { get; set; }
    public int Cooldown { get; set; }
    public string? Icon { get; set; }
    public string? ApplyTypeTooltipKey { get; set; }
    public MessageDialog? ConfirmationMessage { get; set; }
    public AbilityButtonLocationType ButtonLocation { get; set; }
    public string? ShortDescKey { get; set; }
    public AbilityType AbilityType { get; set; }
    public UnitDetailsAbilityLocation DetailLocation { get; set; }
    public string? AllyTargetingRuleId { get; set; }
    public bool UseAsReinforcementDesc { get; set; }
    public string? SubIcon { get; set; }
    public AbilityAIParams? AiParams { get; set; }
    public AbilityCooldownType CooldownType { get; set; }
    public bool AlwaysDisplayInBattleUi { get; set; }
    public bool HighlightWhenReadyInBattleUi { get; set; }
    public bool HideCooldownDescription { get; set; }
    public string? BlockingEffectId { get; set; }
    public string? BlockedLocKey { get; set; }
    public int GrantedPriority { get; set; }
    public AbilitySynergy? Synergy { get; set; }
    public EffectTarget? VisualTarget { get; set; }
    public List<BattleCondition>? TriggerCondition { get; set; }
    public List<AbilityTier> Tier { get; set; } = new();
    public List<EffectTag> DescriptiveTag { get; set; } = new();
    public List<EffectReference> EffectReference { get; set; } = new();
    public List<EffectTag> InteractsWithTag { get; set; } = new();
    public List<int> UltimateChargeRequired { get; set; } = new();
}
