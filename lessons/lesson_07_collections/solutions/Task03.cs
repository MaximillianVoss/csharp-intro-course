namespace Course.Lesson07;

public static class Task03
{
    public static List<int> DistinctInOrder(int[] values)
    {
        HashSet<int> seen = new(); List<int> result = new();
        foreach (int value in values) if (seen.Add(value)) result.Add(value);
        return result;
    }
    public static void Run()
    {
        Check.SequenceEqual(new[] { 4, 2, 3 }, DistinctInOrder(new[] { 4, 2, 4, 3, 2 }));
        Check.SequenceEqual(new[] { 7 }, DistinctInOrder(new[] { 7, 7, 7 }));
        Check.SequenceEqual(Array.Empty<int>(), DistinctInOrder(Array.Empty<int>()));
        Check.SequenceEqual(new[] { int.MaxValue, int.MinValue }, DistinctInOrder(new[] { int.MaxValue, int.MinValue, int.MaxValue }));
    }
}
