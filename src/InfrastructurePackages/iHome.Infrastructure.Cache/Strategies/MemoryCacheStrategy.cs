using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace iHome.Infrastructure.Cache.Strategies
{
    public class MemoryCacheStrategy : ICache
    {
        private readonly IMemoryCache _memoryCache;

        public MemoryCacheStrategy(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public TItem Get<TItem>(object key) => _memoryCache.Get<TItem>(key);

        public TItem Set<TItem>(object key, TItem value) => _memoryCache.Set(key, value);

        public void Dispose() => _memoryCache.Dispose();

        public Task<TItem> GetOrCreateAsync<TItem>(object key, Func<InternalCacheEntry, Task<TItem>> factory) =>
            _memoryCache.GetOrCreateAsync(key, async entry =>
            {
                var internalEntry = new InternalCacheEntry();
                var item = await factory(internalEntry);

                entry.AbsoluteExpiration = internalEntry.AbsoluteExpiration;
                entry.AbsoluteExpirationRelativeToNow = internalEntry.AbsoluteExpirationRelativeToNow;
                entry.SlidingExpiration = internalEntry.SlidingExpiration;

                return item;
            });

        public TItem GetOrCreate<TItem>(object key, Func<InternalCacheEntry, TItem> factory) =>
            _memoryCache.GetOrCreate(key, entry =>
            {
                var internalEntry = new InternalCacheEntry();
                var item = factory(internalEntry);

                entry.AbsoluteExpiration = internalEntry.AbsoluteExpiration;
                entry.AbsoluteExpirationRelativeToNow = internalEntry.AbsoluteExpirationRelativeToNow;
                entry.SlidingExpiration = internalEntry.SlidingExpiration;

                return item;
            });

    }
}