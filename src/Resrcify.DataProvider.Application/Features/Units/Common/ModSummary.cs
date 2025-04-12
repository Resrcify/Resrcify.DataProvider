namespace Resrcify.DataProvider.Application.Features.Units.Common;

public sealed record ModSummary(
    int SixDotMods,
    int Speed10Minus,
    int SpeedBetween10And14,
    int SpeedBetween15And19,
    int SpeedBetween20And24,
    int Speed25Plus,
    int OffencePercentageBetween4And6,
    int OffencePercentageOver6);
