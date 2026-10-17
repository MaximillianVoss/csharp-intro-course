namespace Course.Lesson16;

public static class Task02
{
    private static class Shared
    {
        private static int value;
        public static int Next() => ++value;
        public static void Reset() => value = 0;
    }
    public sealed class Counter { private int value; public int Next() => ++value; }
    public static (int,int) SharedPair() { var shared = new Counter(); return (shared.Next(), shared.Next()); }
    public static void Run()
    {
        Shared.Reset(); Check.Equal((1,2),(Shared.Next(),Shared.Next()));
        Shared.Reset(); int observed=Shared.Next(); Shared.Reset();
        Check.Equal((1,1),(observed,Shared.Next()));
        Check.Equal((1,2), SharedPair());
        var first = new Counter(); var second = new Counter();
        Check.Equal(1,first.Next()); Check.Equal(1,second.Next()); Check.Equal(2,first.Next());
    }
}
