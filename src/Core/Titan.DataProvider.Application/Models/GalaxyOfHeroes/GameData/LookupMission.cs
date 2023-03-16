using System.Collections.Generic;

namespace Titan.DataProvider.Application.Models.GalaxyOfHeroes.GameData
{
    public class LookupMission
    {
        public CampaignElementIdentifier? MissionIdentifier { get; set; }
        public List<string> RequirementId { get; set; } = new();
    }
}
