namespace Resrcify.DataProvider.Application.Features.Units.Common;

public sealed record ProfileSummary(
    int GalacticLegends,
    int CapitalShips,
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
    DatacronSummary Datacrons);
