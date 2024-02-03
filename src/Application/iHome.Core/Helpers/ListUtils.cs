namespace iHome.Core.Helpers;

public class ListUtils
{
    public static List<T> Empty<T>()
    {
        return Enumerable.Empty<T>().ToList();
    }
}
