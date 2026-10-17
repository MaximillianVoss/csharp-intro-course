namespace Course.Lesson07;

public static class Task12
{
    public static bool SameMultiset(int[] left, int[] right)
    {
        if (left.Length != right.Length) return false;
        Dictionary<int, int> counts = new();
        foreach (int value in left) { counts.TryGetValue(value, out int count); counts[value] = count + 1; }
        foreach (int value in right)
        {
            if (!counts.TryGetValue(value, out int count) || count == 0) return false;
            counts[value] = count - 1;
        }
        return true;
    }
    public static void Run()
    {
        Check.True(SameMultiset(new[] { 1, 2, 1 }, new[] { 2, 1, 1 }));
        Check.True(!SameMultiset(new[] { 1, 1, 2 }, new[] { 1, 2, 2 }));
        Check.True(!SameMultiset(new[] { 1 }, Array.Empty<int>()));
        Check.True(SameMultiset(Array.Empty<int>(), Array.Empty<int>()));
        Check.True(!SameMultiset(new[] { int.MinValue }, new[] { int.MaxValue }));
    }
}
