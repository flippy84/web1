using System.Linq;

public static class Extensions
{
    public static IQueryable<T> Page<T>(this IQueryable<T> query, int page)
    {
        return query.Skip((page - 1) * 10).Take(10);
    }

    public static string Ellipsis(this string str, int length)
    {
        return str.Length <= length ? str : str.Substring(0, length - 3) + "...";
    }
}