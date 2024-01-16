using System.Collections.Generic;

namespace Titan.DataProvider.Domain.Models.GalaxyOfHeroes.GameData;

public class GameDataResponse
{
    public List<Ability> Abilities { get; set; } = new();
    public List<EquipmentDef> Equipments { get; set; } = new();
    public List<RelicTierDefinition> RelicTierDefinitions { get; set; } = new();
    public List<SkillDefinition> Skills { get; set; } = new();
    public List<StatModSetDefinition> StatModSets { get; set; } = new();
    public List<StatProgression> StatProgressions { get; set; } = new();
    public List<Table> Tables { get; set; } = new();
    public List<UnitDef> Units { get; set; } = new();
    public List<XpTable> XpTables { get; set; } = new();



    public List<Category> Categories { get; set; } = new();
    public List<EffectTarget> BattleTargetingRules { get; set; } = new();
    public List<DatacronSet> DatacronSets { get; set; } = new();
    public List<DatacronTemplate> DatacronTemplates { get; set; } = new();
    public List<DatacronAffixTemplateSet> DatacronAffixTemplateSets { get; set; } = new();
}
