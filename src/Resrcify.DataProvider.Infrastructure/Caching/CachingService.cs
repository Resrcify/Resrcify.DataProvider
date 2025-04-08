using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Resrcify.DataProvider.Application.Abstractions.Infrastructure;
using Microsoft.Extensions.Caching.Distributed;
using Resrcify.DataProvider.Application.Resolvers;
using Newtonsoft.Json.Serialization;
using System.Collections.Concurrent;

namespace Resrcify.DataProvider.Infrastructure.Caching;


public class CachingService : ICachingService
{
    private readonly IDistributedCache _distributedCache;
    private static readonly ConcurrentDictionary<string, object> MemoryCache = new();

    public CachingService(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class
    {
        if (MemoryCache.TryGetValue(key, out var value))
            return value as T;

        string? cachedValue = await _distributedCache.GetStringAsync(key, cancellationToken);
        if (cachedValue is null)
            return null;

        var deserializedValue = JsonConvert.DeserializeObject<T>(cachedValue,
            new JsonSerializerSettings
            {
                ContractResolver = new CustomConstructorResolver()
            }
        );
        if (deserializedValue != null && !MemoryCache.TryAdd(key, deserializedValue))
            MemoryCache[key] = deserializedValue;
        return deserializedValue;
    }

    public async Task SetAsync<T>(string key, T value, CancellationToken cancellationToken = default) where T : class
    {
        var settings = new JsonSerializerSettings()
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            },
            NullValueHandling = NullValueHandling.Ignore
        };
        string cachedValue = JsonConvert.SerializeObject(value, settings);

        if (!MemoryCache.TryAdd(key, value))
            MemoryCache[key] = value;

        await _distributedCache.SetStringAsync(key, cachedValue, cancellationToken);
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        await _distributedCache.RemoveAsync(key, cancellationToken);
        MemoryCache.TryRemove(key, out _);
    }
}