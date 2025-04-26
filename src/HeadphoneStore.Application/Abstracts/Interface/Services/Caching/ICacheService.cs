using Microsoft.Extensions.Caching.Distributed;

namespace HeadphoneStore.Application.Abstracts.Interface.Services.Caching;

public interface ICacheService
{
    Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        where T : class;

    Task SetAsync<T>(string key, T value, DistributedCacheEntryOptions? options = null, CancellationToken cancellationToken = default) 
        where T : class;

    Task InvalidateAsync(string key, CancellationToken cancellationToken = default);

    Task InvalidateWithPrefixAsync(string prefix, CancellationToken cancellationToken = default);
}
