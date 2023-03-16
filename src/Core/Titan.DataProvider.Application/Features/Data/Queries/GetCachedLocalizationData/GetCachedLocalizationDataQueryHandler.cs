using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Titan.DataProvider.Application.Abstractions.Infrastructure;
using Titan.DataProvider.Application.Abstractions.Application.Messaging;
using Titan.DataProvider.Domain.Shared;
using System.IO.Compression;
using System.IO;
using Titan.DataProvider.Application.Models.GalaxyOfHeroes.Localization;
using System.Collections.Generic;
using System.Text;

namespace Titan.DataProvider.Application.Features.Data.Queries.GetCachedLocalizationData
{
    public sealed class GetCachedLocalizationDataQueryHandler : IQueryHandler<GetCachedLocalizationDataQuery, List<string>>
    {
        private readonly ICachingService _caching;

        public GetCachedLocalizationDataQueryHandler(ICachingService caching)
        {
            _caching = caching;
        }

        public async Task<Result<List<string>>> Handle(GetCachedLocalizationDataQuery request, CancellationToken cancellationToken)
        {
            return await _caching.GetAsync<List<string>>($"Loc_{request.Language}.txt", cancellationToken);
        }
    }
}