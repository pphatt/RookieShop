using HeadphoneStore.Application.Abstracts.Interface.Services.Caching;
using HeadphoneStore.Shared.Abstracts.Shared;

using MediatR;

using Microsoft.Extensions.Caching.Distributed;

namespace HeadphoneStore.Application.Behaviors;

/// <summary>
/// Pipeline behavior that caches query results for requests implementing ICacheable.
/// </summary>
public class CachingPipelineBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICacheable
    where TResponse : Result
{
    private readonly ICacheService _cacheService;

    public CachingPipelineBehavior(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }

    /// <summary>
    /// Handles the request by checking the cache or executing the handler and caching the result.
    /// </summary>
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // Skip caching if explicitly bypassed or cache key is invalid
        if (request.BypassCache || string.IsNullOrEmpty(request.CacheKey)) return await next();

        // Take In Cache
        // Exist => Return
        // Not Exist => Persist, UpdateCache
        var cachedResponse = await _cacheService.GetAsync<TResponse>(request.CacheKey, cancellationToken);

        if (cachedResponse != null)
        {
            // logger.LogInformation("Retrieve from cache with key : {CacheKey}", request.CacheKey);
            return cachedResponse;
        }

        // logger.LogInformation("Added to cache with key : {CacheKey}", request.CacheKey);
        return await GetResponseAndAddToCacheAsync(request, next, cancellationToken);
    }

    private async Task<TResponse> GetResponseAndAddToCacheAsync(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var response = await next();

        if (response == null)
            return response!;

        if (request.SlidingExpirationInMinutes < 0 || request.AbsoluteExpirationInMinutes < 0)
        {
            await _cacheService.SetAsync(request.CacheKey, response, null, cancellationToken);
            return response;
        }

        var slidingExpiration = request.SlidingExpirationInMinutes == 0 ? 30 : request.SlidingExpirationInMinutes;
        var absoluteExpiration = request.AbsoluteExpirationInMinutes == 0 ? 60 : request.AbsoluteExpirationInMinutes;
        var options = new DistributedCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(slidingExpiration))
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(absoluteExpiration));

        await _cacheService.SetAsync(request.CacheKey, response, options, cancellationToken);

        return response;
    }
}
