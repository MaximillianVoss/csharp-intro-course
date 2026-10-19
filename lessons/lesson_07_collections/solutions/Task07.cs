namespace Course.Lesson07;

public static class Task07
{
    public static List<int> CommonInLeftOrder(int[] left, int[] right)
    {
        HashSet<int> available = new(right); List<int> result = new();
        foreach (int value in left) if (available.Remove(value)) result.Add(value);
        return result;
    }
    public static void Run()
    {
        Check.SequenceEqual(new[] { 4, 2 }, CommonInLeftOrder(new[] { 4, 2, 4, 3, 2 }, new[] { 2, 4, 4 }));
        Check.SequenceEqual(Array.Empty<int>(), CommonInLeftOrder(new[] { 1 }, new[] { 2 }));
        Check.SequenceEqual(Array.Empty<int>(), CommonInLeftOrder(Array.Empty<int>(), new[] { 1 }));
        Check.SequenceEqual(Array.Empty<int>(), CommonInLeftOrder(new[] { 1 }, Array.Empty<int>()));
        int[] left = { 4, 2, 4, 3 }; int[] right = { 2, 4, 4 };
        CommonInLeftOrder(left, right);
        Check.SequenceEqual(new[] { 4, 2, 4, 3 }, left); Check.SequenceEqual(new[] { 2, 4, 4 }, right);
    }
}
