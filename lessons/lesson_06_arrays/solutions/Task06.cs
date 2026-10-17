namespace Course.Lesson06;

public static class Task06
{
    public static int[] RotateRight(int[] values, int k)
    {
        int n = values.Length;
        int[] result = new int[n];
        if (n == 0) return result;
        int shift = k % n;
        for (int i = 0; i < n; i++) result[(i + shift) % n] = values[i];
        return result;
    }
    public static void Run()
    {
        Check.SequenceEqual(new[] { 40, 10, 20, 30 }, RotateRight(new[] { 10, 20, 30, 40 }, 5));
        int[] source = { 1, 2 }; int[] copy = RotateRight(source, 0); copy[0] = 9;
        Check.Equal(1, source[0]);
        Check.SequenceEqual(Array.Empty<int>(), RotateRight(Array.Empty<int>(), 7));
        Check.SequenceEqual(new[] { 2, 1 }, RotateRight(new[] { 1, 2 }, int.MaxValue));
    }
}
