using System.Collections.Generic;
using Titan.DataProvider.Domain.Internal.ExpandedDatacron;
using Titan.DataProvider.Domain.Internal.ExpandedUnit;

namespace Titan.DataProvider.Application.Features.Units.Queries.GetExpandedProfiles;

public sealed record GetExpandedProfilesQueryResponse(string PlayerId, long AllyCode, string Name, ProfileSummary ProfileSummary, Dictionary<string, ExpandedUnit> Units, IEnumerable<ExpandedDatacron> Datacrons)
{
}

public sealed record ProfileSummary(
    int GalacticLegends,
    int Characters,
    int Ships,
    double CharacterGp,
    double ShipsGp,
    double TotalGp,
    GearSummary Gear,
    RelicSummary Relics,
    int Zetas,
    OmicronSummary Omicrons,
    ModSummary Mods,
    DatacronSummary Datacrons
    )
{
}

public sealed record DatacronSummary(
    int Level0,
    int Level1,
    int Level2,
    int Level3,
    int Level4,
    int Level5,
    int Level6,
    int Level7,
    int Level8,
    int Level9,
    int RerollCount)
{
}

public sealed record ModSummary(
    int SixDotMods,
    int Speed10Minus,
    int SpeedBetween10And14,
    int SpeedBetween15And19,
    int SpeedBetween20And24,
    int Speed25Plus,
    int OffencePercentageBetween4And6,
    int OffencePercentageOver6)
{
}

public sealed record OmicronSummary(
    int TerritoryBattle,
    int TerritoryWar,
    int GrandArena,
    int Conquest,
    int Raid,
    int Total)
{
}

public sealed record class GearSummary(
    int Gear1,
    int Gear2,
    int Gear3,
    int Gear4,
    int Gear5,
    int Gear6,
    int Gear7,
    int Gear8,
    int Gear9,
    int Gear10,
    int Gear11,
    int Gear12,
    int Gear13)
{
}

public sealed record RelicSummary(
    int Relic0,
    int Relic1,
    int Relic2,
    int Relic3,
    int Relic4,
    int Relic5,
    int Relic6,
    int Relic7,
    int Relic8,
    int Relic9,
    int Total)
{
}