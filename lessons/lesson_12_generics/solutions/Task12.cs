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
    private sealed class Counted : IComparable<Counted>
    {
        public static int Comparisons; public int Key { get; }
        public Counted(int key) { Key = key; }
        public int CompareTo(Counted? other) { Comparisons++; return other is null ? 1 : Key - other.Key; }
    }
    public static void Run()
    {
        var data = new[] { 1, 2, 2, 2, 5 }; Check.Equal(1, FirstIndex(data, 2)); Check.Equal(0, FirstIndex(data, 1));
        foreach (int missing in new[] { 0, 3, 9 }) Check.Equal(-1, FirstIndex(data, missing));
        Check.Equal(-1, FirstIndex(Array.Empty<int>(), 1)); Check.Equal(1, FirstIndex(new[] { "a", "b", "b" }, "b"));
        Check.Throws<ArgumentNullException>(() => FirstIndex<int>(null!, 1)); Check.Throws<ArgumentNullException>(() => FirstIndex(new[] { "a" }, null!));
        var measured = new Counted[1024]; for (int i = 0; i < measured.Length; i++) measured[i] = new Counted(i);
        Counted.Comparisons = 0; Check.Equal(0, FirstIndex(measured, new Counted(0))); Check.True(Counted.Comparisons <= 12, "Linear scanning violates the comparison bound.");
        Counted.Comparisons = 0; Check.Equal(-1, FirstIndex(measured, new Counted(2000))); Check.True(Counted.Comparisons <= 12);
    }
}
