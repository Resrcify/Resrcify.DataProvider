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
        return new GetExpandedProfileQueryResponse(units.ToDictionary(x => x.Key, x => x.Value), datacrons);
    }
}
