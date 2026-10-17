using Course;

namespace Course.Lesson13;

public static class Task13
{
    public static Func<int, int> Memoize(Func<int, int> compute)
    {
        ArgumentNullException.ThrowIfNull(compute); var cache = new Dictionary<int, int>();
        return key => { if (cache.TryGetValue(key, out var value)) return value; value = compute(key); cache.Add(key, value); return value; };
    }
    public static void Run()
    {
        int calls = 0; var memo = Memoize(x => { calls++; return x * x; }); Check.Equal(9, memo(3)); Check.Equal(9, memo(3)); Check.Equal(1, calls);
        Check.Equal(0, memo(0)); Check.Equal(0, memo(0)); Check.Equal(2, calls);
        int attempts = 0; var bad = Memoize(_ => { attempts++; throw new FormatException(); }); Check.Throws<FormatException>(() => bad(1)); Check.Throws<FormatException>(() => bad(1)); Check.Equal(2, attempts);
        var other = Memoize(x => { calls++; return x; }); Check.Equal(3, other(3)); Check.Equal(3, calls);
        Check.Throws<ArgumentNullException>(() => Memoize(null!));
    }
}
