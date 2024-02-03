using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace iHome.Infrastructure.Cache.Strategies
{
    public class DistributedCacheStrategy : ICache
    {
        private readonly IDistributedCache _distributedCache;

        public DistributedCacheStrategy(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public TItem Get<TItem>(object key)
        {
            var result = _distributedCache.GetString(key.ToString());

            if (string.IsNullOrEmpty(result))
            {
                return default!;
            }

            return JsonSerializer.Deserialize<TItem>(result);
        }

        public TItem Set<TItem>(object key, TItem value)
        {
            _distributedCache.SetString(key.ToString(), JsonSerializer.Serialize(value));
            return value;
        }

        public void Dispose()
        {
        }

        public async Task<TItem> GetOrCreateAsync<TItem>(object key, Func<InternalCacheEntry, Task<TItem>> factory)
        {
            var result = await _distributedCache.GetStringAsync(key.ToString());
            TItem item;
            if (string.IsNullOrEmpty(result))
            {
                var entry = new InternalCacheEntry();
                item = await factory(entry);
                await _distributedCache.SetStringAsync(
                    key.ToString(),
                    JsonSerializer.Serialize(item),
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpiration = entry.AbsoluteExpiration,
                        AbsoluteExpirationRelativeToNow = entry.AbsoluteExpirationRelativeToNow,
                        SlidingExpiration = entry.SlidingExpiration
                    });
            }
            else
            {
                item = JsonSerializer.Deserialize<TItem>(result);
            }

            return item;
        }

        public TItem GetOrCreate<TItem>(object key, Func<InternalCacheEntry, TItem> factory)
        {
            var result = _distributedCache.GetString(key.ToString());
            TItem item;
            if (string.IsNullOrEmpty(result))
            {
                var entry = new InternalCacheEntry();
                item = factory(entry);
                _distributedCache.SetString(
                    key.ToString(),
                    JsonSerializer.Serialize(item),
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpiration = entry.AbsoluteExpiration,
                        AbsoluteExpirationRelativeToNow = entry.AbsoluteExpirationRelativeToNow,
                        SlidingExpiration = entry.SlidingExpiration
                    });
            }
            else
            {
                item = JsonSerializer.Deserialize<TItem>(result);
            }

            return item;
        }
    }

}