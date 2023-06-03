namespace iHome.Shared.Helpers
{
    public static class LINQExtensions
    {
        public static IEnumerable<TSource> SafeWhere<TSource>(this IQueryable<TSource> enumerable, Func<TSource, bool>? action)
        {
            if (action == null) return enumerable;

            return enumerable.Where(action);
        }

        public static IEnumerable<TSource> SafeWhere<TSource>(this IEnumerable<TSource> enumerable, Func<TSource, bool>? action)
        {
            if (action == null) return enumerable;

            return enumerable.Where(action);
        }

        public static List<TOutput> SelectToList<TSource, TOutput>(this IEnumerable<TSource> enumerable, Func<TSource, TOutput> action)
        {
            return enumerable
                .Select(action)
                .ToList();
        }
    }
}
