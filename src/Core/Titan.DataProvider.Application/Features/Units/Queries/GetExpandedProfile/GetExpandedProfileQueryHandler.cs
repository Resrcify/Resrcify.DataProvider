using System.Threading;
using System.Threading.Tasks;
using Titan.DataProvider.Application.Abstractions.Infrastructure;
using Titan.DataProvider.Application.Abstractions.Application.Messaging;
using Titan.DataProvider.Domain.Shared;
using Titan.DataProvider.Domain.Internal.BaseData;
using Titan.DataProvider.Domain.Internal.ExpandedUnit;
using Titan.DataProvider.Domain.Errors;
using Titan.DataProvider.Domain.Internal.ExpandedDatacron;
using System.Linq;
using Titan.DataProvider.Domain.Internal.ExpandedUnit.Enums;
using Titan.DataProvider.Domain.Models.GalaxyOfHeroes.Common;
using System.Collections.Generic;
using Titan.DataProvider.Domain.Models.GalaxyOfHeroes.GameData;

namespace Titan.DataProvider.Application.Features.Units.Queries.GetExpandedProfile;

public sealed class GetExpandedProfileQueryHandler : IQueryHandler<GetExpandedProfileQuery, GetExpandedProfileQueryResponse>
{
    private readonly ICachingService _caching;
    public GetExpandedProfileQueryHandler(ICachingService caching)
        => _caching = caching;

    public async Task<Result<GetExpandedProfileQueryResponse>> Handle(GetExpandedProfileQuery request, CancellationToken cancellationToken)
    {
        var baseData = await _caching.GetAsync<BaseData>($"BaseData-{request.Language}", cancellationToken);
        if (baseData is null) return Result.Failure<GetExpandedProfileQueryResponse>(DomainErrors.ExpandedUnit.GameDataFileNotFound);

        var units = request.DefinitionId is null ?
            ExpandedUnit.Create(request.PlayerProfile, request.WithStats, request.WithoutGp, request.WithoutModStats, request.WithoutMods, request.WithoutSkills, baseData) :
            ExpandedUnit.Create(request.DefinitionId, request.PlayerProfile, request.WithStats, request.WithoutGp, request.WithoutModStats, request.WithoutMods, request.WithoutSkills, baseData);

        var datacrons = Enumerable.Empty<ExpandedDatacron>();
        if (!request.WithoutDatacrons) datacrons = ExpandedDatacron.Create(request.PlayerProfile.Datacron, baseData);

        var datacronSummary = ParseDatacronSummary(datacrons);
        var summary = ParseSummaryData(units, datacronSummary);

        return new GetExpandedProfileQueryResponse(summary, units.ToDictionary(x => x.Key, x => x.Value), datacrons);
    }
    private static Datacrons ParseDatacronSummary(IEnumerable<ExpandedDatacron> expandedDatacrons)
    {
        var level3to5 = 0;
        var level6to8s = 0;
        var level9s = 0;
        var rerolls = 0;
        foreach (var datacron in expandedDatacrons)
        {
            if (datacron.ActivatedTiers <= 5) level3to5++;
            if (datacron.ActivatedTiers > 6 && datacron.ActivatedTiers <= 8) level6to8s++;
            if (datacron.ActivatedTiers == 9) level9s++;
            rerolls += datacron.RerollCount;
        }

        return new Datacrons(level3to5, level6to8s, level9s, rerolls);
    }
    private static ProfileSummary ParseSummaryData(IEnumerable<KeyValuePair<string, ExpandedUnit>> expandedUnits, Datacrons datacronSummary)
    {
        var glCount = 0;
        var sixDotModsCount = 0;
        var speedUnder10 = 0;
        var speedBetween10And14 = 0;
        var speedBetween15And19 = 0;
        var speedBetween20And24 = 0;
        var speed25Plus = 0;
        var offenceBetween4And5Percent = 0;
        var offenceOver6Percent = 0;
        var zetaCount = 0;
        var omicronCount = 0;
        var tbOmicronCount = 0;
        var twOmicronCount = 0;
        var gaOmicronCount = 0;
        var cqOmicronCount = 0;
        var raidOmicronCount = 0;

        foreach (var unit in expandedUnits)
        {
            if (unit.Value.IsGalacticLegend) glCount++;
            foreach (var mod in unit.Value.Mods)
            {
                if (mod.Rarity == ModRarity.MODRARITY6) sixDotModsCount++;
                foreach (var secondaryStat in mod.SecondaryStats)
                {
                    if (secondaryStat.UnitStat == UnitStat.UNITSTATSPEED && secondaryStat.Value < 10) speedUnder10++;
                    if (secondaryStat.UnitStat == UnitStat.UNITSTATSPEED && secondaryStat.Value >= 10 && secondaryStat.Value <= 14) speedBetween10And14++;
                    if (secondaryStat.UnitStat == UnitStat.UNITSTATSPEED && secondaryStat.Value >= 15 && secondaryStat.Value <= 19) speedBetween15And19++;
                    if (secondaryStat.UnitStat == UnitStat.UNITSTATSPEED && secondaryStat.Value >= 20 && secondaryStat.Value <= 24) speedBetween20And24++;
                    if (secondaryStat.UnitStat == UnitStat.UNITSTATSPEED && secondaryStat.Value >= 25) speed25Plus++;
                    if (secondaryStat.UnitStat == UnitStat.UNITSTATOFFENSEPERCENTADDITIVE && secondaryStat.Value >= 6) offenceOver6Percent++;
                    if (secondaryStat.UnitStat == UnitStat.UNITSTATOFFENSEPERCENTADDITIVE && secondaryStat.Value >= 4 && secondaryStat.Value < 6) offenceBetween4And5Percent++;
                }
            }

            foreach (var skill in unit.Value.Skills)
            {
                if (skill.HasActivatedZeta) zetaCount++;
                if (!skill.HasActivatedOmicron) continue;

                omicronCount++;

                if (skill.OmicronRestriction == OmicronMode.TERRITORYBATTLEBOTHOMICRON ||
                    skill.OmicronRestriction == OmicronMode.TERRITORYCOVERTOMICRON ||
                    skill.OmicronRestriction == OmicronMode.TERRITORYSTRIKEOMICRON)
                    tbOmicronCount++;
                if (skill.OmicronRestriction == OmicronMode.TERRITORYTOURNAMENTOMICRON ||
                    skill.OmicronRestriction == OmicronMode.TERRITORYTOURNAMENT3OMICRON ||
                    skill.OmicronRestriction == OmicronMode.TERRITORYTOURNAMENT5OMICRON)
                    gaOmicronCount++;
                if (skill.OmicronRestriction == OmicronMode.TERRITORYWAROMICRON)
                    twOmicronCount++;
                if (skill.OmicronRestriction == OmicronMode.CONQUESTOMICRON)
                    cqOmicronCount++;
                if (skill.OmicronRestriction == OmicronMode.GUILDRAIDOMICRON)
                    raidOmicronCount++;
            }
        }

        var mods = new Mods(
            sixDotModsCount,
            speedUnder10,
            speedBetween10And14,
            speedBetween15And19,
            speedBetween20And24,
            speed25Plus,
            offenceBetween4And5Percent,
            offenceOver6Percent);

        var omicrons = new Omicrons(
            tbOmicronCount,
            twOmicronCount,
            gaOmicronCount,
            cqOmicronCount,
            raidOmicronCount,
            omicronCount);

        return new ProfileSummary(
            glCount,
            zetaCount,
            omicrons,
            mods,
            datacronSummary
        );
    }
}
