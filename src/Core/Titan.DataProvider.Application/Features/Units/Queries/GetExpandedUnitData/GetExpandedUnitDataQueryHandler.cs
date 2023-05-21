using System.Threading;
using System.Threading.Tasks;
using Titan.DataProvider.Application.Abstractions.Infrastructure;
using Titan.DataProvider.Application.Abstractions.Application.Messaging;
using Titan.DataProvider.Domain.Shared;
using Titan.DataProvider.Domain.Internal.BaseData;
using Titan.DataProvider.Domain.Internal.ExpandedUnit;
using System.Collections.Generic;

namespace Titan.DataProvider.Application.Features.Units.Queries.GetExpandedUnitData
{
    public sealed class GetExpandedUnitDataQueryHandler : IQueryHandler<GetExpandedUnitDataQuery, List<ExpandedUnit>>
    {
        private readonly ICachingService _caching;

        public GetExpandedUnitDataQueryHandler(ICachingService caching)
        {
            _caching = caching;
        }

        public async Task<Result<List<ExpandedUnit>>> Handle(GetExpandedUnitDataQuery request, CancellationToken cancellationToken)
        {
            var baseData = await _caching.GetAsync<BaseData>($"BaseData-{request.Language}", cancellationToken);
            if (baseData is null) return Result.Failure<List<ExpandedUnit>>(new Error("test", "test")); //TODO: FIX PROPER ERROR
            var units = ExpandedUnit.Create(request.PlayerProfile, baseData);
            return units.Value;
        }
    }
}