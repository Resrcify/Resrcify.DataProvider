using System.Collections.Generic;

namespace Titan.DataProvider.Domain.Models.GalaxyOfHeroes.GameData
{
    public class EffectTarget
    {
        public EffectTargetUnitSelect UnitSelect { get; set; }
        public EffectTargetBattleSide BattleSide { get; set; }
        public EffectTargetCategoryCriteria? Category { get; set; }
        public UnitHealthState HealthState { get; set; }
        public BattleDeploymentState BattleDeploymentState { get; set; }
        public string? Id { get; set; }
        public bool ExcludeSelf { get; set; }
        public bool ExcludeSelectedTarget { get; set; }
        public List<UnitClass> UnitClass { get; set; } = new();
        public List<ForceAlignment> ForceAlignment { get; set; } = new();
        public List<StatValueRange> StatValue { get; set; } = new();
        public List<EffectTagCriteria> ActiveEffectTagCriteria { get; set; } = new();
    }
}
