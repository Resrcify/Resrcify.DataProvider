using System.Threading;
using System.Threading.Tasks;
using Resrcify.DataProvider.Domain.Internal.BaseData;
using Resrcify.DataProvider.Domain.Internal.ExpandedUnit;
using Resrcify.DataProvider.Domain.Internal.ExpandedDatacron;
using System.Linq;
using System.Collections.Generic;
using Resrcify.SharedKernel.Messaging.Abstractions;
using Resrcify.SharedKernel.ResultFramework.Primitives;
using Resrcify.SharedKernel.Caching.Abstractions;
using Resrcify.DataProvider.Application.Extensions;
using Resrcify.DataProvider.Domain.Errors;

namespace Resrcify.DataProvider.Application.Features.Units.GetExpandedProfiles;

internal sealed class GetExpandedProfilesQueryHandler(ICachingService _caching)
    : IQueryHandler<GetExpandedProfilesQuery, IEnumerable<GetExpandedProfilesQueryResponse>>
{
    public async Task<Result<IEnumerable<GetExpandedProfilesQueryResponse>>> Handle(GetExpandedProfilesQuery request, CancellationToken cancellationToken)
    {
        var baseData = await _caching.GetAsync<BaseData>(
            $"BaseData-{request.Language}",
            JsonSerializerExtensions.GetDomainSerializerOptions(),
            cancellationToken);

        if (baseData is null)
            return Result.Failure<IEnumerable<GetExpandedProfilesQueryResponse>>(DomainErrors.ExpandedUnit.GameDataFileNotFound);

        var expandedProfiles = GetExpandedPlayerProfiles(
            request,
            baseData);

        return Result.Success(expandedProfiles);
    }

    private static IEnumerable<GetExpandedProfilesQueryResponse> GetExpandedPlayerProfiles(GetExpandedProfilesQuery request, BaseData baseData)
    {
        foreach (var profile in request.PlayerProfiles)
        {
            var units = ExpandedUnit.Create(
                profile,
                request.WithStats,
                request.WithoutGp,
                request.WithoutModStats,
                request.WithoutMods,
                request.WithoutSkills,
                baseData);

            var datacrons = Enumerable.Empty<ExpandedDatacron>();
            if (!request.WithoutDatacrons)
                datacrons = ExpandedDatacron.Create(profile.Datacrons, baseData);

            yield return new GetExpandedProfilesQueryResponse(
                profile.PlayerId ?? "Unknown",
                profile.AllyCode, profile.Name ?? "Unknown",
                units.ParseSummaryData(datacrons.ParseDatacronSummary()),
                units.ToDictionary(
                    x => x.Key,
                    x => x.Value),
                datacrons);
        }
    }
}
