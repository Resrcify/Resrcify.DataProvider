using System.Collections.Generic;

namespace Titan.DataProvider.Application.Models.GalaxyOfHeroes.GameData
{
    public class CrewMember
    {
        public string? UnitId { get; set; }
        public int Slot { get; set; }
        public string? SkilllessCrewAbilityId { get; set; }
        public List<SkillDefinitionReference> SkillReference { get; set; } = new();
    }
}
