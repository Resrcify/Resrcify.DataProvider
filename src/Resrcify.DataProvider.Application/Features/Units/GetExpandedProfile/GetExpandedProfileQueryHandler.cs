using System.Threading;
using System.Threading.Tasks;
using Resrcify.DataProvider.Domain.Internal.BaseData;
using Resrcify.DataProvider.Domain.Internal.ExpandedUnit;
using Resrcify.DataProvider.Domain.Errors;
using Resrcify.DataProvider.Domain.Internal.ExpandedDatacron;
using System.Linq;
using Resrcify.SharedKernel.ResultFramework.Primitives;
using Resrcify.SharedKernel.Messaging.Abstractions;
using Resrcify.SharedKernel.Caching.Abstractions;
using Resrcify.DataProvider.Application.Extensions;

namespace Resrcify.DataProvider.Application.Features.Units.GetExpandedProfile;

internal sealed class GetExpandedProfileQueryHandler(ICachingService _caching)
    : IQueryHandler<GetExpandedProfileQuery, GetExpandedProfileQueryResponse>
{
    public async Task<Result<GetExpandedProfileQueryResponse>> Handle(GetExpandedProfileQuery request, CancellationToken cancellationToken)
    {
        var baseData = await _caching.GetAsync<BaseData>(
            $"BaseData-{request.Language}",
            JsonSerializerExtensions.GetDomainSerializerOptions(),
            cancellationToken);
        if (baseData is null)
            return Result.Failure<GetExpandedProfileQueryResponse>(DomainErrors.ExpandedUnit.GameDataFileNotFound);

        var units = request.DefinitionId is null
            ? ExpandedUnit.Create(
                request.PlayerProfile,
                request.WithStats,
                request.WithoutGp,
                request.WithoutModStats,
                request.WithoutMods,
                request.WithoutSkills,
                baseData)
            : ExpandedUnit.Create(
                request.DefinitionId,
                request.PlayerProfile,
                request.WithStats,
                request.WithoutGp,
                request.WithoutModStats,
                request.WithoutMods,
                request.WithoutSkills,
                baseData);

        var datacrons = Enumerable.Empty<ExpandedDatacron>();
        if (!request.WithoutDatacrons)
            datacrons = ExpandedDatacron.Create(request.PlayerProfile.Datacrons, baseData);

        return new GetExpandedProfileQueryResponse(
            request.PlayerProfile.PlayerId ?? "Unknown",
            request.PlayerProfile.AllyCode,
            request.PlayerProfile.Name ?? "Unknown",
            units.ParseSummaryData(datacrons.ParseDatacronSummary()),
            units.ToDictionary(
                x => x.Key,
                x => x.Value),
            datacrons);
    }
}
