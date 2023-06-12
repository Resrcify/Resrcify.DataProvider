using System.Collections.Generic;

namespace Titan.DataProvider.Domain.Models.GalaxyOfHeroes.GameData;

public class GameDataResponse
{
    public List<Ability> Ability { get; set; } = new();
    public List<EquipmentDef> Equipment { get; set; } = new();
    public List<RelicTierDefinition> RelicTierDefinition { get; set; } = new();
    public List<SkillDefinition> Skill { get; set; } = new();
    public List<StatModSetDefinition> StatModSet { get; set; } = new();
    public List<StatProgression> StatProgression { get; set; } = new();
    public List<Table> Table { get; set; } = new();
    public List<UnitDef> Units { get; set; } = new();
    public List<XpTable> XpTable { get; set; } = new();



    public List<Category> Category { get; set; } = new();
    public List<EffectTarget> BattleTargetingRule { get; set; } = new();
    public List<DatacronSet> DatacronSet { get; set; } = new();
    public List<DatacronTemplate> DatacronTemplate { get; set; } = new();
    public List<DatacronAffixTemplateSet> DatacronAffixTemplateSet { get; set; } = new();
}
