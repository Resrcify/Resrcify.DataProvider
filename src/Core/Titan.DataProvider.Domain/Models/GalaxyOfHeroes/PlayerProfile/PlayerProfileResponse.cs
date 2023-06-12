using System.Collections.Generic;

namespace Titan.DataProvider.Domain.Models.GalaxyOfHeroes.PlayerProfile;

public class PlayerProfileResponse
{
    public string? Name { get; set; }
    public long AllyCode { get; set; }
    public string? PlayerId { get; set; }
    public List<Unit> RosterUnit { get; set; } = new();
    public List<PlayerPvpProfile> PvpProfile { get; set; } = new();
    public List<Datacron> Datacron { get; set; } = new();
}
