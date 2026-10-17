using Course;

namespace Course.Lesson12;

public static class Task12
{
    public static int FirstIndex<T>(IReadOnlyList<T> sorted, T target) where T : IComparable<T>
    {
        ArgumentNullException.ThrowIfNull(sorted); ArgumentNullException.ThrowIfNull(target);
        int left = 0, right = sorted.Count;
        while (left < right) { int mid = left + (right - left) / 2; if (sorted[mid].CompareTo(target) < 0) left = mid + 1; else right = mid; }
        return left < sorted.Count && sorted[left].CompareTo(target) == 0 ? left : -1;
    }
    public static void Run()
    {
        var data = new[] { 1, 2, 2, 2, 5 }; Check.Equal(1, FirstIndex(data, 2)); Check.Equal(0, FirstIndex(data, 1));
        foreach (int missing in new[] { 0, 3, 9 }) Check.Equal(-1, FirstIndex(data, missing));
        Check.Equal(-1, FirstIndex(Array.Empty<int>(), 1)); Check.Equal(1, FirstIndex(new[] { "a", "b", "b" }, "b"));
        Check.Throws<ArgumentNullException>(() => FirstIndex<int>(null!, 1)); Check.Throws<ArgumentNullException>(() => FirstIndex(new[] { "a" }, null!));
    }
}
