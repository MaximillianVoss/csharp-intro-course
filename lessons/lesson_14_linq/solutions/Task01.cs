using Course;

namespace Course.Lesson14;

public static class Task01
{
    public static int[] EvenSquares(IEnumerable<int> source)
    {
        ArgumentNullException.ThrowIfNull(source); var items = source.ToArray();
        if (items.Any(x => x < -1000 || x > 1000)) throw new ArgumentOutOfRangeException(nameof(source));
        return items.Where(x => x % 2 == 0).Select(x => x * x).ToArray();
    }
    public static void Run()
    {
        Check.SequenceEqual(new[] { 4, 0, 16, 4 }, EvenSquares(new[] { 3, -2, 0, 4, -2 }));
        Check.Equal(0, EvenSquares(Array.Empty<int>()).Length); Check.SequenceEqual(new[] { 1000000 }, EvenSquares(new[] { -1000 }));
        Check.Throws<ArgumentOutOfRangeException>(() => EvenSquares(new[] { 1001 })); Check.Throws<ArgumentNullException>(() => EvenSquares(null!));
        int enumerations = 0;
        IEnumerable<int> Once() { if (++enumerations != 1) throw new InvalidOperationException("Second pass."); yield return 2; yield return 3; }
        Check.SequenceEqual(new[] { 4 }, EvenSquares(Once())); Check.Equal(1, enumerations);
    }
}
