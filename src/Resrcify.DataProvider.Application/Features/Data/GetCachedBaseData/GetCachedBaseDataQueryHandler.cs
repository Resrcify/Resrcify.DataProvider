using System.Threading;
using System.Threading.Tasks;
using Resrcify.DataProvider.Domain.Internal.BaseData;
using Resrcify.DataProvider.Domain.Errors;
using Resrcify.SharedKernel.Messaging.Abstractions;
using Resrcify.SharedKernel.ResultFramework.Primitives;
using Resrcify.SharedKernel.Caching.Abstractions;
using System.Text.Json;
using Resrcify.DataProvider.Application.Resolvers;

namespace Resrcify.DataProvider.Application.Features.Data.GetCachedBaseData;

public sealed class GetCachedBaseDataQueryHandler(ICachingService _caching)
    : IQueryHandler<GetCachedBaseDataQuery, BaseData>
{
    public async Task<Result<BaseData>> Handle(GetCachedBaseDataQuery request, CancellationToken cancellationToken)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            IncludeFields = true
            // PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        options.Converters.Add(new CustomConstructorConverterFactory());
        var cached = await _caching.GetAsync<Result<BaseData>>($"BaseData-{request.Language}", options, cancellationToken);
        if (cached is null)
            return DomainErrors.BaseData.GameDataFileNotFound;
        return cached;
    }
}
