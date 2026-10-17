using Course;

namespace Course.Lesson13;

public static class Task10
{
    public static int CountPassing(int[] values, Predicate<int> accept)
    {
        ArgumentNullException.ThrowIfNull(values); ArgumentNullException.ThrowIfNull(accept); int count = 0;
        foreach (var x in values) if (accept(x)) count++; return count;
    }
    private static int Broken(int[] values, Predicate<int> accept) { int count = 0; foreach (var x in values) if (accept(x) && accept(x)) count++; return count; }
    public static void Run()
    {
        int goodCalls = 0; Check.Equal(2, CountPassing(new[] { 1, 2, 4 }, x => { goodCalls++; return x % 2 == 0; })); Check.Equal(3, goodCalls);
        int badCalls = 0; Check.Equal(2, Broken(new[] { 1, 2, 4 }, x => { badCalls++; return x % 2 == 0; })); Check.Equal(5, badCalls); Check.True(badCalls != goodCalls);
        int emptyCalls = 0; Check.Equal(0, CountPassing(Array.Empty<int>(), x => { emptyCalls++; return true; })); Check.Equal(0, emptyCalls);
        Check.Throws<ArgumentNullException>(() => CountPassing(new[] { 1 }, null!));
    }
}
