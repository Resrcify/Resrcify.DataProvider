namespace Resrcify.DataProvider.Domain.Models.GalaxyOfHeroes.Common;

public class Stat
{
    public UnitStat UnitStatId { get; set; }
    public long StatValueDecimal { get; set; }
    public long UnscaledDecimalValue { get; set; }
    public long UiDisplayOverrideValue { get; set; }
    public long Scalar { get; set; }
}
