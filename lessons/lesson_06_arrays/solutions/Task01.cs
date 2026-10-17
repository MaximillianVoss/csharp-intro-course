namespace Course.Lesson06;

public static class Task01
{
    public static int FirstMaximumIndex(int[] values)
    {
        if (values.Length == 0) return -1;
        int best = 0;
        for (int i = 1; i < values.Length; i++)
            if (values[i] > values[best]) best = i;
        return best;
    }
    public static void Run()
    {
        Check.Equal(1, FirstMaximumIndex(new[] { 3, 8, 8, -2 }));
        Check.Equal(1, FirstMaximumIndex(new[] { -8, -2, -9 }));
        Check.Equal(-1, FirstMaximumIndex(Array.Empty<int>()));
        Check.Equal(0, FirstMaximumIndex(new[] { int.MinValue }));
    }
}
