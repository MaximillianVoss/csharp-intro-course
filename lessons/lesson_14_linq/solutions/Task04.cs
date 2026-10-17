using Course;

namespace Course.Lesson14;

public static class Task04
{
    public static bool AllPassed(IEnumerable<int> scores, int threshold)
    {
        ArgumentNullException.ThrowIfNull(scores); if (threshold < 0 || threshold > 100) throw new ArgumentOutOfRangeException(nameof(threshold));
        var items = scores.ToArray(); if (items.Any(x => x < 0 || x > 100)) throw new ArgumentOutOfRangeException(nameof(scores));
        return items.Length > 0 && items.All(x => x >= threshold);
    }
    public static void Run()
    {
        Check.True(Array.Empty<int>().All(x => x >= 50)); Check.True(!AllPassed(Array.Empty<int>(), 50));
        Check.True(AllPassed(new[] { 50, 70 }, 50)); Check.True(!AllPassed(new[] { 49, 100 }, 50)); Check.True(AllPassed(new[] { 0 }, 0));
        Check.Throws<ArgumentOutOfRangeException>(() => AllPassed(new[] { 0, 101 }, 50)); Check.Throws<ArgumentOutOfRangeException>(() => AllPassed(new[] { 1 }, -1)); Check.Throws<ArgumentNullException>(() => AllPassed(null!, 50));
    }
}
