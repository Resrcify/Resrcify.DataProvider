using System.Threading;
using System.Threading.Tasks;
using Resrcify.DataProvider.Domain.Internal.BaseData;
using Resrcify.DataProvider.Domain.Errors;
using Resrcify.SharedKernel.Messaging.Abstractions;
using Resrcify.SharedKernel.ResultFramework.Primitives;
using Resrcify.SharedKernel.Caching.Abstractions;
using Resrcify.DataProvider.Application.Converters;

namespace Resrcify.DataProvider.Application.Features.Data.GetCachedBaseData;

public sealed class GetCachedBaseDataQueryHandler(ICachingService _caching)
    : IQueryHandler<GetCachedBaseDataQuery, BaseData>
{
    public async Task<Result<BaseData>> Handle(GetCachedBaseDataQuery request, CancellationToken cancellationToken)
    {
        var cached = await _caching.GetAsync<BaseData>(
            $"BaseData-{request.Language}",
            JsonSerializerExtensions.GetJsonSerializerOptions(),
            cancellationToken);
        if (cached is null)
            return DomainErrors.BaseData.GameDataFileNotFound;
        return cached;
    }
}
