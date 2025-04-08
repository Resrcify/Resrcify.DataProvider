using System.Collections.Generic;

namespace Resrcify.DataProvider.Domain.Models.GalaxyOfHeroes.GameData;

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
    public List<BattleCondition>? TriggerConditions { get; set; }
    public List<AbilityTier> Tiers { get; set; } = [];
    public List<EffectTag> DescriptiveTags { get; set; } = [];
    public List<EffectReference> EffectReferences { get; set; } = [];
    public List<EffectTag> InteractsWithTags { get; set; } = [];
    public List<int> UltimateChargeRequireds { get; set; } = [];
}
