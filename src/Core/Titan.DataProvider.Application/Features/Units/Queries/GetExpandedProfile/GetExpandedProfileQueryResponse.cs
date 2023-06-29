using System.Collections.Generic;
using Titan.DataProvider.Domain.Internal.ExpandedDatacron;
using Titan.DataProvider.Domain.Internal.ExpandedUnit;

namespace Titan.DataProvider.Application.Features.Units.Queries.GetExpandedProfile;

public sealed record GetExpandedProfileQueryResponse(ProfileSummary ProfileSummary, Dictionary<string, ExpandedUnit> Units, IEnumerable<ExpandedDatacron> Datacrons)
{
}

public sealed record ProfileSummary(
    int GalacticLegends,
    int Zetas,
    Omicrons Omicrons,
    Mods Mods,
    Datacrons Datacrons
    )
{
}

public sealed record Datacrons(
    int Level3to5s,
    int Level6to8s,
    int Level9s,
    int RerollCount)
{
}

public sealed record Mods(
    int SixDotMods,
    int Speed10Minus,
    int SpeedBetween10And14,
    int SpeedBetween15And19,
    int SpeedBetween20And24,
    int Speed25Plus,
    int OffencePercentageBetween4And5,
    int OffencePercentageOver6)
{
}

public sealed record Omicrons(
    int TerritoryBattle,
    int TerritoryWar,
    int GrandArena,
    int Conquest,
    int Raid,
    int Total)
{
}