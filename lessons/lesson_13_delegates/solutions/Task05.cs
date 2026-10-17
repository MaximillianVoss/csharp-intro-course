using Course;

namespace Course.Lesson13;

public static class Task05
{
    public static Predicate<int> InRange(int minimum, int maximum)
    {
        if (minimum > maximum) throw new ArgumentException("Invalid range."); return x => x >= minimum && x <= maximum;
    }
    public static void Run()
    {
        var rule = InRange(2, 4); Check.True(!rule(1)); Check.True(rule(2)); Check.True(rule(4)); Check.True(!rule(5));
        var point = InRange(3, 3); Check.True(point(3)); Check.True(!point(2)); Check.True(rule(2));
        var all = InRange(int.MinValue, int.MaxValue); Check.True(all(int.MinValue)); Check.True(all(int.MaxValue));
        Check.Throws<ArgumentException>(() => InRange(5, 4));
    }
}
