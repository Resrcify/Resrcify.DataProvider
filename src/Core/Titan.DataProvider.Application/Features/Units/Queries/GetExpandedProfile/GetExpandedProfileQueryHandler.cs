using System.Threading;
using System.Threading.Tasks;
using Titan.DataProvider.Application.Abstractions.Infrastructure;
using Titan.DataProvider.Application.Abstractions.Application.Messaging;
using Titan.DataProvider.Domain.Shared;
using Titan.DataProvider.Domain.Internal.BaseData;
using Titan.DataProvider.Domain.Internal.ExpandedUnit;
using System.Collections.Generic;
using Titan.DataProvider.Domain.Errors;

namespace Titan.DataProvider.Application.Features.Units.Queries.GetExpandedProfile;

public sealed class GetExpandedProfileQueryHandler : IQueryHandler<GetExpandedProfileQuery, Dictionary<string, ExpandedUnit>>
{
    private readonly ICachingService _caching;
    public GetExpandedProfileQueryHandler(ICachingService caching)
        => _caching = caching;

    public async Task<Result<Dictionary<string, ExpandedUnit>>> Handle(GetExpandedProfileQuery request, CancellationToken cancellationToken)
    {
        var baseData = await _caching.GetAsync<BaseData>($"BaseData-{request.Language}", cancellationToken);
        if (baseData is null) return Result.Failure<Dictionary<string, ExpandedUnit>>(DomainErrors.ExpandedUnit.GameDataFileNotFound);
        var units = ExpandedUnit.Create(request.PlayerProfile, request.WithStats, request.WithoutGp, request.WithoutMods, request.WithoutSkills, request.WithoutDatacrons, baseData);
        return units.Value;
    }
}
