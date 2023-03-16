namespace Titan.DataProvider.Application.Models.GalaxyOfHeroes.GameData
{
    public class StatValueRange
    {
        public UnitStat Stat { get; set; }
        public BattleUnitStateStat BattleStat { get; set; }
        public StatValueRangeNumber? Min { get; set; }
        public StatValueRangeNumber? Max { get; set; }

    }
}
