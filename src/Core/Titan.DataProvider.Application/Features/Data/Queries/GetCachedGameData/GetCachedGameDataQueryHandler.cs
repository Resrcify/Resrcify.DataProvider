using System.Threading;
using System.Threading.Tasks;
using Titan.DataProvider.Application.Abstractions.Infrastructure;
using Titan.DataProvider.Application.Abstractions.Application.Messaging;
using Titan.DataProvider.Domain.Shared;
using Titan.DataProvider.Application.Models.GalaxyOfHeroes.GameData;

namespace Titan.DataProvider.Application.Features.Data.Queries.GetCachedGameData
{
    public sealed class GetCachedGameDataQueryHandler : IQueryHandler<GetCachedGameDataQuery, GameDataResponse>
    {
        private readonly ICachingService _caching;

        public GetCachedGameDataQueryHandler(ICachingService caching)
        {
            _caching = caching;
        }

        public async Task<Result<GameDataResponse>> Handle(GetCachedGameDataQuery request, CancellationToken cancellationToken)
        {
            return await _caching.GetAsync<GameDataResponse>(
                nameof(GameDataResponse), cancellationToken);
        }
    }
}