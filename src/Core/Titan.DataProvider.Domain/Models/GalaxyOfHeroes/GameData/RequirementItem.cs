namespace Titan.DataProvider.Domain.Models.GalaxyOfHeroes.GameData
{
    public class RequirementItem
    {
        public RequirementType Type { get; set; }
        public string? Id { get; set; }
        public int? Value { get; set; }
        public string? LocKey { get; set; }
        public bool Negate { get; set; }

    }
}
