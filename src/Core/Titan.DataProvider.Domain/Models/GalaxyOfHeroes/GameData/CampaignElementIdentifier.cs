namespace Titan.DataProvider.Domain.Models.GalaxyOfHeroes.GameData
{
    public class CampaignElementIdentifier
    {
        public string? CampaignId { get; set; }
        public string? CampaignMapId { get; set; }
        public string? CampaignNodeId { get; set; }
        public CampaignNodeDifficulty CampaignNodeDifficulty { get; set; }
        public string? CampaignMissionId { get; set; }
    }
}
