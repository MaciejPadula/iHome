using System.Collections.Generic;
using System;
using System.Linq;

namespace iHome.Infrastructure.Cache.Logic
{
    public interface ICacheFetcher
    {
        TEntity FetchCachedByKey<TKey, TEntity>(TKey key, Func<TKey, TEntity> entityFetcher, Func<TEntity, TKey> keyAccessor, string methodKey);
        IEnumerable<TEntity> FetchCachedByKeys<TKey, TEntity>(IEnumerable<TKey> keys, Func<IEnumerable<TKey>, IEnumerable<TEntity>> entityFetcher, Func<TEntity, TKey> keyAccessor, string methodKey);
    }

    public class CacheFetcher : ICacheFetcher
    {
        private readonly ICache _cache;

        public CacheFetcher(ICache cache)
        {
            _cache = cache;
        }

        public TEntity FetchCachedByKey<TKey, TEntity>(TKey key, Func<TKey, TEntity> entityFetcher, Func<TEntity, TKey> keyAccessor, string methodKey)
        {
            var entitiesFromCache = _cache.Get<List<TEntity>>(methodKey) ?? new List<TEntity>();
            var entity = entitiesFromCache
                .FirstOrDefault(e => keyAccessor(e).Equals(key));

            if (entity != null)
            {
                return entity;
            }

            entity = entityFetcher(key);

            if (entity != null)
            {
                entitiesFromCache.Add(entity);
                _cache.Set(methodKey, entitiesFromCache);
            }

            return entity;
        }

        public IEnumerable<TEntity> FetchCachedByKeys<TKey, TEntity>(IEnumerable<TKey> keys, Func<IEnumerable<TKey>, IEnumerable<TEntity>> entityFetcher, Func<TEntity, TKey> keyAccessor, string methodKey)
        {
            var entitiesFromCache = _cache.Get<List<TEntity>>(methodKey) ?? new List<TEntity>();

            var existingEntities = entitiesFromCache
                .GroupBy(e => keyAccessor(e))
                .Where(kv => keys.Contains(kv.Key))
                .ToDictionary(kv => kv.Key, kv => kv.First());

            var entitiesToReturn = new List<TEntity>();
            var keysToFetch = new List<TKey>();

            foreach (var key in keys)
            {
                if (existingEntities.TryGetValue(key, out var entity) == false || entity is null)
                {
                    keysToFetch.Add(key);
                    continue;
                }

                entitiesToReturn.Add(entity);
            }

            if (keysToFetch.Count == 0)
            {
                return entitiesToReturn;
            }

            var entities = entityFetcher(keysToFetch);

            if (entities.Any())
            {
                entitiesFromCache.AddRange(entities);
                _cache.Set(methodKey, entitiesFromCache);

                entitiesToReturn.AddRange(entities);
            }

            return entitiesToReturn;
        }
    }
}
