using System.Collections.Generic;

namespace Titan.DataProvider.Application.Models.GalaxyOfHeroes.GameData
{
    public class Requirement
    {
        public EvaluationType EvalType { get; set; }
        public string? Id { get; set; }
        public string? DescKey { get; set; }
        public List<RequirementItem> RequirementItem { get; set; } = new();

    }
}
