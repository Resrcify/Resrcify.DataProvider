using System.Threading;
using System.Threading.Tasks;
using Resrcify.DataProvider.Application.Abstractions.Infrastructure;
using Resrcify.DataProvider.Application.Models.GalaxyOfHeroes.Metadata;
using Newtonsoft.Json;
using Resrcify.DataProvider.Application.Errors;
using Resrcify.SharedKernel.ResultFramework.Primitives;
using Resrcify.SharedKernel.Messaging.Abstractions;

namespace Resrcify.DataProvider.Application.Features.Data.GetMetadataVersion;

internal sealed class GetMetadataVersionQueryHandler(IGalaxyOfHeroesService _api)
    : IQueryHandler<GetMetadataVersionQuery, MetadataResponse>
{
    public async Task<Result<MetadataResponse>> Handle(GetMetadataVersionQuery request, CancellationToken cancellationToken)
    {
        var response = await _api.GetMetadata(cancellationToken: cancellationToken);
        if (!response.IsSuccessStatusCode)
            return Result.Failure<MetadataResponse>(ApplicationErrors.HttpClient.RequestNotSuccessful);
        return JsonConvert.DeserializeObject<MetadataResponse>(
                await response.Content.ReadAsStringAsync(cancellationToken));
    }
}