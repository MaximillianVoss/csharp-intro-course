using Course;

namespace Course.Lesson13;

public static class Task06
{
    public static T[] Filter<T>(IEnumerable<T> source, Predicate<T> accept)
    {
        ArgumentNullException.ThrowIfNull(source); ArgumentNullException.ThrowIfNull(accept); var result = new List<T>();
        foreach (var item in source) if (accept(item)) result.Add(item); return result.ToArray();
    }
    public static void Run()
    {
        int calls = 0; Check.SequenceEqual(new[] { 2, 2 }, Filter(new[] { 3, 2, 2, 1 }, x => { calls++; return x % 2 == 0; })); Check.Equal(4, calls);
        Check.Equal(0, Filter(Array.Empty<int>(), _ => true).Length);
        int visits = 0; Check.Throws<FormatException>(() => Filter(new[] { 1, 2, 3 }, x => { visits++; if (x == 2) throw new FormatException(); return true; })); Check.Equal(2, visits);
        Check.Throws<ArgumentNullException>(() => Filter<int>(null!, _ => true)); Check.Throws<ArgumentNullException>(() => Filter(new[] { 1 }, null!));
    }
}
