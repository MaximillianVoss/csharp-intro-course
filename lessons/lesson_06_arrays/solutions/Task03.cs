namespace Course.Lesson06;

public static class Task03
{
    public static long Sum(int[] values)
    {
        long total = 0;
        for (int i = 0; i < values.Length; i++) total += values[i];
        return total;
    }
    public static void Run()
    {
        Check.Equal(10L, Sum(new[] { 5, -2, 7 }));
        Check.Equal(0L, Sum(Array.Empty<int>()));
        Check.Equal(4294967294L, Sum(new[] { int.MaxValue, int.MaxValue }));
        Check.Equal(-9L, Sum(new[] { -9 }));
    }
}
