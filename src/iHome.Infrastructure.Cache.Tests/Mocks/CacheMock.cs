namespace iHome.Infrastructure.Cache.Tests.Mocks;

internal class CacheMock : ICache
{
    private readonly Dictionary<object, object> _cache = new();

    public void Dispose()
    {
        _cache.Clear();
    }

    public TItem? Get<TItem>(object key)
    {
        if (_cache.TryGetValue(key, out var val))
        {
            return (TItem)val;
        }

        return default;
    }

    public TItem GetOrCreate<TItem>(object key, Func<InternalCacheEntry, TItem> factory)
    {
        throw new NotImplementedException();
    }

    public Task<TItem> GetOrCreateAsync<TItem>(object key, Func<InternalCacheEntry, Task<TItem>> factory)
    {
        throw new NotImplementedException();
    }

    public TItem Set<TItem>(object key, TItem value)
    {
        if (_cache.ContainsKey(key))
        {
            _cache.Remove(key);
        }

        _cache.Add(key, value!);

        return value;
    }
}
