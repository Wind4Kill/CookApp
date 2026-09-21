using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using CookApp.Application.Interfaces.Caching;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Distributed;

namespace CookApp.Data.Caching
{
    public class CacheService<T>(IDistributedCache cache) : ICacheService<T> where T : class
    {
        public async Task AddValueAsync(T entity, int id, CancellationToken cancellationToken)
        {
            string key = GetCacheKey(typeof(T), id);
            DistributedCacheEntryOptions cacheOptions = new()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                SlidingExpiration = TimeSpan.FromMinutes(5)
            };
            string jsonCachedString = JsonSerializer.Serialize<T>(entity);
            await cache.SetStringAsync(key, jsonCachedString,
            cacheOptions, cancellationToken);
        }

        public async Task<T?> GetValueAsync(int id, CancellationToken cancellationToken)
        {
            string key = GetCacheKey(typeof(T), id);
            string? cacheJsonValue = await cache.GetStringAsync(key, cancellationToken);
            if (cacheJsonValue is null)
            {
                return default;
            }
            T? cachedValue = JsonSerializer.Deserialize<T>(cacheJsonValue);
            return cachedValue;
        }

        public async Task RemoveAsync(int id, CancellationToken cancellationToken)
        {
            string key = GetCacheKey(typeof(T), id);
            await cache.RemoveAsync(key, cancellationToken);
        }

        private string GetCacheKey(Type type, int id) => $"{nameof(type)}:{id}";
    }
}