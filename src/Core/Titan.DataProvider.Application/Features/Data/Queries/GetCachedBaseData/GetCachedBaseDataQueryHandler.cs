using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Titan.DataProvider.Application.Abstractions.Infrastructure;
using Titan.DataProvider.Application.Abstractions.Application.Messaging;
using Titan.DataProvider.Domain.Shared;
using Titan.DataProvider.Domain.Internal.BaseData;
using System;
using System.Collections.Generic;

namespace Titan.DataProvider.Application.Features.Data.Queries.GetCachedBaseData
{
    public sealed class GetCachedBaseDataQueryHandler : IQueryHandler<GetCachedBaseDataQuery, BaseData>
    {
        private readonly ICachingService _caching;

        public GetCachedBaseDataQueryHandler(ICachingService caching)
        {
            _caching = caching;
        }

        public async Task<Result<BaseData>> Handle(GetCachedBaseDataQuery request, CancellationToken cancellationToken)
        {
            var cached = await _caching.GetAsync<BaseData>("BaseData", cancellationToken);
            if (cached is null) return Result.Failure<BaseData>(new Error("test", "test")); //TODO: FIX PROPER ERROR
            if (request.Language != GetCachedBaseDataQueryRequest.ENG_US)
            {
                var localization = await _caching.GetAsync<List<string>>($"Loc_{request.Language}.txt", cancellationToken);
                if (localization is null) return Result.Failure<BaseData>(new Error("test", "test")); //TODO: FIX PROPER ERROR
                //TODO: DO MAGIC TO REPLACE LANGUAGE PARAMS
            }
            return cached;
        }
    }
}