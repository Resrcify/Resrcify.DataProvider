using System.Collections.Generic;

namespace Titan.DataProvider.Domain.Models.GalaxyOfHeroes.GameData;

public class AbilityAIParams
{
    public string? PreferredAllyTargetingRuleId { get; set; }
    public string? PreferredEnemyTargetingRuleId { get; set; }
    public bool RequireEnemyPreferredTargets { get; set; }
    public bool RequireAllyTargets { get; set; }
    public List<string> PreferredAllyTargetingRuleIdLists { get; set; } = [];
    public List<string> PreferredEnemyTargetingRuleIdLists { get; set; } = [];

}
