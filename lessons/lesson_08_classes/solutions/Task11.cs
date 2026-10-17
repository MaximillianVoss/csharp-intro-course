namespace Course.Lesson08;

public static class Task11
{
    public struct Snapshot { public int Revision; public int[] Values; }
    public static int[] Replay()
    {
        Snapshot a = new() { Revision = 1, Values = new[] { 10 } };
        Snapshot b = a; b.Revision = 2; b.Values[0] = 99;
        return new[] { a.Revision, b.Revision, a.Values[0], b.Values[0] };
    }
    public static Snapshot DeepCopy(Snapshot source) => new() { Revision = source.Revision, Values = (int[])source.Values.Clone() };
    public static void Run()
    {
        Check.SequenceEqual(new[] { 1, 2, 99, 99 }, Replay());
        Snapshot source = new() { Revision = 7, Values = new[] { 10 } }; Snapshot copy = DeepCopy(source);
        copy.Values[0] = 20; Check.Equal(10, source.Values[0]); Check.Equal(7, copy.Revision);
        Check.Equal(0, DeepCopy(new Snapshot { Revision = 0, Values = Array.Empty<int>() }).Values.Length);
    }
}
