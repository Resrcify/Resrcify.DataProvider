using System.Threading;
using System.Threading.Tasks;
using Titan.DataProvider.Application.Abstractions.Infrastructure;
using Titan.DataProvider.Domain.Internal.BaseData;
using Titan.DataProvider.Domain.Errors;
using Resrcify.SharedKernel.Messaging.Abstractions;
using Resrcify.SharedKernel.ResultFramework.Primitives;

namespace Titan.DataProvider.Application.Features.Data.Queries.GetCachedBaseData;

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
