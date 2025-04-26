using HeadphoneStore.Application.Abstracts.Interface.Services.Caching;
using HeadphoneStore.Infrastructure.DependencyInjection.Options;

using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using StackExchange.Redis;

namespace HeadphoneStore.Infrastructure.Services.Caching;

public class CacheService : ICacheService
{
    private readonly IDistributedCache _distributedCache;
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    private readonly CacheOption _cacheOption;

    public CacheService(
        IDistributedCache distributedCache,
        IConnectionMultiplexer connectionMultiplexer,
        IOptions<CacheOption> cacheOption)
    {
        _distributedCache = distributedCache;
        _connectionMultiplexer = connectionMultiplexer;
        _cacheOption = cacheOption.Value;
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException("Key cannot be null or whitespace.", nameof(key));

        string? cachedValue = await _distributedCache.GetStringAsync(key, cancellationToken);

        if (string.IsNullOrEmpty(cachedValue))
            return null;

        return JsonConvert.DeserializeObject<T>(cachedValue, new JsonSerializerSettings
        {
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            }
        });
    }

    public async Task SetAsync<T>(string key, T value, DistributedCacheEntryOptions? options = null, CancellationToken cancellationToken = default) where T : class
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException("Key cannot be null or whitespace.", nameof(key));

        if (value == null)
            throw new ArgumentNullException(nameof(value));

        string serializedValue = JsonConvert.SerializeObject(value, new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            }
        });

        if (options is null)
        {
            await _distributedCache.SetStringAsync(key, serializedValue, cancellationToken);
            return;
        }

        if (options.AbsoluteExpirationRelativeToNow is null)
        {
            options.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_cacheOption.ExpirationMinutes);
        }

        await _distributedCache.SetStringAsync(key, serializedValue, options, cancellationToken);
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException("Key cannot be null or whitespace.", nameof(key));

        await _distributedCache.RemoveAsync(key, cancellationToken);
    }

    // References: https://stackoverflow.com/a/60385140
    public async Task RemoveByPrefixAsync(string prefix, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(prefix))
            throw new ArgumentException("Prefix cannot be null or whitespace.", nameof(prefix));

        if (_connectionMultiplexer != null)
        {
            var keys = new List<string>();

            foreach (var endpoint in _connectionMultiplexer.GetEndPoints())
            {
                var server = _connectionMultiplexer.GetServer(endpoint);

                await foreach (var key in server.KeysAsync(pattern: $"{prefix}*").WithCancellation(cancellationToken))
                {
                    keys.Add(key.ToString());
                }
            }

            await Task.WhenAll(keys.Select(k => _distributedCache.RemoveAsync(k, cancellationToken)));
        }
        else
        {
            throw new ArgumentException("Missing redis server or redis is not supported", nameof(_connectionMultiplexer));
        }
    }

    public IEnumerable<RedisFeatures> GetRedisFeatures()
    {
        foreach (var endpoint in _connectionMultiplexer.GetEndPoints())
        {
            var server = _connectionMultiplexer.GetServer(endpoint);
            yield return server.Features;
        }
    }
}
