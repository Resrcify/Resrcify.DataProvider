using System.Threading;
using System.Threading.Tasks;
using Titan.DataProvider.Application.Abstractions.Infrastructure;
using Titan.DataProvider.Application.Abstractions.Application.Messaging;
using Titan.DataProvider.Domain.Shared;
using Titan.DataProvider.Application.Models.GalaxyOfHeroes.Metadata;
using Newtonsoft.Json;

namespace Titan.DataProvider.Application.Features.Data.Queries.GetMetadataVersion
{
    public sealed class GetMetadataVersionQueryHandler : IQueryHandler<GetMetadataVersionQuery, MetadataResponse>
    {
        private readonly IGalaxyOfHeroesWrapperService _api;

        public GetMetadataVersionQueryHandler(IGalaxyOfHeroesWrapperService api)
        {
            _api = api;
        }

        public async Task<Result<MetadataResponse>> Handle(GetMetadataVersionQuery request, CancellationToken cancellationToken)
        {
            var response = await _api.GetMetadata();
            if (!response.IsSuccessStatusCode)
                return Result.Failure<MetadataResponse>(new Error("test", "test")); //TODO: FIX PROPER ERROR
            return JsonConvert.DeserializeObject<MetadataResponse>(
                    await response.Content.ReadAsStringAsync(cancellationToken));
        }
    }
}