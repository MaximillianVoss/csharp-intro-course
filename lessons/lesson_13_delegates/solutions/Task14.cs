using Course;

namespace Course.Lesson13;

public static class Task14
{
    public static Predicate<T> All<T>(params Predicate<T>[] rules)
    {
        if (rules is null) throw new ArgumentException("Null rules."); foreach (var rule in rules) if (rule is null) throw new ArgumentException("Null rule.");
        var copy = (Predicate<T>[])rules.Clone(); return value => { foreach (var rule in copy) if (!rule(value)) return false; return true; };
    }
    public static void Run()
    {
        int later = 0; var both = All<int>(x => x > 0, x => { later++; return x % 2 == 0; }); Check.True(!both(-2)); Check.Equal(0, later); Check.True(both(2)); Check.Equal(1, later);
        Check.True(All<int>()(0)); Predicate<int>[] source = { x => x > 0 }; var saved = All(source); source[0] = _ => false; Check.True(saved(1));
        Check.Throws<ArgumentException>(() => All<int>(null!)); Check.Throws<ArgumentException>(() => All<int>(new Predicate<int>[] { null! }));
        var error = All<int>(_ => throw new FormatException(), _ => { later++; return true; }); Check.Throws<FormatException>(() => error(1)); Check.Equal(1, later);
    }
}
