namespace Course.Lesson07;

public static class Task10
{
    public static List<int> FilterAllowed(int[] requested, int[] allowed)
    {
        HashSet<int> index = new(allowed); List<int> result = new();
        foreach (int value in requested) if (index.Contains(value)) result.Add(value);
        return result;
    }
    public static void Run()
    {
        Check.SequenceEqual(new[] { 3, 3, 2 }, FilterAllowed(new[] { 3, 1, 3, 2 }, new[] { 3, 3, 2 }));
        Check.SequenceEqual(new[] { 1 }, FilterAllowed(new[] { 1 }, new[] { 1, 1 }));
        Check.Equal(0, FilterAllowed(new[] { 1, 2 }, Array.Empty<int>()).Count);
        Check.Equal(0, FilterAllowed(Array.Empty<int>(), new[] { 1 }).Count);
    }
}
