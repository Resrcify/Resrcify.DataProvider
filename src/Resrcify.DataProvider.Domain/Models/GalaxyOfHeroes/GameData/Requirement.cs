using System.Collections.Generic;

namespace Resrcify.DataProvider.Domain.Models.GalaxyOfHeroes.GameData;

public class Requirement
{
    public EvaluationType EvalType { get; set; }
    public string? Id { get; set; }
    public string? DescKey { get; set; }
    public List<RequirementItem> RequirementItems { get; set; } = [];

}
