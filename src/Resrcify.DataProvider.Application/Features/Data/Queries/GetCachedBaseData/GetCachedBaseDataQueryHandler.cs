using System.Threading;
using System.Threading.Tasks;
using Resrcify.DataProvider.Application.Abstractions.Infrastructure;
using Resrcify.DataProvider.Domain.Internal.BaseData;
using Resrcify.DataProvider.Domain.Errors;
using Resrcify.SharedKernel.Messaging.Abstractions;
using Resrcify.SharedKernel.ResultFramework.Primitives;

namespace Resrcify.DataProvider.Application.Features.Data.Queries.GetCachedBaseData;

public sealed class GetCachedBaseDataQueryHandler : IQueryHandler<GetCachedBaseDataQuery, BaseData>
{
    private readonly ICachingService _caching;

    public GetCachedBaseDataQueryHandler(ICachingService caching)
        => _caching = caching;

    public async Task<Result<BaseData>> Handle(GetCachedBaseDataQuery request, CancellationToken cancellationToken)
    {
        var cached = await _caching.GetAsync<BaseData>($"BaseData-{request.Language}", cancellationToken);
        if (cached is null)
            return Result.Failure<BaseData>(DomainErrors.BaseData.GameDataFileNotFound);
        return cached;
    }
}
