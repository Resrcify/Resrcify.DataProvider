namespace Resrcify.DataProvider.Domain.Models.GalaxyOfHeroes.GameData;

public class BattleCondition
{
    public ConditionType ConditionType { get; set; }
    public string? ConditionValue { get; set; }
    public int Priority { get; set; }
}
