namespace Course.Lesson06;

public static class Task09
{
    public static int FirstIndexSorted(int[] values, int target)
    {
        int lo = 0, hi = values.Length;
        while (lo < hi)
        {
            int middle = lo + (hi - lo) / 2;
            if (values[middle] < target) lo = middle + 1;
            else hi = middle;
        }
        return lo < values.Length && values[lo] == target ? lo : -1;
    }
    public static void Run()
    {
        Check.Equal(1, FirstIndexSorted(new[] { 1, 2, 2, 2, 8 }, 2));
        Check.Equal(-1, FirstIndexSorted(new[] { 1, 2 }, 3));
        Check.Equal(-1, FirstIndexSorted(Array.Empty<int>(), 1));
        Check.Equal(0, FirstIndexSorted(new[] { int.MinValue, 0 }, int.MinValue));
        Check.Equal(-1, FirstIndexSorted(new[] { 1, 4 }, 2));
    }
}
