using Course;

namespace Course.Lesson14;

public static class Task08
{
    public sealed record Seat(int Row, int Column, string Label);
    public static Seat[] Flatten(string[][] rows)
    {
        ArgumentNullException.ThrowIfNull(rows); if (rows.Any(row => row is null || row.Any(x => x is null))) throw new ArgumentException("Null row/label.");
        return rows.SelectMany((row, r) => row.Select((label, c) => new Seat(r, c, label))).ToArray();
    }
    public static void Run()
    {
        Check.SequenceEqual(new[] { new Seat(0, 0, "A"), new Seat(0, 1, "B"), new Seat(2, 0, "A") }, Flatten(new[] { new[] { "A", "B" }, Array.Empty<string>(), new[] { "A" } }));
        Check.Equal(new Seat(0, 0, ""), Flatten(new[] { new[] { "" } })[0]); Check.Equal(0, Flatten(Array.Empty<string[]>()).Length);
        Check.Throws<ArgumentException>(() => Flatten(new string[][] { null! })); Check.Throws<ArgumentException>(() => Flatten(new[] { new string[] { null! } }));
    }
}
