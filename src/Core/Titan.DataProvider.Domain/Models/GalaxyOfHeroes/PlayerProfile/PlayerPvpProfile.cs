namespace Titan.DataProvider.Domain.Models.GalaxyOfHeroes.PlayerProfile
{
    public class PlayerPvpProfile
    {
        public PlayerProfileTab Tab { get; set; }
        public int Rank { get; set; }
        public Squad? Squad { get; set; }
    }
}
