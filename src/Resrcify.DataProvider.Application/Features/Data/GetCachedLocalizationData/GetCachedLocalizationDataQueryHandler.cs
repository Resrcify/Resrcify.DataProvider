using System.Threading;
using System.Threading.Tasks;
using Resrcify.SharedKernel.Messaging.Abstractions;
using Resrcify.SharedKernel.ResultFramework.Primitives;
using Resrcify.SharedKernel.Caching.Abstractions;
using System.Collections.Generic;

namespace Resrcify.DataProvider.Application.Features.Data.GetCachedLocalizationData;

public sealed class GetCachedLocalizationDataQueryHandler(ICachingService _caching)
    : IQueryHandler<GetCachedLocalizationDataQuery, List<string>>
{
    public async Task<Result<List<string>>> Handle(GetCachedLocalizationDataQuery request, CancellationToken cancellationToken)
        => Result.Create(await _caching.GetAsync<List<string>>($"Loc_{request.Language}.txt", null, cancellationToken));

}
