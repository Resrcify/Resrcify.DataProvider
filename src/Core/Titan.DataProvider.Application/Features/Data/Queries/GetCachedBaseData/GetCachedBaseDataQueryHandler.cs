using System.Threading;
using System.Threading.Tasks;
using Titan.DataProvider.Application.Abstractions.Infrastructure;
using Titan.DataProvider.Application.Abstractions.Application.Messaging;
using Titan.DataProvider.Domain.Shared;
using Titan.DataProvider.Domain.Internal.BaseData;
using System;

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
            if (cached is null) Console.WriteLine("WTF");
            return cached;
        }
    }
}