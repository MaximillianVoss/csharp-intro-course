using Course;

namespace Course.Lesson13;

public static class Task02
{
    public static Func<T, T> Compose<T>(Func<T, T> first, Func<T, T> second)
    {
        ArgumentNullException.ThrowIfNull(first); ArgumentNullException.ThrowIfNull(second); return x => second(first(x));
    }
    public static void Run()
    {
        int calls = 0; var composed = Compose<int>(x => { calls++; return x + 1; }, x => x * 2);
        Check.Equal(0, calls); Check.Equal(8, composed(3)); Check.Equal(12, composed(5)); Check.Equal(2, calls);
        bool second = false; var bad = Compose<int>(_ => throw new FormatException(), x => { second = true; return x; });
        Check.Throws<FormatException>(() => bad(0)); Check.True(!second);
        Check.Throws<ArgumentNullException>(() => Compose<int>(null!, x => x)); Check.Throws<ArgumentNullException>(() => Compose<int>(x => x, null!));
    }
}
