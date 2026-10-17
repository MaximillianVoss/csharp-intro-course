using Course;

namespace Course.Lesson13;

public static class Task08
{
    public static long Accumulate(IReadOnlyList<int> values, Action<long> report)
    {
        ArgumentNullException.ThrowIfNull(values); ArgumentNullException.ThrowIfNull(report); long sum = 0;
        foreach (int value in values) { sum = checked(sum + value); report(sum); } return sum;
    }
    public static void Run()
    {
        var seen = new List<long>(); Check.Equal(5L, Accumulate(new[] { 2, -1, 4 }, seen.Add)); Check.SequenceEqual(new long[] { 2, 1, 5 }, seen);
        Check.Equal(4294967294L, Accumulate(new[] { int.MaxValue, int.MaxValue }, _ => { }));
        int calls = 0; Check.Equal(0L, Accumulate(Array.Empty<int>(), _ => calls++)); Check.Equal(0, calls);
        Check.Throws<FormatException>(() => Accumulate(new[] { 1, 2 }, _ => { calls++; throw new FormatException(); })); Check.Equal(1, calls);
        Check.Throws<ArgumentNullException>(() => Accumulate(new[] { 1 }, null!));
    }
}
