namespace Course.Lesson06;

public static class Task10
{
    public static double Average(int[] values)
    {
        if (values.Length == 0) return double.NaN;
        long total = 0;
        foreach (int value in values) total += value;
        return (double)total / values.Length;
    }
    public static void Run()
    {
        Check.Near(1.5, Average(new[] { 1, 2 }));
        Check.Near(int.MaxValue, Average(new[] { int.MaxValue, int.MaxValue }));
        Check.True(double.IsNaN(Average(Array.Empty<int>())));
        Check.Near(-2.5, Average(new[] { -2, -3 }));
    }
}
