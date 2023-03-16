using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Titan.DataProvider.Application.Abstractions.Infrastructure;
using Microsoft.Extensions.Caching.Distributed;
using Titan.DataProvider.Application.Resolvers;
using Newtonsoft.Json.Serialization;

namespace Titan.DataProvider.Infrastructure.Caching
{

    public class CachingService : ICachingService
    {
        private readonly IDistributedCache _distributedCache;

        public CachingService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class
        {
            string? cachedValue = await _distributedCache.GetStringAsync(key, cancellationToken);
            if (cachedValue is null) return null;
            return JsonConvert.DeserializeObject<T>(cachedValue,
                new JsonSerializerSettings
                {
                    ContractResolver = new CustomConstructorResolver()
                }
            );
        }

        public async Task SetAsync<T>(string key, T value, CancellationToken cancellationToken = default) where T : class
        {
            var settings = new JsonSerializerSettings()
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                    {
                        OverrideSpecifiedNames = false
                    }
                },
                NullValueHandling = NullValueHandling.Ignore
            };
            string cachedValue = JsonConvert.SerializeObject(value, settings);
            await _distributedCache.SetStringAsync(key, cachedValue, cancellationToken);
        }

        public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            await _distributedCache.RemoveAsync(key, cancellationToken);
        }
    }
}