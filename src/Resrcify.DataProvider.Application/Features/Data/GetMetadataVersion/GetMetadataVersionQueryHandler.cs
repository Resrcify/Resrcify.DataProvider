using System.Threading;
using System.Threading.Tasks;
using Resrcify.DataProvider.Application.Models.GalaxyOfHeroes.Metadata;
using Resrcify.SharedKernel.ResultFramework.Primitives;
using Resrcify.SharedKernel.Messaging.Abstractions;
using Resrcify.DataProvider.Application.Abstractions;

namespace Resrcify.DataProvider.Application.Features.Data.GetMetadataVersion;

internal sealed class GetMetadataVersionQueryHandler(IGalaxyOfHeroesService _api)
    : IQueryHandler<GetMetadataVersionQuery, MetadataResponse>
{
    public async Task<Result<MetadataResponse>> Handle(GetMetadataVersionQuery request, CancellationToken cancellationToken)
    {
        var response = await _api.GetMetadata(cancellationToken: cancellationToken);
        return response;
    }
}