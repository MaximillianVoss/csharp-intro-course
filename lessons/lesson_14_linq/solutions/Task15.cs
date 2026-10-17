using Course;

namespace Course.Lesson14;

public static class Task15
{
    public sealed record LogEntry(string Path, int Status, double Milliseconds); public sealed record Endpoint(string Path, int Requests, int Failures, double AverageMs);
    public static Endpoint[] Report(IEnumerable<LogEntry> entries, int top)
    {
        ArgumentNullException.ThrowIfNull(entries); if (top < 0 || top > 100) throw new ArgumentOutOfRangeException(nameof(top)); var items = entries.ToArray();
        if (items.Any(x => x is null || string.IsNullOrWhiteSpace(x.Path) || x.Status < 100 || x.Status > 599 || !double.IsFinite(x.Milliseconds) || x.Milliseconds < 0 || x.Milliseconds > 1000000)) throw new ArgumentException("Entry.");
        return items.GroupBy(x => x.Path, StringComparer.Ordinal).Select(g => new Endpoint(g.Key, g.Count(), g.Count(x => x.Status >= 500), g.Average(x => x.Milliseconds)))
            .OrderByDescending(x => x.Failures).ThenBy(x => x.Path, StringComparer.Ordinal).Take(top).ToArray();
    }
    public static void Run()
    {
        var data = new[] { new LogEntry("/b", 200, 10), new LogEntry("/a", 500, 20), new LogEntry("/a", 200, 0), new LogEntry("/c", 500, 4) };
        var report = Report(data, 2); Check.SequenceEqual(new[] { "/a", "/c" }, report.Select(x => x.Path)); Check.Equal(2, report[0].Requests); Check.Equal(1, report[0].Failures); Check.Near(10, report[0].AverageMs);
        Check.Equal(0, Report(data, 0).Length); Check.Equal(0, Report(Array.Empty<LogEntry>(), 5).Length);
        Check.Throws<ArgumentException>(() => Report(new[] { new LogEntry("/a", 200, double.NaN) }, 0)); Check.Throws<ArgumentException>(() => Report(new[] { new LogEntry("/a", 600, 1) }, 1)); Check.Throws<ArgumentOutOfRangeException>(() => Report(data, 101));
    }
}
