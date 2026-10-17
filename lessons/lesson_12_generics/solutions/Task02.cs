using Course;

namespace Course.Lesson12;

public static class Task02
{
    public static T FirstOr<T>(IReadOnlyList<T> items, T fallback)
    {
        ArgumentNullException.ThrowIfNull(items); return items.Count == 0 ? fallback : items[0];
    }
    public static void Run()
    {
        var values = new[] { 0, 5 }; Check.Equal(0, FirstOr(values, 9)); Check.SequenceEqual(new[] { 0, 5 }, values);
        Check.Equal(9, FirstOr(Array.Empty<int>(), 9));
        Check.Equal<string?>(null, FirstOr(new string?[] { null }, "x"));
        Check.Throws<ArgumentNullException>(() => FirstOr<int>(null!, 0));
    }
}
