namespace Titan.DataProvider.Domain.Models.GalaxyOfHeroes.GameData;

public class StatModSetDefinition
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? Icon { get; set; }
    public StatModSetBonus? CompleteBonus { get; set; }
    public StatModSetBonus? MaxLevelBonus { get; set; }
    public int SetCount { get; set; }
    public StatModSetBonus? OverclockBonus { get; set; }
}