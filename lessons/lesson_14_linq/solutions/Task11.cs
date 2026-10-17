using Course;

namespace Course.Lesson14;

public static class Task11
{
    public sealed record Revision(string Key, int Version, string Value);
    public static Dictionary<string, Revision> Latest(IEnumerable<Revision> source)
    {
        ArgumentNullException.ThrowIfNull(source); var items = source.ToArray();
        if (items.Any(x => x is null || string.IsNullOrWhiteSpace(x.Key) || x.Version < 0 || x.Value is null)) throw new ArgumentException("Revision.");
        return items.Select((value, index) => new { Value = value, Index = index }).GroupBy(x => x.Value.Key, StringComparer.Ordinal)
            .ToDictionary(g => g.Key, g => g.OrderByDescending(x => x.Value.Version).ThenByDescending(x => x.Index).First().Value, StringComparer.Ordinal);
    }
    public static void Run()
    {
        var last = new Revision("a", 2, "new"); var map = Latest(new[] { new Revision("a", 2, "old"), new Revision("a", 1, "x"), last, new Revision("a", 0, "later"), new Revision("A", 0, "upper") });
        Check.True(ReferenceEquals(last, map["a"])); Check.Equal(2, map.Count); Check.Equal("upper", map["A"].Value);
        Check.Equal(0, Latest(Array.Empty<Revision>()).Count); Check.Throws<ArgumentException>(() => Latest(new[] { new Revision("x", -1, "x") })); Check.Throws<ArgumentNullException>(() => Latest(null!));
    }
}
