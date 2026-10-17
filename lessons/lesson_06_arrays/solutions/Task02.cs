namespace Course.Lesson06;

public static class Task02
{
    public static int[] Replay()
    {
        int[] a = { 2, 4 };
        int[] alias = a;
        int[] copy = (int[])a.Clone();
        alias[0] = 9;
        copy[1] = 7;
        return new[] { a[0], a[1], copy[1] };
    }
    public static void Run()
    {
        Check.SequenceEqual(new[] { 9, 4, 7 }, Replay());
        int[] a = { 2, 4 };
        int[] alias = a;
        int[] copy = a;
        alias[0] = 9;
        copy[1] = 7;
        Check.SequenceEqual(new[] { 9, 7, 7 }, new[] { a[0], a[1], copy[1] });
        Check.True(a[1] != Replay()[1]);
    }
}
