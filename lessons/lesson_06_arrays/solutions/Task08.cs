namespace Course.Lesson06;

public static class Task08
{
    public static int[] MergeSorted(int[] left, int[] right)
    {
        int[] result = new int[left.Length + right.Length];
        int i = 0, j = 0, write = 0;
        while (i < left.Length && j < right.Length)
            result[write++] = left[i] <= right[j] ? left[i++] : right[j++];
        while (i < left.Length) result[write++] = left[i++];
        while (j < right.Length) result[write++] = right[j++];
        return result;
    }
    public static void Run()
    {
        Check.SequenceEqual(new[] { 1, 2, 3, 3, 3 }, MergeSorted(new[] { 1, 3, 3 }, new[] { 2, 3 }));
        Check.SequenceEqual(new[] { 4 }, MergeSorted(Array.Empty<int>(), new[] { 4 }));
        Check.SequenceEqual(Array.Empty<int>(), MergeSorted(Array.Empty<int>(), Array.Empty<int>()));
        Check.SequenceEqual(new[] { -4, -2, 1, 5 }, MergeSorted(new[] { -2, 1, 5 }, new[] { -4 }));
        int[] shared = { 1, 2, 2 };
        int[] merged = MergeSorted(shared, shared);
        Check.SequenceEqual(new[] { 1, 1, 2, 2, 2, 2 }, merged);
        merged[0] = 99;
        Check.SequenceEqual(new[] { 1, 2, 2 }, shared);
    }
}
