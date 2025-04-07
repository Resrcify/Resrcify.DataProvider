using System.Collections.Generic;

namespace Titan.DataProvider.Domain.Models.GalaxyOfHeroes.GameData;

public class GameDataResponse
{
    public List<Ability> Abilities { get; set; } = [];
    public List<EquipmentDef> Equipments { get; set; } = [];
    public List<RelicTierDefinition> RelicTierDefinitions { get; set; } = [];
    public List<SkillDefinition> Skills { get; set; } = [];
    public List<StatModSetDefinition> StatModSets { get; set; } = [];
    public List<StatProgression> StatProgressions { get; set; } = [];
    public List<Table> Tables { get; set; } = [];
    public List<UnitDef> Units { get; set; } = [];
    public List<XpTable> XpTables { get; set; } = [];



    public List<Category> Categories { get; set; } = [];
    public List<EffectTarget> BattleTargetingRules { get; set; } = [];
    public List<DatacronSet> DatacronSets { get; set; } = [];
    public List<DatacronTemplate> DatacronTemplates { get; set; } = [];
    public List<DatacronAffixTemplateSet> DatacronAffixTemplateSets { get; set; } = [];
}
