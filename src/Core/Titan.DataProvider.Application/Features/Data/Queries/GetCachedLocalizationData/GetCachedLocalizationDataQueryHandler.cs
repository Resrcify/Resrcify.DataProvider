using System.Threading;
using System.Threading.Tasks;
using Titan.DataProvider.Application.Abstractions.Infrastructure;
using System.Collections.Generic;
using Resrcify.SharedKernel.Messaging.Abstractions;
using Resrcify.SharedKernel.ResultFramework.Primitives;

namespace Titan.DataProvider.Application.Features.Data.Queries.GetCachedLocalizationData;

public sealed class GetCachedLocalizationDataQueryHandler : IQueryHandler<GetCachedLocalizationDataQuery, List<string>>
{
    private readonly ICachingService _caching;

    public GetCachedLocalizationDataQueryHandler(ICachingService caching)
        => _caching = caching;

    public async Task<Result<List<string>>> Handle(GetCachedLocalizationDataQuery request, CancellationToken cancellationToken)
    {
        return await _caching.GetAsync<List<string>>($"Loc_{request.Language}.txt", cancellationToken);
    }
}
