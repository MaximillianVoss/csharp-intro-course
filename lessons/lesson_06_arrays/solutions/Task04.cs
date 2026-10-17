namespace Course.Lesson06;

public static class Task04
{
    public static int[] PositiveCopy(int[] values)
    {
        int count = 0;
        foreach (int value in values) if (value > 0) count++;
        int[] result = new int[count];
        int write = 0;
        foreach (int value in values) if (value > 0) result[write++] = value;
        return result;
    }
    public static void Run()
    {
        int[] source = { -2, 4, 0, 3, 4 };
        int[] result = PositiveCopy(source);
        Check.SequenceEqual(new[] { 4, 3, 4 }, result);
        Check.SequenceEqual(Array.Empty<int>(), PositiveCopy(new[] { 0, -1 }));
        Check.SequenceEqual(Array.Empty<int>(), PositiveCopy(Array.Empty<int>()));
        result[0] = 99;
        Check.SequenceEqual(new[] { -2, 4, 0, 3, 4 }, source);
    }
}
