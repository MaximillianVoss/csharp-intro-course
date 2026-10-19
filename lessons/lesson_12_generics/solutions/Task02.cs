using Course;

namespace Course.Lesson12;

public static class Task02
{
    public static T FirstOr<T>(IReadOnlyList<T> items, T fallback)
    {
        ArgumentNullException.ThrowIfNull(items); return items.Count == 0 ? fallback : items[0];
    }
    private sealed class HeadOnly : IReadOnlyList<int>
    {
        public int Count => 2;
        public int this[int index] => index == 0 ? 0 : throw new InvalidOperationException("Unexpected tail access.");
        public IEnumerator<int> GetEnumerator() => throw new InvalidOperationException("Unexpected enumeration.");
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
    }
    public static void Run()
    {
        var values = new[] { 0, 5 }; Check.Equal(0, FirstOr(values, 9)); Check.SequenceEqual(new[] { 0, 5 }, values);
        Check.Equal(9, FirstOr(Array.Empty<int>(), 9));
        Check.Equal<string?>(null, FirstOr(new string?[] { null }, "x"));
        Check.Throws<ArgumentNullException>(() => FirstOr<int>(null!, 0));
        Check.Equal(0, FirstOr(new HeadOnly(), 9));
    }
}
