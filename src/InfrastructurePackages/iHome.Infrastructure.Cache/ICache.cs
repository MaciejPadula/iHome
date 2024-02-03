using System;
using System.Threading.Tasks;

namespace iHome.Infrastructure.Cache
{
    public interface ICache : IDisposable
    {
        TItem Get<TItem>(object key);
        TItem Set<TItem>(object key, TItem value);
        TItem GetOrCreate<TItem>(object key, Func<InternalCacheEntry, TItem> factory);
        Task<TItem> GetOrCreateAsync<TItem>(object key, Func<InternalCacheEntry, Task<TItem>> factory);
    }
}