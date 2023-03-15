using System.Linq;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Titan.DataProvider.Application.Abstractions.Infrastructure;
using Microsoft.Extensions.Caching.Distributed;

namespace Titan.DataProvider.Infrastructure.Caching
{

    public class CachingService : ICachingService
    {
        private readonly IDistributedCache _distributedCache;
        private static readonly ConcurrentDictionary<string, bool> CacheKeys = new();

        public CachingService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class
        {
            string? cachedValue = await _distributedCache.GetStringAsync(key, cancellationToken);
            if (cachedValue is null) return null;
            return JsonConvert.DeserializeObject<T>(cachedValue);
        }

        public async Task SetAsync<T>(string key, T value, CancellationToken cancellationToken = default) where T : class
        {
            string cachedValue = JsonConvert.SerializeObject(value);
            await _distributedCache.SetStringAsync(key, cachedValue, cancellationToken);
            CacheKeys.TryAdd(key, false);
        }

        public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            await _distributedCache.RemoveAsync(key, cancellationToken);
            CacheKeys.TryRemove(key, out _);
        }

        public async Task RemoveByPrefixAsync(string prefixKey, CancellationToken cancellationToken = default)
        {
            var tasks = CacheKeys.Keys
                .Where(k => k.StartsWith(prefixKey))
                .Select(k => RemoveAsync(k, cancellationToken));

            await Task.WhenAll(tasks);
        }
    }
}