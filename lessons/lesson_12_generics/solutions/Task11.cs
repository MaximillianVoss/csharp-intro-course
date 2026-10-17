using Course;

namespace Course.Lesson12;

public static class Task11
{
    public sealed class MultiMap<TKey, TValue> where TKey : notnull
    {
        private readonly Dictionary<TKey, List<TValue>> groups;
        public MultiMap(IEqualityComparer<TKey>? comparer = null) { groups = new(comparer); }
        public void Add(TKey key, TValue value)
        {
            ArgumentNullException.ThrowIfNull(key);
            if (!groups.TryGetValue(key, out var list)) { list = new(); groups.Add(key, list); } list.Add(value);
        }
        public TValue[] Get(TKey key)
        {
            ArgumentNullException.ThrowIfNull(key); return groups.TryGetValue(key, out var list) ? list.ToArray() : new TValue[0];
        }
    }
    public static void Run()
    {
        var map = new MultiMap<string, int>(StringComparer.OrdinalIgnoreCase); map.Add("A", 1); map.Add("a", 2); map.Add("A", 2);
        Check.SequenceEqual(new[] { 1, 2, 2 }, map.Get("a")); Check.Equal(0, map.Get("B").Length);
        var snapshot = map.Get("A"); snapshot[0] = 99; Check.Equal(1, map.Get("A")[0]);
        var nullable = new MultiMap<int, string?>(); nullable.Add(1, null); Check.Equal<string?>(null, nullable.Get(1)[0]);
        Check.Throws<ArgumentNullException>(() => map.Add(null!, 1)); Check.Throws<ArgumentNullException>(() => map.Get(null!));
    }
}
