using Course;

namespace Course.Lesson14;

public static class Task06
{
    public sealed record WordCount(string Word, int Count);
    public static WordCount[] Frequencies(IEnumerable<string> words)
    {
        ArgumentNullException.ThrowIfNull(words); var items = words.ToArray(); if (items.Any(x => x is null)) throw new ArgumentException("Null word.");
        return items.Select(x => x.Trim().ToLowerInvariant()).Where(x => x.Length > 0).GroupBy(x => x, StringComparer.Ordinal)
            .Select(g => new WordCount(g.Key, g.Count())).OrderBy(x => x.Word, StringComparer.Ordinal).ToArray();
    }
    public static void Run()
    {
        Check.SequenceEqual(new[] { new WordCount("cat", 2), new WordCount("dog", 1) }, Frequencies(new[] { " Cat ", "DOG", "cat", " " }));
        Check.Equal(0, Frequencies(new[] { "", " " }).Length); Check.Equal(0, Frequencies(Array.Empty<string>()).Length);
        Check.Throws<ArgumentException>(() => Frequencies(new string[] { "x", null! })); Check.Throws<ArgumentNullException>(() => Frequencies(null!));
    }
}
