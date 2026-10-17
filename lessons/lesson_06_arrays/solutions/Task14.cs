namespace Course.Lesson06;

public static class Task14
{
    public static void ShiftRight(int[] values, int places)
    {
        for (int i = values.Length - 1; i >= places; i--) values[i] = values[i - places];
        for (int i = 0; i < places; i++) values[i] = 0;
    }
    public static void Run()
    {
        int[] wrong = { 1, 2, 3, 4 }; int places = 1;
        for (int i = places; i < wrong.Length; i++) wrong[i] = wrong[i - places];
        for (int i = 0; i < places; i++) wrong[i] = 0;
        Check.SequenceEqual(new[] { 0, 1, 1, 1 }, wrong);
        int[] right = { 1, 2, 3, 4 }; ShiftRight(right, 1); Check.SequenceEqual(new[] { 0, 1, 2, 3 }, right);
        ShiftRight(right, 4); Check.SequenceEqual(new[] { 0, 0, 0, 0 }, right);
        int[] unchanged = { 5, 6 }; ShiftRight(unchanged, 0); Check.SequenceEqual(new[] { 5, 6 }, unchanged);
        int[] empty = Array.Empty<int>(); ShiftRight(empty, 0); Check.Equal(0, empty.Length);
    }
}
