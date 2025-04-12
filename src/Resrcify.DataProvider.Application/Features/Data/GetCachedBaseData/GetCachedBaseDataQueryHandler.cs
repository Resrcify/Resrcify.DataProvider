using System.Threading;
using System.Threading.Tasks;
using Resrcify.DataProvider.Domain.Internal.BaseData;
using Resrcify.DataProvider.Domain.Errors;
using Resrcify.SharedKernel.Messaging.Abstractions;
using Resrcify.SharedKernel.ResultFramework.Primitives;
using Resrcify.SharedKernel.Caching.Abstractions;
using Resrcify.DataProvider.Application.Extensions;

namespace Resrcify.DataProvider.Application.Features.Data.GetCachedBaseData;

public sealed class GetCachedBaseDataQueryHandler(ICachingService _caching)
    : IQueryHandler<GetCachedBaseDataQuery, BaseData>
{
    public async Task<Result<BaseData>> Handle(GetCachedBaseDataQuery request, CancellationToken cancellationToken)
        => Result
            .Create(
                await _caching.GetAsync<BaseData>(
                    $"BaseData-{request.Language}",
                    JsonSerializerExtensions.GetDomainSerializerOptions(),
                    cancellationToken))
            .Match(
                respone => respone,
                DomainErrors.BaseData.GameDataFileNotFound);
}
