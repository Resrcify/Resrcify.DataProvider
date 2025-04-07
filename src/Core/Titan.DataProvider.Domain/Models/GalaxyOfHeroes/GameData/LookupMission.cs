using System.Collections.Generic;

namespace Titan.DataProvider.Domain.Models.GalaxyOfHeroes.GameData;

public class LookupMission
{
    public CampaignElementIdentifier? MissionIdentifier { get; set; }
    public List<string> RequirementIds { get; set; } = [];
}
